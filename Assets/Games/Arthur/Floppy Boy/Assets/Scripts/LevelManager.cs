﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckPoint;
   // private PlayerController player;

	// Use this for initialization
	void Start ()
    {
        //player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    //GameObject.FindGameObjectsWithTag.
	}

    public void RespawnPlayer()
    {
        //Debug.Log ("Player Respawn");
        GameObject.FindWithTag("Player").transform.position = currentCheckPoint.transform.position; //player.
        //Time.timeScale = 0;
    }

    public void RespawnPlayer2()
    {
        //Debug.Log("Player Respawn");
        GameObject.FindWithTag("Player2").transform.position = currentCheckPoint.transform.position; //player.
        //Time.timeScale = 0;
    }

}
      /*public int playerdeath;
        playerdeath++;
        Debug.Log("Death Count: " + playerdeath);*/