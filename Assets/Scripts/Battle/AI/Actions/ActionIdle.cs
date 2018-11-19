using BTree;

public class ActionIdle :BTAction  {

    public ActionIdle() : base() { }
    public ActionIdle(BTNode _parent)
            : base(_parent) { }

    protected override void OnEnter(BTInput _input)
    {
        base.OnEnter(_input);
    }

    protected override BTResult OnExecute(ref BTInput _input)
    {
        AIInput input = _input as AIInput;
        if (input != null)
        {
           return input.DoAction(AIActionType.Idle);
        }


        return BTResult.Success;
    }
}
