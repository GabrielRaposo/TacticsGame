using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Team { Player, Enemy, Other }

public class TurnManager : MonoBehaviour
{
    [Header("Turn")]
    public GameObject turnDisplayBox;
    public TextMeshProUGUI turnDisplayValue;

    [Header("Team")]
    public GameObject teamDisplayBox;
    public TextMeshProUGUI teamDisplayValue;

    private Dictionary<Team, List<TacticsUnit>> teamDictionary;

    private List<TacticsUnit> playerTeam;
    private List<TacticsUnit> enemyTeam;
    private List<TacticsUnit> otherTeam;

    private List<Team> teamOrder;
    private int currentTeam;
    private int turn;

    public static TurnManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        teamOrder = new List<Team>();
        teamDictionary = new Dictionary<Team, List<TacticsUnit>>();

        playerTeam = CreateTeam(playerTeam, "Player", Team.Player);
        enemyTeam  = CreateTeam(enemyTeam,  "Enemy",  Team.Enemy);
        otherTeam  = CreateTeam(otherTeam,  "Other",  Team.Other);

        turn = currentTeam = 0;
        StartCoroutine(TurnTransitionAnimation());
    }

    private List<TacticsUnit> CreateTeam(List<TacticsUnit> list, string tag, Team team)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
        if(units != null && units.Length > 0)
        {
            list = new List<TacticsUnit>();
            foreach(GameObject unit in units)
            {
                TacticsUnit u = unit.GetComponent<TacticsUnit>();
                if (u)
                {
                    u.SetStamina(true);
                    list.Add(u);
                }
            }

            teamOrder.Add(team);
            teamDictionary.Add(team, list);
            return list;
        }
        return null;
    }

    //---DESCULPA COELHO---
    private IEnumerator TurnTransitionAnimation()
    {
        SelectionController.disabled = true;

        turnDisplayValue.text = "Turn " + turn;
        switch (teamOrder[currentTeam])
        {
            default:
            case Team.Player:
                teamDisplayValue.text = "Player Phase";
                break;

            case Team.Enemy:
                teamDisplayValue.text = "Enemy Phase";
                break;

            case Team.Other:
                teamDisplayValue.text = "Other Phase";
                break;
        }

        turnDisplayBox.SetActive(true);
        teamDisplayBox.SetActive(true);

        yield return new WaitForSeconds(1);

        turnDisplayBox.SetActive(false);
        teamDisplayBox.SetActive(false);

        StartTurn();
    }

    private void StartTurn()
    {
        List<TacticsUnit> list = teamDictionary[teamOrder[currentTeam]];

        //Se for Player, libera para que todos 
        if (teamOrder[currentTeam] == Team.Player)
        {
            foreach (TacticsUnit unit in list)
            {
                unit.SetStamina(true);
                unit.BeginTurn();
            }
            SelectionController.disabled = false;
        }
        //Se for Enemy ou Other, libera o primeiro e o resto será liberado na chamada do EndTurn
        else
        {
            foreach (TacticsUnit unit in list)
            {
                unit.SetStamina(true);
            }
            list[0].BeginTurn();
        }

        TacticsCamera.FocusOn(list[0].gameObject);
    }

    public void EndTurn()
    {
        List<TacticsUnit> list = teamDictionary[teamOrder[currentTeam]];

        int count = 0;
        foreach(TacticsUnit unit in list)
        {
            if (unit.CheckStamina())
            {
                count++;
            }
        }

        if (count < 1)
        {
            foreach (TacticsUnit unit in list)
            {
                unit.SetStamina(true);
            }

            CallNextTeam();
        }
        else if (teamOrder[currentTeam] != Team.Player)
        {
            CallNextUnit(list);
        }
    }

    private void CallNextUnit(List<TacticsUnit> list)
    {
        foreach(TacticsUnit unit in list)
        {
            if (!unit.CheckStamina())
            {
                continue;
            }
            unit.BeginTurn();
            break;
        }
    }

    private void CallNextTeam()
    {
        currentTeam++;
        if(currentTeam > teamOrder.Count - 1)
        {
            currentTeam = 0;
            turn++;
        }

        StartCoroutine(TurnTransitionAnimation());
    }
}
