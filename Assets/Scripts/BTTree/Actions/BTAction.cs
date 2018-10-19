using System;
using System.Collections.Generic;

public class BTAction : BTNode
{
    protected virtual void OnEnter() { }
    protected virtual BTResult OnExcute() { return BTResult.Failed; }
    protected virtual void OnExit() { }


    public override BTResult Update()
    {
        BTResult result = BTResult.Success;

        if (status == BTStatus.Ready)
        {
            OnEnter();
            status = BTStatus.Doing;
        }

        if (status == BTStatus.Doing)
        {
            result = OnExcute();
            if (result != BTResult.Doing)
            {
                OnExit();
                status = BTStatus.Ready;
            }
        }

        return result;
    }
}

