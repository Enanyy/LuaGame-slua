using BTree;

public class AIInputComponent : HeroComponent, AIInput
{
    private BTInput mInput;

    public AIInputComponent()
    {
        mInput = this;
    }

    public virtual bool CheckCondition(AIConditionType type)
    {
        bool result = false;
        switch (type)
        {
            case AIConditionType.HasTarget: result = entity.HasTarget(); break;
            case AIConditionType.IsDead: result = entity.IsDead(); break;
            case AIConditionType.HasAttack: result = entity.HasAttack(); break;
            case AIConditionType.IsAttacking: result = entity.IsAttacking(); break;
            case AIConditionType.IsAttackRange: result = entity.IsAttackRange(); break;
        }

        return result;
    }

    public virtual BTResult DoAction(AIActionType type)
    {
        switch (type)
        {
            case AIActionType.Idle:
                {
                   entity.PlayAction(ActionEnum.free);

                }
                break;
            case AIActionType.Attack:
                {
                    var skill = entity.GetReleasableSkill( SkillType.Automatic);

                    entity.ReleaseSkill(skill);
                }
                break;
            case AIActionType.Die:
                {
                    entity.data.target = 0;
                    entity.PlayAction(ActionEnum.death);
                }
                break;
            case AIActionType.FindTarget:
                {
                    entity.FindTarget();

                }
                break;
        }
        return BTResult.Success;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(BattleManager.instance.logic!=null)
        {
            BattleManager.instance.logic.Tick(entity.data.ai, ref mInput);
        }
    }
}

