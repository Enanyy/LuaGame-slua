﻿
namespace BTree
{
    public abstract class BTPrecondition
    {
        public BTPrecondition() { }
        public abstract bool Check(BTInput _input);
    }

    public class BTPreconditionTRUE : BTPrecondition
    {
        public BTPreconditionTRUE() { }
        public override bool Check(BTInput _input)
        {
            return true;
        }
    }
    public class BTPreconditionFALSE : BTPrecondition
    {
        public BTPreconditionFALSE() { }
        public override bool Check(BTInput _input)
        {
            return false;
        }
    }

}
