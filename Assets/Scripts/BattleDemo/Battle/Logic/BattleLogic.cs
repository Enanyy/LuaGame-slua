
using BTree;
using System.Collections.Generic;

namespace Battle.Logic
{
    public class BattleLogic
    {

        private static float m_logicTime = 1f / BattleLogicDefine.logicSecFrame;

        public BattleData m_BattleData { get; private set; }
        public RandHelper m_Rand { get; private set; }

        private bool m_isReport = false;

        private bool m_isFinish = false;
        private float m_logicAddTime = 0f;

        public int m_currentFrame = 0; //当前帧
        public int m_finishFrame = 0; //目标帧

        private Dictionary<uint, BTRoot> m_TroopBTreeDic;
        private bool isFinish = false;
        private BTInputData m_Input = new BTInputData();

        public BattleLogic(BattleData battleData, int _seed)
        {
            m_BattleData = battleData;
           
            m_Rand = new RandHelper(_seed);
            m_BattleData.mSeed = m_Rand.GetSeed();
            SetCurrentFrame(1);
            m_finishFrame = 0;
            m_logicAddTime = 0f;
            m_isFinish = false;
            m_TroopBTreeDic = new Dictionary<uint, BTRoot>();
        }

        public LogicState Update(float dt = 0)
        {
            if (m_isFinish)
            {
                return LogicState.End;
            }
            if (!BattleLogicDefine.isServer && !m_isReport)
            {
                if (m_currentFrame > m_finishFrame)
                {
                    return LogicState.Nothing;
                }
            }
            if (dt > 0)
            {
                m_logicAddTime += dt;
                if (m_logicAddTime < m_logicTime)
                {
                    return LogicState.Nothing;
                }
                else
                {
                    m_logicAddTime -= m_logicTime;
                }
            }

         
            if (UpdateLogic(m_currentFrame))
            {
                m_isFinish = true;
            }
            SetCurrentFrame(m_currentFrame + 1);

            return LogicState.Playing;
        }

        private void SetCurrentFrame(int currentFrame)
        {
            m_currentFrame = currentFrame;
            m_BattleData.mCurrentFrame = currentFrame;
        }
        public void SetFinishFrame(int finishFrame)
        {
            m_finishFrame = finishFrame;
        }
        public void SetSpeed(int speed)
        {
            m_logicTime = 1f / BattleLogicDefine.logicSecFrame / speed;
        }

        private bool UpdateLogic(int currentFrame)
        {
            if (isFinish)
            {
                return true;
            }
            var m_AtkTroopList = m_BattleData.mAtcTroopList;
            var m_DefTroopList = m_BattleData.mDefTroopList;
            bool isTroopEmpty = false;
            isTroopEmpty = CheckTroopCount(m_AtkTroopList) || CheckTroopCount(m_DefTroopList);
            if (!isTroopEmpty)
            {
                DoSoldierLogic();
            }
            if (isTroopEmpty)
            {
                Debugger.Log("War End!!!!");
                SetFinishState();
                isFinish = true;
            }
            return false;
        }



        private void DoSoldierLogic()
        {
            var m_AtkTroopList = m_BattleData.mAtcTroopList;
            var m_DefTroopList = m_BattleData.mDefTroopList;
            for (int i = 0; i < m_AtkTroopList.Count; i++)
            {
                var _atkTroop = m_AtkTroopList[i];
                m_Input.SetData(_atkTroop, m_BattleData);

                if (!m_TroopBTreeDic.ContainsKey(_atkTroop.key))
                {
                    m_TroopBTreeDic.Add(_atkTroop.key, new BTRoot());
                    m_TroopBTreeDic[_atkTroop.key].InitXML("RootParallel");
                }
                BTInput _input = m_Input as BTInput;
                m_TroopBTreeDic[_atkTroop.key].Tick(ref _input);


            }
            for (int i = 0; i < m_DefTroopList.Count; i++)
            {
                var _defTroop = m_DefTroopList[i];
                m_Input.SetData(_defTroop, m_BattleData);

                if (!m_TroopBTreeDic.ContainsKey(_defTroop.key))
                {
                    m_TroopBTreeDic.Add(_defTroop.key, new BTRoot());
                    m_TroopBTreeDic[_defTroop.key].InitBinary("RootParallel");
                }
                BTInput _input = m_Input as BTInput;
                m_TroopBTreeDic[_defTroop.key].Tick(ref _input);

            }
        }

        private void SetFinishState()
        {
            var m_AllTroopDic = m_BattleData.mAllTroopDic;
            foreach (var kv in m_AllTroopDic)
            {
                kv.Value.state = (int)TroopAnimState.Idle;
            }
        }
        private bool CheckTroopCount(List<TroopData> _TroopList)
        {
            for (int i = 0; i < _TroopList.Count; i++)
            {
                if (_TroopList[i].ghostTime > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
