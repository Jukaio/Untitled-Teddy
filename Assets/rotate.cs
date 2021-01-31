using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    //adjust this to change speed
float speed = 5f;
//adjust this to change how high it goes
float height = 0.5f;

    // Update is called once per frame
    void Update()
    {
         transform.Rotate( new Vector3(0, 1, 0), Space.Self );
Vector3 pos = transform.position;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed);
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, newY, pos.z) * height;

    }
}
