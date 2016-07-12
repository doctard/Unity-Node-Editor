using UnityEngine;
using System.Collections;

public class If : Branch
{
    public override Node GetResult()
    {
        if (referencedBy.Count == 1 && connectedTo.Count >= 1)
        {
            Bool temp = referencedBy[0] as Bool;
            if (temp.GetResult() == true)
            {
                return connectedTo[0];
            }
            else
            if (connectedTo.Count == 2)
            {
                return connectedTo[1];
            }
            else
                return null;
        }
        else
        {
            return null;
        }
    }
    public override string Type()
    {
        return "If";
    }
    public override void Attach()
    {
        if (attachNode.BaseType() == "Bool" && referencedBy.Count == 0)
            base.Attach();
        else
            Debug.LogError("Wrong type (should be Float) or you have more than one Bool connected");
    }
    public override void DrawNode(int id = 0)
    {
        value = GetResult();
        if (referencedBy.Count == 1)
        {
            GUILayout.Label("Condition node: " + referencedBy[0].name);
        }
        else
        {
            GUILayout.Label("Condition node: null");
        }
        if (connectedTo.Count == 0)
        {
            GUILayout.Label("Answer 1: null");
            GUILayout.Label("Answer 2: null");
        }
        else
        if (connectedTo.Count == 1)
        {
            GUILayout.Label("Answer 1: " + connectedTo[0].name);
            GUILayout.Label("Answer 2: null");
        }
        else
        {
            GUILayout.Label("Answer 1: " + connectedTo[0].name);
            GUILayout.Label("Answer 2: " + connectedTo[1].name);
        }
        if (referencedBy.Count == 1 && connectedTo.Count >= 1)
        {
            Bool temp = referencedBy[0] as Bool;
            if (temp.GetResult() == true)
            {
                GUILayout.Label("Result: " + value.name);
            }
            else
            if (connectedTo.Count == 2)
            {
                GUILayout.Label("Result: " + value.name);
            }
        }
        else
        {
            GUILayout.Label("Result: null");
        }

        base.DrawNode(id);
    }

}
