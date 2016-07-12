using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Multiply : Float
{
    public override float GetResult()
    {
        float p = 1;
        for (int i = 0; i < referencedBy.Count; i++)
        {

            Float temp = referencedBy[i] as Float;
            p *= temp.GetResult();
        }
        return p;
    }
    public override string Type()
    {
        return "Multiply";
    }
    public override void Attach()
    {
        if (attachNode.BaseType() == "Float")
            base.Attach();
        else
            Debug.LogError("Wrong type (should be Float)");
    }
#if UNITY_EDITOR

    public override void DrawNode(int id = 0)
    {
        for (int i = 0; i < referencedBy.Count; i++)
        {
            GUILayout.Label("Node " + (i + 1).ToString() + ": " + referencedBy[i].name);
        }
        base.DrawNode(id);
    }
    protected override void PrintValue()
    {
        value = GetResult();
        EditorGUILayout.LabelField("Value " + value.ToString());
    }

#endif
}
