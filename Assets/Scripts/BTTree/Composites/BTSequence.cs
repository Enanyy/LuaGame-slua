﻿using System;
using System.Collections.Generic;
/// <summary>
/// 序列（Sequence）节点：顺序执行所有子节点返回成功，如果某个子节点失败返回失败。
/// </summary>
public class BTSequence : BTComposite
{
    public int index { get; private set; }

    public BTSequence()
    {
        index = -1;
    }
    /// <summary>
    /// 全部节点成功才算成功
    /// </summary>
    /// <returns></returns>
    public override BTResult Update()
    {
        if(index == -1)
        {
            index = 0;
        }
        for (; index < children.Count; ++index)
        {
            var child = children[index];
            switch (child.Update())
            {
                case BTResult.Doing:
                    {
                        status = BTStatus.Doing;
                        return BTResult.Doing;
                    }
                case BTResult.Success:
                    {
                        child.Reset();
                    }
                    break;
                case BTResult.Failed:
                    {
                        Reset();
                        return BTResult.Failed;
                    }
            }
        }

        Reset();

        return  BTResult.Success;
    }

    public override void Reset()
    {
        base.Reset();
        index = -1;
    }
}
