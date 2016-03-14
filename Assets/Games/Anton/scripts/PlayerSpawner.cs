﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class PlayerSpawner : MonoBehaviour {
    [SerializeField]
    GameObject m_player;//is set inside the Unity Engine
                        // Use this for initialization
    private List<GameObject> m_players;

    void Start ()
    {
        m_players = new List<GameObject>();
        int loop = GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().Controllers;
        for (int i = 0; i < loop; i++)
        {
            m_players.Add(Instantiate(m_player));
            m_players[i].GetComponent<PlayerScript>().AssignManager(GameObject.FindGameObjectWithTag("ManagerP"+(i+1)).GetComponent<Manager>());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0;i< m_players.Count;i++)
        {
            if (m_players[i].GetComponent<PlayerScript>().GetShallDestroy()==true)
            {

                m_players[i].GetComponent<PlayerScript>().SetScore(8-m_players.Count * 2);
                Destroy(m_players[i]);
                m_players.RemoveAt(i);
            }
        }
        if (m_players.Count == 0)
        {
            SceneManager.LoadScene("Staging");
        }
    }
}