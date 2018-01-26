using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BallGameApp
{
    public static class BallGateModelRunner
    {
        #region Private fiedls

        /// <summary>
        /// Int maintained to provide the HeadId for the last level children nodes
        /// </summary>
        static long _globalHeadId;

        #endregion

        #region Public static methods

        /// <summary>
        /// Runs the complete ball gate model
        /// 1. Creates the ball gate model initially -- handles OutOfMemory Exception
        /// 2. Passes the balls throught the model
        /// 3. Checks for the empty container 
        /// </summary>
        /// <param name="depth">depth of the ball gate model</param>
        /// <param name="totalContainers">total number of nodes in the ball gate model</param>
        /// <param name="guessEmptyContainer">empty container id guessed by the user</param>
        public static void RunBallGateModel(int depth, long totalContainers, long guessEmptyContainer)
        {
            _globalHeadId = 0;

            var tokenSource = new CancellationTokenSource();

            try
            {
                Console.WriteLine("\nCreating the Ball Gate model ... ");
                using (NodeInfo rootNode = new NodeInfo(NodeInfo.GetGateState()))
                {
                    CreateBallGateTreeRecursive(0, depth, rootNode, tokenSource, tokenSource.Token);
                    Console.WriteLine("Ball Gate model created. ");

                    Console.WriteLine("\nPassing the balls, {0} through the model... ", totalContainers - 1);
                    PassBalls(totalContainers - 1, rootNode);
                    Console.WriteLine("Passed all the balls through the model.");

                    Console.WriteLine("\nEvaluating Empty container started...");
                    CheckEmptyContainer(rootNode, guessEmptyContainer, tokenSource, tokenSource.Token);
                    Console.WriteLine("Evaluating Empty container completed.");
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("System cannot handle depth of {0}; please try with lesser depth. \n Error: {1}", depth, ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("System cannot handle depth of {0}; please try with lesser depth. \n Error: {1}", depth, e.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Checking for the empty container after all the balls are passed
        /// </summary>
        /// <param name="rootNode">Root node of the ball gate problem</param>
        /// <param name="guessEmptyContainer">Head Id guessed by the user</param>
        /// <param name="tokenSource">TokenSource object for creating tasks</param>
        /// <param name="token">Token of the cancellation token source</param>
        public static void CheckEmptyContainer(NodeInfo rootNode, long guessEmptyContainer, CancellationTokenSource tokenSource, CancellationToken token)
        {
            if (rootNode == null)
            {
                return;
            }

            if (rootNode.BallCount == 0 && rootNode.RightChild == null && rootNode.LeftChild == null)
            {
                Console.WriteLine("\n Empty container: {0}", rootNode.NodeId);
                if (guessEmptyContainer == rootNode.NodeId)
                {
                    Console.WriteLine("\n Your guess is correct.");
                }
                tokenSource.Cancel();
                return;
            }

            try
            {
                if (!token.IsCancellationRequested)
                {
                    var t1 = Task.Factory.StartNew(() => CheckEmptyContainer(rootNode.LeftChild, guessEmptyContainer, tokenSource, token), token);
                    var t2 = Task.Factory.StartNew(() => CheckEmptyContainer(rootNode.RightChild, guessEmptyContainer, tokenSource, token), token);
                    if (!t1.IsCanceled && !t2.IsCanceled)
                    {
                        Task.WaitAll(t1, t2);
                    }
                }
            }
            catch (Exception)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
            finally
            {
                tokenSource.Dispose();
            }
        }

        /// <summary>
        /// Passing the balls
        /// Running sequentially as the tree state should be maintained 
        /// </summary>
        /// <param name="noOfBalls">Number of balls</param>
        /// <param name="startPoint">Start point</param>
        public static void PassBalls(long noOfBalls, NodeInfo startPoint)
        {
            for (long ithBall = 1; ithBall <= noOfBalls; ithBall++)
            {
                NodeInfo ballPosition = startPoint;

                do
                {
                    if (ballPosition.GateState == GateState.Left)
                    {
                        ballPosition.GateState = GateState.Right;
                        ballPosition = ballPosition.LeftChild;
                    }
                    else
                    {
                        ballPosition.GateState = GateState.Left;
                        ballPosition = ballPosition.RightChild;
                    }
                } while (ballPosition.LeftChild != null && ballPosition.RightChild != null);

                ballPosition.BallCount += 1;
            }
        }

        /// <summary>
        /// Creates the ball gate model based on depth using Recursion and Dynamic tasks
        /// </summary>
        /// <param name="initial">Starting level of model</param>
        /// <param name="depth">Depth of model</param>
        /// <param name="node">Node for which tree is created</param>
        /// <param name="tokenSource">TokenSource object for creating tasks</param>
        /// <param name="token">Token of the cancellation token source</param>
        public static void CreateBallGateTreeRecursive(int initial, int depth, NodeInfo node, CancellationTokenSource tokenSource, CancellationToken token)
        {
            try
            {
                if (initial >= depth)
                {
                    node.NodeId = ++_globalHeadId;
                    return;
                }

                node.LeftChild = new NodeInfo(NodeInfo.GetGateState());
                node.RightChild = new NodeInfo(NodeInfo.GetGateState());

                if (!token.IsCancellationRequested)
                {
                    ++initial;
                    var t1 = Task.Factory.StartNew(() => CreateBallGateTreeRecursive(initial, depth, node.LeftChild, tokenSource, token), token);
                    var t2 = Task.Factory.StartNew(() => CreateBallGateTreeRecursive(initial, depth, node.RightChild, tokenSource, token), token);

                    if (!t1.IsCanceled && !t2.IsCanceled)
                    {
                        Task.WaitAll(t1, t2);
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("out of memory exception");
                tokenSource.Cancel();
                throw;
            }
            catch (AggregateException)
            {
                throw;
            }
        }

        #endregion
    }
}
