using System;
using System.Collections.Generic;

public abstract class BTNode
{
    public int id { get; set; }
    public BTStatus status { get; protected set; }

    public BTNode()
    {
        status = BTStatus.Ready;
    }

    public virtual BTResult Update() { return BTResult.Failed; }
    public virtual void Reset()
    {
        status = BTStatus.Ready;
    }
}

