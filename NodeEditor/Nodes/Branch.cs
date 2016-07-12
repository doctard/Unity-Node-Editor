using UnityEngine;
using System.Collections;

public class Branch : Node
{

    public override string Type()
    {
        return "Branch";
    }
    public override string BaseType()
    {
        return "Branch";
    }
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        if (value != null)
        {
            Branch temp = (Branch)other;
            temp.value = temp.parent.nodes[value.ID];
        }
    }
    public Node value;
    public virtual Node GetResult()
    {
        return value;
    }
}
