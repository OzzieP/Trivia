using System;

namespace Trivia
{
    public class Player
    {
        public string Name { get; set; }

        public bool JokerIsAvailable { get; set; } = true;

        public bool IsInPenaltyBox { get; set; }

        public bool IsWinner { get; set; } = false;

        public int QuestionsAnsweredInARow { get; set; }

        public int TimeInPenaltyBox { get; set; }

        public int PercentBonus { get; set; }

        public Player(string name)
        {
            Name = name;
        }


        public void UseJoker()
        {
            if (JokerIsAvailable)
                JokerIsAvailable = !JokerIsAvailable;

            Console.WriteLine($"The player {Name} uses his Joker and doesn't win any Gold Coins.");
        }

        public int Roll()
        {
            Random random = new Random();
            return random.Next(5) + 1;
        }

        public int AnswerQuestion()
        {
            Random random = new Random();
            return random.Next(9);
        }

        public bool IsOutOfPenaltyBox()
        {
            Random random = new Random();
            return random.Next(100) <= (100  /  TimeInPenaltyBox + PercentBonus  /  100);
        }

        public void Reset()
        {
            QuestionsAnsweredInARow = 0;
            TimeInPenaltyBox = 0;
            JokerIsAvailable = true;
            IsInPenaltyBox = false;
            IsWinner = false;
        }
    }
}