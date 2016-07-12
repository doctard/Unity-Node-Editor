﻿using UnityEngine;
using System.Collections;

public class EqualTo : Bool
{
    public override bool GetResult()
    {
        if (referencedBy.Count == 1)
        {
            GUILayout.Label("Node 1: " + referencedBy[0].name);
            GUILayout.Label("Node 2: null");
            return false;
        }
        else
        if (referencedBy.Count == 2)
        {
            GUILayout.Label("Node 1: " + referencedBy[0].name);
            GUILayout.Label("Node 2: " + referencedBy[1].name);
            if (ConnectType == "Float")
            {
                Float temp1 = referencedBy[0] as Float;
                Float temp2 = referencedBy[1] as Float;
                return temp1.GetResult() == temp2.GetResult();
            }
            else
            if (ConnectType == "Bool")
            {
                Bool temp1 = referencedBy[0] as Bool;
                Bool temp2 = referencedBy[1] as Bool;
                return temp1.GetResult() == temp2.GetResult();
            }
            else
                return false;
        }
        else
        {
            ConnectType = "";
            GUILayout.Label("Node 1: null");
            GUILayout.Label("Node 2: null");
            return false;
        }
    }
    public override string Type()
    {
        return "Equal to";
    }
    string ConnectType = "";
    public override void Attach()
    {
        if (referencedBy.Count == 0)
        {
            ConnectType = attachNode.BaseType();
            base.Attach();
        }
        else
        if (referencedBy.Count == 1 && attachNode.BaseType() == ConnectType)
        {
            base.Attach();
        }
        else
        {
            Debug.LogError("There can only be two connections, or you've connected an incorrect type (should have been a " + ConnectType + ")");
        }
    }
    public override void DrawNode(int id = 0)
    {
        value = GetResult();
        base.DrawNode(id);
    }
    public override void CopyTo(Node other)
    {
        base.CopyTo(other);
        EqualTo temp = (EqualTo)other;
        temp.ConnectType = ConnectType;
    }
}