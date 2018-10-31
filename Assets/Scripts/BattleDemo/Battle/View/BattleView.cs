
using Battle.Logic;
using System.Collections.Generic;

namespace Battle.View
{
    public class BattleView
    {
        private BattleData m_BattleData;
        private BattleLogic m_BattleLogic;


        private Dictionary<uint, TroopEntity> m_AtkTroopDic = new Dictionary<uint, TroopEntity>();
        private Dictionary<uint, TroopEntity> m_DefTroopDic = new Dictionary<uint, TroopEntity>();


        private int m_Speed;
        private object m_TimeObj;

        public void Dispose()
        {
            Exit();
        }

        public void Init(BattleData _battleData)
        {
            m_BattleData = _battleData;
            m_BattleLogic = new BattleLogic(m_BattleData, m_BattleData.mSeed);

            m_TimeObj = TimerHelper.AddFrame(m_BattleData.mBattleKey, OnUpdate);
        }

        public void SyncFrame(BattleData data)
        {
            Debugger.Log("完成帧：" + data.mFinishFrame);
            m_BattleLogic.SetFinishFrame(data.mFinishFrame);
            AddCommand(data.mOperators);
            int currentFrame = data.mCurrentFrame;
            while (true)
            {
                if (m_BattleLogic.m_currentFrame >= currentFrame - 1)
                {
                    return;
                }
                LogicState state = m_BattleLogic.Update();
                if (state == LogicState.End)
                {
                    Exit();
                    return;
                }
            }
        }
        public void SetSpeed(int speed)
        {
            m_Speed = speed;
            m_BattleLogic.SetSpeed(speed);
          

            foreach (var kv in m_AtkTroopDic)
            {
                kv.Value.SetSpeed(speed);
            }
            foreach (var kv in m_DefTroopDic)
            {
                kv.Value.SetSpeed(speed);
            }
        }
        private void OnUpdate(float dt)
        {
            LogicState result = m_BattleLogic.Update(dt);
            if (result == LogicState.Playing)
            {
                RefreshTroop();
            }
            else if (result == LogicState.End)
            {
                Exit();
            }
        }

        private void RefreshTroop()
        {
            var m_AtkTroopList = m_BattleLogic.m_BattleData.mAtcTroopList;
            var m_DefTroopList = m_BattleLogic.m_BattleData.mDefTroopList;
            for (int i = 0; i < m_AtkTroopList.Count; i++)
            {
                TroopData _atkTroop = m_AtkTroopList[i];
                float x = _atkTroop.x;
                float y = _atkTroop.y;
                CorrectPos(ref x, ref y);
                float dir_x = _atkTroop.dir_x;
                float dir_y = _atkTroop.dir_y;
                CorrectPos(ref dir_x, ref dir_y);
                if (m_AtkTroopDic.ContainsKey(_atkTroop.key))
                {
                    m_AtkTroopDic[_atkTroop.key].Doing(x, y, dir_x, dir_y, _atkTroop);
                }
                else
                {
                    TroopEntity soldierGroup = BattleManager.GetSingleton().CreateTroop(x, y, dir_x, dir_y, _atkTroop);
                    m_AtkTroopDic[_atkTroop.key] = soldierGroup;
                }
                _atkTroop.addTroop = false;
            }
            for (int i = 0; i < m_DefTroopList.Count; i++)
            {
                TroopData _defTroop = m_DefTroopList[i];
                float x = _defTroop.x;
                float y = _defTroop.y;
                CorrectPos(ref x, ref y);
                float dir_x = _defTroop.dir_x;
                float dir_y = _defTroop.dir_y;
                CorrectPos(ref dir_x, ref dir_y);
                if (m_DefTroopDic.ContainsKey(_defTroop.key))
                {
                    m_DefTroopDic[_defTroop.key].Doing(x, y, dir_x, dir_y, _defTroop);
                }
                else
                {
                    TroopEntity soldierGroup = BattleManager.GetSingleton().CreateTroop(x, y, dir_x, dir_y, _defTroop);
                    m_DefTroopDic[_defTroop.key] = soldierGroup;
                }
                _defTroop.addTroop = false;
            }
        }

        private void CorrectPos(ref float x, ref float y)
        {
            x = x / 10;
            y = y / 10;
            return;
        }


        //添加指令
        private void AddCommand(string operators)
        {

        }

        //退出战场
        private void Exit()
        {
            TimerHelper.Remove(m_TimeObj);
            //if (BattleLogicDefine.isServer)
            //{

            //}
        }
    }

}
