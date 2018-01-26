using BallGameApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BallGate
{
    [TestClass]
    public class InputValidatorUnitTest
    {
        [TestMethod]
        public void DepthValidator_NULL_Input_Test()
        {
            string error = InputValidator.DepthValidator("", out int depth);

            Assert.AreEqual("Invalid input for Depth!", error);
            Assert.AreEqual(0, depth);
        }

        [TestMethod]
        public void DepthValidator_Blank_Input_Test()
        {
            string error = InputValidator.DepthValidator(" ", out int depth);

            Assert.AreEqual("Invalid input for Depth!", error);
            Assert.AreEqual(0, depth);
        }

        [TestMethod]
        public void DepthValidator_String_Input_Test()
        {
            string error = InputValidator.DepthValidator("aaa", out int depth);

            Assert.AreEqual("Invalid input for Depth!", error);
            Assert.AreEqual(0, depth);
        }

        [TestMethod]
        public void DepthValidator_Negative_Input_Test()
        {
            string error = InputValidator.DepthValidator("-10", out int depth);

            Assert.AreEqual("Depth should always be more than 1!", error);
            Assert.AreEqual(-10, depth);
        }

        [TestMethod]
        public void DepthValidator_One_Input_Test()
        {
            string error = InputValidator.DepthValidator("1", out int depth);

            Assert.AreEqual("Depth should always be more than 1!", error);
            Assert.AreEqual(1, depth);
        }

        [TestMethod]
        public void DepthValidator_Valid_Input_Test()
        {
            string error = InputValidator.DepthValidator("10", out int depth);

            Assert.AreEqual("", error);
            Assert.AreEqual(10, depth);
        }

        [TestMethod]
        public void GuessValidator_NULL_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator("", 10, out int emptyContainerGuess);

            Assert.AreEqual("Invalid guess!", error);
            Assert.AreEqual(0, emptyContainerGuess);
        }

        [TestMethod]
        public void GuessValidator_Blank_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator(" ", 10, out int emptyContainerGuess);

            Assert.AreEqual("Invalid guess!", error);
            Assert.AreEqual(0, emptyContainerGuess);
        }

        [TestMethod]
        public void GuessValidator_String_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator("aaa", 10, out int emptyContainerGuess);

            Assert.AreEqual("Invalid guess!", error);
            Assert.AreEqual(0, emptyContainerGuess);
        }

        [TestMethod]
        public void GuessValidator_Negative_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator("-100", 10, out int emptyContainerGuess);

            Assert.AreEqual("\n Guess should be in the range [1, 10]", error);
            Assert.AreEqual(-100, emptyContainerGuess);
        }

        [TestMethod]
        public void GuessValidator_LessThan_Lower_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator("-1", 10, out int emptyContainerGuess);

            Assert.AreEqual("\n Guess should be in the range [1, 10]", error);
            Assert.AreEqual(-1, emptyContainerGuess);
        }

        [TestMethod]
        public void GuessValidator_GreaterThan_Upper_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator("12", 10, out int emptyContainerGuess);

            Assert.AreEqual("\n Guess should be in the range [1, 10]", error);
            Assert.AreEqual(12, emptyContainerGuess);
        }

        [TestMethod]
        public void GuessValidator_Valid_Input_Test()
        {
            string error = InputValidator.GuessEmptyContainerValidator("2", 10, out int emptyContainerGuess);

            Assert.AreEqual(string.Empty, error);
            Assert.AreEqual(2, emptyContainerGuess);
        }
    }
}
