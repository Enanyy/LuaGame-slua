
using BTree;

namespace Battle.Logic
{
    public class HasReachedTargetCondition : BTPrecondition
    {
        public HasReachedTargetCondition() { }
        public override bool Check(BTInput input)
        {
            var _input = (BTInputData)input;

            var troop = _input.troop;
            if (troop.targetKey != 0)
            {
                var target = _input.battleData.mAllTroopDic[troop.targetKey];
                var dis = MathHelper.DistanceV2(troop.x, troop.y, target.x, target.y);
                if (dis<=0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
