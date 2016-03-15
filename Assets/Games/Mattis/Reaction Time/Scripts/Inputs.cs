﻿using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour
{

    [SerializeField] private int m_playerNumber;
    private int m_clickCount;
    private int m_buttonHp;
    //private Manager m_myManager;

	// Use this for initialization
	void Start ()
    {
        m_clickCount = 1;
        //m_myManager = GameObject.FindGameObjectWithTag("ManagerP" + m_playerNumber).GetComponent<Manager>();

        /// Idea for a win condition:
        /// m_buttonHp = 0 + m_numberofPlayers;
        /// m_buttonHp -= 1; //in le inputs
        /// if (m_buttonHp == 0)
        /// {
        ///      UnityEngine.SceneManagement.SceneManager.LoadScene("Celebration");
        /// }
        /// can abuse if everyone presses before button spawn and just skip the game. solution: cant press shit until button actually spawns (variables n shit in button spawn)
        m_buttonHp = 1;
    }

    // Update is called once per frame
    void Update ()
    {
        /// funkar perfekomundo

        if ((Input.GetKeyDown(KeyCode.A)/*(Input.GetButtonDown(m_myManager.Inputs[0].name)*/) && (GameObject.FindGameObjectWithTag("buttonA") == true) && (m_clickCount == 1))
        {
            //Score
            Debug.Log("buttonA works mate");
        }

        if ((Input.GetKeyDown(KeyCode.S)/*(Input.GetButtonDown(m_myManager.Inputs[1].name)*/) && (GameObject.FindGameObjectWithTag("buttonB") == true) && (m_clickCount == 1))
        {
            //Score
            Debug.Log("buttonB works mate");
        }

        if ((Input.GetKeyDown(KeyCode.D)/*(Input.GetButtonDown(m_myManager.Inputs[2].name)*/) && (GameObject.FindGameObjectWithTag("buttonX") == true) && (m_clickCount == 1))
        {
            //Score
            Debug.Log("buttonX works mate");
        }

        if ((Input.GetKeyDown(KeyCode.F)/*(Input.GetButtonDown(m_myManager.Inputs[3].name)*/) && (GameObject.FindGameObjectWithTag("buttonY") == true) && (m_clickCount == 1))
        {
            //Score
            Debug.Log("buttonY works mate");
        }

        // You only got one shot, one opportunity to sieze everything you ever wanted. Will you capture it or just let it slip?
        if (((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.F))) && (m_clickCount == 1))
        {
            m_buttonHp -= 1;
            m_clickCount -= 1;
        }

        if (m_buttonHp <= 0)
        {
            //Destroy(GameObject.FindGameObjectWithTag("buttonA"));
            //Destroy(GameObject.FindGameObjectWithTag("buttonB"));
            //Destroy(GameObject.FindGameObjectWithTag("buttonX"));
            //Destroy(GameObject.FindGameObjectWithTag("buttonY"));
            UnityEngine.SceneManagement.SceneManager.LoadScene("Celebration");
        }
    }
}