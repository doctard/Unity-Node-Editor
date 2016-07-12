using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class If : Branch
{
    //the result
    //if there's only one possible answer, the result is either it, or null
    //otherwise if it's not it, and there is another answer, it's the other answer
    public Bool condition;
    public override Node GetResult()
    {
        if (condition != null && connectedTo.Count >= 1)
        {
            if (condition.GetResult() == true)
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
    public override void CopyTo(Node other)
    {
        If temp = other as If;
        temp.condition = temp.parent.nodes[condition.ID] as Bool;
        base.CopyTo(other);
    }
    //only attach bools, and 
    public override void Attach()
    {
        if (attachNode.BaseType() == "Bool")
        {
            if (condition != null)
            {
                condition.connectedTo.Remove(this);
                referencedBy.Remove(condition);
            }
            condition = attachNode as Bool;
            base.Attach();
        }
        else
        if (attachNode.BaseType() == "Branch")
        {
            base.Attach();
        }
        else
            Debug.LogError("Wrong type (should be Float) or you have more than one Bool connected");
    }
#if UNITY_EDITOR
    public override void DrawNode(int id = 0)
    {
        value = GetResult();
        if (condition != null)
        {
            GUILayout.Label("Condition node: " + condition.name);
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
        if (condition != null && connectedTo.Count >= 1)
        {
            if (condition.GetResult() == true)
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
#endif
}
