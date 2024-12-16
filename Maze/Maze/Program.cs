using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "1.txt";
            var pathFinder = new PathFinder(filePath);

            try
            {
                using (StreamWriter sw = new StreamWriter("vystup.txt"))
                {
                    if (pathFinder.path.Count > 0)
                    {
                        sw.WriteLine(pathFinder.path.Count-1);
                    }
                    else
                    {
                        sw.WriteLine("-1");
                    }
                    foreach (var x in pathFinder.path)
                    {
                        sw.Write($"[{x.Item1}, {x.Item2}] ");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Output File Error");
            }
        }
    }

    public class PathFinder : StreamReader
    {
        private int[,] field;
        private int n;
        private Queue<(int r, int c, int t)> mountainQ;
        private List<(int hor, int ver)> directions = new List<(int hor, int ver)>();
        public List<(int, int)> path = new List<(int, int)>();
        public PathFinder(string filePath) : base(filePath)
        {
            try
            {
                ProcessInput();
                FindPath();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Soubor jménem {filePath} neexistuje.");
            }
            catch (Exception)
            {
                Console.WriteLine("Input File Error");
            }
        }

        public void ProcessInput()
        {
            mountainQ = new Queue<(int, int, int)>();
            using (this)
            {
                n = int.Parse(ReadLine());
                int m = int.Parse(ReadLine());
                
                field = new int[n, n];
                field[n-1, n-1] = 18;
                for (int i = 0; i < m; i++)
                {
                    string[] mountain = ReadLine().Split();
                    int r = int.Parse(mountain[0]);
                    int c = int.Parse(mountain[1]);
                    int t = int.Parse(mountain[2]);
                    
                    if (t != 0)
                    {
                        mountainQ.Enqueue((r, c, t));
                    }
                    else
                    {
                        field[r, c] = -1;
                    }
                }
                
                int k = int.Parse(ReadLine());

                for (int i = 0; i < k; i++)
                {
                    string[] moves = ReadLine().Split();
                    int hor = int.Parse(moves[0]);
                    int ver = int.Parse(moves[1]);


                    directions.Add((hor, ver));
                }
            }
        }

        private void FindPath()
        {
            bool[,] visited = new bool[n, n];
            bool yippee = false;
            Queue<(int row, int col, int time)> moveQ = new Queue<(int, int, int)>();
            
            moveQ.Enqueue((0, 0, 0));
            visited[0, 0] = true;

            (int, int)[,] parents = new (int, int)[n, n];

            while (moveQ.Count > 0)
            {
                var (r,c,t) = moveQ.Dequeue();

                if (field[r, c] == 18)
                {
                    yippee = true;
                    PrintPath(parents, path, r, c);
                    break;
                }
                Mountains(t, visited);

                for (int i = 0; i < directions.Count(); i++)
                {
                    int newRow = r + directions[i].Item1;
                    int newCol = c + directions[i].Item2;
                   

                    if (Bounds(newRow, newCol) && !MountainInTheWay(r, c, newRow, field) && !visited[newRow, newCol] && field[newRow, newCol] != -1)
                    {
                        visited[newRow, newCol] = true;
                        parents[newRow, newCol] = (r, c);
                        moveQ.Enqueue((newRow, newCol, t + 1));
                    }
                }
            }
        }
        private void PrintPath((int, int)[,] parents, List<(int,int)> path, int r, int c)
        {
            path.Add((r, c));
            while (parents[r, c] != (0, 0))
            {
                (r, c) = parents[r, c];
                path.Add((r, c));
            }
            path.Add((0, 0));
            path.Reverse();
        }

        private bool MountainInTheWay(int r, int c, int x, int[,] field) // r,c - original positions, x - updated x position (to see how many spaces jumped diagonally)
        {
            for (int i = 1; i < (Math.Abs(x - r)); i++) // it will only work for diagonal movements of the same x, y coordinates
            {
                if (field[r+i, c+i] == -1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Bounds(int x, int y)
        {
            if(x>= 0 && y>=0 && x <= n-1 && y <= n-1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Mountains(int time, bool[,] visited)
        {
            while (mountainQ.Count > 0 && mountainQ.Peek().t == time)
            {
                var (r, c, t) = mountainQ.Dequeue();
                int x = field[r, c];
                if (visited[r,c] == false && x != -1 && x != 18)
                {
                    field[r, c] = -1;
                }
            }
        }
    }   
}