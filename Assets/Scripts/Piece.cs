using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public bool isDragging;
    public bool isDraggable = true;
    public bool isFinished;

    public bool isTheSlot;
    public Vector2 SlotPosition;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        string piecename = name.Replace("(Piece)", "").Trim();
        string slotname = collision.name.Replace("(Slot)", "").Trim();
        if (slotname == piecename)
        {

         
            isTheSlot = true;
            SlotPosition = collision.transform.position;

        }
        else
        {
     
            isTheSlot = false;
        }
    }

}
