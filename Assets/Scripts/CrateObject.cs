using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateObject : MonoBehaviour
{
    //Reference to ammo and health prefabs
    //public enum objectToSpawn => content
    public GameObject ammoPrefab;
    public GameObject healthPrefab;
    public GameObject grenadePrefab;

    public GameObject BoxDestroyedParticle;

    public enum objectToSpawn { ammo, health, explosive, none};
    public objectToSpawn content;

    public void OpenCrate()
    {
        switch(content)
        {
            case objectToSpawn.ammo:
                Instantiate(ammoPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
            case objectToSpawn.health:
                Instantiate(healthPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
            case objectToSpawn.explosive:
                Instantiate(grenadePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
        }
        Instantiate(BoxDestroyedParticle, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        gameObject.SetActive(false);
    }

    //When triggered, destroy object and spawn prop (can also spawn nothing)
}
