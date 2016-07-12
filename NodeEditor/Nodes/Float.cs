using UnityEngine;
using System.Collections;
using UnityEditor;
public class Float : Node
{
    public float value;
    public virtual float GetResult()
    {
        return value;
    }
    public override string Type() { return "Float"; }
    public override string BaseType() { return "Float"; }
    public override void DrawNode(int id = 0)
    {
        value = EditorGUILayout.FloatField("Value ", value);
        base.DrawNode(id);
    }
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        Float temp = (Float)other;
        temp.value = value;
    }
}
