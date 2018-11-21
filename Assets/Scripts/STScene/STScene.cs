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
    public override void UpdateAttribute()
    {
        base.UpdateAttribute();
    }

    public override void SetAttribute()
    {
        base.SetAttribute();
    }


    public override void ParseXml(SecurityElement node)
    {
        if(node == null)
        {
            return;
        }
        base.ParseXml(node);
        
        SetAttribute();

    }

    public void LoadXml(string text)
    {
        if(string.IsNullOrEmpty(text))
        {
            return;

        }

        SecurityParser sp = new SecurityParser();

        sp.LoadXml(text);


        SecurityElement se = sp.ToXml();

        ParseXml(se);
    }



    
}
