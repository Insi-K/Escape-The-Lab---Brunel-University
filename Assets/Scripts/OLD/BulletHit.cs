using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public enum collisionTarget { enemy, player}

    public collisionTarget collidesWith;

    public void SetCollisionTarget(int target)
    {
        switch(target)
        {
            case 1:
                collidesWith = collisionTarget.enemy;
                break;
            case 2:
                collidesWith = collisionTarget.player;
                break;
        }
    }
}
