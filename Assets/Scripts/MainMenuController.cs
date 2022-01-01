using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
  
  
    InterSceneVars globalVars;

    bool loadingScene;
    SoundManager soundManager;
    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;

        soundManager = FindObjectOfType<SoundManager>();
        globalVars = FindObjectOfType<InterSceneVars>();
        InterSceneVars.Lang = PlayerPrefs.GetString("Lang", InterSceneVars.Lang);

        Debug.Log(InterSceneVars.Lang);

        int selectedLocale = globalVars.LangToLocale(InterSceneVars.Lang);
        
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLocale];
    }


        // Update is called once per frame
        void Update()
    {

    }

    public void StartAnimals()
    {
        if (loadingScene == true)
        {
          
            return;
        }
   
        InterSceneVars.ChosenGame = "Animals";
        StartCoroutine(LoadNextScene("Game"));
        
    }
    public void StartLetters()
    {
        if (loadingScene == true)
        {
            return;
        }
       
        InterSceneVars.ChosenGame = "Letters";
        StartCoroutine(LoadNextScene("Game"));
    }

    public void StartNumbers()
    {
        if (loadingScene == true)
        {
            return;
        }
    
        InterSceneVars.ChosenGame = "Numbers";
        StartCoroutine(LoadNextScene("Game"));
    }

    public void StartSettings()
    {
     
        if (loadingScene == true)
        {
            return;
        }
        StartCoroutine(LoadNextScene("Settings"));
        soundManager.PlaySound("buttons");
      
 
    }

    public void QuitGame()
    {
   
        Application.Quit();
    }

    IEnumerator LoadNextScene(string sceneName)
    {
        loadingScene = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);

    }

}
