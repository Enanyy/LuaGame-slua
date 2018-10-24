
namespace BTree
{
    public class BTPreconditionOR : BTPrecondition
    {
        private BTPrecondition[] mPreconditions;
        public BTPreconditionOR() { }
        public BTPreconditionOR(params BTPrecondition[] param)
        {
            if (param == null)
            {
                Debugger.Log("BTreeNodePreconditionOR is null");
                return;
            }
            if (param.Length == 0)
            {
                Debugger.Log("BTreeNodePreconditionOR's length is 0");
                return;
            }
            mPreconditions = param;
        }
        public override bool Check(BTData _input)
        {
            for (int i = 0; i < mPreconditions.Length; i++)
            {
                if (mPreconditions[i].Check(_input))
                {
                    return true;
                }
            }
            return false;
        }
        public void SetChildPrecondition(params BTPrecondition[] param)
        {
            mPreconditions = param;
        }
        public BTPrecondition[] GetChildPrecondition()
        {
            return mPreconditions;
        }
        public int GetChildPreconditionCount()
        {
            if (mPreconditions != null)
            {
                return mPreconditions.Length;
            }
            return 0;
        }

    }
}
