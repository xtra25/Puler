using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsController : MonoBehaviour
{

    SoundManager soundManager;
    public GameObject sFX;
    public GameObject sMusic;

    InterSceneVars globalVars;
    Slider sliderFX;
    Slider sliderMusic;

    public AudioMixer mixer;
    public AudioMixerGroup music;
    public AudioMixerGroup fX;
    public GameObject pnlLanguage;
    public TMPro.TMP_Text txtNum1;
    public TMPro.TMP_Text txtNum2;
    public TMPro.TMP_InputField inptFieldSum;
    public GameObject pnlDonate;
    int rnd1;
    int rnd2;

    // Start is called before the first frame update
    void Start()
    {
        string lang = PlayerPrefs.GetString("Lang", InterSceneVars.Lang);
        GameObject btnLang = GameObject.Find("BtnLang");
        btnLang.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + lang);

        globalVars = FindObjectOfType<InterSceneVars>();
        sFX = GameObject.Find("SliderFX");
           
        sliderFX = sFX.GetComponent<Slider>();
        sMusic = GameObject.Find("SliderMusic");
        
        sliderMusic = sMusic.GetComponent<Slider>();

        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", globalVars.musicVol);
        sliderFX.value = PlayerPrefs.GetFloat("FXVolume", globalVars.fXVol);

        sliderFX.onValueChanged.AddListener(delegate { SetFXLevel(); });
        sliderMusic.onValueChanged.AddListener(delegate { SetMusicLevel(); });


        soundManager = FindObjectOfType<SoundManager>();
        soundManager.LoadResources();             
    }
  

    public void SetMusicLevel()

    {
        float sliderValue = sliderMusic.value;
        music.audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        globalVars.musicVol = sliderValue;
       
    }

    public void SetFXLevel()
    {
        float sliderValue = sliderFX.value;
        fX.audioMixer.SetFloat("FX", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("FXVolume", sliderValue);
        globalVars.fXVol = sliderValue;
      
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ValueChangeChek();
        }
        else if (Input.touchCount < 0) 
            ValueChangeChek();

        else return;

    }
  

    private void ValueChangeChek()
    {
        soundManager.PlaySound("piecesMoved");
    }

    public void BtnLang()
    {
        pnlLanguage.SetActive(true);
    }

    public void BtnBeer()
    {
        pnlDonate.SetActive(true);
        rnd1 = UnityEngine.Random.Range(0, 20);
        rnd2 = UnityEngine.Random.Range(0, 10);
        txtNum1.text = rnd1.ToString();
        txtNum2.text = rnd2.ToString();
    }


    public void ConfirmBeer()
    {
        string value = inptFieldSum.GetComponent<TMPro.TMP_InputField>().text;
        int result;
        int.TryParse(value, out result);
        if (result == rnd1+rnd2)
        {
            pnlDonate.SetActive(false);
          //  Application.OpenURL("https://www.paypal.com/donate?hosted_button_id=W4J4TTCKFRVTE");
        }else
            pnlDonate.SetActive(false);
    }

    public void BtnLangSelected()
    {
        int selectedLocale =0;
   
        GameObject btnLangSelected = EventSystem.current.currentSelectedGameObject;
        
        string lang = btnLangSelected.tag;
        GameObject btnLang = GameObject.Find("BtnLang");
        btnLang.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/"+lang);
        PlayerPrefs.SetString("Lang", lang);
        InterSceneVars.Lang = lang;

        selectedLocale= globalVars.LangToLocale(lang);
       
        pnlLanguage.SetActive(false);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLocale];
    }


    public void OKButton()
    {

        PlayerPrefs.Save();       
        SceneManager.LoadScene("MainMenu");
    }


   
 
}
