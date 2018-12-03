using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe para auxiliar o controle de turnos de uma unidade
public class TacticsUnit : UnitData
{
    [Header("Tactics Components")]
    public GameObject healthDisplay;
    [HideInInspector] public PlayerPanel playerPanel;

    private TacticsHealth tacticsHealth;
    private TacticsMove tacticsMove;

    private bool movementPoint;
    public bool MovementPoint
    {
        get { return movementPoint; }
        set {
            movementPoint = value;
            if (!CheckStamina()) EndTurn();
        }
    }

    private bool actionPoint;
    public bool ActionPoint
    {
        get { return actionPoint; }
        set {
            actionPoint = value;
            if (!CheckStamina()) EndTurn();
        }
    }

    //---temp
    private SpriteRenderer _renderer;
    private Color originalColor;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        tacticsMove = GetComponent<TacticsMove>();

        hp = maxHp;
        if(healthDisplay != null)
        {
            healthDisplay = Instantiate(healthDisplay, transform.position + Vector3.down * .4f, Quaternion.identity, transform);
            tacticsHealth = healthDisplay.GetComponent<TacticsHealth>();
            if (tacticsHealth != null)
            {
                tacticsHealth.Init(maxHp);
            }
        }

        //TurnManager.AddUnit(this);
    }

    public void SetStamina(bool value)
    {
        movementPoint = value;
        actionPoint = value;

        ChangeColor(value);
    }

    public bool CheckStamina()
    {
        if(!movementPoint && !actionPoint)
        {
            return false;
        }
        return true;
    }

    public void BeginTurn()
    {
        if (tacticsMove)
        {
            tacticsMove.CallMovement();
        }
    }

    public void EndTurn()
    {
        SetStamina(false);

        TurnManager.instance.EndTurn();
    }

    public void SetActionTarget(Tile tile)
    {
        //if moving?
        if (tacticsMove && CompareTag("Player"))
        {
            tacticsMove.BuildPathToTile(tile);
        }

        //if attack 1?
        //if push?
        //if heal ?
        //if skill move + attack
        //if skill attack + move

        //---como organizar?
    }

    private void ChangeColor(bool active)
    {
        if (_renderer)
        {
            if (originalColor == (Color)Vector4.zero)
            {
                originalColor = _renderer.color;
            }
            Color _color = _renderer.color;
            _renderer.color = (active) ? originalColor : Color.grey;
        }
    }

    public void ChangeHealth(int value)
    {
        //TODO - apresentar dano

        hp += value;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        else if (hp < 0)
        {
            hp = 0;
            //TODO - unidade desmaia
        }

        if (tacticsHealth != null)
        {
            tacticsHealth.SetValue(hp);
        }
    }
}
