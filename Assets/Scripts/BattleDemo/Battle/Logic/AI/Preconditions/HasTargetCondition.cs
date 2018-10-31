
using BTree;

namespace Battle.Logic
{
    public class HasTargetCondition : BTPrecondition
    {
        public HasTargetCondition() { }
        public override bool Check(BTInput input)
        {
            var _input = (BTInputData)input;

            var troop = _input.troop;
            if (troop.targetKey != 0)
            {
                if (_input.battleData.mAllTroopDic.ContainsKey(troop.targetKey))
                {
                    var target = _input.battleData.mAllTroopDic[troop.targetKey];
                    if (target.count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
