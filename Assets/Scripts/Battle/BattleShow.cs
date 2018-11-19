using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleShow
{
    public GameObject gameObject { get; private set; }
    public BattleShow()
    {
        string assetBundleName = "assets/assetBundle/ui/ui_widget/ui_fight.prefab";
        UIManager.LoadUIAsset(assetBundleName, assetBundleName, (GameObject go) =>
        {
            gameObject = go;

            var parent = BattleManager.instance.uiCamera.transform.parent;
            gameObject.transform.SetParent(parent);

            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.identity;
         
        });
    }
    public void Destroy()
    {
        UIManager.DeleteUI("ui/ui_topshow", "ui_topshow");

    }
}

