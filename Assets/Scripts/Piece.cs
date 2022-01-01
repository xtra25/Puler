using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public bool isDragging;
    public bool isDraggable = true;

    public bool isTheSlot;
    public Vector2 SlotPosition;



    // Start is called before the first frame update
    void Start()
    {
       
      
    }

    // Update is called once per frame
   
       void Update()
    {
      
       
       
    }

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
