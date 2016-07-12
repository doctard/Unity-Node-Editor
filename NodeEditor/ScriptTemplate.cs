using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ScriptTemplate : ScriptableObject
{
    //the node from which the reader is supposed to start reading this script
    public int StartNode;
    //the list of nodes
    public List<Node> nodes = new List<Node>();
}
