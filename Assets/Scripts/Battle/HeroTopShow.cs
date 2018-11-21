using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroTopShow
{
    class TopShow
    {
        public int id;
        public GameObject gameObject;
        public UILabel label;
        public UISlider slider;
    }
    private Dictionary<int, TopShow> mTopShowDic = new Dictionary<int, TopShow>();

    public GameObject gameObject { get; private set; }
    private GameObject mItem;
    public HeroTopShow()
    {
        /*
        string assetBundleName = "assets/assetBundle/ui/ui_widget/ui_topshow.prefab";
        UIManager.LoadUIAsset(assetBundleName, assetBundleName, (GameObject go) => {

            gameObject = go;

            var parent = BattleManager.instance.uiCamera.transform.parent ;
            gameObject.transform.SetParent(parent);

            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.identity;
            mItem = gameObject.transform.Find("Item").gameObject;
            mItem.SetActive(false);
        });
      */
    }



    public void SetPosition(HeroData data, Vector3 worldPosition)
    {
        if(mItem==null || data == null)
        {
            return;
        }
        if (mTopShowDic.ContainsKey(data.id) == false)
        {
            GameObject go = GameObject.Instantiate(mItem);
            go.transform.SetParent(gameObject.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            go.SetActive(true);
            TopShow show = new TopShow();
            show.id = data.id;
            show.gameObject = go;
            show.label = go.transform.GetComponentInChildren<UILabel>();
            show.label.color = data.camp == HeroCamp.Attack ? Color.red : Color.green;
            UISprite sprite = go.transform.Find("Overlay").GetComponent<UISprite>();
            sprite.color = show.label.color;
            show.slider = go.GetComponent<UISlider>();
            mTopShowDic.Add(data.id, show);             
        }

        mTopShowDic[data.id].label.text = data.name;
        mTopShowDic[data.id].slider.value = data.maxhp != 0 ? data.hp * 1f / data.maxhp : 0;
        
        var mainCamera = BattleManager.instance.mainCamera;
        var uiCamera = BattleManager.instance.uiCamera;
        if (mainCamera && uiCamera)
        {
            Vector3 screenPosition =mainCamera.WorldToScreenPoint(worldPosition);

            Vector3 position = uiCamera.ScreenToWorldPoint(screenPosition);
            position.z = 0;
            mTopShowDic[data.id].gameObject.transform.position = position;
        }
    }

    public void Remove(int id)
    {
        if(mTopShowDic.ContainsKey(id))
        {
            var show = mTopShowDic[id];
            mTopShowDic.Remove(id);
            UnityEngine.Object.Destroy(show.gameObject);
        }
    }

    public void Destroy()
    {
        //UIManager.DeleteUI("ui/ui_topshow", "ui_topshow");
        mTopShowDic.Clear();
    }
}

