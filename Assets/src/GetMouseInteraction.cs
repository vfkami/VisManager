using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMouseInteraction : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) //interaction with right click
        {
            print("mouse has clicked in visualization");
        }
    }
    void OnMouseDrag() //drag with left click
    {
        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z; //z
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
        transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);
    }
}
