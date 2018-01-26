using System;

namespace BallGameApp
{
    static class Program
    {
        #region Main method

        /// <summary>
        /// Main method of the program
        /// </summary>
        /// <param name="args">No arguments passed in Ball Gate problem</param>
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO BALL GATE APP");

            ConsoleKeyInfo option;
            do
            {
                Console.WriteLine("\n\nEnter Depth of the Model: ");
                string inputValue = Console.ReadLine();

                string depthErrorMessage = InputValidator.DepthValidator(inputValue, out int depth);
                if (string.IsNullOrEmpty(depthErrorMessage))
                {
                    long totalContainers = (long)Math.Pow(2, depth);
                    Console.WriteLine("\n Guess the Empty container, enter a number between 1 to {0} : ", totalContainers);
                    string guessValue = Console.ReadLine();

                    string guessErrorMessage = InputValidator.GuessEmptyContainerValidator(guessValue, totalContainers, out long guessEmptyContainer);
                    if (string.IsNullOrEmpty(guessErrorMessage))
                    {
                        BallGateModelRunner.RunBallGateModel(depth, totalContainers, guessEmptyContainer);
                    }
                    else
                    {
                        Console.WriteLine(guessErrorMessage);
                    }
                }
                else
                {
                    Console.WriteLine(depthErrorMessage);
                }

                Console.WriteLine("\n\nDo you want to guess one more time (y/n)? ");
                option = Console.ReadKey();
            } while (option.Key == ConsoleKey.Y);
        }

        #endregion
    }
}
