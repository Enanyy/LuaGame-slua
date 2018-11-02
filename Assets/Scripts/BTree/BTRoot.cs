
namespace BTree
{
    public class BTRoot
    {
        public BTNode mTreeRoot { get; private set; }

        public void InitXML(byte[] data)
        {
            var _config = BTSerialization.ReadXML(data);
            mTreeRoot = BTFactory.CreateBTreeRootFromConfig(_config);
        }

        public void InitBinary(byte[] data)
        {
            var _config = BTSerialization.ReadBinary(data);

            mTreeRoot = BTFactory.CreateBTreeRootFromConfig(_config);
        }

        public void Tick(ref BTInput _input)
        {
            if(mTreeRoot==null)
            {
                return;
            }
            if (mTreeRoot.Evaluate(_input))
            {
                var result = mTreeRoot.Tick(ref _input);
            }
            else
            {
                mTreeRoot.Transition(_input);
            }
        }
    }
    
}
