using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Addition : Float
{
    //get the sum of all the connected nodes
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
    //make sure we're only attaching Floats
    public override void Attach()
    {
        if (attachNode.BaseType() == "Float")
            base.Attach();
        else
            Debug.LogError("Wrong type (should be Float)");
    }
#if UNITY_EDITOR
    //print the name of each connected node
    public override void DrawNode(int id = 0)
    {
        for (int i = 0; i < referencedBy.Count; i++)
        {
            GUILayout.Label("Node " + (i + 1).ToString() + ": " + referencedBy[i].name);
        }
        base.DrawNode(id);
    }
    //use a label instead of an Input field
    protected override void PrintValue()
    {
        value = GetResult();
        EditorGUILayout.LabelField("Value " + value.ToString());
    }
#endif
}
