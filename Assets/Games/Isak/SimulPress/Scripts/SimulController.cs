﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimulController : MonoBehaviour
{
    private List<Manager> m_teamManagers = null;
    private SimulPress[] m_teamSimulPressers = null;
    [SerializeField] private GameObject[] m_timerObjects = null;
    [SerializeField] private GameObject[] m_activeInputIcons = null;
    [SerializeField] private Text m_countDownText = null;
    private float m_countDownTime = 0f;
    private bool m_displayingScores = false;
    private string m_timeFormat = ("{0:00.00}");

    private List<int> m_trueIndices;
    private List<int> m_falseIndices;

    private void Start()
    {
        int xBoxInputs = 17;
        m_falseIndices = MakeFalseIndices(xBoxInputs); // gör detta centralt istället eller?
        m_trueIndices = MakeTrueIndices(m_falseIndices);
        MakePlayers();
        StartCountdown(20f);
    }

    private void Update()
    {
        if ( !PlayersUpdating(m_teamSimulPressers)  ||  (!UpdatingCountDown()))
        {
            if (!m_displayingScores)
            {
                StartCoroutine(AllDone(4f));
            }
        }
        DisplayActiveInputs();
    }

    private void MakePlayers()
    {
        m_teamManagers = new List<Manager>();

        int controllers = GameObject.FindGameObjectWithTag("ManagerP1").GetComponent<Manager>().Controllers;
        m_teamSimulPressers = new SimulPress[controllers];

        for (int i = 0; i < controllers; i++)
        {
            m_teamManagers.Add(GameObject.FindGameObjectWithTag("ManagerP" + (i + 1)).GetComponent<Manager>());
            Debug.Log("m_teamManagers[" + i + "] = " + m_teamManagers[i].TeamName);
        }
        for (int i = 0; i < controllers; i++)
        {
            m_timerObjects[i].GetComponentInChildren<Text>().text = "eh?"; 
            m_teamSimulPressers[i] = m_timerObjects[i].GetComponent<SimulPress>();
            m_teamSimulPressers[i].TeamManager = m_teamManagers[i];
            m_teamSimulPressers[i].TrueIndices = m_trueIndices;
            m_teamSimulPressers[i].FalseIndices = m_falseIndices;
            Debug.Log("m_timerObjects[" + i + "] is " + m_timerObjects[i].gameObject.name);
            Debug.Log(" m_teamSimulPressers.teamManager[" + i + "] = " + m_teamSimulPressers[i].TeamManager.TeamName);
            // nr.4 doesn't seem to get a manager here?
        }
    }

    private void StartCountdown(float timeToCountDown)
    {
        m_countDownTime = timeToCountDown;
        m_countDownText.gameObject.SetActive(true);
    }

    private bool UpdatingCountDown()
    {
        if (m_countDownTime > 0f)
        {
            m_countDownTime -= Mathf.Clamp(Time.deltaTime, 0f, m_countDownTime);
            m_countDownText.text = string.Format(m_timeFormat, m_countDownTime);
            return true;
        }
        else
        {
            // fancy pants flashy trash
            return false;
        }
    }

    private void DisplayCurrentActives()
    {
        for (int i = 0; i < m_teamSimulPressers.Length; i++)
        {
            m_teamSimulPressers[i].GetComponent<Text>().text = string.Format(m_timeFormat, m_teamSimulPressers[i].TimeSinceFirstPress);
        }
    }

    private bool PlayersUpdating(SimulPress[] simulPressers)
    {
        bool allDone = false;
        foreach (SimulPress simulPresser in simulPressers)
        {
            if (simulPresser.ClockIsRunning)
            {
                allDone = true;
            }
        }
        return allDone;
    }

    private IEnumerator AllDone(float time)
    {
        m_displayingScores = true;
        m_countDownText.text = "waiting for results...";
        List<float> finishedTimes = new List<float>();
        for (int i = 0; i < m_teamSimulPressers.Length; i++)
        {
            finishedTimes.Add(m_teamSimulPressers[i].TimeSinceFirstPress);
        }
        ScoreManager[] scoreManagers = new ScoreManager[finishedTimes.Count];
        for (int i = 0; i < scoreManagers.Length; i++)
        {
            scoreManagers[i] = (new ScoreManager(m_teamSimulPressers[i].TimeSinceFirstPress, m_teamSimulPressers[i].TeamManager));
        }

        yield return new WaitForSeconds(time);

        scoreManagers = scoreManagers.OrderBy(n => n.score).ToArray();

        m_countDownText.text =  ((finishedTimes.Count > 0) ? ("1 - " + string.Format(m_timeFormat, scoreManagers[0].score) + " - " + scoreManagers[0].manager.TeamName + "\n") : ("\n")) +
                                ((finishedTimes.Count > 1) ? ("2 - " + string.Format(m_timeFormat, scoreManagers[1].score) + " - " + scoreManagers[1].manager.TeamName + "\n") : ("\n")) +
                                ((finishedTimes.Count > 2) ? ("3 - " + string.Format(m_timeFormat, scoreManagers[2].score) + " - " + scoreManagers[2].manager.TeamName + "\n") : ("\n")) +
                                ((finishedTimes.Count > 3) ? ("4 - " + string.Format(m_timeFormat, scoreManagers[3].score) + " - " + scoreManagers[3].manager.TeamName + "\n") : ("\n"));

        StartCoroutine(ExitToCelebration(4f));
    } 

    struct ScoreManager
    {
        public float score;
        public Manager manager;

        public ScoreManager(float newScore, Manager newManager)
        {
            this.score = newScore;
            this.manager = newManager;
        }
    }

    private IEnumerator ExitToCelebration(float time)
    {
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Celebration");
    }

    private void DisplayActiveInputs()
    {
        List<int> activeInputs = m_teamSimulPressers[0].TrueIndices;
        for (int i = 0; i < ( m_activeInputIcons.Length); i++)
        {
            {
                m_activeInputIcons[i].SetActive(true);
            }
        }
    }
    
    private ScoreManager[] CompareScores(ScoreManager[] scoreManagers)
    {
        ScoreManager[] rankedScoreManagers = new ScoreManager[scoreManagers.Length];
        for (int i = 0; i < scoreManagers.Length; i++)
        {
            foreach (ScoreManager otherScoreManager in scoreManagers)
            {
                int rank = 0;
                if ((scoreManagers[i].manager != otherScoreManager.manager) && (scoreManagers[i].score > otherScoreManager.score))
                {
                    rank++;
                } 
            }
        }
        return rankedScoreManagers;
    }

    private List<int> MakeFalseIndices(int amount)
    {
        List<int> tempList = new List<int>();
        for (int i = 0; i < amount; i++)
        {
            tempList.Add(i);
        }
        return tempList;
    }

    private List<int> MakeTrueIndices(List<int> falseIndices)
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, falseIndices.Count);
            falseIndices.Remove(index);
            tempList.Add(index);
        }

        return tempList;
    }

    //public class StructComparer : IComparer
    //{
    //    public float CompareStructs(object x, object y)
    //    {
    //        if (!(x is ScoreManager) || !(y is ScoreManager)) return 0;
    //        ScoreManager a = (ScoreManager)x;
    //        ScoreManager b = (ScoreManager)y;

    //        return a.score.CompareTo(b.score);
    //    }

    //}
}