using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class SpawnController : MonoBehaviour
{
  
    public Sprite[] piecesSprites; 
    public float xOffset;
    float xOffsetNext;

    [SerializeField] Piece newPiece;
    [SerializeField] Slot newSlot;
    [SerializeField] Balloon newBalloon;
    [SerializeField] Sprite[] BalloonSprites;
    public List<int> randomSprites = new List<int>();
   
    public List<int> piecesToMake = new List<int>();
    // List<int> randomBakgrounds;
    // public List<Sprite> backgroundSprites = new List<Sprite>();
    Piece[] cleaningPieces;
    Slot[] cleaningSlots;
    Sprite[] backgroundSprites;
    string chosenGame = InterSceneVars.ChosenGame;
    GameManager gameManager;

    private void Start()
    {
        
          
    }


    public void MakePieces()
    {
       

        gameManager = GetComponent<GameManager>();
        int numPieces = gameManager.SlotsToFill;
        xOffsetNext=xOffset;
        int actualSprite;           
            
         
        if (piecesToMake.Count >= numPieces) 
        {

            for (int i = 0; i < numPieces; i++)
            {         
                actualSprite = piecesToMake[0];
                piecesToMake.RemoveAt(0);
                make(actualSprite);
            }
        }
        else
        {
            gameManager.SlotsToFill = piecesToMake.Count;
            for (int i = piecesToMake.Count; i > 0; i--)
            {
                actualSprite = piecesToMake[0];
                piecesToMake.RemoveAt(0);
                make(actualSprite);              
            }
        }
    }

    private void make(int actualSprite)
    {
        
        Sprite pieceSprite = piecesSprites[actualSprite];
        string pieceName = pieceSprite.name;
        newPiece.name = pieceName;
        newPiece.GetComponent<SpriteRenderer>().sprite = pieceSprite;

        Piece tempPiece = Instantiate(newPiece, new Vector3(xOffset, 3.3F, 0), Quaternion.identity);
        GameObject tempPieceAsGO = tempPiece.gameObject;
        //  PolygonCollider2D polygonCollider2D = tempPieceAsGO.AddComponent<PolygonCollider2D>();
        //  polygonCollider2D.isTrigger = true;
        CircleCollider2D circleCollider2D = tempPieceAsGO.AddComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        tempPiece.name = tempPiece.name.Replace("(Clone)", "(Piece)").Trim();
        //tempPiece.tag = "Clean";
        MakeSlot(pieceName, pieceSprite);

        xOffsetNext *= Mathf.Sign(xOffset);
        xOffset += xOffsetNext;


    }

    public void MakeSlot(string animalName, Sprite animalSprite)

    { 
        newSlot.name = animalName;
        newSlot.GetComponent<SpriteRenderer>().sprite = animalSprite;
       

        Vector2 slotCenter = RandomizeSlotPositions();
        
        //Detecta amb l'overlapcircle si es solapen ( poder es podria fer també amb el raycast ?) 
          Collider2D collider=null;
          collider = Physics2D.OverlapCircle(slotCenter, 3f);
           while (collider != null)
           {
           
              collider = null;
              slotCenter = RandomizeSlotPositions();
              collider = Physics2D.OverlapCircle(slotCenter, 3f);
           }


        Slot tempSlot = Instantiate(newSlot, slotCenter, Quaternion.identity);
        GameObject tempSlotAsGO = tempSlot.gameObject;
        //PolygonCollider2D polygonCollider2D = tempSlotAsGO.AddComponent<PolygonCollider2D>();
        //polygonCollider2D.isTrigger = true;
        CircleCollider2D circleCollider2D = tempSlotAsGO.AddComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = 0.2F;
        tempSlot.name = tempSlot.name.Replace("(Clone)", "(Slot)").Trim();
     //   tempSlot.GetComponent<PlayerInput>().enabled = false;
    }


 

    private Vector2 RandomizeSlotPositions() {
        float randomX = Random.Range(-7.7f, 7.6f); // de -7.7 a 7.6
        float randomY = Random.Range(-1.1f, -4.3f); // de -1.1 a -4.3
        Vector2 slotPos = new Vector2(randomX, randomY);
        return slotPos;


    }

    public void LoadSpritesFromResources(string folder)
    {
        string lang = InterSceneVars.Lang;
        Sprite loadspecial;
        object[] loadedSprites = Resources.LoadAll("Sprites/" + folder, typeof(Sprite));
        if (lang =="eng" || chosenGame!="Letters")
            piecesSprites = new Sprite[loadedSprites.Length];
        else
            piecesSprites = new Sprite[loadedSprites.Length+1];
       
        int i = 0;
        foreach (Sprite loaded in loadedSprites)
        {
            if (lang == "esp" && loaded.name == "letters O")
            {
              
                loadspecial = Resources.Load<Sprite>("Sprites/" + lang + "/" + folder  +"/letters Ñ" );                              
                piecesSprites[i] = loadspecial;
                i++;

            }
            else if (lang == "cat" && loaded.name == "letters D")
            {
                loadspecial = Resources.Load<Sprite>("Sprites/" + lang + "/" + folder  + "/letters Ç");             
                piecesSprites[i] = loadspecial;
                i++;
            }

            piecesSprites[i] = loaded;
            i++;

        }

    }
    internal void CleanScreen()
    {
        cleaningPieces = GameObject.FindObjectsOfType<Piece>();
        cleaningSlots = GameObject.FindObjectsOfType<Slot>();

        foreach (Piece piece in cleaningPieces)
        {                      
            GameObject objectToClean = GameObject.Find(piece.name);
            Destroy(objectToClean);

        }
        foreach (Slot slot in cleaningSlots)
        {      
            GameObject objectToClean = GameObject.Find(slot.name);
            Destroy(objectToClean);           
        }
    }

    internal void ChangeBackground()
    {
        
        SpriteRenderer spriteRenderer;
        
        GameObject background = GameObject.Find("/BackGround");      
        int random = Random.Range(0, backgroundSprites.Length);
        Sprite actualSprite = backgroundSprites[random];

        spriteRenderer = background.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = actualSprite;
    }

    internal void LoadBackgrounds()
    {
       
        object[] loadedBackgrounds = Resources.LoadAll("Backgrounds/", typeof(Sprite));
        backgroundSprites = new Sprite[loadedBackgrounds.Length];


        int i = 0;
        foreach (Sprite loaded in loadedBackgrounds)
        {
            backgroundSprites[i] = loaded;
            i++;
        }
     
    }

  

    public void MakeBalloons()
    {
      

        int randomBallonNumber = Random.Range(20, 25);
        for (int i = 0; i < randomBallonNumber; i++)
        {
            int randomX = Random.Range(-8, 10); ; 
            int randomBalloonSprite = Random.Range(0, BalloonSprites.Length);
            Sprite balloonSprite = BalloonSprites[randomBalloonSprite];
            string balloonName = balloonSprite.name;
            newBalloon.name = balloonName;
            newBalloon.GetComponent<SpriteRenderer>().sprite = balloonSprite;
            Balloon rename = Instantiate(newBalloon, new Vector3(randomX, -7F, 0), Quaternion.identity);
            rename.name = rename.transform.name.Replace("(Clone)", "(Balloon)").Trim();     


        }
    }

    public void RandoomizeOrNot()
    {
       
        if (chosenGame == "Animals")
        {
            piecesToMake = RandomSprites(piecesSprites);
        }
        else
        {
            for (int i = 0; i < piecesSprites.Length; i++)
            {
                piecesToMake.Add(i);
            }
        }
    }
   
    public List<int> RandomSprites(Sprite[] sprites)
    {
        int temp;
       
            do  
            {
            temp = Random.Range(0, sprites.Length);
                if (!randomSprites.Contains(temp)){
                    randomSprites.Add(temp);
               
                }
                   
            } while (randomSprites.Count != sprites.Length);

       return randomSprites;
    }

   
}
