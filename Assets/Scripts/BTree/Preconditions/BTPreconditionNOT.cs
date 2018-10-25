
namespace BTree
{
    public class BTPreconditionNOT : BTPrecondition
    {
        private BTPrecondition mPrecondition;
        public BTPreconditionNOT() { }
        public BTPreconditionNOT(BTPrecondition _precondition)
        {
            if (_precondition == null)
            {
                Debugger.Log("BTreeNodePreconditionNOT is null");
            }
            mPrecondition = _precondition;
        }
        public override bool Check(BTInput _input)
        {
            return mPrecondition!=null&&!mPrecondition.Check(_input);
        }
        public BTPrecondition GetChildPrecondition()
        {
            return mPrecondition;
        }
        public void SetChildPrecondition(BTPrecondition _precondition)
        {
            mPrecondition = _precondition;
        }
    }
}
