using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections;

namespace BenchmarkKnapsack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var results = BenchmarkRunner.Run<MyBenchmark>(); // spustíme benchmarky (vše, co je označeno [Benchmark])
        }
    }

    [MemoryDiagnoser]
    public class MyBenchmark
    {
        int[] weights;
        int[] costs;
        int capacity;
        Dictionary<int, (int, int)> pairs = new Dictionary<int, (int, int)>();

        public MyBenchmark()
        {
            weights = new int[] { 3, 1, 3, 4, 2 };
            costs = new int[] { 2, 2, 4, 5, 3 };
            capacity = 7;

            for (int i = 0; i < weights.Length; i++)
            {
                pairs[i] = (weights[i], costs[i]);
            }
        }

        [Benchmark]
        public void Knapsack_Backtracking()
        {
            List<int> bestItems = new List<int>();
            int maxImportance = 0;

            FindComb(weights, costs, capacity, pairs, new List<int>(), ref maxImportance, bestItems);
        }

        static void FindComb(int[] weights, int[] importances, int capacity, Dictionary<int, (int, int)> pairs, List<int> used, ref int maxImportance, List<int> bestItems)
        {
            int current = used.Sum(i => pairs[i].Item2);
            if (current > maxImportance)
            {
                maxImportance = current;
                bestItems.Clear();
                bestItems.AddRange(used);
            }

            foreach (var pair in pairs)
            {
                if (!used.Contains(pair.Key) && pair.Value.Item1 <= capacity)
                {
                    used.Add(pair.Key);
                    FindComb(weights, importances, capacity - pair.Value.Item1, pairs, used, ref maxImportance, bestItems);
                    used.RemoveAt(used.Count - 1);
                }
            }
        }

        [Benchmark]
        public void Knapsack_DynamicProgramming()
        {
            List<int> bestItems = new List<int>();
            int[,] m = new int[weights.Count() + 1, capacity + 1];

            for (int i = 0; i <= weights.Count(); i++)
            {
                if (i == 0) continue;
                int weight = pairs[i - 1].Item1;
                int cost = pairs[i - 1].Item2;

                for (int j = 0; j <= capacity; j++)
                {
                    if (weight > j)
                    {
                        m[i, j] = m[i - 1, j];
                    }
                    else
                    {
                        m[i, j] = Math.Max(m[i - 1, j], cost + m[i - 1, j - weight]);
                    }
                }
            }

            FindUsedItems(m, m[weights.Count(), capacity], weights.Count(), capacity, pairs, bestItems);
        }

        static void FindUsedItems(int[,] m, int maxCost, int i, int j, Dictionary<int, (int, int)> pairs, List<int> bestItems)
        {
            if (i == 0 || j == 0) return;

            if (maxCost > m[i - 1, j])
            {
                bestItems.Add(i - 1);
                j = j - pairs[i - 1].Item1;
                FindUsedItems(m, maxCost - pairs[i - 1].Item2, i - 1, j, pairs, bestItems);
            }
            else
            {
                FindUsedItems(m, maxCost, i - 1, j, pairs, bestItems);
            }
        }
    }
}