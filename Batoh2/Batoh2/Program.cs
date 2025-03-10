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
        long capacity;
        Dictionary<int, (int, int)> pairs = new Dictionary<int, (int, int)>();

        public MyBenchmark()
        {
            weights = Console.ReadLine().Split().Select(int.Parse).ToArray();
            costs = Console.ReadLine().Split().Select(int.Parse).ToArray();
            capacity = int.Parse(Console.ReadLine());


            pairs[0] = (0, 0);
            for (int i = 1; i < weights.Length+1; i++)
            {
                pairs[i] = (weights[i], costs[i]);
            }
        }

        [Benchmark]
        public void Knapsack_Backtracking()
        {
            List<int> bestItems = new List<int>();
            int maxValue = 0;

            FindBestComboWombo(weights, costs, capacity, pairs, new List<int>(), ref maxValue, bestItems);
        }

        static void FindBestComboWombo(int[] weights, int[] importances, long capacity, Dictionary<int, (int, int)> pairs, List<int> used, ref int maxValue, List<int> bestItems)
        {
            int currentValue = used.Sum(i => pairs[i].Item2);
            if (currentValue > maxValue)
            {
                maxValue = currentValue;
                bestItems.Clear();
                bestItems.AddRange(used);
            }

            foreach (var pair in pairs)
            {
                if (!used.Contains(pair.Key) && pair.Value.Item1 <= capacity)
                {
                    used.Add(pair.Key);
                    FindBestComboWombo(weights, importances, capacity - pair.Value.Item1, pairs, used, ref maxValue, bestItems);
                    used.RemoveAt(used.Count - 1);
                }
            }
        }

        [Benchmark]
        public void Knapsack_DynamicProgramming()
        {
            int[,] m = new int[weights.Count()+1, capacity + 1];

            for(int i = 0; i < weights.Count() + 1; i++)
            {
                for(int j = 0; j < capacity + 1; j++)
                {
                    if (pairs[i].Item1 > j)
                    {
                        int weight = pairs[i].Item1;
                        int cost = pairs[i].Item2;

                        if (cost > m[i - 1, j])
                        {
                            m[i, j] = cost;
                        }


                    }
                }
            }
        }
    }
}