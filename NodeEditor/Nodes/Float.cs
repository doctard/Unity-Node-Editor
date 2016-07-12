using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Float : Node
{
    //the value for the float that you'll see in the editor window
    public float value;
    //the function to get the value.
    //it get's overwriten by the other nodes that inherit from Float
    //we need it because getting results from a branch of nodes needs to be done via recursion
    //whenever a node needs to get some value from a float/bool node, it will call GetResult(), so that each unique kind of node
    //can get it's correct result, rather than just using whatever was left in 'value'
    public virtual float GetResult()
    {
        return value;
    }
    public virtual float GetResult(AIReader reader)
    {
        return value;
    }
    //we use this to show the type of the node in the editor. Every unique node has it's own version
    public override string Type() { return "Float"; }
    //we use this to check what kind of node this is, mainly for inherited ones.
    //Float/Multiplication/Addition are Float
    //Bool/And/Or/EqualTo/GreaterThan/LessThan are Bool
    //If is Branch (will add Switch later)
    public override string BaseType() { return "Float"; }
    //editor function that draws the node
#if UNITY_EDITOR
    public override void DrawNode(int id = 0)
    {
        PrintValue();
        base.DrawNode(id);
    }
    protected virtual void PrintValue()
    {
        value = EditorGUILayout.FloatField("Value ", value);
    }
#endif
    //function used for when we're saving/loading the nodes, since we want new nodes, not references
    //we do this because it's very easy to accidentally ruin all the data in an already existing canvas this way
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        Float temp = (Float)other;
        temp.value = value;
    }
}
