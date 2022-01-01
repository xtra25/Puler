using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterSceneVars : MonoBehaviour
{
    //<Singleton>
    InterSceneVars _interSceneVars;
    public static InterSceneVars inst;
    //</Singleton>


   
    private static string chosenGame ="Animals";
    private static string lang = "cat";
    public float musicVol = 0.0001f;
    public float fXVol = 0.0001f;

    void Start()
    {//<Singleton>
        if (InterSceneVars.inst == null)
        { 
            InterSceneVars.inst = this;
            DontDestroyOnLoad(this.gameObject);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            _interSceneVars = GetComponent<InterSceneVars>();

        }
        else
           
            Destroy(gameObject);
        //</Singleton>


    }

    public static string ChosenGame
    {
        get
        {
            //Some other code
            return chosenGame;
        }
        set
        {
            //Some other code
            chosenGame = value;
        }
    }

    public static string Lang
    {
        get
        {
            //Some other code
            return lang;
        }
        set
        {
            //Some other code
            lang = value;
        }
    }


    public int LangToLocale(string lang)
    {
        int locale = 0;
        switch (lang)
        {
            case "cat":
                locale = 0;
                break;
            case "eng":
                locale = 1;
                break;
            case "esp":
                locale = 2;
                break;
        }
        return locale;
    }
}





