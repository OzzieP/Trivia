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
            var addPlayer = aGame.Add("Chet");
            Assert.True(addPlayer);
        }

        [Fact]
        public void TestPasAssezDeJoueur()
        {
            var aGame = new Game();
            aGame.Add("Joueur1");
            var estJouable = aGame.IsPlayable();
            Assert.False(estJouable);
        }

        [Fact]
        public void TestAssezDeJoueur()
        {
            var aGame = new Game();
            aGame.Add("Joueur1");
            aGame.Add("Joueur2");
            var estJouable = aGame.IsPlayable();
            Assert.True(estJouable);
        }

        [Fact]
        public void TestTropDeJoueur()
        {
            var aGame = new Game();
            aGame.Add("Joueur1");
            aGame.Add("Joueur2");
            aGame.Add("Joueur3");
            aGame.Add("Joueur4");
            aGame.Add("Joueur5");
            aGame.Add("Joueur6");
            
            Assert.False(aGame.Add("Joueur7"));
        }
    }
}