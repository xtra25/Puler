using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Linq;

public class DragController : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isDragActive = false;

    private Vector2 originalPosition;

    Piece selectedPiece;
    Vector3 fingerTouchPos;

    GameManager gameManager;
    SoundManager soundManager;

    private int fingerId = -1; // keep track of the finger id of the touch that is currently interacting with the target object

    private Finger MovementFinger;
    public Vector2 MovementAmount;
    private Finger currentFinger;
    private int fingerBtnTouchId;

    BalloonsController balloonController;


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

   
    private void HandleFingerDown(Finger finger)
    {
        // print("fingerDown");
        ExplodeBalloons(finger);

        if (IsFingerTouchingTarget(finger)) // check if the finger is touching the target GameObject
        {
            fingerId = finger.index; // store the finger id of the touch that is interacting with the target object
        
        }
    }

    private void HandleFingerUp(Finger finger)
    {
        if (finger.index == fingerId) // check if the lifted finger is the one that was interacting with the target object
        {
            fingerId = -1; // reset the finger id
                          
            Drop();
        }
    }

    private void HandleFingerMove(Finger finger)
    {
       
        if (finger.index == fingerId && IsFingerTouchingTarget(finger)) // check if the finger is the one that is interacting with the target object and is still touching the object
        {
    
            if (selectedPiece != null)
            {
                print("fingerMove withPiece Selected");
                selectedPiece.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(finger.currentTouch.screenPosition.x, finger.currentTouch.screenPosition.y, 1));
            }
        }
    }

    private bool IsFingerTouchingTarget(Finger finger)
    {
        foreach (var touch in finger.touchHistory) // loop through all the touches associated with the finger
        {
            fingerTouchPos = finger.screenPosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(fingerTouchPos), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Piece") && hit.collider.GetComponent<Piece>().isDraggable)
                {
                    if (selectedPiece == null)
                    {
                        SelectPiece(hit.collider);
                    }                    
                    return true;
                }
            }         
        }
        return false;
    }

    private void ExplodeBalloons(Finger finger)
    {
        fingerTouchPos = finger.screenPosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(fingerTouchPos), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Balloon"))
        {
            balloonController.OnTouchBalloon(hit);
            return;
        }
    }


    private void SelectPiece(Collider2D hitSomething)
    {
         
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
