using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DistanceToEnemy : Float
{
    public override string Type()
    {
        return "Distance to Enemy";
    }
    public override float GetResult()
    {
        AIScript par = parent as AIScript;
        if (par.reader != null)
            return Vector3.Distance(par.reader.controls.mainBody.position, par.reader.enemy.mainBody.position);
        else
            return 0;
    }
#if UNITY_EDITOR
    protected override void PrintValue()
    {
        value = GetResult();
        EditorGUILayout.LabelField("Value: Determined ingame");
    }
#endif
}
