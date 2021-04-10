using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour{
    private float health1 = 100;
    private float health2 = 100;


    /// <summary>
    /// collision script for bullets, not working properly I think the collision detection is set wrongly, also not a good way of doing this
    /// </summary>

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            health1 -= 20;
 
        }
        if (other.gameObject.CompareTag("enemy")) {
            health2 -= 20;
        }

        if(health1 < 0) {
            other.gameObject.SetActive(false);
        }

        if (health2 < 0) {
            other.gameObject.SetActive(false);
        }
    }
}
