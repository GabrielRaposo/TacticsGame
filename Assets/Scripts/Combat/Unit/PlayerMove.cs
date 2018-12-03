using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
	void Start ()
    {
        Init();
	}

    public override void EndMovement()
    {
        unit.MovementPoint = false;
        if (unit.playerPanel)
        {
            unit.playerPanel.StartMainPanel(unit);
        }
    }
}
