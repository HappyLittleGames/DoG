﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    private float m_moveCamera = 1.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
            transform.Translate((Vector2.up * (Time.deltaTime * m_moveCamera)), Space.World);
    }
}
