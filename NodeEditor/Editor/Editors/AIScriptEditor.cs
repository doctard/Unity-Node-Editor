using UnityEngine;
using System.Collections;
using UnityEditor;
public class AIScriptEditor : GraphEditorWindow
{
    static AIScriptEditor window;
    protected override void SetupNodeTypes()
    {
        base.SetupNodeTypes();
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
