using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        protected Categories? NextCategory = null;
        protected Categories RockOrTechno;
        protected int ScoreToWin;
        
        protected readonly List<Player> Players = new List<Player>();
        protected readonly List<int> Places = new List<int>();
        protected readonly List<int> Purses = new List<int>();
        protected readonly List<bool> InPenaltyBox = new List<bool>();

        protected readonly LinkedList<string> PopQuestions = new LinkedList<string>();
        protected readonly LinkedList<string> ScienceQuestions = new LinkedList<string>();
        protected readonly LinkedList<string> SportsQuestions = new LinkedList<string>();
        protected readonly LinkedList<string> RockQuestions = new LinkedList<string>();
        protected readonly LinkedList<string> TechnoQuestions = new LinkedList<string>();

        protected readonly List<LinkedList<string>> QuestionsList;
        protected readonly List<Player> PlayersWin = new List<Player>();

        protected int CurrentPlayer;
        protected bool IsGettingOutOfPenaltyBox;

        public Game()
        {
            QuestionsList = new List<LinkedList<string>>
            {
                PopQuestions,
                ScienceQuestions,
                SportsQuestions,
                RockQuestions,
                TechnoQuestions
            };
        }

        public virtual void StartGame()
        {
            int categorieChoice = Utils.Ask("Do you want to play with: ", new[] { "Techno questions ?", "Rock questions ?" });
            RockOrTechno = categorieChoice == 0 ? Categories.Techno : Categories.Rock;

            ScoreToWin = Utils.AskANumber("How many gold to win ?", 6);
            AddQuestions();
        }

        protected void AddQuestions()
        {
            for (var i = 0; i < 5; i++)
            {
                PopQuestions.AddLast($"Pop Question {i}");
                ScienceQuestions.AddLast($"Science Question {i}");
                SportsQuestions.AddLast($"Sports Question {i}");

                switch (RockOrTechno)
                {
                    case Categories.Rock:
                        RockQuestions.AddLast($"Rock Question {i}");
                        break;
                    case Categories.Techno:
                        TechnoQuestions.AddLast($"Techno Question {i}");
                        break;
                }
            }
        }

        public bool IsPlayable()
        {
            return HowManyPlayers() >= 2 && HowManyPlayers() <= 6;
        }

        public virtual bool Add(Player player)
        {
            Players.Add(player);
            Places.Add(0);
            Purses.Add(0);
            InPenaltyBox.Add(false);

            Console.WriteLine($"{player.Name} was added");
            Console.WriteLine($"They are player number {Players.Count}");
            return true;
        }

        public void Remove(Player player)
        {
            int indexPlayer = Players.IndexOf(player);

            Places.RemoveAt(indexPlayer);
            Purses.RemoveAt(indexPlayer);
            Players.RemoveAt(indexPlayer);

            Console.WriteLine($"{player.Name} has left the game.");
        }

        public void NextPlayer()
        {
            CurrentPlayer++;

            if (CurrentPlayer >= Players.Count)
                CurrentPlayer = 0;

            Console.WriteLine("===================================================");
        }

        public int HowManyPlayers()
        {
            return Players.Count;
        }

        public void Roll()
        {
            Console.WriteLine($"{Players[CurrentPlayer].Name} is the current player");
            int quitChoice = Utils.Ask("Do you want to:", new[] { "Play !", "Quit the game." });
            if (quitChoice == 1)
            {
                Remove(Players[CurrentPlayer]);
                NextPlayer();
                return;
            }

            int roll = Players[CurrentPlayer].Roll();
            Console.WriteLine($"They have rolled a {roll}");

            if (Players[CurrentPlayer].IsInPenaltyBox)
            {
                Console.WriteLine($"{Players[CurrentPlayer].Name} is in penalty box. The chance to get out is 1/{Players[CurrentPlayer].TimeInPenaltyBox}");
                if (Players[CurrentPlayer].IsOutOfPenaltyBox())
                {
                    IsGettingOutOfPenaltyBox = true;
                    Players[CurrentPlayer].IsInPenaltyBox = false;

                    Console.WriteLine($"{Players[CurrentPlayer].Name} is getting out of the penalty box");
                    Places[CurrentPlayer] = Places[CurrentPlayer] + roll;

                    if (Places[CurrentPlayer] > 11) Places[CurrentPlayer] = Places[CurrentPlayer] - 12;

                    Console.WriteLine($"{Players[CurrentPlayer].Name}'s new location is {Places[CurrentPlayer]}");
                    Console.WriteLine($"The category is {CurrentCategory()}");
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine($"{Players[CurrentPlayer].Name} is not getting out of the penalty box");
                    IsGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                Places[CurrentPlayer] = Places[CurrentPlayer] + roll;
                if (Places[CurrentPlayer] > 11) Places[CurrentPlayer] = Places[CurrentPlayer] - 12;

                Console.WriteLine($"{Players[CurrentPlayer].Name}'s new location is {Places[CurrentPlayer]}");
                Console.WriteLine($"The category is {CurrentCategory()}");

                AskQuestion();
            }
        }

        protected void CheckQuestionsCount()
        {
            if (QuestionsList.Any(l => l.Count == 0))
                AddQuestions();
        }

        protected void AskQuestion()
        {
            switch (CurrentCategory())
            {
                case Categories.Pop:
                    Console.WriteLine(PopQuestions.First());
                    PopQuestions.RemoveFirst();
                    break;
                case Categories.Science:
                    Console.WriteLine(ScienceQuestions.First());
                    ScienceQuestions.RemoveFirst();
                    break;
                case Categories.Sports:
                    Console.WriteLine(SportsQuestions.First());
                    SportsQuestions.RemoveFirst();
                    break;
                case Categories.Rock:
                    Console.WriteLine(RockQuestions.First());
                    RockQuestions.RemoveFirst();
                    break;
                case Categories.Techno:
                    Console.WriteLine(TechnoQuestions.First());
                    TechnoQuestions.RemoveFirst();
                    break;
            }

            CheckQuestionsCount();

            if (Players[CurrentPlayer].JokerIsAvailable)
            {
                int choice = Utils.Ask("What do you want to do ?", new[] { "Answer the question", "Use the Joker" });

                if (choice == 1)
                {
                    Players[CurrentPlayer].UseJoker();
                    NextPlayer();
                    return;
                }
            }

            AnswerQuestion();
        }

        protected virtual void AnswerQuestion()
        {
            // Test
                //if (_players[_currentPlayer].AnswerQuestion() > 2)
            if (Players[CurrentPlayer].AnswerQuestion() == 7)
                WrongAnswer();
            else
                WasCorrectlyAnswered();
        }

        public bool IsAWinner()
        {
            if (PlayersWin.Count >= 3)
                return true;

            return false;
        }

        public void DisplayLeaderBoard()
        {
            int rank = 1;
            Console.WriteLine("===================================================");
            Console.WriteLine("THE LEADERBOARD :");

            foreach (Player player in PlayersWin)
            {
                Console.WriteLine($" Rank {rank} : {player.Name}");
                rank++;
            }
           
            foreach(Player player in Players)
                Console.WriteLine($" Unranked : {player.Name}");
        }

        protected virtual Categories CurrentCategory()
        {
            if (NextCategory is null)
            {
                switch (Places[CurrentPlayer])
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
                        return RockOrTechno;
                }
            }

            return (Categories) NextCategory;
        }

        public void WasCorrectlyAnswered()
        {
            NextCategory = null;

            if (Players[CurrentPlayer].IsInPenaltyBox)
            {
                if (IsGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct !!!!");
                    Players[CurrentPlayer].QuestionsAnsweredInARow++;
                    Purses[CurrentPlayer] += Players[CurrentPlayer].QuestionsAnsweredInARow;
                    Console.WriteLine($"{Players[CurrentPlayer].Name} now has {Purses[CurrentPlayer]} Gold Coins.");

                    var winner = DidPlayerWin();
                    NextPlayer();
                    Players[CurrentPlayer].isWinner = winner;
                }
                else
                {
                    NextPlayer();
                    Players[CurrentPlayer].isWinner = true;
                }
            }
            else
            {
                Console.WriteLine("Answer was correct !!!!");
                Players[CurrentPlayer].QuestionsAnsweredInARow++;
                Purses[CurrentPlayer] += Players[CurrentPlayer].QuestionsAnsweredInARow;
                Console.WriteLine($"{Players[CurrentPlayer].Name} now has {Purses[CurrentPlayer]} Gold Coins.");

                var winner = DidPlayerWin();
                NextPlayer();
                Players[CurrentPlayer].isWinner = winner;
            }
        }

        public void WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine($"{Players[CurrentPlayer].Name} was sent to the penalty box");
            
            Players[CurrentPlayer].IsInPenaltyBox = true;
            Players[CurrentPlayer].QuestionsAnsweredInARow = 0;
            Players[CurrentPlayer].TimeInPenaltyBox++;
            
            PlayerChooseNextQuestionCategory();
            NextPlayer();
        }


        protected bool DidPlayerWin()
        {
            if (Purses[CurrentPlayer] >= ScoreToWin)
            {
                PlayersWin.Add(Players[CurrentPlayer]);
                Remove(Players[CurrentPlayer]);
                return true;
            }
            
            return false;
        }


        protected void PlayerChooseNextQuestionCategory()
        {
            List<Categories> categories = Enum.GetValues(typeof(Categories)).Cast<Categories>().Where(c =>
            {
                if (c == Categories.Rock && RockOrTechno == Categories.Techno)
                    return false;

                if (c == Categories.Techno && RockOrTechno == Categories.Rock)
                    return false;

                return true;
            }).ToList();
            
            int indexOfCategories = Utils.Ask("Choose categorie for the next player.", categories.Select((c) => c.ToString()));
            NextCategory = categories[indexOfCategories];
        }
    }
}
