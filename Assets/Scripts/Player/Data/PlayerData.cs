using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Base Variables")]
    public int maxHp = 100;
    public int maxAmmo = 10;

    public float attackDuration = 0.2f;

    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;

    [Header("Crouch State")]
    public float standColliderHeight = 1.25f;
    public float crouchColliderHeight = 0.6f;
    //Can add crouch movement velocity if the player is allowed to move on crouch.

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
