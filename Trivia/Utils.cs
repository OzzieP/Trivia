using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Utils
    {
        public static int Ask(string question, IEnumerable<string> responses)
        {
            while (true)
            {
                Console.WriteLine(question);
                int i = 0;
                foreach (var s in responses)
                {
                    Console.WriteLine(i + ": " + s);
                    i++;
                }

                string line = Console.ReadLine();
                bool isANumber = int.TryParse(line, out int response);
                if (isANumber && response >= 0 && response < responses.Count())
                {
                    return response;
                }

                Console.WriteLine("Please, type a correct number.");
            }
        }

        public static int AskANumber(string question, int minimalValue)
        {
            return AskANumber(question, minimalValue, Int32.MaxValue);
        }

        public static int AskANumber(string question, int minimalValue, int maximalValue)
        {
            while (true)
            {
                Console.WriteLine(question + $" (with minimal value: {minimalValue})");

                string line = Console.ReadLine();
                bool isANumber = int.TryParse(line, out int response);
                if (isANumber && response >= minimalValue && response <= maximalValue)
                {
                    return response;
                }

                Console.WriteLine("Please, type a correct number.");
            }
        }

        public static int AskACategory(Categories rockOrTechno)
        {
            while (true)
            {
                Console.WriteLine("Choose the question theme for the next player.");

                Array enumValueArray = Enum.GetValues(typeof(Categories));
                foreach (int enumValue in enumValueArray)
                {
                    string name = Enum.GetName(typeof(Categories), enumValue);

                    if (name == Categories.Rock.ToString() && rockOrTechno == Categories.Techno)
                        continue;

                    if (name == Categories.Techno.ToString() && rockOrTechno == Categories.Rock)
                        continue;
                    
                    Console.WriteLine($"{enumValue} - {name}");
                }

                string line = Console.ReadLine();
                bool isANumber = int.TryParse(line, out int response);

                if (isANumber)
                {
                    if (response == (int)Categories.Rock && (int)rockOrTechno == (int)Categories.Techno)
                    {
                        Console.WriteLine("Please, type a correct number.");
                        continue;
                    }

                    if (response == (int)Categories.Techno && (int)rockOrTechno == (int)Categories.Rock)
                    {
                        Console.WriteLine("Please, type a correct number.");
                        continue;
                    }

                    return response;
                }

                Console.WriteLine("Please, type a correct number.");
            }
        }


        //public static int AskACategory(Categories rockOrTechno)
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("Choose the question theme for the next player.");

        //        Array enumValueArray = Enum.GetValues(typeof(Categories));
        //        foreach (int enumValue in enumValueArray)
        //        {
        //            string name = Enum.GetName(typeof(Categories), enumValue);

        //            if (name == Categories.Rock.ToString() && rockOrTechno == Categories.Techno)
        //                continue;

        //            if (name == Categories.Techno.ToString() && rockOrTechno == Categories.Rock)
        //                continue;
                    
        //            Console.WriteLine($"{enumValue} - {name}");
        //        }

        //        string line = Console.ReadLine();
        //        bool isANumber = int.TryParse(line, out int response);

        //        if (isANumber)
        //        {
        //            if (response == (int)Categories.Rock && (int)rockOrTechno == (int)Categories.Techno)
        //            {
        //                Console.WriteLine("Please, type a correct number.");
        //                continue;
        //            }

        //            if (response == (int)Categories.Techno && (int)rockOrTechno == (int)Categories.Rock)
        //            {
        //                Console.WriteLine("Please, type a correct number.");
        //                continue;
        //            }

        //            return response;
        //        }

        //        Console.WriteLine("Please, type a correct number.");
        //    }
        //}
    }
}