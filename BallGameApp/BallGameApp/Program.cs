using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BallGameApp
{
    class Program
    {
        static int _globalHeadId;

        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO BALL GATE APP");

            ConsoleKeyInfo option;
            do
            {
                Console.WriteLine("\n\nEnter Level: ");
                string inputValue = Console.ReadLine();

                if (int.TryParse(inputValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int level))
                {
                    if (level < 2)
                    {
                        Console.WriteLine("Depth should always be more than 1!");
                    }
                    else
                    {
                        Console.WriteLine("\n Guess the Empty container, enter a number between {0} to {1} : ", 1, Math.Pow(2, level));
                        string guessValue = Console.ReadLine();
                        if (int.TryParse(guessValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int guessEmptyContainer))
                        {
                            if (guessEmptyContainer >= 1 && guessEmptyContainer <= Math.Pow(2, level))
                            {
                                _globalHeadId = 0;
                                //NodeInfo head = new NodeInfo(GetGateSatate());
                                //CreateBallGameTreeRecursive(0, level, head);

                                try
                                {
                                    Console.WriteLine("Creation start ");
                                    //using (NodeInfo head = CreateBallGameTreeQueue(level))
                                    using (NodeInfo head = new NodeInfo(GetGateSatate()))
                                    {
                                        CreateBallGameTreeRecursive(0, level, head);
                                        Console.WriteLine("Creation end ");

                                        Console.WriteLine("Check balls start ");
                                        CheckBalls((int)Math.Pow(2, level) - 1, head);
                                        Console.WriteLine("Check balls end ");

                                        Console.WriteLine("Empty container start");
                                        DisplayEmptyContainer(head, false, guessEmptyContainer);
                                        Console.WriteLine("Empty container end");
                                        //head.Dispose();
                                        //head = null;
                                        //GC.Collect();
                                    }

                                    //GC.Collect();
                                    //GC.WaitForPendingFinalizers();
                                }
                                catch (OutOfMemoryException ex)
                                {
                                    Console.WriteLine("System cannot handle depth of {0}; please try with lesser depth. \n Error: {1}", level, ex.Message);
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid guess!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for Depth!");
                }

                Console.WriteLine("\n\nDo you want to guess one more time (y/n)? ");
                option = Console.ReadKey();
            } while (option.Key == ConsoleKey.Y);
        }

        static void DrawTree(int level)
        {
        }

        static void DisplayEmptyContainer(NodeInfo head, bool found, int guessEmptyContainer)
        {
            if (head == null || found)
            {
                return;
            }

            if (head.BallCount == 0 && head.RightChild == null && head.LeftChild == null)
            {
                found = true;
                Console.WriteLine("\n Empty container: {0}", head.HeadId);
                if (guessEmptyContainer == head.HeadId)
                {
                    Console.WriteLine("\n Your guess was correct");
                }
                return;
            }

            DisplayEmptyContainer(head.LeftChild, found, guessEmptyContainer);
            DisplayEmptyContainer(head.RightChild, found, guessEmptyContainer);
        }

        static void CheckBalls(int ballsCount, NodeInfo startPoint)
        {
            int ithBall = 1;

            while (ithBall <= ballsCount)
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
                ++ithBall;
            }
        }

        static GateState GetGateSatate()
        {
            Random random = new Random();
            int number = random.Next(1, 10000);

            if (number % 2 == 0)
            {
                return GateState.Right;
            }

            return GateState.Left;
        }

        static void CreateBallGameTreeRecursive(int initial, int level, NodeInfo head)
        {
            if (initial >= level)
            {
                head.HeadId = ++_globalHeadId;
                return;
            }

            head.LeftChild = new NodeInfo(GetGateSatate());
            head.RightChild = new NodeInfo(GetGateSatate());

            ++initial;
            CreateBallGameTreeRecursive(initial, level, head.LeftChild);
            CreateBallGameTreeRecursive(initial, level, head.RightChild);
        }

        static NodeInfo CreateBallGameTreeQueue(int level)
        {
            NodeInfo rootNode = new NodeInfo(GetGateSatate());

            Queue<NodeInfo> newQueue = new Queue<NodeInfo>(new List<NodeInfo> { rootNode });
            int nodeId = 1;
            for (int index = 1; index <= level; index++)
            {
                var previousQueue = new Queue(newQueue.ToArray().ToList());
                newQueue.Clear();

                do
                {
                    NodeInfo item = (NodeInfo)previousQueue.Dequeue();

                    item.LeftChild = new NodeInfo(GetGateSatate());
                    item.RightChild = new NodeInfo(GetGateSatate());

                    if (index == level)
                    {
                        item.LeftChild.HeadId = nodeId++;
                        item.RightChild.HeadId = nodeId++;
                    }
                    else
                    {
                        newQueue.Enqueue(item.LeftChild);
                        newQueue.Enqueue(item.RightChild);
                    }

                } while (previousQueue.Count != 0);

                Console.WriteLine("index -- {0}", index);
            }

            newQueue.Clear();            
            return rootNode;
        }
    }
}
