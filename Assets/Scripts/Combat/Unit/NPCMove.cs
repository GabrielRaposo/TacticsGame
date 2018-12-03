using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove
{
    private GameObject target;

    private void Start()
    {
        Init();
    }

    private void CalculatePath()
    {
        if (target != null)
        {
            Tile targetTile = GetTargetTile(target);
            FindPath(targetTile);
        }
    }

    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject obj in targets)
        {
            float d = Vector2.Distance(transform.position, obj.transform.position);
            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }

    public override void CallMovement()
    {
        if (unit && unit.MovementPoint)
        {
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.Target = true;

            TacticsCamera.FocusOn(gameObject);
        }
    }

    public override void EndMovement()
    {
        unit.EndTurn();
    }
}
