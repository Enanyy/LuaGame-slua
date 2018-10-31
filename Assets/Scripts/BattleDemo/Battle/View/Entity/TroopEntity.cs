
using Battle.Logic;
using Neatly.Timer;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.View
{
    public class TroopEntity : EntityBase<TroopData>
    {
        private SoldierType m_Type;
        private uint m_BaseCount;
        private TroopAnimState m_CurrentState;
        private bool m_IsAttacker;
        private uint m_Line;
        private uint m_Row;
        private float m_Scale;
        private float m_LineInterval;
        private float m_RowInterval;

        private SoldierEntity[,] soldierArray;
        private List<int> soldierIndexList;
        private List<SoldierEntity> m_CacheDieSoldier = new List<SoldierEntity>();

        public void Init(float x, float y, float dir_x, float dir_y, TroopData data)
        {
            base.Init(data);
            m_Type = data.type;
            m_IsAttacker = data.isAtkTroop;
            m_Line = data.line;
            m_Row = data.row;
            m_Scale = 1f;
            m_LineInterval = 1f;
            m_RowInterval = 1f;
            //初始化坐标
            InitPosition(x, y);
            m_BaseCount = data.count;
            if (data.count > 0)
            {
                //float deg = CalcRad(dir_x, dir_y);
                InitAllSoldiers(data.count, (TroopAnimState)data.state);
            }
        }
        /// <summary>
        /// 初始化士兵
        /// </summary>
        private void InitAllSoldiers(uint count, TroopAnimState nState, float deg = 0f)
        {
            m_BaseCount = count;
            if (soldierArray == null)
            {
                soldierArray = new SoldierEntity[mData.line, mData.row];
            }
            if (soldierIndexList == null)
            {
                soldierIndexList = new List<int>();
            }

            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null) continue;
                    SoldierData soldierData = new SoldierData(mData, i, j);
                    var soldier = BattleManager.GetSingleton().CreateSoldier(soldierData.type, transform);
                    soldier.Init(soldierData);
                    soldierArray[i, j] = soldier;
                    soldierIndexList.Add(GetIndexValue(i, j));
                }
            }
        }

        public void Doing(float x, float y, float dir_x, float dir_y, TroopData data)
        {
            m_CurrentState = (TroopAnimState)data.state;
            //士兵已死
            if (m_BaseCount == 0 && data.count == 0) return;
            //复活(从出生点)
            if (m_BaseCount == 0)
            {
                InitPosition(x, y);
            }
            //初始化显示
            if (!m_IsInit)
            {
                Show(true);
            }
            float deg = CalcRad(dir_x, dir_y);
            //有其他玩家加入，士兵刷新
            if (data.addTroop && data.count > 0)
            {
                InitAllSoldiers(data.count, (TroopAnimState)data.state, deg);
                //不影响移动
                if (m_CurrentState == TroopAnimState.Move)
                {
                    SetTweenPosition(x, y);
                }
                return;
            }
            //判断死亡数
            uint allObject = data.row * data.line;
            int remainIndexCount = soldierIndexList == null ? 0 : soldierIndexList.Count;
            int loseCount = (int)((m_BaseCount - data.count) * allObject / m_BaseCount + remainIndexCount - allObject);
            for (int i = 0; i < loseCount; i++)
            {
                DieOne(deg);
            }

            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null)
                    {
                        soldierArray[i, j].SetState(m_CurrentState);
                    }
                }
            }
            if (m_CurrentState == TroopAnimState.Move)
            {
                //如果状态是移动则播放
                SetTweenPosition(x, y);
            }
            SetSoldierLocalRotation(deg);
            if (data.count == 0)
            {
                m_BaseCount = 0;
                //ShowUI(false);
            }
        }

        private void DieOne(float deg)
        {
            int randIndex = Random.Range(0, soldierIndexList.Count);
            var soldierObject = GetSoldierByIndex(soldierIndexList[randIndex]);
            soldierObject.SetState(TroopAnimState.Die);
            m_CacheDieSoldier.Add(soldierObject);
            //固定时间后回收
            soldierObject.SetParent(BattleManager.GetSingleton().m_BattleViewRoot);
            //SoarDTween.Alpha(soldierObject.Renderer, 0, 1);
            NeatlyTimer.AddClock(soldierObject, f =>
            {
                soldierObject.Dispose();
                m_CacheDieSoldier.Clear();
            }, 1.2f, true);
            soldierIndexList.RemoveAt(randIndex);
        }

        private void SetSoldierLocalRotation(float y)
        {
            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null)
                    {
                        soldierArray[i, j].SetLocalRotation(y);
                    }
                }
            }
        }

        private SoldierEntity GetSoldierByIndex(int index)
        {
            int i = index >> 4;
            int j = index & 0xf;
            Debugger.Log(i + "  " + j);
            var soldier = soldierArray[i, j];
            soldierArray[i, j] = null;
            return soldier;
        }

        private int GetIndexValue(int i, int j)
        {
            int r = i << 4;
            r += j;
            return r;
        }

        private float CalcRad(float x, float y)
        {
            var disX = x - m_CurPosX;
            var disY = y - m_CurPosY;
            float deg = Mathf.Atan2(disY, disX) * Mathf.Rad2Deg;
            deg = -deg + 270;
            if (deg > 180)
            {
                deg -= 360;
            }
            if (deg < -180)
            {
                deg += 360;
            }
            return deg;
        }
        public void Dispose()
        {
            for (int i = 0; i < m_CacheDieSoldier.Count; i++)
            {
                m_CacheDieSoldier[i].Dispose();
            }
            m_CacheDieSoldier.Clear();
            if (soldierArray != null)
            {
                for (int i = 0; i < soldierArray.GetLength(0); i++)
                {
                    for (int j = 0; j < soldierArray.GetLength(1); j++)
                    {
                        if (soldierArray[i, j] != null)
                        {
                            soldierArray[i, j].Dispose();
                        }
                    }
                }
                soldierArray = null;
            }
            soldierIndexList = null;

            BattleManager.GetSingleton().RecycleTroop(this);
        }
        public void SetSpeed(int speed)
        {
            for (int i = 0; i < soldierArray.GetLength(0); i++)
            {
                for (int j = 0; j < soldierArray.GetLength(1); j++)
                {
                    if (soldierArray[i, j] != null)
                    {
                        soldierArray[i, j].SetSpeed(speed);
                    }
                }
            }
        }

        public override void Init(TroopData _data)
        {
            base.Init(_data);
        }
        #region 显示
        protected bool m_IsInit;

        protected void Show(bool value)
        {
            gameObject.SetActive(value);
        }

        #endregion

        #region 移动
        protected float m_LastPosX;
        protected float m_LastPosY;
        protected float m_CurPosX;
        protected float m_CurPosY;
        protected float m_TarPosX;
        protected float m_TarPosY;
        protected float m_DeltaTime;
        protected bool m_Move;

        protected void Start()
        {
            NeatlyTimer.AddFrame(this, FrameUpdate);
        }

        void FrameUpdate(float deltaTime)
        {
            m_DeltaTime = m_DeltaTime + deltaTime;
            DoMove();
            UnitUpdate();
        }

        protected virtual void UnitUpdate() { }

        protected void DoMove()
        {
            float radio = m_DeltaTime / BattleManager.GetSingleton().m_LogicTime;
            if (m_Move)
            {
                m_CurPosX = Mathf.Lerp(m_LastPosX, m_TarPosX, radio);
                m_CurPosY = Mathf.Lerp(m_LastPosY, m_TarPosY, radio);
                SetLocalPosition(m_CurPosX, m_CurPosY);
                if (radio >= 1)
                {
                    m_Move = false;
                }
            }
        }

        protected void OnCorrectMove()
        {
            m_CurPosX = m_TarPosX;
            m_CurPosY = m_TarPosY;
            m_LastPosX = m_TarPosX;
            m_LastPosY = m_TarPosY;
            SetLocalPosition(m_CurPosX, m_CurPosY);
            m_Move = false;
        }

        protected void SetTweenPosition(float x, float y)
        {
            m_DeltaTime = 0;
            m_LastPosX = m_CurPosX;
            m_LastPosY = m_CurPosY;
            m_TarPosX = x;
            m_TarPosY = y;
            m_Move = true;
        }
        #endregion

        #region Interface

        protected virtual void InitPosition(float posX, float posY)
        {
            m_TarPosX = posX;
            m_TarPosY = posY;
            OnCorrectMove();
        }
        #endregion
    }

}
