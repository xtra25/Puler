using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isDragActive = false;

    private Vector2 screenPosition;

    private Vector3 worldPosition;

    private Piece lastDraggedPiece;

    private Vector2 originalPosition;

    Piece draggablePiece;

    GameManager gameManager;
    SoundManager soundManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragActive)
        {
            if ((Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
            {

                Drop();
                return;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            screenPosition = new Vector2(mousePos.x, mousePos.y);
        } 
        else if (Input.touchCount < 0)
            screenPosition = Input.GetTouch(0).position;
        else
        {
          return;
        }
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
       

        if (isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider!= null)
            {
                draggablePiece = hit.transform.gameObject.GetComponent<Piece>();
                if (draggablePiece != null )
                {
                    lastDraggedPiece = draggablePiece;
                    if (draggablePiece.isDraggable)
                    {
                        initDrag();
                    }
                }
            }
        }
    }

    void initDrag()
    {
        isDragActive = true;
        originalPosition = draggablePiece.transform.position;
    }

    private void Drag()
    {
        lastDraggedPiece.transform.position = new Vector2(worldPosition.x, worldPosition.y);
    }

    void Drop()
    {
        isDragActive = false;
        if (draggablePiece.isTheSlot)
        {
            draggablePiece.transform.position = draggablePiece.SlotPosition;
            draggablePiece.isDraggable = false;
            draggablePiece.tag = "Dragged";
            gameManager.SlotsToFill--;
            string piecename = draggablePiece.name;
            piecename = piecename.Replace("(Piece)", "");
            soundManager.PlayPiece(piecename);

        }
        else
        {
            draggablePiece.transform.position = originalPosition;
            soundManager.PlaySound("piecesMoved");

        }
            
    }


    
}
