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

    private Camera mCamera;
    public Camera mainCamera { get {
            if(mCamera==null)
            {
                mCamera = Camera.main;
            }
            return mCamera;
        } }

    private PlayerController player;

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

        player = go.AddComponent<PlayerController>();
        player.Init(data);
        if (action != null)
        {
            action(player);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
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
    }
}

