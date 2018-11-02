using System;
using System.Collections.Generic;
public static class PlayerAnimations
{
    static Dictionary<string, Dictionary<PlayerAnimationType, float>> mAnimations = new Dictionary<string, Dictionary<PlayerAnimationType, float>> {
        {"ai_ge_001_ty",new Dictionary<PlayerAnimationType, float>{
            { PlayerAnimationType.attack,1.40f },
            { PlayerAnimationType.attack2,1.4f },
           
            { PlayerAnimationType.death,1.50f },
            { PlayerAnimationType.free,1.333f },
            { PlayerAnimationType.idle,5.0f },
            { PlayerAnimationType.walk,0.867f },
        
            { PlayerAnimationType.skill,0.733f },
            { PlayerAnimationType.skill2,0.733f }
            }
        },

         {"bian_fu_001_ty",new Dictionary<PlayerAnimationType, float>{
            { PlayerAnimationType.attack,1.40f },
            { PlayerAnimationType.attack2,1.4f },

            { PlayerAnimationType.death,1.333f },
            { PlayerAnimationType.free,0.80f },
            { PlayerAnimationType.idle,3.667f },
            { PlayerAnimationType.walk,0.60f },

            { PlayerAnimationType.skill,1.433f },
            { PlayerAnimationType.skill2,0.8f }
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

