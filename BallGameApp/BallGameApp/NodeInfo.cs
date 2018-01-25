using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallGameApp
{
    public enum GateState
    {
        Left = 0,

        Right
    }

    public class NodeInfo : IDisposable
    {
        public NodeInfo LeftChild { get; set; }

        public NodeInfo RightChild { get; set; }

        public GateState GateState { get; set; }

        public int BallCount { get; set; }

        public int HeadId { get; set; }

        public NodeInfo(GateState state)
        {
            GateState = state;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                //if (LeftChild != null)
                //{
                //    LeftChild.Dispose(dispose);
                //    LeftChild = null;
                //}

                //if (RightChild != null)
                //{
                //    RightChild.Dispose(dispose);
                //    RightChild = null;
                //}

                LeftChild = null;
                RightChild = null;
                GC.Collect();
                GC.SuppressFinalize(this);
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
