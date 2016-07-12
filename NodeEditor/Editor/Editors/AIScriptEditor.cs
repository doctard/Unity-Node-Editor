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
