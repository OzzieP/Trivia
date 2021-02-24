using System;
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
            
            Assert.False(aGame.Add(new Player("Joueur7")));
        }

        [Fact]
        public void TestQuestionEquitable()
        {
            ConsoleMock.MockConsole();
            var aGame = new GameTest();
            Player p1 = new Player("Joueur 1");
            Player p2 = new Player("Joueur 2");
            aGame.Add(p1);
            aGame.Add(p2);

            for  (int i  =  0; i  >  aGame.HowManyPlayers(); i++)
            {

            }
            ConsoleMock.UnMockConsole();
        }
    }
}