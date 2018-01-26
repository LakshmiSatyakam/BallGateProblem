using BallGameApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace BallGateApp.UnitTest
{
    [TestClass]
    public class BallGateModelRunnerUnitTest
    {
        [TestMethod]
        public void BallGateModelRunnerUnitTest_Create_Model_Test()
        {
            NodeInfo node = new NodeInfo(GateState.Left);
            Assert.IsTrue(node.RightChild == null);
            Assert.IsTrue(node.LeftChild == null);

            var tokenSource = new CancellationTokenSource();
            BallGateModelRunner.CreateBallGateTreeRecursive(0, 2, node, tokenSource, tokenSource.Token);

            Assert.IsTrue(node.RightChild != null);
            Assert.IsTrue(node.RightChild.RightChild != null);
            Assert.IsTrue(node.RightChild.RightChild.NodeId != 0);
            Assert.IsTrue(node.RightChild.RightChild.RightChild == null);
        }

        [TestMethod]
        public void BallGateModelRunnerUnitTest_PassBalls_Test()
        {
            NodeInfo node = new NodeInfo(GateState.Left);
            Assert.IsTrue(node.RightChild == null);
            Assert.IsTrue(node.LeftChild == null);

            var tokenSource = new CancellationTokenSource();
            BallGateModelRunner.CreateBallGateTreeRecursive(0, 2, node, tokenSource, tokenSource.Token);

            Assert.IsTrue(node.RightChild != null);
            Assert.IsTrue(node.RightChild.RightChild != null);
            Assert.IsTrue(node.RightChild.RightChild.RightChild == null);

            BallGateModelRunner.PassBalls(3, node);

            Assert.IsTrue(node.RightChild.BallCount == 0);
            Assert.IsTrue(node.LeftChild.BallCount == 0);
        }

        [TestMethod]
        public void BallGateModelRunnerUnitTest_EmptyContainer_Test()
        {
            NodeInfo node = new NodeInfo(GateState.Left);
            Assert.IsTrue(node.RightChild == null);
            Assert.IsTrue(node.LeftChild == null);

            var tokenSource = new CancellationTokenSource();
            BallGateModelRunner.CreateBallGateTreeRecursive(0, 2, node, tokenSource, tokenSource.Token);

            Assert.IsTrue(node.RightChild != null);
            Assert.IsTrue(node.RightChild.RightChild != null);
            Assert.IsTrue(node.RightChild.RightChild.RightChild == null);

            BallGateModelRunner.PassBalls(3, node);

            Assert.IsTrue(node.RightChild.BallCount == 0);
            Assert.IsTrue(node.LeftChild.BallCount == 0);

            BallGateModelRunner.CheckEmptyContainer(node, 1, tokenSource, tokenSource.Token);
        }
    }
}
