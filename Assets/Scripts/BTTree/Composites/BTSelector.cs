using System;
using System.Collections.Generic;

/// <summary>
/// 选择节点,该节点会从左到右的依次执行其子节点，只要子节点返回“失败”，
/// 就继续执行后面的节点，直到有一个节点返回“运行中”或“成功” 时，会停
/// 止后续节点的运行，并且向父节点返回“运行中”或“成 功”，如果所有子节
/// 点都返回“失败”则向父节点返回“失败”。
/// </summary>
public class BTSelector : BTComposite
{
    public int index { get; private set; }

    public BTSelector()
    {
        index = -1;
    }
    public override BTResult Update()
    {
        for(int i = 0; i < children.Count; ++i )
        {
            var child = children[i];

            switch(child.Update())
            {
                case BTResult.Doing:
                    {
                        index = i;

                        status = BTStatus.Doing;
                        return BTResult.Doing;
                    }
                case BTResult.Success:
                    {
                        child.Reset();

                        return BTResult.Success;

                    }
                case BTResult.Failed:
                    {
                       
                    }break;
            }
        }

        Reset();

        return base.Update();
    }

    public override void Reset()
    {
        base.Reset();
        index = -1;
    }
}

