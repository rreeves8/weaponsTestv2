using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour {

    /// <summary>
    /// ignore this it was an intial attempt, its supposed to be a sublass of gun
    /// </summary>

    public static  GameObject slug;

    public static GameObject birdshot;

    private static List<GameObject> projectiles = new List<GameObject>();

    public static void setAmmoType(string name, int num) {
        if(name == "slug") {
            projectiles.Add(slug);
        }
        
        if(name == "birdshot") {
            for(int i = 0; i < num; i++) {
                projectiles.Add(birdshot);
            }
        }
    }

    public static List<GameObject> getProjectile() {
        return projectiles;
    }

    public static float getMuzzleForce() {
        return 20445f;
    }

    public static float getDragForce(GameObject obj) {
        float speed = obj.GetComponent<Rigidbody>().velocity.magnitude;
        return 0.5f * 0.5f * 0.00008364678f * speed*speed * 1.2041f;
    } 
}
