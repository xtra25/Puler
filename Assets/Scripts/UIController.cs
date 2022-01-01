using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pausepanel;
    SoundManager soundManager;


    // Start is called before the first frame update
    SpawnController spawnPieces;
    void Start()
    {
        spawnPieces = FindObjectOfType<SpawnController>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void WonBalloons()
    {
        GameObject btnBack = GameObject.Find("BtnBack");
        btnBack.SetActive(false);
        spawnPieces.MakeBalloons();
    }


    public void Backbutton()
    {
        pausepanel.SetActive(true);
        Piece[] pieces =  FindObjectsOfType<Piece>();
        soundManager.PlaySound("pause");
        foreach (Piece piece in pieces)
        {
            piece.isDraggable = false;
        }
    }

    public void Exitbutton()
    {     
        SceneManager.LoadScene("MainMenu");
        soundManager.PlaySound("unpause");
    }

    public void Continuebutton()
    {       
        Piece[] pieces = FindObjectsOfType<Piece>();
        foreach (Piece piece in pieces)
        {
            if (!piece.CompareTag("Dragged"))
            piece.isDraggable = true;
        }
        pausepanel.SetActive(false);
        soundManager.PlaySound("unpause");
    }



}
