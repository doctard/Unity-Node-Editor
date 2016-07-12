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
            NodeTypes.Add("DistanceToEnemy");
            NodeTypes.Add("StopMoving");
        }
        [MenuItem("Window/AI Node Editor")]
        static void Init()
        {
            window = (AIScriptEditor)EditorWindow.GetWindow(typeof(AIScriptEditor));
        }
        protected override void CreateNewCanvas()
        {
            ClearMemory();
            canvas = CreateInstance<AIScript>();
        }
        protected override void LoadCanvas()
        {
            assetPathAndName = "Assets/NodeEditor/Resources/" + canvas.name + ".asset";
            AIScript newc = AssetDatabase.LoadAssetAtPath<AIScript>(assetPathAndName);
            //reset the canvas, since we don't want it to reference a file in the projects, but to be it's own thing
            CreateNewCanvas();
            CopyList(canvas, newc);
        }
        protected override void SaveCanvas()
        {
            assetPathAndName = "Assets/NodeEditor/Resources/" + canvas.name + ".asset";
            AIScript outPut = AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(AIScript)) as AIScript;
            if (outPut != null)
            {
                for (int i = 0; i < outPut.nodes.Count; i++)
                {
                    DestroyImmediate(outPut.nodes[i], true);
                }
                CopyList(outPut, canvas);
                for (int i = 0; i < outPut.nodes.Count; i++)
                {
                    AssetDatabase.AddObjectToAsset(outPut.nodes[i], assetPathAndName);
                }
            }
            else
            {
                outPut = CreateInstance<AIScript>();
                CopyList(outPut, canvas);
                AssetDatabase.CreateAsset(outPut, assetPathAndName);
                for (int i = 0; i < outPut.nodes.Count; i++)
                {
                    AssetDatabase.AddObjectToAsset(outPut.nodes[i], assetPathAndName);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
