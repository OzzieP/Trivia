using System;

namespace Trivia
{
    public class GameRunner
    {
        public static void Main(string[] args)
        {
            var aGame = new Game();

            aGame.Add(new Player("Chet"));
            aGame.Add(new Player("Pat"));
            aGame.Add(new Player("Sue"));

            if (!aGame.IsPlayable())
                Console.WriteLine("Il n'y a pas assez ou trop de joueurs !");
            else
            {
                aGame.StartGame();

                while (!aGame.IsAWinner() && aGame.IsPlayable())
                    aGame.Roll();

                if (!aGame.IsAWinner() && !aGame.IsPlayable())
                    Console.WriteLine("Il n'y a plus assez de joueurs !");
            }
        }
    }
}