using System;
using Trivia;
using System.IO;
using Xunit;

namespace Tests
{
    public class ConsoleMock : TextReader
    {
        private static readonly TextReader defaultIn = Console.In;

        public static void MockConsole()
        {
            Console.SetIn(new ConsoleMock());
        }

        public static void UnMockConsole()
        {
            Console.SetIn(defaultIn);
        }

        public override string ReadLine()
        {
            return "0";
        }
    }

    public class GameTest : Game
    {
        protected override void AnswerQuestion()
        {
            WasCorrectlyAnswered();
        }

        protected override Categories CurrentCategory()
        {

            if (_nextCategory is null)
            {

                switch (_places[_currentPlayer])
                {
                    case 0:
                    case 4:
                    case 8:
                        return Categories.Pop;
                    case 1:
                    case 5:
                    case 9:
                        return Categories.Science;
                    case 2:
                    case 6:
                    case 10:
                        return Categories.Sports;
                    default:
                        return _rockOrTechno;
                }
            }

            return (Categories)_nextCategory;
        }
    }


}