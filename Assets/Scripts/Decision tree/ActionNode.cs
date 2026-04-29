using System;

public class ActionNode : INode
{
    private Action action;

    public ActionNode(Action action)
    {
        this.action = action;
    }

    void INode.Execute()
    {
        action();
    }
}
