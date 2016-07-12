using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class Node : ScriptableObject
{
    [HideInInspector]
    public ScriptTemplate parent;
    public List<Node> connectedTo = new List<Node>(), referencedBy = new List<Node>();
    public int ID;
    public Rect pos = new Rect(100, 100, 120, 100);
    public virtual string Type() { return "node"; }
    public virtual string BaseType() { return "node"; }
    static bool attaching = false;
    public static Node attachNode = null;
    public virtual void Attach()
    {
        attachNode.connectedTo.Add(this);
        referencedBy.Add(attachNode);
    }
    public virtual void DrawNode(int id = 0)
    {
        ID = id;
        if (GUILayout.Button("Attach"))
        {
            if (attaching == false)
            {
                attaching = true;
                attachNode = this;
            }
            else
            {
                if (attachNode != this && !attachNode.connectedTo.Contains(this))
                {
                    if (attachNode.Type() == "If" && attachNode.connectedTo.Count == 2)
                    {
                        Debug.LogError("Too many connections");
                    }
                    else
                        Attach();
                }
                attaching = false;
                attachNode = null;

            }
        }
        if (GUILayout.Button("Detach"))
        {
            Detach();
        }
        if (GUILayout.Button("Delete"))
        {
            Detach();
            Delete();
        }
        GUI.DragWindow();
    }
    protected void Detach()
    {
        for (int i = connectedTo.Count - 1; i >= 0; i--)
        {
            connectedTo[i].referencedBy.Remove(this);
            connectedTo.RemoveAt(i);
        }
        for (int i = referencedBy.Count - 1; i >= 0; i--)
        {
            referencedBy[i].connectedTo.Remove(this);
            referencedBy.RemoveAt(i);
        }
    }
    public virtual void CopyTo(Node other)
    {
        other.pos = pos;
        other.name = ID.ToString() + "-" + Type();
        for (int j = 0; j < connectedTo.Count; j++)
        {
            other.connectedTo.Add(other.parent.nodes[connectedTo[j].ID]);
        }
        for (int j = 0; j < referencedBy.Count; j++)
        {
            other.referencedBy.Add(other.parent.nodes[referencedBy[j].ID]);
        }
    }
    void Delete()
    {
        parent.nodes.Remove(this);
        DestroyImmediate(this);
    }
}
