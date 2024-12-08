using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace HTML_analyser
{
    class Program
    {
        static void Main(string[] args)
        {
            string FilePath = "xkcd_ Periodic Table Regions.html";
            var analyser = new TextAnalyzer(FilePath);

            try
            {
                using (StreamWriter sw = new StreamWriter("vystup.html"))
                {
                    sw.WriteLine("<! -- Přepsaný HTML file s vtipným popiskem pod obrázkem -->");
                    sw.WriteLine(analyser.rewrite);
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
        public string title { get; private set; }
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
                string? line;
                bool foundTitle = false;
                bool foundComic = false;
                while ((line = ReadLine()) != null)
                {
                    rewrite.AppendLine(line);
                    if (foundComic)
                    {
                        string[] HTML_split = line.Split("\"");
                        foreach (string word in HTML_split)
                        {
                            if (foundTitle)
                            {
                                title = word;
                                rewrite.AppendLine($"<p> {title} </p>");
                                break;
                            }
                            if (word.EndsWith("title="))
                            {
                                foundTitle = true;
                            }

                        }
                        foundComic = false;
                    }

                    if (line.Contains("<div id=\"comic\">"))
                    {
                        foundComic = true;
                    }   
                }
            }
        }
    }
}
