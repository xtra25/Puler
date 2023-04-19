using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    UIController uIController;
    SpawnController spawnController;
    [SerializeField] int slotsToFill = 3;
    int slotsRemains;
    Balloon balloon;
    int secondsToWait;
    SoundManager soundManager;
    public GameObject btnBack;
   // string chosenGame = InterSceneVars.ChosenGame;
   // InterSceneVars globalVars;


    // Start is called before the first frame update
    void Start()
    {    
         
        
       

        try
        {
            spawnController = FindObjectOfType<SpawnController>();
            soundManager = FindObjectOfType<SoundManager>();
            soundManager.UnloadPiecesSounds();
            soundManager.LoadPiecesSounds();
            
          
        }
        catch (Exception )
        {
            Console.WriteLine("Something went wrong.");
        }
       

        uIController = FindObjectOfType<UIController>();
        spawnController.LoadBackgrounds();
        spawnController.ChangeBackground();
        spawnController.LoadSpritesFromResources(InterSceneVars.ChosenGame);       
        spawnController.RandoomizeOrNot();        
        spawnController.MakePieces();
        slotsRemains = slotsToFill;
    }


    public int SlotsToFill
    {
        get { return slotsToFill; }
        set
        {
            slotsToFill = value;
            
            if (slotsToFill == 0)
            {
                soundManager.PlaySound("win");
                uIController.WonBalloons();

                balloon = FindObjectOfType<Balloon>();
                secondsToWait = balloon.SecondsToDestroyBalloons;

            
                if (spawnController.piecesToMake.Count() == 0)
                {
                    //ENDGAME
                    soundManager.PlaySound("pause");
                    soundManager.PlaySound("end");
                    StartCoroutine(LoadNextScene("MainMenu"));
                    soundManager.PlaySound("unpause");
                }
                else {
                    slotsToFill = slotsRemains;
                    StartCoroutine(NextLevel());
                    
                }

            }
          
        }

    }

    IEnumerator NextLevel()
    {
        
        yield return new WaitForSeconds(secondsToWait);  
        spawnController.CleanScreen();
        spawnController.ChangeBackground();
        spawnController.xOffset=-6;
        spawnController.MakePieces();
        btnBack.SetActive(true);
      
    }

    IEnumerator LoadNextScene(string sceneName)
    {
       
        secondsToWait = balloon.SecondsToDestroyBalloons;
        yield return new WaitForSeconds(secondsToWait);
        SceneManager.LoadScene(sceneName);

    }


}
