using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [Header("Temporary Unit Data")]
    public string title;
    public int level;
    public int weight;
    public int movement;

    [HideInInspector] public int hp;
    [HideInInspector] public int mp;

    [Header("Duel")]
    public int maxHp;
    public int maxMp;
    public int strength;
    public int intelligence;
    public int defense;
    public int resistence;
    public int speed;
    public int luck;

    //item list
}
