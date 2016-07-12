using UnityEngine;
using System.Collections;

public class GreaterThan : Bool
{
    public override bool GetResult()
    {
        if (referencedBy.Count == 1)
        {
            return false;
        }
        else
       if (referencedBy.Count == 2)
        {
            Float temp1 = referencedBy[0] as Float;
            Float temp2 = referencedBy[1] as Float;
            return temp1.GetResult() > temp2.GetResult();
        }
        else
        {
            return false;
        }
    }
    public override string Type()
    {
        return "Greater than";
    }
    public override void Attach()
    {
        if (referencedBy.Count < 2 && attachNode.BaseType() == "Float")
        {
            base.Attach();
        }
        else
        {
            Debug.LogError("There can only be two connections, or you've connected an incorrect type (should have been a float)");
        }
    }
    public override void DrawNode(int id = 0)
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
        value = GetResult();
        base.DrawNode(id);
    }
}
