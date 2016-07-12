using UnityEngine;
using System.Collections;

public class Or : Bool
{
    public override bool GetResult()
    {
        if (referencedBy.Count >= 1)
        {
            Bool temp = referencedBy[0] as Bool;
            bool s = temp.GetResult();
            for (int i = 1; i < referencedBy.Count; i++)
            {
                temp = referencedBy[i] as Bool;
                s = s || temp.GetResult();
            }
            return s;
        }
        else
            return false;
    }
    public override string Type()
    {
        return "Or";
    }
    public override void Attach()
    {
        if (attachNode.BaseType() == "Bool")
            base.Attach();
        else
            Debug.LogError("Wrong type (should be Bool)");
    }
    public override void DrawNode(int id = 0)
    {
        value = GetResult();
        if (referencedBy.Count >= 1)
        {
            GUILayout.Label("Node 1: " + referencedBy[0].name);
            for (int i = 1; i < referencedBy.Count; i++)
            {
                GUILayout.Label("Node " + (i + 1).ToString() + ": " + referencedBy[i].name);
            }
        }
        base.DrawNode(id);
    }
}
