using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsCamera : MonoBehaviour {

    public float moveSpeed;

    private static bool focus;
    private static Transform target;

	public static void FocusOn(GameObject unit)
    {
        focus = true;
        target = unit.transform;
    }

    private void Update()
    {
        //Controle de camera temporário
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -10;
            transform.position = mousePosition;
            focus = false;
        }

        if (focus)
        {
            if(Vector2.Distance(transform.position, target.position) > moveSpeed * Time.deltaTime)
            {
                Vector2 direction = target.position - transform.position;
                direction.Normalize();
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                Vector3 cameraPosition = target.position;
                cameraPosition.z = -10;
                transform.position = cameraPosition;
            }
        }
    }
}
