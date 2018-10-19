using System;
using System.Collections.Generic;
/// <summary>
/// 条件（Condition）节点：根据条件的比较结果，返回成功或失败。
/// </summary>
public class BTConditional:BTNode
{
    protected virtual bool Check() { return false; }

    public override BTResult Update()
    {
        if(Check())
        {
            return BTResult.Success;
        }
        return base.Update();
    }
}

