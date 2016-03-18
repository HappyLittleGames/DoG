﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour
{
    [SerializeField] private Image[] m_teamIcons;

    [SerializeField]
    private GameObject buttonA;
    [SerializeField]
    private GameObject buttonB;
    [SerializeField]
    private GameObject buttonX;
    [SerializeField]
    private GameObject buttonY;

    private List<Manager> m_playerManagers = new List<Manager>();
    private int[] m_pressCounts = new int[4] { 1, 1, 1, 1 };

    private int m_countdown;       //Time before button is instantiated
    private int m_buttonChoice;    //Which button to instantiate

    // Use this for initialization
    void Start ()
    {
        m_buttonChoice = Random.Range(1, 5);
        m_countdown = Random.Range(1, 10);
        StartCoroutine(ExecuteAfterTime(m_countdown));
    }
	
	// Update is called once per frame
	void Update ()
    {
        //m_buttonChoice = Random.Range(1, 5);
        //m_countdown = Random.Range(1, 2);

        //if (m_buttonChoice == 1)
        //{
        //    Instantiate(buttonA);
        //}
        //else if (m_buttonChoice == 2)
        //{
        //    Instantiate(buttonB);
        //}
        //else if (m_buttonChoice == 3)
        //{
        //    Instantiate(buttonX);
        //}
        //else if (m_buttonChoice == 4)
        //{
        //    Instantiate(buttonY);
        //}
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        m_pressCounts = new int[4]{ 1, 1, 1, 1};
        yield return new WaitForSeconds(m_countdown);
        if(m_buttonChoice == 1)
        {
            Instantiate(buttonA);
        }
        else if (m_buttonChoice == 2)
        {
            Instantiate(buttonB);
        }
        else if (m_buttonChoice == 3)
        {
            Instantiate(buttonX);
        }
        else if (m_buttonChoice == 4)
        {
            Instantiate(buttonY);
        }
    }

    private void CheckButtons()
    {
        foreach (Manager teamManager in m_playerManagers)
        {
            if (PlayerPressed(teamManager))
            {
                teamManager.Score += 4;
                m_playerManagers.Remove(teamManager);
                m_teamIcons[teamManager.TeamNumber - 1].gameObject.SetActive(false);
                // nån slags bild som man tar bort från canvas
            }
        }
    }

    private bool PlayerPressed(Manager myManager)
    {
        /// funkar perfektomundo

        if (/*(Input.GetKeyDown(KeyCode.A)*/(Input.GetButtonDown(myManager.Inputs[0].name)) && (GameObject.FindGameObjectWithTag("buttonA") == true) && (m_pressCounts[myManager.TeamNumber-1] == 1))
        {
            //Score
            Debug.Log("buttonA works mate");
            return true;
        }

        if (/*(Input.GetKeyDown(KeyCode.S)*/(Input.GetButtonDown(myManager.Inputs[1].name)) && (GameObject.FindGameObjectWithTag("buttonB") == true) && (m_pressCounts[myManager.TeamNumber - 1] == 1))
        {
            //Score
            Debug.Log("buttonB works mate");
            return true;
        }

        if (/*(Input.GetKeyDown(KeyCode.D)*/(Input.GetButtonDown(myManager.Inputs[2].name)) && (GameObject.FindGameObjectWithTag("buttonX") == true) && (m_pressCounts[myManager.TeamNumber - 1] == 1))
        {
            //Score
            Debug.Log("buttonX works mate");
            return true;
        }

        if (/*(Input.GetKeyDown(KeyCode.F)*/(Input.GetButtonDown(myManager.Inputs[3].name)) && (GameObject.FindGameObjectWithTag("buttonY") == true) && (m_pressCounts[myManager.TeamNumber - 1] == 1))
        {
            //Score
            Debug.Log("buttonY works mate");
            return true;
        }
        return false;
    }

    private void FindTeams()
    {
        int controllers = GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().Controllers;
        for (int i = 1; i <= controllers; i++)
        {
            m_playerManagers.Add(GameObject.FindGameObjectWithTag("ManagerP" + i).GetComponent<Manager>());
        }
    }
}
