using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour {
    public GameObject player;
    public GameObject fire_point;
    public GameObject birdshot;
    public GameObject weapon;

    public GameObject fire_point2;
    public GameObject birdshot2;
    public GameObject weapon2;

    public float speed = 1.0f;
    public float strength = 0.5f;
    public float time;
    // Update is called once per frame
    void Update() {

        transform.LookAt(player.transform);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);

        time += Time.deltaTime;
        
        if (time > 0.5f) {
            
            time = 0;
            
            Transform pos = weapon.transform;
            Transform pos2 = weapon2.transform;

            GameObject obj = Instantiate(birdshot, fire_point.transform.position, fire_point.transform.rotation) as GameObject;
            GameObject obj2 = Instantiate(birdshot2, fire_point2.transform.position, fire_point2.transform.rotation) as GameObject;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            Rigidbody rb2 = obj2.GetComponent<Rigidbody>();

            rb.AddForce(rb.transform.forward * 100);
            rb2.AddForce(rb.transform.forward * 100);

            obj.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
            obj2.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
        }
    }
}
