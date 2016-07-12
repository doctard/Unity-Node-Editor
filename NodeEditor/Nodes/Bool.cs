using UnityEngine;
using System.Collections;
using UnityEditor;
public class Bool : Node
{

    public bool value;
    public virtual bool GetResult()
    {
        return value;
    }
    public override string Type() { return "Bool"; }
    public override string BaseType() { return "Bool"; }
    public override void DrawNode(int id = 0)
    {
        value = EditorGUILayout.Toggle("Value ", value);
        base.DrawNode(id);
    }
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        Bool temp = (Bool)other;
        temp.value = value;
    }
}
