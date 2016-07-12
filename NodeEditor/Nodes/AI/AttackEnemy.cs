using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AttackEnemy : Branch
{

    public List<string> Attacks;
    public override string Type() { return "Attack enemy"; }
}
