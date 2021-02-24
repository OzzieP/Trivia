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
            var addPlayer = aGame.AddPlayer(new Player("Chet"));
            Assert.True(addPlayer);
        }

        [Fact]
        public void TestPasAssezDeJoueur()
        {
            var aGame = new Game();
            aGame.AddPlayer(new Player("Joueur1"));
            var estJouable = aGame.IsPlayable();
            Assert.False(estJouable);
        }

        [Fact]
        public void TestAssezDeJoueur()
        {
            var aGame = new Game();
            aGame.AddPlayer(new Player("Joueur1"));
            aGame.AddPlayer(new Player("Joueur2"));
            var estJouable = aGame.IsPlayable();
            Assert.True(estJouable);
        }

        [Fact]
        public void TestTropDeJoueur()
        {
            var aGame = new Game();
            aGame.AddPlayer(new Player("Joueur1"));
            aGame.AddPlayer(new Player("Joueur2"));
            aGame.AddPlayer(new Player("Joueur3"));
            aGame.AddPlayer(new Player("Joueur4"));
            aGame.AddPlayer(new Player("Joueur5"));
            aGame.AddPlayer(new Player("Joueur6"));
            aGame.AddPlayer(new Player("Joueur7"));
            
            Assert.False(aGame.IsPlayable());
        }

        [Fact]
        public void TestQuestionEquitable()
        {
            var aGame = new GameTest();

            aGame.AddPlayer(new Player("Joueur 1"));
            aGame.AddPlayer(new Player("Joueur 2"));

            ConsoleMock.MockConsole("0");
            aGame.StartGame();

            for(int i = 0; i < 20000; i++)
            {
                aGame.ResetPlayerScore();
                aGame.Roll();
            }
                

            ConsoleMock.UnMockConsole();

            foreach (KeyValuePair<string, Dictionary<string, int>> entry in aGame.stats)
            {
                Debug.WriteLine($"Player: {entry.Key}");
                Debug.WriteLine("---------------------------");
                foreach (KeyValuePair<string, int> entryValue in entry.Value)
                {
                    Debug.WriteLine($"{entryValue.Key}: {entryValue.Value / (double) 20000 * 100}%");
                }
                Debug.WriteLine("===========================");
            }
        }
    }
}