
namespace BTree
{
    public class BTRoot
    {
        public BTNode mTreeRoot { get; private set; }

        public void InitXML(string name)
        {
            var _config = BTSerialization.ReadXML(name);
            mTreeRoot = BTFactory.CreateBTreeRootFromConfig(_config);
        }

        public void InitBinary(string name)
        {
            var _config = BTSerialization.ReadBinary(name);

            mTreeRoot = BTFactory.CreateBTreeRootFromConfig(_config);
        }

        public void Tick(ref BTInput _input)
        {
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
