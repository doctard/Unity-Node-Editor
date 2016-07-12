using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EqualTo : Bool
{
    //get the equation result
    //check if we're checking floats or bools
    public override bool GetResult()
    {
        if (referencedBy.Count == 1)
        {
            return false;
        }
        else
        if (referencedBy.Count == 2)
        {
            if (ConnectType == "Float")
            {
                Float temp1 = referencedBy[0] as Float;
                Float temp2 = referencedBy[1] as Float;
                return temp1.GetResult() == temp2.GetResult();
            }
            else
            if (ConnectType == "Bool")
            {
                Bool temp1 = referencedBy[0] as Bool;
                Bool temp2 = referencedBy[1] as Bool;
                return temp1.GetResult() == temp2.GetResult();
            }
            else
                return false;
        }
        else
        {
            ConnectType = "";
            return false;
        }
    }
    public override string Type()
    {
        return "Equal to";
    }
    string ConnectType = "";
    //attach one element, but the next one has to be the same type
    public override void Attach()
    {
        if (referencedBy.Count == 0)
        {
            ConnectType = attachNode.BaseType();
            base.Attach();
        }
        else
        if (referencedBy.Count == 1 && attachNode.BaseType() == ConnectType)
        {
            base.Attach();
        }
        else
        {
            Debug.LogError("There can only be two connections, or you've connected an incorrect type (should have been a " + ConnectType + ")");
        }
    }
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        EqualTo temp = (EqualTo)other;
        temp.ConnectType = ConnectType;
    }
#if UNITY_EDITOR
    public override void DrawNode(int id = 0)
    {
        value = GetResult();
        base.DrawNode(id);
    }
    //always print that there SHOULD be two connected nodes
    protected override void PrintValue()
    {
        if (referencedBy.Count == 1)
        {
            GUILayout.Label("Node 1: " + referencedBy[0].name);
            GUILayout.Label("Node 2: null");
        }
        else
        if (referencedBy.Count == 2)
        {
            GUILayout.Label("Node 1: " + referencedBy[0].name);
            GUILayout.Label("Node 2: " + referencedBy[1].name);
        }
        else
        {
            GUILayout.Label("Node 1: null");
            GUILayout.Label("Node 2: null");
        }
        EditorGUILayout.LabelField("Value " + value.ToString());
    }
#endif
}
