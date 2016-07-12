using UnityEngine;
using System.Collections;
using UnityEditor;
public class Addition : Float
{
    public override float GetResult()
    {
        float s = 0;
        for (int i = 0; i < referencedBy.Count; i++)
        {
            Float temp = referencedBy[i] as Float;
            s += temp.GetResult();
        }
        return s;
    }
    public override string Type()
    {
        return "Addition";
    }
    public override void Attach()
    {
        if (attachNode.BaseType() == "Float")
            base.Attach();
        else
            Debug.LogError("Wrong type (should be Float)");
    }
    public override void DrawNode(int id = 0)
    {
        for (int i = 0; i < referencedBy.Count; i++)
        {
            GUILayout.Label("Node " + (i + 1).ToString() + ": " + referencedBy[i].name);
        }
        value = GetResult();
        base.DrawNode(id);
    }
}
