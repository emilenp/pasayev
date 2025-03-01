using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mince
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int[] coins = Console.ReadLine().Split().Select(x => int.Parse(x)).OrderByDescending(x => x).ToArray();
                int goal = int.Parse(Console.ReadLine());
                Console.WriteLine();

                if (goal == 0)
                {
                    Console.WriteLine("Nelze");
                }
                else
                {
                    FindCombinations(coins, goal, coins.Max(), new List<int>());
                }
                Console.WriteLine("-----");
            }
        }
        
        static void FindCombinations(int[] coins, int goal, int last, List<int> current)
        {   
            if (goal == 0)
            {
                Console.WriteLine(string.Join(" ", current));
                return;
            }
            foreach (int coin in coins)
            {
                if (coin <= goal && coin <= last)
                {
                    current.Add(coin);
                    FindCombinations(coins, goal - coin, coin, current);
                    current.RemoveAt(current.Count - 1);
                }
            }
        }
    }
}
