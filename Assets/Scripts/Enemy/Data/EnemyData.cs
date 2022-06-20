using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class EnemyData : ScriptableObject
{
    [Header("Base Variables")]
    public int maxHp = 100;

    public float attackDuration = 0.2f;

    [Header("Other Variables")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public float runSpeed;

    [Header("Gizmos - Raycast references (Debug)")]
    public float rayCastLength;
    public float rayCastOffsetY;
    public float rayCastOffsetX;

    [Header("Ledge Check (Debug)")]
    public float ledgeOffsetX;
    public float ledgeOffsetY;
    public float ledgeCheckLength;

    [Header("ShootTrace (Debug)")]
    public float frontTraceDistance;
    public float backTraceDistance;
}
