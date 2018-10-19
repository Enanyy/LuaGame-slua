using System;
using System.Collections.Generic;

public class BTComposite : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    public void AddChild(BTNode node)
    {
        children.Add(node);
    }

    public void RemoveChild(BTNode node)
    {
        children.Remove(node);
    }

    public override void Reset()
    {
        base.Reset();

        for(int i = 0; i < children.Count; ++i)
        {
            children[i].Reset();
        }
    }
}

