﻿using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class And : Bool
{
    //get the && of all the connected nodes. hopefully this is the correct formula
    public override bool GetResult()
    {
        if (referencedBy.Count >= 1)
        {
            Bool temp = referencedBy[0] as Bool;
            bool s = temp.GetResult(); ;
            for (int i = 1; i < referencedBy.Count; i++)
            {
                temp = referencedBy[i] as Bool;
                s = s && temp.GetResult();
            }
            return s;
        }
        else
            return false;
    }
    public override string Type()
    {
        return "And";
    }
    //make sure we're only attaching bools
    public override void Attach()
    {
        if (attachNode.BaseType() == "Bool")
            base.Attach();
        else
            Debug.LogError("Wrong type (should be Bool)");
    }
#if UNITY_EDITOR
    public override void DrawNode(int id = 0)
    {
        value = GetResult();
        if (referencedBy.Count >= 1)
        {
            GUILayout.Label("Node 1: " + referencedBy[0].name);
            for (int i = 1; i < referencedBy.Count; i++)
            {
                GUILayout.Label("Node " + (i + 1).ToString() + ": " + referencedBy[i].name);
            }
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
