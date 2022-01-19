using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesController : MonoBehaviour
{
    //private Vector3 offset;
    private Vector2 origPos;
    private Vector2 SlotPosition;

    private Vector2 screenPosition;

    private Vector3 worldPosition;

    GameObject touchedPiece;
    GameManager gameManager;
    SoundManager soundManager;

  

   // bool isTheSlot = false;
    


    private void Start()
    {
       
        origPos = transform.position;
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();

    }
    private void Update()
    {
        
     
        //OnMouseDown
        if (Input.GetMouseButton(0))
        {

            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            screenPosition = new Vector2(worldPosition.x, worldPosition.y);
        }
        else if (Input.touchCount < 0)
            screenPosition = Input.GetTouch(0).position;
        
        
        RaycastHit2D hit = Physics2D.Raycast(screenPosition, Vector2.zero, 1 << LayerMask.NameToLayer("Pieces"));
          
        if (hit.collider != null)
        {
                
            //  Debug.Log(collider.name+ "GetMouseButtonDown");
            if (hit.collider.name.Contains("(Piece)") ){
                touchedPiece = hit.transform.gameObject;
                TouchDown(screenPosition, hit);
            }                
        }

        // if ((Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        //OnMouseUp
        if (Input.GetMouseButtonUp(0))
        {

            OnTouchUp();

        }


    }

    private void TouchDown(Vector2 mouseposition, RaycastHit2D hit)
    {// TODO onMouseDown
        origPos =  touchedPiece.transform.position;
        if (touchedPiece.GetComponent<Piece>().isDraggable) 
            {
            touchedPiece.transform.position = new Vector2(mouseposition.x, mouseposition.y);
        }
    }    

    
    private void OnTouchUp()
    {//TODO  OnMouseUp


        touchedPiece.transform.position = origPos;
       

    }
    
   
 

}
