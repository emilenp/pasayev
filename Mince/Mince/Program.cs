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
                // načtení hodnot
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
            if (goal == 0) // od cíle se odečítají použité mince - pokud se cíl bude rovnat 0, znamená to, že součet použitých mincí v current se už rovná cíli - tj. byla dokončena jedna kombinace
            {
                Console.WriteLine(string.Join(" ", current)); // vypíše list a vrátí
                return;
            }
            foreach (int coin in coins) // chceme zkoumat možnosti kombinací pro každou mincí
            {
                if (coin <= goal && coin <= last) // nechceme přesahnout cíl, nechceme vzít větší minci než minulou abysme nevytvořili víc stejných kombinací v jiném pořadí
                {
                    current.Add(coin); // přidáme minci "X" do listu
                    FindCombinations(coins, goal - coin, coin, current); 
                    // rekurzivně voláme funkcí s odečtením mince X od cíle a s listem, který obsahuje minci X. Opakování než  se cíl pro konkrétní větev bude rovnat 0.
                    current.RemoveAt(current.Count - 1); 
                    // backtracking - smažeme minci X abychom mohli opakovat hledání nových kombinací pro další minci Y v této určité pozici v listu, kde byla mince X. 
                }
            }
        }
    }
}
