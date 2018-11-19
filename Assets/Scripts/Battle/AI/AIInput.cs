using System;
using System.Collections.Generic;
using BTree;

public interface AIInput : BTInput
{
    BTResult DoAction(AIActionType type);

    bool CheckCondition(AIConditionType type);
}