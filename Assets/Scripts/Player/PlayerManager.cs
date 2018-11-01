using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerManager : MonoBehaviour
{
    static PlayerManager mInstance;
    public static PlayerManager GetSingleton()
    {
        if (mInstance == null)
        {
            GameObject go = new GameObject(typeof(PlayerManager).ToString());
            DontDestroyOnLoad(go);
            mInstance = go.AddComponent<PlayerManager>();
        }
        return mInstance;
    }
    public static void Destroy()
    {
        Destroy(mInstance.gameObject);
        mInstance = null;
    }

    private Camera mCamera;
    public Camera mainCamera { get {
            if(mCamera==null)
            {
                mCamera = Camera.main;
            }
            return mCamera;
    } }

    private PlayerEntity mControledPlayer;
   
    public List<PlayerEntity> players { get; private set; }

    public PlayerGroup attackGroup { get; private set; }
    public PlayerGroup defenseGroup { get; private set; }

    public void Init()
    {
        players = new List<PlayerEntity>();

        PlayerData data = new PlayerData();
        data.id = 1;
        data.camp = 1;
        data.x = 2;
        data.z = 2;
        data.hp = 10;
        data.model = "Akali";
        data.config = "Akali";
        data.destination = new Vector3(2f, 0, 8);
      
        mControledPlayer =  CreatePlayer(data);

        PlayerData data1 = new PlayerData();
        data1.id = 2;
        data1.camp = 2;
        data1.x = 2;
        data1.z = 2;
        data1.hp = 5;
        data1.model = "Akali";

        data1.config = "AI_Akali";
        data1.destination = new Vector3(1f, 0, 4);
      

        CreatePlayer(data1);
    }

   

    public void InitBattle()
    {
        players = new List<PlayerEntity>();

        PlayerGroupData attackData = new PlayerGroupData();
        attackData.id = 1;
        attackData.camp = 1;
        attackData.target = 0;
        attackData.columns = 5;
        attackData.x = 0;
        attackData.z = -15;
        attackData.dirX = 0;
        attackData.dirZ = 1;
        attackData.spaceColumn = -2;
        attackData.spaceRow = 2;
        attackData.count = 10;
        attackData.model = "Akali";
        attackData.config = "AI_Akali";
        attackData.hp = 10; 

         GameObject attackGo = new GameObject("Attack Group");
        attackGo.transform.SetParent(transform);
        attackGroup = attackGo.AddComponent<PlayerGroup>();
        attackGroup.SetData(attackData);


        PlayerGroupData defenseData = new PlayerGroupData();
        defenseData.id = 2;
        defenseData.camp = 2;
        defenseData.target = 0;
        defenseData.columns = 5;
        defenseData.x = 0;
        defenseData.z = 15;
        defenseData.dirX = 0;
        defenseData.dirZ = -1;
        defenseData.spaceColumn = 2;
        defenseData.spaceRow = 2;
        defenseData.count = 10;
        defenseData.model = "Akali";
        defenseData.config = "AI_Akali";
        defenseData.hp = 8;


        GameObject defenseGo = new GameObject("Attack Group");
        defenseGo.transform.SetParent(transform);
        defenseGroup = defenseGo.AddComponent<PlayerGroup>();
        defenseGroup.SetData(defenseData);
    }

    public PlayerEntity CreatePlayer(PlayerData data)
    {
        var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/R/Charactor/Akali/Prefab/Akali.prefab");
        var go = Instantiate(obj) as GameObject;
        go.transform.SetParent(transform);
        
        var p = go.AddComponent<PlayerEntity>();
        p.SetData(data);
        p.SetPosition(data.x, data.z);
        p.SetForword(data.dirX, data.dirZ);

        players.Add(p);
      
        return p;
    }

    public PlayerEntity GetPlayer(int id)
    {
        for(int i = 0;  i< players.Count; ++i)
        {
            if(players[i].data.id == id)
            {
                return players[i];
            }
        }
        return null;
    }

    private void Update()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            players[i].Tick(Time.deltaTime);
        }

        if(attackGroup!=null)
        {
            attackGroup.Tick(Time.deltaTime);
        }
        if (defenseGroup != null)
        {
            defenseGroup.Tick(Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (mainCamera)
            {
                Ray tmpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit tmpHit;
                if (Physics.Raycast(tmpRay, out tmpHit, 100))
                {
                    if(mControledPlayer)
                    {
                        mControledPlayer.MoveToPoint(tmpHit.point);
                    }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(mControledPlayer)
            {
                mControledPlayer.ReleaseSkill(PlayerAnimationType.attack1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (mControledPlayer)
            {
                mControledPlayer.ReleaseSkill(PlayerAnimationType.attack2);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (mControledPlayer)
            {
                mControledPlayer.ReleaseSkill(PlayerAnimationType.spell1);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (mControledPlayer)
            {
                mControledPlayer.ReleaseSkill(PlayerAnimationType.spell3);
            }
        }
    }
}

