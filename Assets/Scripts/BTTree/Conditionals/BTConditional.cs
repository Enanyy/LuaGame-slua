using System;
using System.Collections.Generic;

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

