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
            int maxValue = 0;

            FindBestComboWombo(weights, importances, capacity, pairs, new List<int>(), ref maxValue, bestItems);

            Console.WriteLine(maxValue);
            Console.WriteLine(string.Join(" ", bestItems.Select(i => i + 1)));
        }

        static void FindBestComboWombo(int[] weights, int[] importances, int capacity, Dictionary<int, (int, int)> pairs, List<int> used, ref int maxValue, List<int> bestItems)
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
    }
}
