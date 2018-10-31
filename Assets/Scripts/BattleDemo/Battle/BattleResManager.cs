
using Neatly;
using UnityEngine;
using Battle.Logic;
using Battle.View;

public sealed class BattleResManager 
{
   
    #region Pools

   
    private int m_SoldierGroupCC=10;
   
    private int m_SoldierCC = 10;

    public BattlePool<TroopEntity, TroopData> m_TroppPool { get; private set; }
    public BattlePool<SoldierEntity,SoldierData>[] m_SoldierPool { get; private set; }
    #endregion

    public void Init()
    {
        //士兵Group
        m_TroppPool = new BattlePool<TroopEntity, TroopData>();
        GameObject troop = new GameObject("troop");
        troop.AddComponent<TroopEntity>();
        m_TroppPool.Init(troop);
        //士兵
        m_SoldierPool = new BattlePool<SoldierEntity, SoldierData>[1];
        for (int i = 0; i < m_SoldierPool.Length; i++)
        {
            m_SoldierPool[i] = new BattlePool<SoldierEntity, SoldierData>();
            var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/R/Charactor/Akali/Prefab/Akali.prefab");
            var soldier = GameObject.Instantiate(obj) as GameObject;
            
            m_SoldierPool[i].Init(soldier);
        }
        InitCache();
    }

    private void InitCache()
    {
        m_TroppPool.Prepare(m_SoldierGroupCC);
        for (int i = 0; i < m_SoldierPool.Length; i++)
        {
            m_SoldierPool[i].Prepare(m_SoldierCC);
        }
    }
    public void Dispose()
    {

    }

}
