using System;
using System.Collections.Generic;
using Trivia;
using System.IO;
using Xunit;

namespace Tests
{
    public class ConsoleMock : TextReader
    {
        private static readonly TextReader defaultIn = Console.In;

        public static void MockConsole(string str)
        {
            Console.SetIn(new ConsoleMock(str));
        }

        public static void UnMockConsole()
        {
            Console.SetIn(defaultIn);
        }

        private string str;

        public ConsoleMock(string str)
        {
            this.str = str;
        }

        public override string ReadLine()
        {
            return str;
        }
    }

    public class GameTest : Game
    {
        public Dictionary<String, Dictionary<String, Int32>> stats = new Dictionary<String, Dictionary<String, Int32>>();

        public override bool Add(Player player)
        {
            stats.Add(player.Name, new Dictionary<String, Int32>());
            return base.Add(player);
        }

        public override void StartGame()
        {
            RockOrTechno = Categories.Techno;
            ScoreToWin = 6;
            AddQuestions();
        }

        protected override void AnswerQuestion()
        {
            WasCorrectlyAnswered();
        }

        protected override Categories CurrentCategory()
        {
            int lastStat = 0;

            if (NextCategory is null)
            {
                switch (Places[CurrentPlayer])
                {
                    case 0:
                    case 4:
                    case 8:
                        lastStat = 0;
                        if (stats[Players[CurrentPlayer].Name].ContainsKey(Categories.Pop.ToString()))
                            lastStat = stats[Players[CurrentPlayer].Name][Categories.Pop.ToString()];

                        stats[Players[CurrentPlayer].Name][Categories.Pop.ToString()] = lastStat + 1;
                        return Categories.Pop;
                    case 1:
                    case 5:
                    case 9:
                        lastStat = 0;
                        if (stats[Players[CurrentPlayer].Name].ContainsKey(Categories.Science.ToString()))
                            lastStat = stats[Players[CurrentPlayer].Name][Categories.Science.ToString()];
                        stats[Players[CurrentPlayer].Name][Categories.Science.ToString()] = lastStat + 1;
                        return Categories.Science;
                    case 2:
                    case 6:
                    case 10:
                        lastStat = 0;
                        if (stats[Players[CurrentPlayer].Name].ContainsKey(Categories.Sports.ToString()))
                            lastStat = stats[Players[CurrentPlayer].Name][Categories.Sports.ToString()];
                        stats[Players[CurrentPlayer].Name][Categories.Sports.ToString()] = lastStat + 1;
                        return Categories.Sports;
                    default:
                        lastStat = 0;
                        if (stats[Players[CurrentPlayer].Name].ContainsKey(RockOrTechno.ToString()))
                            lastStat = stats[Players[CurrentPlayer].Name][RockOrTechno.ToString()];
                        stats[Players[CurrentPlayer].Name][RockOrTechno.ToString()] = lastStat + 1;
                        return RockOrTechno;
                }
            }

            return (Categories)NextCategory;
        }
    }


}