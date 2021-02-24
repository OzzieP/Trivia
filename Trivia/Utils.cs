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
    }
}