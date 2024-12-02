using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace FileAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            string FilePath = "vstupy/2_vstup.txt";
            var analyser = new TextAnalyzer(FilePath);

            try
            {
                using (StreamWriter sw = new StreamWriter("vystup.txt"))
                {
                    sw.WriteLine($"Počet slov v souboru: {analyser.WordCount} \r\n" +
                        $"Počet znaků bez bílých znaků: {analyser.CharactersNoWhiteCount} \r\n" +
                        $"Počet znaků s bílými znaky: {analyser.CharactersCount} \r\n");

                    foreach (var x in analyser.words)
                    {
                        sw.WriteLine($"{x.Key}: {x.Value}");
                    }
                    sw.WriteLine();
                    sw.WriteLine($"{analyser.rewrite}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Output File Error");
            }
        }
    }

    public class TextAnalyzer : StreamReader
    {
        public Dictionary<string, int> words = new();
        public int WordCount { get; private set; }
        public int CharactersNoWhiteCount { get; private set; }
        public int CharactersCount { get; private set; }

        public StringBuilder rewrite = new();

        public TextAnalyzer(string file) : base(file)
        {
            try
            {
                FileAnalyse();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Soubor jménem {file} neexistuje.");
            }
            catch (Exception)
            {
                Console.WriteLine("Input File Error");
            }
        }

        private void FileAnalyse()
        {
            using (this)
            {
                words = [];

                string? line;

                while ((line = ReadLine()) != null)
                {
                    CharactersNoWhiteCount += line.Count(c => !char.IsWhiteSpace(c));
                    CharactersCount += line.Length + 2;

                    string[] split = line.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (string word in split)
                    {
                        WordCount++;

                        rewrite.Append(word + " ");

                        if (!words.TryAdd(word, 1))
                        {
                            words[word] += 1;
                        }
                    }

                    rewrite.AppendLine();
                }

                CharactersCount = Math.Max(0, CharactersCount - 2);
            }
        }
    }
}
