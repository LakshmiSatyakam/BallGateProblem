using System.Globalization;

namespace BallGameApp
{
    public static class InputValidator
    {
        #region Public static methods

        /// <summary>
        /// Validates the input provided for DEPTH of the model
        /// </summary>
        /// <param name="inputValue">input value for depth</param>
        /// <param name="depth">depth -- an out argument</param>
        /// <returns>Error message if any; else string.empty</returns>
        public static string DepthValidator(string inputValue, out int depth)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                depth = 0;
                return "Invalid input for Depth!";
            }

            if (int.TryParse(inputValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out depth))
            {
                if (depth < 2)
                {
                    return "Depth should always be more than 1!";
                }
            }
            else
            {
                return "Invalid input for Depth!";
            }

            return string.Empty;
        }

        /// <summary>
        /// Validates whether input for Guess empty container is valid or not
        /// </summary>
        /// <param name="guessValue">guess empty container value</param>
        /// <param name="totalContainers">total containers basedon depth</param>
        /// <param name="guessEmptyContainer">guess empty container value if input is valid</param>
        /// <returns>Error message if any; else string.empty</returns>
        public static string GuessEmptyContainerValidator(string guessValue, long totalContainers, out long guessEmptyContainer)
        {
            if (long.TryParse(guessValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out guessEmptyContainer))
            {
                if (guessEmptyContainer >= 1 && guessEmptyContainer <= totalContainers)
                {
                    return string.Empty;
                }

                return string.Format("\n Guess should be in the range [1, {0}]", totalContainers);
            }

            return "Invalid guess!";
        }

        #endregion
    }
}
