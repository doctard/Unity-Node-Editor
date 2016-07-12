using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//check whether we're in the editor or not
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Node : ScriptableObject
{
    //the canvas
    [HideInInspector]
    public ScriptTemplate parent;
    //lists of nodes that we're connected to
    public List<Node> connectedTo = new List<Node>(), referencedBy = new List<Node>();
    //id in the canvas list
    public int ID;
    //position in the window
    public Rect pos = new Rect(100, 100, 120, 100);
    //what kind of node this is
    public virtual string Type() { return "node"; }
    //what kind of base this node is (usually node/float/bool/branch
    public virtual string BaseType() { return "node"; }
    //are we attaching a node to another node
    static bool attaching = false;
    //the node we're attaching to this node
    public static Node attachNode = null;
    //the attach function
    public virtual void Attach()
    {
        attachNode.connectedTo.Add(this);
        referencedBy.Add(attachNode);
        //if there's a branch that isn't an If, tell it that this is it's next step
        if (attachNode.BaseType() == "Branch" && attachNode.Type() != "If")
        {
            Branch temp = attachNode as Branch;
            temp.value = this;
        }
    }
    //draw the node in the editor
    //also handle when to attach/detach/delete
#if UNITY_EDITOR
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
#endif
    //detach function
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
    //copy function, used when saving/loading canvases since we want new nodes, not references to existing ones
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
    //delete the node to free up space
    void Delete()
    {
        parent.nodes.Remove(this);
        DestroyImmediate(this);
    }
}
