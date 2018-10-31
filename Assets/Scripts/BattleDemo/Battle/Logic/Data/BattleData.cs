
using System.Collections.Generic;
namespace Battle.Logic
{
    public class BattleData
    {
        public string mBattleKey;
        public Dictionary<uint, TroopData> mAllTroopDic;
        public List<TroopData> mAtcTroopList;
        public List<TroopData> mDefTroopList;

        public int mCurrentFrame; //当前帧
        public int mFinishFrame; //完成帧
        public int mSeed = 0;

        public string mOperators; //指令（预留）

        public void Init()
        {
            RandHelper rand = new RandHelper(1);
            mBattleKey = "test";
            mCurrentFrame = 1;
            mFinishFrame = 10000;
            mAtcTroopList = new List<TroopData>();
            mDefTroopList = new List<TroopData>();
            mAllTroopDic = new Dictionary<uint, TroopData>();
            int atkCount = 1;
            for (int i = 0; i < atkCount; i++)
            {
                TroopData troop = new TroopData();
                troop.count = 100;
                troop.isAtkTroop = true;
                troop.key = (uint)i+1;
                troop.type = (SoldierType)rand.Random(4);
                troop.x = i * 100;
                troop.y = 100 ;
                troop.line = 1;
                troop.row = 1;
                mAtcTroopList.Add(troop);
                mAllTroopDic.Add(troop.key, troop);
            }
            int defCount = 1;
            for (int i = 0; i < defCount; i++)
            {
                TroopData troop = new TroopData();
                troop.count = 100;
                troop.isAtkTroop = false;
                troop.key = (uint)i+100;
                troop.type = (SoldierType)rand.Random(4);
                troop.x = i * 100;
                troop.y = -100;
                troop.line = 1;
                troop.row = 1;
                mDefTroopList.Add(troop);
                mAllTroopDic.Add(troop.key, troop);
            }

        }
        public void Dispose()
        {

        }
    }
}
