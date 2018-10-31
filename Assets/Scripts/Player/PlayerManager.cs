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

    }

    private Camera mCamera;
    public Camera mainCamera { get {
            if(mCamera==null)
            {
                mCamera = Camera.main;
            }
            return mCamera;
        } }

    private PlayerController player;
    private List<PlayerController> mPlayerList = new List<PlayerController>();
    public List<PlayerController> players { get { return mPlayerList; } }

    public void CreatePlayer(PlayerData data, Action<PlayerController> action = null)
    {
        var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/R/Charactor/Akali/Prefab/Akali.prefab");
        var go = Instantiate(obj) as GameObject;
        go.transform.SetParent(transform);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(Vector3.zero, out hit, 5, NavMesh.AllAreas))
        {
            go.transform.position = hit.position;
        }

        var pc = go.AddComponent<PlayerController>();
        pc.Init(data);
        mPlayerList.Add(pc);
        if(data.id == 0)
        {
            player = pc;
        }

        if (action != null)
        {
            action(player);
        }
    }

    public PlayerController GetPlayer(int id)
    {
        for(int i = 0;  i< mPlayerList.Count; ++i)
        {
            if(mPlayerList[i].data.id == id)
            {
                return mPlayerList[i];
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCamera)
            {
                Ray tmpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit tmpHit;
                if (Physics.Raycast(tmpRay, out tmpHit, 100))
                {
                    if(player)
                    {
                        player.MoveToPoint(tmpHit.point);
                    }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(player)
            {
                player.ReleaseSkill(PlayerAnimationType.attack1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (player)
            {
                player.ReleaseSkill(PlayerAnimationType.attack2);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (player)
            {
                player.ReleaseSkill(PlayerAnimationType.spell1);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (player)
            {
                player.ReleaseSkill(PlayerAnimationType.spell3);
            }
        }
    }
}

