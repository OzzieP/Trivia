﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private Categories _rockOrTechno;
        private readonly List<Player> _players = new List<Player>();

        private readonly List<int> _places = new List<int>();
        private readonly List<int> _purses = new List<int>();

        private readonly List<bool> _inPenaltyBox = new List<bool>();

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _technoQuestions = new LinkedList<string>();

        private static bool _isAWinner;
        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game() { }

        public void StartGame()
        {
            int categorieChoice = Utils.Ask("Do you want to play with: ", new[] { "Techno questions ?", "Rock questions ?" });
            _rockOrTechno = categorieChoice == 0 ? Categories.Techno : Categories.Rock;

            for (var i = 0; i < 50; i++)
            {
                _popQuestions.AddLast($"Pop Question {i}");
                _scienceQuestions.AddLast($"Science Question {i}");
                _sportsQuestions.AddLast($"Sports Question {i}");

                switch (_rockOrTechno)
                {
                    case Categories.Rock:
                        _rockQuestions.AddLast($"Rock Question {i}");
                        break;
                    case Categories.Techno:
                        _technoQuestions.AddLast($"Techno Question {i}");
                        break;
                }
            }
        }

        public bool IsPlayable()
        {
            return HowManyPlayers() >= 2 && HowManyPlayers() <= 6;
        }

        public bool Add(Player player)
        {
            _players.Add(player);
            _places.Add(0);
            _purses.Add(0);
            _inPenaltyBox.Add(false);

            Console.WriteLine($"{player.Name} was added");
            Console.WriteLine($"They are player number {_players.Count}");
            return true;
        }

        public void Remove(Player player)
        {
            int indexPlayer = _players.IndexOf(player);

            _places.RemoveAt(indexPlayer);
            _purses.RemoveAt(indexPlayer);
            _players.RemoveAt(indexPlayer);

            Console.WriteLine($"{player.Name} has left the game.");
        }

        public void NextPlayer()
        {
            _currentPlayer++;

            if (_currentPlayer >= _players.Count)
                _currentPlayer = 0;

            Console.WriteLine("===================================================");
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll()
        {
            Console.WriteLine($"{_players[_currentPlayer].Name} is the current player");
            int quitChoice = Utils.Ask("Do you want to:", new[] { "Play !", "Quit the game." });
            if (quitChoice == 1)
            {
                Remove(_players[_currentPlayer]);
                NextPlayer();
                return;
            }

            int roll = _players[_currentPlayer].Roll();
            Console.WriteLine($"They have rolled a {roll}");

            if (_players[_currentPlayer].IsInPenaltyBox)
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;
                    _players[_currentPlayer].IsInPenaltyBox = false;

                    Console.WriteLine($"{_players[_currentPlayer].Name} is getting out of the penalty box");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;

                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                    Console.WriteLine($"{_players[_currentPlayer].Name}'s new location is {_places[_currentPlayer]}");
                    Console.WriteLine($"The category is {CurrentCategory()}");
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine($"{_players[_currentPlayer].Name} is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                Console.WriteLine($"{_players[_currentPlayer].Name}'s new location is {_places[_currentPlayer]}");
                Console.WriteLine($"The category is {CurrentCategory()}");

                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            switch (CurrentCategory())
            {
                case Categories.Pop:
                    Console.WriteLine(_popQuestions.First());
                    _popQuestions.RemoveFirst();
                    break;
                case Categories.Science:
                    Console.WriteLine(_scienceQuestions.First());
                    _scienceQuestions.RemoveFirst();
                    break;
                case Categories.Sports:
                    Console.WriteLine(_sportsQuestions.First());
                    _sportsQuestions.RemoveFirst();
                    break;
                case Categories.Rock:
                    Console.WriteLine(_rockQuestions.First());
                    _rockQuestions.RemoveFirst();
                    break;
                case Categories.Techno:
                    Console.WriteLine(_technoQuestions.First());
                    _technoQuestions.RemoveFirst();
                    break;
            }

            if (_players[_currentPlayer].JokerIsAvailable)
            {
                int choice = Utils.Ask("What do you want to do ?", new[] { "Answer the question", "Use the Joker" });

                if (choice == 1)
                {
                    _players[_currentPlayer].UseJoker();

                    NextPlayer();

                    return;
                }
            }

            AnswerQuestion();
        }

        private void AnswerQuestion()
        {
            // Test
            //if (_players[_currentPlayer].AnswerQuestion() > 2)
            if (_players[_currentPlayer].AnswerQuestion() == 7)
                WrongAnswer();
            else
                WasCorrectlyAnswered();
        }

        public bool IsAWinner()
        {
            return _isAWinner;
        }

        private Categories CurrentCategory()
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

        public void WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct !!!!");
                    _purses[_currentPlayer]++;
                    Console.WriteLine($"{_players[_currentPlayer].Name} now has {_purses[_currentPlayer]} Gold Coins.");

                    var winner = DidPlayerWin();
                    NextPlayer();

                    _isAWinner = winner;
                }
                else
                {
                    NextPlayer();
                    _isAWinner = true;
                }
            }
            else
            {
                Console.WriteLine("Answer was correct !!!!");
                _purses[_currentPlayer]++;
                Console.WriteLine($"{_players[_currentPlayer].Name} now has {_purses[_currentPlayer]} Gold Coins.");

                var winner = DidPlayerWin();
                NextPlayer();

                _isAWinner = winner;
            }
        }

        public void WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine($"{_players[_currentPlayer].Name} was sent to the penalty box");
            _players[_currentPlayer].IsInPenaltyBox = true;

            NextPlayer();
        }


        private bool DidPlayerWin()
        {
            return _purses[_currentPlayer] == 6;
        }
    }

}
