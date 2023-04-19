using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{

    public static AudioClip piecesMoved,piecesCorrectlyDone, balloonPop, win,end,buttons,background,mainMenu;
    public List<AudioClip> piecesClips = new List<AudioClip>();
    public AudioSource audioSrc;
    public AudioSource audioSrcMusic;
    
    public AudioMixerGroup music;
    public AudioMixerGroup fX;

    public AudioMixer mixer;
    InterSceneVars globalVars;

    //<Singleton>
    SoundManager _soundManager;
    public static SoundManager inst;
    //</Singleton>


    // Start is called before the first frame update
    void Start()
    {

        //<Singleton>
        if (SoundManager.inst == null)
        { 
            SoundManager.inst = this;
            DontDestroyOnLoad(this.gameObject);
            _soundManager = GetComponent<SoundManager>();
            LoadResources();
            if (!audioSrcMusic.isPlaying)
            {
                PlaySound("background");
            }
        }
        else
        
            Destroy(gameObject);
        //</Singleton>
       
    }



    public void UnloadPiecesSounds()
    {
        piecesClips.Clear();
    }

    public void LoadResources()
    {
        globalVars = FindObjectOfType<InterSceneVars>();

        globalVars.musicVol = PlayerPrefs.GetFloat("MusicVolume", globalVars.musicVol);
        globalVars.fXVol = PlayerPrefs.GetFloat("FXVolume", globalVars.fXVol);


        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrcMusic = gameObject.AddComponent<AudioSource>();
        


        audioSrc.outputAudioMixerGroup = fX;
        audioSrc.volume =globalVars.fXVol;

        audioSrcMusic.outputAudioMixerGroup = music;
        audioSrcMusic.volume = globalVars.musicVol;

        background = Resources.Load<AudioClip>("Audio/background");
        piecesMoved = Resources.Load<AudioClip>("Audio/piecesMoved");
        balloonPop = Resources.Load<AudioClip>("Audio/balloonPop");
        win = Resources.Load<AudioClip>("Audio/win");
        end = Resources.Load<AudioClip>("Audio/end");
        buttons = Resources.Load<AudioClip>("Audio/buttons");
        mainMenu = Resources.Load<AudioClip>("Audio/mainMenu");
        audioSrcMusic.clip = background;
        audioSrcMusic.loop = true;

    }

    public void LoadPiecesSounds()
    {
        object[] Pieces;
        if (InterSceneVars.ChosenGame== "Animals")
        {
            Pieces = Resources.LoadAll("Audio/" + InterSceneVars.ChosenGame + "/", typeof(AudioClip));
        }
        else
            Pieces = Resources.LoadAll("Audio/" + InterSceneVars.Lang + "/" + InterSceneVars.ChosenGame + "/", typeof(AudioClip));

        foreach (AudioClip loaded in Pieces)
        {
            piecesClips.Add(loaded);
        }

    }

    public void PlayPiece(string clip)
    {
      
        AudioClip piece = piecesClips.Where(obj => obj.name == clip).SingleOrDefault();     
        audioSrc.PlayOneShot(piece);
    }

    public void PlaySound (string clip)
    {
        switch (clip)
        {
            case "pause":
                audioSrcMusic.Pause();
                break;
            case "unpause":
                if (!audioSrcMusic.isPlaying)
                {
                    audioSrcMusic.UnPause();
                }
                break;
            case "background":
                if (!audioSrcMusic.isPlaying)
                {
                    audioSrcMusic.Play();
                }
                break;
            case "piecesMoved":
                audioSrc.PlayOneShot(piecesMoved);
                break;
            case "piecesCorrectlyDone":
                audioSrc.PlayOneShot(piecesCorrectlyDone);                
                break;
            case "balloonPop":
                audioSrc.PlayOneShot(balloonPop);
                break;
            case "win":
                audioSrcMusic.PlayOneShot(win);
                break;
            case "end":
                audioSrcMusic.PlayOneShot(end);
                break;
            case "buttons":
                audioSrc.PlayOneShot(buttons);
                break;
            case "mainMenu":
                audioSrc.PlayOneShot(mainMenu);
                break;

        }
          
    }

   
}
