using System;

public class QuestionNode : INode
{
    private Func<bool> question;
    private INode nodeTrue;
    private INode nodeFalse;

    public QuestionNode(Func<bool> question, INode nodeTrue, INode nodeFalse)
    {
        this.question = question;
        this.nodeTrue = nodeTrue;
        this.nodeFalse = nodeFalse;
    }

    public void Execute()
    {
        if (question())
        {
            nodeTrue.Execute();
        }
        else
        {
            nodeFalse.Execute();
        }
    }
}
