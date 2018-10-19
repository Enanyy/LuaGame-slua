using System;
using System.Collections.Generic;

/// <summary>
/// 动作（Action）节点：根据动作结果返回成功，失败，或运行。
/// </summary>
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

