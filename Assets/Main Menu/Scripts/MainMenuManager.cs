﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuManager : Manager {

    [SerializeField] private Camera m_controller1Cam = null;
    [SerializeField] private Camera m_controller2Cam = null;
    [SerializeField] private Camera m_controller3Cam = null;
    [SerializeField] private Camera m_controller4Cam = null;

    private Manager m_ManagerP1 = null;
    private Manager m_ManagerP2 = null;
    private Manager m_ManagerP3 = null;
    private Manager m_ManagerP4 = null;
    private List<Manager> m_managerList = new List<Manager>();

    // the sliders that will set the teamSize on each manager.
    [SerializeField] private Slider m_slider1 = null;
    [SerializeField] private Slider m_slider2 = null;
    [SerializeField] private Slider m_slider3 = null;
    [SerializeField] private Slider m_slider4 = null;

    [SerializeField] private int m_joysticks = 4;
    [SerializeField] private GameObject m_playerManager = null;

    // private SerializedProperty[] m_inputManagerArray;
    private Button[] m_buttonArray;
    // private string[] m_controller = Input.GetJoystickNames();

    // Standard Colors (there needs to be as many as of these as there are supported controllers)
    private Color m_lemonTruck = new Color32(249, 227, 105, 255);
    private Color m_beachBlueCoathanger = new Color32(148, 233, 246, 255);
    private Color m_flamingoParachute = new Color32(234, 160, 223, 255);
    private Color m_pepperGreenGranola = new Color32(140, 253, 125, 255);

    // Standard Viewport Rectangles
    private Rect m_fullscreen = new Rect(0f, 0f, 1f, 1f);
    private Rect m_topLeft = new Rect(0f, 0.5f, 0.5f, 1f);
    private Rect m_topRight = new Rect(0.5f, 0.5f, 0.5f, 1f);
    private Rect m_bottomLeft = new Rect(0f, 0f, 0.5f, 0.5f);
    private Rect m_bottomRight = new Rect(0.5f, -0.5f, 1f, 1f);
    private Rect m_topHalf = new Rect(0f, 0.5f, 1f, 1f);
    private Rect m_bottomHalf = new Rect(0f, 0f, 1f, 0.5f);
    private List<Color> m_colorList = new List<Color>();


    void Awake()
    {
        MakeInputsFromIM();
        MakeColors();   
        MakeTeams();

        TestCameras(m_joysticks);

    }

    private void Start()
    {
        m_ManagerP1.Controllers = m_joysticks;
    }

    void Update()
    {
        if (Input.GetKeyDown("f4")) { TestCameras(4); }
        if (Input.GetKeyDown("f3")) { TestCameras(3); }
        if (Input.GetKeyDown("f2")) { TestCameras(2); }
        if (Input.GetKeyDown("f1")) { TestCameras(1); }

        TogglePlayersActive();
        TestCameras(Manager1.Controllers);

        m_ManagerP1.TeamSize = SliderSelect(m_slider1);
        m_ManagerP2.TeamSize = SliderSelect(m_slider2);
        m_ManagerP3.TeamSize = SliderSelect(m_slider3);
        m_ManagerP4.TeamSize = SliderSelect(m_slider4);

    }

    void MakePlayers()
    {
        m_joysticks = Input.GetJoystickNames().Length;
        
    }

    private void MakeInputsFromIM()
    {
        m_buttonArray = new Button[68];
        for (int i = 0; i < 4; i++)
        {
            int p = i + 1;

            m_buttonArray[  0 + (17 * i)].name = "A_P" + (p);
            m_buttonArray[  1 + (17 * i)].name = "B_P" + (p);
            m_buttonArray[  2 + (17 * i)].name = "X_P" + (p);
            m_buttonArray[  3 + (17 * i)].name = "Y_P" + (p);
            m_buttonArray[  4 + (17 * i)].name = "LBumper_P" + ( p);
            m_buttonArray[  5 + (17 * i)].name = "RBumper_P" + (p);
            m_buttonArray[  6 + (17 * i)].name = "Back_P" + ( p);
            m_buttonArray[  7 + (17 * i)].name = "Start_P" + ( p);
            m_buttonArray[  8 + (17 * i)].name = "LeftStickClick_P" + ( p);
            m_buttonArray[  9 + (17 * i)].name = "RightStickClick_P" + (p);
            m_buttonArray[ 10 + (17 * i)].name = "DPadX_P" + ( p);
            m_buttonArray[ 11 + (17 * i)].name = "DPadY_P" + (p);
            m_buttonArray[ 12 + (17 * i)].name = "LeftStickX_P" + (p);
            m_buttonArray[ 13 + (17 * i)].name = "LeftStickY_P" + (p);
            m_buttonArray[ 14 + (17 * i)].name = "RightStickX_P" + (p);
            m_buttonArray[ 15 + (17 * i)].name = "RightStickY_P" + (p);
            m_buttonArray[ 16 + (17 * i)].name = "TriggerAxis_P" + (p);
           

            for (int j = 0; j < 10; j++)
            {
                m_buttonArray[j * i].isAxis = false;
            }

            for (int k = 10; k < 17; k++)
            {
                m_buttonArray[k * i].isAxis = false;
            }
        }  
    }

    private void MakeManager(int controller)
    {
        // Find and don't bother if there already is one
        if (!GameObject.FindGameObjectWithTag("ManagerP" + controller))
        {
            // Initialize a new instance of Manager to the parametered team number and player count.
            GameObject newPlayerManager = (GameObject)Instantiate(m_playerManager, transform.position, transform.rotation);
            newPlayerManager.gameObject.AddComponent<Persistency>();
            newPlayerManager.name = "Team " + controller + " Manager";
            Manager newManager = newPlayerManager.gameObject.AddComponent<Manager>();
            newManager.gameObject.tag = "ManagerP" + controller;
            newManager.TeamNumber = controller;
            newManager.Active = true;

            // Set up Inputs array, really only supports xBox controllers. :(
            newManager.Inputs = new Button[17];
            for (int i = 0; i < 17; i++)
            {
                newManager.Inputs[i] = m_buttonArray[i + (17 * (controller - 1))];
            }

            // Give it a pretty color! (just set colors for team 1-4 atm...)
            newManager.PlayerColor = m_colorList[controller-1];

            // Set up whatever variables it will need.
            newManager.Score = 0;
            newManager.enabled = true; //??
            switch (controller)
            {
                case 1:
                    newManager.TeamName = "Pepper-Green Granola";
                    break;
                case 2:
                    newManager.TeamName = "Pink Pelican Parachute";
                    break;
                case 3:
                    newManager.TeamName = "Lemon Truck";
                    break;
                case 4:
                    newManager.TeamName = "Blue Coathanger";
                    break;
                default:
                    newManager.TeamName = "Special Snowflake";
                    break;
            }


            // Add it to the list to sort them upon leaving Main Menu (obsolete).
            m_managerList.Add(newManager);
        }
    }

    private int SliderSelect(Slider slider)
    {
        int players = 1;
        //Debug.Log(GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().TeamSize);
        players = (int)slider.value;

        return players;
    }

    private void MakeTeams()
    {

        MakeManager(1);
        MakeManager(2);
        MakeManager(3);
        MakeManager(4);

        m_ManagerP1 = GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>();
        m_ManagerP2 = GameObject.FindGameObjectWithTag("ManagerP2").GetComponent<Manager>();
        m_ManagerP3 = GameObject.FindGameObjectWithTag("ManagerP3").GetComponent<Manager>();
        m_ManagerP4 = GameObject.FindGameObjectWithTag("ManagerP4").GetComponent<Manager>();

    }

    private void SetCameras()
    {
        if (m_joysticks == 4)
        {
            m_ManagerP1.Controllers = 4;

            m_controller1Cam.rect = m_topLeft;
            GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().PlayerRect = m_topLeft;
            m_controller1Cam.gameObject.SetActive(true);
            m_controller2Cam.rect = m_topRight;
            GameObject.FindGameObjectWithTag("ManagerP2").GetComponent<Manager>().PlayerRect = m_topRight;
            m_controller2Cam.gameObject.SetActive(true);
            m_controller3Cam.rect = m_bottomLeft;
            GameObject.FindGameObjectWithTag("ManagerP3").GetComponent<Manager>().PlayerRect = m_bottomLeft;
            m_controller3Cam.gameObject.SetActive(true);
            m_controller4Cam.rect = m_bottomRight;
            GameObject.FindGameObjectWithTag("ManagerP4").GetComponent<Manager>().PlayerRect = m_bottomRight;
            m_controller4Cam.gameObject.SetActive(true);
        }
        if (m_joysticks == 3)
        {
            m_ManagerP1.Controllers = 3;

            m_controller1Cam.rect = m_topHalf;
            GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().PlayerRect = m_topHalf;
            m_controller2Cam.gameObject.SetActive(true);
            m_controller2Cam.rect = m_bottomLeft;
            GameObject.FindGameObjectWithTag("ManagerP2").GetComponent<Manager>().PlayerRect = m_bottomLeft;
            m_controller2Cam.gameObject.SetActive(true);
            m_controller3Cam.rect = m_bottomRight;
            GameObject.FindGameObjectWithTag("ManagerP3").GetComponent<Manager>().PlayerRect = m_bottomRight;
            m_controller3Cam.gameObject.SetActive(true);
            m_controller4Cam.gameObject.SetActive(false);
        }
        if (m_joysticks == 2)
        {
            m_ManagerP1.Controllers = 2;

            m_controller1Cam.rect = m_topHalf;
            GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().PlayerRect = m_topHalf;
            m_controller1Cam.gameObject.SetActive(true);
            m_controller2Cam.rect = m_bottomHalf;
            GameObject.FindGameObjectWithTag("ManagerP2").GetComponent<Manager>().PlayerRect = m_bottomHalf;
            m_controller2Cam.gameObject.SetActive(true);
            m_controller3Cam.gameObject.SetActive(false);
            m_controller4Cam.gameObject.SetActive(false);
        }
        if (m_joysticks == 1)
        {
            m_ManagerP1.Controllers = 1;

            m_controller1Cam.rect = m_fullscreen;
            GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().PlayerRect = m_fullscreen;
            m_controller1Cam.gameObject.SetActive(true);
            m_controller2Cam.gameObject.SetActive(false);
            m_controller3Cam.gameObject.SetActive(false);
            m_controller4Cam.gameObject.SetActive(false);
        }

        /*
        m_controller1Cam.backgroundColor = m_flamingoParachute;
        GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().PlayerColor = m_controller1Cam.backgroundColor;
        m_controller2Cam.backgroundColor = SetColor();
        GameObject.FindGameObjectWithTag("ManagerP2").GetComponent<Manager>().PlayerColor = m_controller2Cam.backgroundColor;
        m_controller3Cam.backgroundColor = SetColor();
        GameObject.FindGameObjectWithTag("ManagerP3").GetComponent<Manager>().PlayerColor = m_controller3Cam.backgroundColor;
        m_controller4Cam.backgroundColor = SetColor();
        GameObject.FindGameObjectWithTag("ManagerP4").GetComponent<Manager>().PlayerColor = m_controller4Cam.backgroundColor;
        */
    }

    private void TestCameras(int players)
    {
        //MakeColors();

        if (players == 4)
        {
            m_ManagerP1.Controllers = 4;

            m_controller1Cam.rect = m_topLeft;
            m_controller1Cam.gameObject.SetActive(true);
            m_ManagerP1.Active = true;
            m_controller2Cam.rect = m_topRight;
            m_controller2Cam.gameObject.SetActive(true);
            m_ManagerP2.Active = true;
            m_controller3Cam.rect = m_bottomLeft;
            m_controller3Cam.gameObject.SetActive(true);
            m_ManagerP3.Active = true;
            m_controller4Cam.rect = m_bottomRight;
            m_controller4Cam.gameObject.SetActive(true);
            m_ManagerP4.Active = true;
        }
        else if (players == 3)
        {
            m_ManagerP1.Controllers = 3;

            m_controller1Cam.rect = new Rect(0f, 0.5f, 1f, 1f);
            m_controller1Cam.gameObject.SetActive(true);
            m_ManagerP1.Active = true;
            m_controller2Cam.rect = new Rect(0f, 0f, 0.5f, 0.5f);
            m_controller2Cam.gameObject.SetActive(true);
            m_ManagerP2.Active = true;
            m_controller3Cam.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
            m_controller3Cam.gameObject.SetActive(true);
            m_ManagerP3.Active = true;
            m_controller4Cam.gameObject.SetActive(false);
            m_ManagerP4.Active = false;
        }
        else if (players == 2)
        {
            m_ManagerP1.Controllers = 2;

            m_controller1Cam.rect = new Rect(0f, 0.5f, 1f, 1f);
            m_controller1Cam.gameObject.SetActive(true);
            m_ManagerP1.Active = true;
            m_controller2Cam.rect = new Rect(0f, 0f, 1f, 0.5f);
            m_controller2Cam.gameObject.SetActive(true);
            m_ManagerP2.Active = true;
            m_controller3Cam.gameObject.SetActive(false);
            m_ManagerP3.Active = false;
            m_controller4Cam.gameObject.SetActive(false);
            m_ManagerP4.Active = false;
        }   
        else if (players == 1)
        {
            m_ManagerP1.Controllers = 1;

            m_controller1Cam.rect = new Rect(0f, 0f, 1f, 1f);
            m_controller1Cam.enabled = true;
            m_ManagerP1.Active = true;
            m_controller2Cam.gameObject.SetActive(false);
            m_ManagerP2.Active = false;
            m_controller3Cam.gameObject.SetActive(false);
            m_ManagerP3.Active = false;
            m_controller4Cam.gameObject.SetActive(false);
            m_ManagerP4.Active = false;
        }
    }

    private void TogglePlayersActive()
    {
        for (int i = 0; i < 4; i++)
        {
            Manager myManager = GameObject.FindGameObjectWithTag("ManagerP" + (i + 1)).GetComponent<Manager>();
            if (Input.GetButtonDown(myManager.Inputs[7].name))
            {
                Debug.Log(myManager.gameObject.tag + " pressed that button");
                if (myManager.Active)
                {
                    if (myManager.TeamNumber == m_ManagerP1.Controllers)
                    {
                        m_ManagerP1.Controllers--;
                        myManager.Active = false;
                    }
                }
                else
                {
                    if (myManager.TeamNumber == (m_ManagerP1.Controllers +1))
                    {
                        m_ManagerP1.Controllers++;
                        myManager.Active = true;
                    }
                }
            }
            //SortManagers();
        }

        //List<Manager> tempManagerList = m_managerList;

        //for (int i = 1; i < tempManagerList.Count; i++)
        //{
        //    Manager tempManager = tempManagerList[i];

        //    if (!tempManager.Active) 
        //    {
        //        if (Input.GetButtonDown(tempManager.Inputs[0].name))
        //        {
        //            m_ManagerP1.Controllers++;
        //            tempManager.Active = true;
        //            m_managerList.Remove(tempManager);
        //            m_managerList.Add(tempManager);

        //        }
        //    }
        //    if (tempManager.Active)
        //    {
        //        if (Input.GetButtonDown(tempManager.Inputs[1].name))
        //        {
        //            m_ManagerP1.Controllers--;
        //            tempManager.Active = false;
        //            m_managerList.Remove(tempManager);
        //            m_managerList.Add(tempManager);
        //        }
        //    }
        //}

        //SortManagers();
    }

    private void SortManagers()
    {
        // note that we never sort Manager 1.
        for (int i = 1; i < m_managerList.Count; i++)
        {
            m_managerList[i].gameObject.tag = "ManagerP" + (i + 1);
        }
    }

    void MakeColors()
    {
        //List<Color> colorLm_colorListist = new List<Color>();

        m_colorList = new List<Color>();

        m_colorList.Add(m_pepperGreenGranola);
        m_colorList.Add(m_flamingoParachute);
        m_colorList.Add(m_lemonTruck);
        m_colorList.Add(m_beachBlueCoathanger);

        
    }

    private Color SetColor()
    {
        Color color = Color.cyan;
        if (m_colorList.Count >= 1)
        {
            int index = Random.Range(0, (m_colorList.Count));
            color = m_colorList[index];
            m_colorList.Remove(m_colorList[index]);
        }
        return color;
    }

    public Button[] InputManagerArray
    {
        set {m_buttonArray = value; }
    }

    public Manager Manager1 { get { return m_ManagerP1; } }
    public Manager Manager2 { get { return m_ManagerP1; } }
    public Manager Manager3 { get { return m_ManagerP1; } }
    public Manager Manager4 { get { return m_ManagerP1; } }
}
