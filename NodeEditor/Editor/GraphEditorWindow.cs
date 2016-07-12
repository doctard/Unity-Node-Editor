using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using VNKit;
using System.Text.RegularExpressions;
public class GraphEditorWindow : EditorWindow
{
    protected virtual void SetupNodeTypes()
    {
        NodeTypes = new List<string>();
        NodeTypes.Add("Float");
        NodeTypes.Add("Bool");
        NodeTypes.Add("And");
        NodeTypes.Add("Or");
        NodeTypes.Add("Multiply");
        NodeTypes.Add("Addition");
        NodeTypes.Add("GreaterThan");
        NodeTypes.Add("LessThan");
        NodeTypes.Add("EqualTo");
        NodeTypes.Add("If");
    }
    protected List<string> NodeTypes = new List<string>();
    string NodeSearch = "";
    List<string> SearchResults = new List<string>();
    void FindNodes()
    {
        SearchResults = new List<string>();
        for (int i = 0; i < NodeTypes.Count; i++)
        {

            string temp = NodeTypes[i];
            if (temp.ToLower().Contains(NodeSearch.ToLower()))
            {
                SearchResults.Add(temp);
            }
        }
    }
    void DisplayNodeResults()
    {
        RightClickMenu.height = 20 + SearchResults.Count * 20;
        Rect buttonrect = new Rect(mousepos.x, mousepos.y + 30, 100, 20);
        for (int i = 0; i < SearchResults.Count; i++)
        {
            string temp = SearchResults[i];
            if (GUI.Button(buttonrect, temp))
            {
                Node node = CreateInstance(temp) as Node;
                AddNode(node);
            }
            buttonrect.y += 20;
        }
    }
    static GraphEditorWindow window;
    protected Event e;
    protected bool showPosition;
    protected Vector2 mousepos;
    protected Rect RightClickMenu = new Rect(0, 0, 130, 20);
    protected ScriptTemplate canvas;
    [MenuItem("Window/Node Editor")]
    static void Init()
    {
        window = (GraphEditorWindow)EditorWindow.GetWindow(typeof(GraphEditorWindow));
    }
    string StartNode = "";
    protected virtual void OnGUI()
    {
        SetupNodeTypes();
        DrawNodes();
        Rect buttonRect = new Rect(position.width - 120, 10, 100, 20);
        if (canvas == null)
        {
            CreateNewCanvas();
        }
        if (GUI.Button(buttonRect, "new canvas"))
        {
            CreateNewCanvas();
        }
        buttonRect.y += 25;
        if (GUI.Button(buttonRect, "save canvas"))
        {
            SaveCanvas();
        }
        buttonRect.y += 25;
        if (GUI.Button(buttonRect, "load canvas"))
        {
            LoadCanvas();
        }
        buttonRect.y += 25;
        canvas.name = GUI.TextField(buttonRect, canvas.name);
        buttonRect.y += 25;
        StartNode = GUI.TextField(buttonRect, StartNode);
        StartNode = Regex.Replace(StartNode, @"[^0-9]", "");
        if (StartNode != "")
        {
            canvas.StartNode = int.Parse(StartNode);
        }
        else
        {
            canvas.StartNode = 0;
        }
    }
    protected virtual void CreateNewCanvas()
    {
        ClearMemory();
        canvas = CreateInstance<ScriptTemplate>();
    }
    protected void ClearMemory()
    {
        if (canvas != null)
        {
            if (canvas.nodes.Count > 0)
            {
                for (int i = canvas.nodes.Count - 1; i >= 0; i--)
                {
                    DestroyImmediate(canvas.nodes[i]);
                }
            }
            DestroyImmediate(canvas);
        }
    }
    protected string assetPathAndName;
    protected void CopyList(ScriptTemplate a, ScriptTemplate b)
    {

        a.name = b.name;
        a.nodes = new List<Node>();
        a.StartNode = b.StartNode;
        for (int i = 0; i < b.nodes.Count; i++)
        {
            Node temp = CreateInstance(b.nodes[i].GetType()) as Node;
            temp.name = i.ToString() + "-" + b.nodes[i].Type();
            a.nodes.Add(temp);
            a.nodes[i].ID = i;
            b.nodes[i].ID = i;
            a.nodes[i].parent = a;
        }
        for (int i = 0; i < b.nodes.Count; i++)
        {
            b.nodes[i].CopyTo(a.nodes[i]);
        }
    }
    protected virtual void LoadCanvas()
    {
        assetPathAndName = "Assets/NodeEditor/Resources/" + canvas.name + ".asset";
        ScriptTemplate newc = AssetDatabase.LoadAssetAtPath<ScriptTemplate>(assetPathAndName);
        //reset the canvas, since we don't want it to reference a file in the projects, but to be it's own thing
        CreateNewCanvas();
        CopyList(canvas, newc);
    }
    protected virtual void SaveCanvas()
    {
        assetPathAndName = "Assets/NodeEditor/Resources/" + canvas.name + ".asset";
        ScriptTemplate outPut = AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(ScriptTemplate)) as ScriptTemplate;
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
            outPut = CreateInstance<ScriptTemplate>();
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
    void HandleRightClickEvent()
    {
        e = Event.current;

        if (e.type == EventType.MouseDown)
        {
            if (e.button == 1)
            {
                showPosition = true;
                mousepos = e.mousePosition;
                RightClickMenu = new Rect(mousepos.x, mousepos.y, 100, 110);
                Repaint();
                NodeSearch = "";
            }
            else
            {
                if (!RightClickMenu.Contains(e.mousePosition))
                {
                    showPosition = false;
                    Repaint();
                }
            }
        }


        if (!RightClickMenu.Contains(e.mousePosition))
        {
            showPosition = false;
        }

        if (showPosition == true)
        {
            OnRightClick();
        }
    }
    void DrawNodes()
    {
        testRect = new Rect(5, 5, position.width - 140, position.height - 5);
        GUILayout.BeginVertical();
        {
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(testRect.height), GUILayout.MaxWidth(position.width - 140));
            EditorZoomArea.Begin(zoom, new Rect(testRect.x + 5, testRect.y, position.width - 150, testRect.height - 10));
            BeginWindows();
            HandleRightClickEvent();
            HandleEvents();
            EndWindows();
            EditorZoomArea.End();
        }
        GUILayout.EndVertical();
    }
    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 3);
    }
    protected void OnRightClick()
    {
        Rect buttonrect = new Rect(mousepos.x, mousepos.y + 10, 100, 20);
        GUI.SetNextControlName("searchfield");
        NodeSearch = GUI.TextField(buttonrect, NodeSearch);
        GUI.FocusControl("searchfield");
        if (NodeSearch != "")
        {

            FindNodes();
            if (SearchResults.Count > 0)
            {
                DisplayNodeResults();
            }
        }

    }
    protected void AddNode(Node temp)
    {
        temp.parent = canvas;
        temp.pos = new Rect(mousepos.x - 120, mousepos.y, 120, 100);
        temp.name = canvas.nodes.Count.ToString() + "-" + temp.Type();
        canvas.nodes.Add(temp);
        showPosition = false;
        Repaint();

    }
    //gets called everytime the GUI gets repainted
    Vector2 zoomOrigin = Vector2.zero;
    Vector2 zoomDelta = Vector2.zero;
    //the zoom scale
    float zoom = 1;
    Rect testRect = new Rect(5, 5, 400, 680);
    void HandleEvents()
    {
        Vector2 mousePos = Event.current.mousePosition;

        if (Event.current.type == EventType.ScrollWheel)
        {
            Vector2 delta = Event.current.delta;
            Vector2 zoomedMousePos = (mousePos - testRect.min) / zoom + zoomOrigin;

            float oldZoomScale = zoom;

            float zoomDelta = -delta.y / 150.0f;
            zoom += zoomDelta * 4;
            zoom = Mathf.Clamp(zoom, 0.1f, 2.0f);

            zoomOrigin += (zoomedMousePos - zoomOrigin) - (oldZoomScale / zoom) * (zoomedMousePos - zoomOrigin);

            Event.current.Use();
        }

        if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
        {
            zoomDelta = Event.current.delta;
            zoomDelta /= zoom * 2;
            zoomOrigin += zoomDelta;
            Event.current.Use();
        }
        else
        {
            zoomDelta = Vector2.zero;
        }
        if (canvas != null)
        {
            for (int i = 0; i < canvas.nodes.Count; i++)
            {
                Node temp = canvas.nodes[i];
                temp.ID = i;
                temp.pos = GUILayout.Window(i, new Rect(temp.pos.x + zoomDelta.x, temp.pos.y + zoomDelta.y, temp.pos.width, temp.pos.height), temp.DrawNode, i.ToString() + "-" + temp.Type());
                for (int j = 0; j < temp.connectedTo.Count; j++)
                {
                    DrawNodeCurve(temp.pos, temp.connectedTo[j].pos);
                }
            }
        }
    }
}
