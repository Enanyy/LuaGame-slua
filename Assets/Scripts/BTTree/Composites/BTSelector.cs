using System;
using System.Collections.Generic;

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

