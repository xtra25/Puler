using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Linq;

public class DragController_OLD : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isDragActive = false;

    private Vector2 screenPosition;

    private Vector3 worldPosition;

    private Vector2 originalPosition;

    Piece selectedPiece;
    Vector3 touch;
    int touchingControl;
    int totalTouches;
    int realTouchNum;
    GameManager gameManager;
    SoundManager soundManager;

    int fingerId;


    //////////////////// New Drag Controller
    private Finger MovementFinger;
    public Vector2 MovementAmount;
    private Finger currentFinger;
    private List<Finger> allFingers = new List<Finger>();
    private int fingerBtnTouchId;

    BalloonsController balloonController;
    //////////////////// New Drag Controller\\\\\\\\\\\\\\

    private void Awake()
    {
        DragController[] controller = FindObjectsOfType<DragController>();
        if (controller.Length < 1)
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        balloonController = FindObjectOfType<BalloonsController>();
    }

    //////////////////// New Drag Controller

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();

    }
    //////////////////// New Drag Controller\\\\\\\\\\\\\\

    private void HandleFingerDown(Finger finger)
    {
        touch = finger.screenPosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch), Vector2.zero);
    
        if (hit.collider != null && hit.collider.CompareTag("Balloon"))
        {
            balloonController.OnTouchBalloon(hit);
            return;
        }
     
        if ( fingerId == -1) // check if this is the first finger to touch the screen
        {
            fingerId = finger.index; // store the finger id
            try
            {
                if ( hit.collider != null)
                {
                    if (hit.collider.CompareTag("Piece") && hit.collider.GetComponent<Piece>().isDraggable)
                    {
                        SelectPiece(hit.collider);
                    }
                }
               
            }
            catch (InvalidCastException e)
            {
                // recover from exception
            }


            // perform object selection code here
        }

        /*
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Piece") && hit.collider.GetComponent<Piece>().isDraggable)
            {
                SelectPiece(hit.collider);              
            }
            else if (hit.collider.CompareTag("Balloon"))
            {
                balloonController.OnTouchBalloon(hit);
                return;
            }

        }
        */
    }

private void HandleFingerUp(Finger finger)
{
    if (fingerId == finger.index) // check if the lifted finger is the first finger to touch the screen
    {
        fingerId = -1; // reset the finger id
                       // perform object deselection code here
        Drop();
    }
}

private void HandleFingerMove(Finger finger)
{
    if (fingerId == finger.index) // check if this is the first finger to touch the screen
    {
            ETouch.Touch currentTouch = finger.currentTouch;
            if( selectedPiece != null)
            {
                selectedPiece.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.screenPosition.x, currentTouch.screenPosition.y, 1));
            }            
    }
}


private void SelectPiece(Collider2D hitSomething)
    {
        // touch = Touchscreen.current.position.ReadValue();     
        if (hitSomething.GetComponent<Piece>().isDraggable && !hitSomething.GetComponent<Piece>().isFinished)
        {
            selectedPiece = hitSomething.transform.gameObject.GetComponent<Piece>();
            OnlyOneMoving();
            isDragActive = true;
            originalPosition = selectedPiece.transform.position;
        }
        else
        {
            selectedPiece = null;
            RestoreMoving();
            isDragActive = false;
        }
        
        
    }

    private void OnlyOneMoving()
    {
        Piece[] pieces = FindObjectsOfType<Piece>();
        foreach (Piece p in pieces)
        {
            if (p.name != selectedPiece.name)
            {
                p.isDraggable = false;
            }
        }
    }

    private void RestoreMoving()
    {
        Piece[] pieces = FindObjectsOfType<Piece>();
        foreach (Piece p in pieces)
        {
            if (!p.isFinished)
            {
                p.isDraggable = true;
            }
        }
    }
 

    public void Drop()
    {
        if (isDragActive)
        {
            isDragActive = false;
            RestoreMoving();
            if (selectedPiece.isTheSlot)
            {
                selectedPiece.transform.position = selectedPiece.SlotPosition;
                selectedPiece.isDraggable = false;
                selectedPiece.isFinished = true;
                selectedPiece.tag = "Dragged";
                gameManager.SlotsToFill--;
                string piecename = selectedPiece.name;
                piecename = piecename.Replace("(Piece)", "");
                if (soundManager != null)
                {
                    soundManager.PlayPiece(piecename);
                }
               

            }
            else
            {
                selectedPiece.transform.position = originalPosition;
                if (soundManager != null)
                {
                    soundManager.PlaySound("piecesMoved");
                }
               

            }

        }
        selectedPiece = null;

    }


    
}
