﻿using UnityEngine;
using System.Collections;

public class collisionShot : MonoBehaviour {
    bool m_shallDestroy;//not instant destruction
	// Use this for initialization
	void Start () {
        m_shallDestroy = false;
        Vector3 temp = transform.position;
        temp.z = 0.0f;
        transform.position = temp;//ensure all parts have same z value http://answers.unity3d.com/questions/600421/how-to-change-xyz-values-in-a-vector3-properly-in.html

    }
	
	// Update is called once per frame
	void Update () {
        
        if (m_shallDestroy)
        {//destroys the object if it has collided with a player since last frame
            Destroy(gameObject);
        }
        /*else if ((transform.position.x < -32)
            || (transform.position.x > Screen.width + 32)
            || (transform.position.y < -32)
            || (transform.position.y > Screen.height + 32))
        {//or if it is outside of bounds
            Destroy(gameObject);
        }*/

    }

    void OnCollisionEnter2D(Collision2D collision)
    {//destroys the shot if it collides with a player
        if (collision.gameObject.tag == "Player")
        {
            m_shallDestroy = true;
            print("blue");
            collision.gameObject.GetComponent<PlayerScript>().SetShallDestroy();

        }
        else if (collision.gameObject.tag =="Wall")
        {
            m_shallDestroy = true;

        }
    }
}
