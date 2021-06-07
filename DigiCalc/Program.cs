using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "stay";
            do
            {
                Console.WriteLine("please enter the number string, preceeded by the delimiter definition (it can contain one delimiter or multiple delimiters, like this: //[delim1]// or this: //[delim1][delim2]//)");
                Console.WriteLine();
                string input = Console.ReadLine();
                Console.WriteLine();
                List<string> delimiters = readDelimiters(input);
                // List<string> tempDelimiters = new List<string> { "test", "one" };
                if (!delimiters.Any())
                {
                    Console.WriteLine("please provide or check the syntax of the delimiters");
                    Console.WriteLine();
                }
                else
                {
                    input = cutDelimiters(input);
                    // List<string> delimiters = parseDelimiters(delimitersString);
                    List<double> numbers = parseResult(input, new List<double>(), delimiters);
                    double computed = 0;
                    foreach (double element in numbers)
                    {
                        if (element < 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("--- exception ---");
                            Console.WriteLine(string.Format("Negative number {0}. It will be ignored", element));
                            Console.WriteLine("--- endException ---");
                            Console.WriteLine();
                        }
                        else if (element > 1000)
                        {
                            Console.WriteLine();
                            Console.WriteLine("--- exception ---");
                            Console.WriteLine(string.Format("Number {0} is above 1000, it will be ignored", element));
                            Console.WriteLine("--- endException ---");
                            Console.WriteLine();
                        }
                        else
                        {
                            computed += element;
                        }
                    }
                    Console.WriteLine(computed);
                    Console.WriteLine();
                }
                Console.WriteLine("type q to exit or any char to redo the computation");
                Console.WriteLine();
                s = Console.ReadLine();
                Console.WriteLine();
            } while (s != "q");
        }

        private static string cutDelimiters(string input)
        {
            string part1 = input.Substring(0, input.IndexOf("//"));
            string post = input.Substring(input.IndexOf("//") + 2);
            int test = post.IndexOf("//") + 2;
            int test2 = post.Length;
            string part2 = post.Substring(post.IndexOf("//") + 2, test2 - test);

            return part1 + part2;
        }

        private static List<string> readDelimiters(string input)
        {
            List<string> delimiters = new List<string>();
            int io = input.IndexOf("//");
            if (io < 0)
            {
                return delimiters;
            }
            input = input.Substring(io + 2, input.Length - (io + 2));
            io = input.IndexOf("//");
            if (io < 0)
            {
                return delimiters;
            }
            input = input.Substring(0, io);
            while (!String.IsNullOrEmpty(nextDelimiter(input)))
            {
                delimiters.Add(nextDelimiter(input));
                input = input.Substring(nextDelimiter(input).Length + 2);
            }
            return delimiters;
        }

        private static string nextDelimiter(string input)
        {
            if (input.Contains("[") && input.Contains("]"))
            {
                int io = input.IndexOf("[") + 1;
                return input.Substring(io, input.IndexOf("]") - 1);
            }
            return null;
        }

        // deve tornare il delimitatore e la sua lunghezza
        private static DelimitationStrategy firstDelimiterMatch(string input, List<string> deliminters, string lastDelimiter)
        {
            foreach (string delimiter in deliminters)
            {
                if (input.Contains(delimiter))
                {
                    string subset = input.Substring(input.IndexOf(delimiter) + delimiter.Length);
                    if (!String.IsNullOrEmpty(hasDelimiters(input, deliminters)))
                    {
                        return firstDelimiterMatch(subset, deliminters, hasDelimiters(input, deliminters));
                    }
                    else
                    {
                        DelimitationStrategy runtimeDs = new DelimitationStrategy();
                        runtimeDs.Deliminter = lastDelimiter;
                        runtimeDs.DelimiterLength = lastDelimiter.Length;
                        return runtimeDs;
                    }
                }
            }
            if (!String.IsNullOrEmpty(lastDelimiter))
            {
                DelimitationStrategy ds = new DelimitationStrategy();
                ds.Deliminter = lastDelimiter;
                ds.DelimiterLength = lastDelimiter.Length;
                return ds;
            }
            else
            {
                return null;
            }
        }

        private static string hasDelimiters(string input, List<string> deliminters)
        {
            foreach (var delimiter in deliminters)
            {
                if (input.Contains(delimiter)) { return delimiter; }
            }
            return null;
        }

        private static List<double> parseResult(string input, List<double> returnList, List<string> delimiters)
        {
            DelimitationStrategy ds = new DelimitationStrategy();
            ds.Deliminter = "";
            ds.DelimiterLength = 0;
            if (!double.TryParse(input, out double aNumber))
            {
                ds = firstDelimiterMatch(input, delimiters, null);
            }
            if (ds != null)
            {
                if (double.TryParse(input, out double test) || input.Contains(ds.Deliminter))
                {
                    bool last = true;
                    string substring = input;
                    string substringBefore = input;

                    string difference;
                    if (input.Contains(ds.Deliminter))
                    {
                        substring = input.Substring(input.IndexOf(ds.Deliminter) + ds.DelimiterLength, input.Length - (input.IndexOf(ds.Deliminter) + ds.DelimiterLength));
                        substringBefore = input.Substring(0, input.IndexOf(ds.Deliminter));
                        last = false;
                    }
                    if (double.TryParse(substring, out double parsed))
                    {
                        returnList.Add(parsed);
                        if (input.Length > ds.DelimiterLength)
                        {
                            difference = input.Substring(0, input.IndexOf(ds.Deliminter));
                            if (!last)
                            {
                                parseResult(difference, returnList, delimiters);
                            }
                        }
                    }
                    else if (double.TryParse(substringBefore, out double parsedBefore))
                    {
                        returnList.Add(parsedBefore);
                        if (input.Length > ds.DelimiterLength)
                        {
                            difference = input.Substring(input.IndexOf(ds.Deliminter) + ds.DelimiterLength);
                            if (!last)
                            {
                                parseResult(difference, returnList, delimiters);
                            }
                        }
                    }
                    else if (!last)
                    {
                        difference = input.Substring(input.IndexOf(ds.Deliminter) + ds.DelimiterLength, input.Length - (input.IndexOf(ds.Deliminter) + ds.DelimiterLength));
                        // difference = input.Substring(0, input.IndexOf(ds.Deliminter));
                        // difference = input.Substring(input.LastIndexOf(ds.Deliminter) + ds.DelimiterLength);
                        parseResult(difference, returnList, delimiters);
                    }
                }
            }

            return returnList;
        }
    }

    /*private static List<double> oldParseResult(string input, List<double> returnList, List<string> delimiters)
        {
            if (double.TryParse(input, out double test) || input.Contains("ciccio"))
            {
                bool last = true;
                string substring = input;
                string difference;
                if (input.Contains("ciccio"))
                {
                    substring = input.Substring(0, input.IndexOf("ciccio"));
                    last = false;
                }
                if (double.TryParse(substring, out double parsed))
                {
                    returnList.Add(parsed);
                    if(input.Length > "ciccio".Length)
                    {
                        difference = input.Substring(input.IndexOf("ciccio") + "ciccio".Length);
                        if (!last)
                        {
                            parseResult(difference, returnList, delimiters);
                        }
                    }
                } else if (!last)
                {
                    difference = input.Substring(input.IndexOf("ciccio") + "ciccio".Length);
                    parseResult(difference, returnList, delimiters);
                }
            }

            return returnList;
        }
    }*/
    class DelimitationStrategy
    {
        private string _deliminter;
        private int _delimiterLength;

        public string Deliminter { get => _deliminter; set => _deliminter = value; }
        public int DelimiterLength { get => _delimiterLength; set => _delimiterLength = value; }
    }
}
