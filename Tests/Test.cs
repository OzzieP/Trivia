using System;
using System.Collections.Generic;
using System.Diagnostics;
using Trivia;
using Xunit;

namespace Tests
{
    public class Test
    {
        [Fact]
        public void TestNombreDeJoueurEstZero()
        {
            var aGame = new Game();
            var nombrePlayer = aGame.HowManyPlayers();
            Assert.Equal(0, nombrePlayer);
        }

        [Fact]
        public void TestAdd()
        {
            var aGame = new Game();
            var addPlayer = aGame.Add(new Player("Chet"));
            Assert.True(addPlayer);
        }

        [Fact]
        public void TestPasAssezDeJoueur()
        {
            var aGame = new Game();
            aGame.Add(new Player("Joueur1"));
            var estJouable = aGame.IsPlayable();
            Assert.False(estJouable);
        }

        [Fact]
        public void TestAssezDeJoueur()
        {
            var aGame = new Game();
            aGame.Add(new Player("Joueur1"));
            aGame.Add(new Player("Joueur2"));
            var estJouable = aGame.IsPlayable();
            Assert.True(estJouable);
        }

        [Fact]
        public void TestTropDeJoueur()
        {
            var aGame = new Game();
            aGame.Add(new Player("Joueur1"));
            aGame.Add(new Player("Joueur2"));
            aGame.Add(new Player("Joueur3"));
            aGame.Add(new Player("Joueur4"));
            aGame.Add(new Player("Joueur5"));
            aGame.Add(new Player("Joueur6"));
            aGame.Add(new Player("Joueur7"));
            
            Assert.False(aGame.IsPlayable());
        }

        [Fact]
        public void TestQuestionEquitable()
        {
            ConsoleMock.MockConsole("6");
            var aGame = new GameTest();

            aGame.Add(new Player("Joueur 1"));
            aGame.Add(new Player("Joueur 2"));

            ConsoleMock.MockConsole("0");
            aGame.StartGame();

            while (aGame.IsPlayable())
                aGame.Roll();

            ConsoleMock.UnMockConsole();

            foreach (KeyValuePair<string, Dictionary<string, int>> entry in aGame.stats)
            {
                Debug.WriteLine($"Player: {entry.Key}");
                Debug.WriteLine("---------------------------");
                foreach (KeyValuePair<string, int> entryValue in entry.Value)
                {
                    Debug.WriteLine($"{entryValue.Key}: {entryValue.Value}");
                }
                Debug.WriteLine("===========================");
            }
        }
    }
}