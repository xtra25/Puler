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
    
  /*  private void ONTouchMove(Vector2 mouseposition, Collider2D collider)
    { //TODO ONTouchMove OnMouseDrag
        touchedPiece = GameObject.Find(collider.name);
        if (touchedPiece.GetComponent<Piece>().isDragable)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            touchedPiece.transform.position = Camera.main.ScreenToWorldPoint(mousePos) + offset;
        }
    }*/
    
    private void OnTouchUp()
    {//TODO  OnMouseUp


        touchedPiece.transform.position = origPos;
        /*
        //transform.position = origPos;
        if (isTheSlot == true && touchedPiece.GetComponent<Piece>().isDraggable == true)
        {
            //asignar la variable de la posicio de slot guardada al ontrigger al transform de l'objecte arrossegat
            //  Debug.Log("Eureka");
            touchedPiece.transform.position = SlotPosition;
            //bloquejar animal en concret
            touchedPiece.GetComponent<Piece>().isDraggable = false;
            gameManager.SlotsToFill--;
            //SoundManager.PlaySound("piecesMoved");

            //agafar el nom de la peça i fer el PlayPiece ( nomdelapeça)
            //soundManager = FindObjectOfType<SoundManager>();
            string piecename = touchedPiece.name;
            piecename = piecename.Replace("(Piece)","");
            soundManager.PlayPiece(piecename);

        }
        else if (touchedPiece.GetComponent<Piece>().isDragable == true)
        {
            touchedPiece.transform.position = origPos;
            soundManager.PlaySound("piecesMoved");
        }
        */

    }
    
   
 

}
