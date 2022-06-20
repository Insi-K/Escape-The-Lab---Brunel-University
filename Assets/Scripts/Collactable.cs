using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collactable : MonoBehaviour
{
    public enum CollectableType { health, ammo, key, explosive};

    public CollectableType type;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            switch(type)
            {
                case CollectableType.health:
                    player.CurePlayer(30);
                    break;
                case CollectableType.ammo:
                    player.ReloadAmmo(7);
                    break;
                case CollectableType.key:
                    player.ObtainKey(); //Could make the player prevent from collecting more keys until used current one;
                    break;
                case CollectableType.explosive:
                    player.addGrenade();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
