Unity Node Editor framework
a simple framework for creating node editors in unity (similar to mecanim)
currently only has some nodes for math/bool related functions:
Float - has a float value
Bool - has a bool value
Addition - adds multiple float nodes
Multiplication - multiplies multiple float nodes
And - '&&' for multiple bool nodes
Or - '||' for multiple bool nodes
EqualTo - compares two floats or bools
If - checks if a bool is true or not, and returns one of two possible nodes as the next correct step

In order to create and save a canvas in the framework do this:
Open up the framework window from Window/Node Editor
Right click somewhere in the rectangle in the node editor window
Type in what node you want to create from the nodes i just listed
Click on whichever result you want
Set any values you want on that node
If you want to connect nodes, press Attach on one node and Attach on another. If you want to cancel the attachment, just press the first node a second time
Type a name in the text field on the right side of the window
Press Save
To load a canvas, type a name and press Load
If you want to make your own editor using the framework, first of all you'd need to edit the SetupNodeTypes function, to include the nodes unique to your editor
 
using UnityEngine;
using System.Collections;
using UnityEditor;
//inherits the default node editor
public class AIScriptEditor : GraphEditorWindow
{
    static AIScriptEditor window;
    
    protected override void SetupNodeTypes()
    {
        //adds the default nodes (floats, bools etc)
        base.SetupNodeTypes();
        //adds the nodes that are unique to this editor
        //make sure to get the names the same as they are in the scripts, since we use them to create instances of the nodes
        //CreateInstance(NodeTypeName) as Node;
        NodeTypes.Add("DoNothing");
        NodeTypes.Add("WalkTowardsEnemy");
        NodeTypes.Add("AttackEnemy");
    }
    [MenuItem("Window/AI Node Editor")]
    static void Init()
    {
        window = (AIScriptEditor)EditorWindow.GetWindow(typeof(AIScriptEditor));
    }
}


After that you'd have to actually create said nodes as C# scripts. They need to inherit from the Node class, or anything else that inherits from it(Multiplication/Addition inherit from Float, Float inherits from Node)
 
 using UnityEngine;
using System.Collections;
using UnityEditor;
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
    //we use this to show the type of the node in the editor. Every unique node has it's own version
    public override string Type() { return "Float"; }
    //we use this to check what kind of node this is, mainly for inherited ones.
    //Float/Multiplication/Addition are Float
    //Bool/And/Or/EqualTo/GreaterThan/LessThan are Bool
    //If is Branch (will add Switch later)
    public override string BaseType() { return "Float"; }
    //editor function that draws the node
    public override void DrawNode(int id = 0)
    {
        value = EditorGUILayout.FloatField("Value ", value);
        base.DrawNode(id);
    }
    //function used for when we're saving/loading the nodes, since we want new nodes, not references
    //we do this because it's very easy to accidentally ruin all the data in an already existing canvas this way
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        Float temp = (Float)other;
        temp.value = value;
    }
}



