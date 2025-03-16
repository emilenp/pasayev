using System;
using System.Collections.Generic;
using System.Linq;

namespace Knapsack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] weights = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int[] importances = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int capacity = int.Parse(Console.ReadLine());

            Dictionary<int, (int, int)> pairs = new Dictionary<int, (int, int)>();
            for (int i = 0; i < weights.Length; i++)
            {
                pairs[i] = (weights[i], importances[i]);
            }

            List<int> bestItems = new List<int>();
            int maxImportance = 0;

            FindComb(weights, importances, capacity, pairs, new List<int>(), ref maxImportance, bestItems);

            Console.WriteLine(maxImportance);
            Console.WriteLine(string.Join(" ", bestItems.Select(i => i + 1)));
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
    }
}
