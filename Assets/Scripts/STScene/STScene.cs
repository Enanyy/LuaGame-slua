using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using System.Xml;
#endif
using System.Security;
using Mono.Xml;

public class STScene : STSceneGroup
{
    private Action<int,int> mCallback;

    protected override void OnLoadFinish()
    {
        int count = 0;
        int finishCount = 0;

        CheckFinish(out count, out finishCount);

        if (mCallback != null)
        {
            mCallback(count, finishCount);
        }
    }

    public override void ParseXml(SecurityElement node)
    {
        if(node == null)
        {
            return;
        }
        base.ParseXml(node);    
    }

    public void LoadXml(string text, Action<int,int> callback= null)
    {
        if(string.IsNullOrEmpty(text))
        {
            return;

        }
        mCallback = callback;
        SecurityParser sp = new SecurityParser();

        sp.LoadXml(text);


        SecurityElement se = sp.ToXml();

        ParseXml(se);

        SetAttribute();
    }



    
}
