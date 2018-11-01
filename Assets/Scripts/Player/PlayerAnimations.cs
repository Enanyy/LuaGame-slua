using System;
using System.Collections.Generic;
public static class PlayerAnimations
{
    static Dictionary<string, Dictionary<PlayerAnimationType, float>> mAnimations = new Dictionary<string, Dictionary<PlayerAnimationType, float>> {
        {"Akali",new Dictionary<PlayerAnimationType, float>{
            { PlayerAnimationType.attack1,1.250f },
            { PlayerAnimationType.attack2,1.250f },
            { PlayerAnimationType.dance,8.875f },
            { PlayerAnimationType.die,1.750f },
            { PlayerAnimationType.idle,1.250f },
            { PlayerAnimationType.run,0.833f },
            { PlayerAnimationType.sneak,0.583f },
            { PlayerAnimationType.spell1,1.250f },
            { PlayerAnimationType.spell3,1.250f }
            }
        }
    };

    public static float GetAnimationLength(string name, PlayerAnimationType type)
    {
        if(mAnimations.ContainsKey(name) && mAnimations[name].ContainsKey(type))
        {
            return mAnimations[name][type];
        }
        return 0;
    }
}

