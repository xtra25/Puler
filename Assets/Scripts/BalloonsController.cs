using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsController : MonoBehaviour
{
    SoundManager soundManager;
    private Vector2 screenPosition;
    private Vector3 worldPosition;
    GameObject touchedBalloon;
    [SerializeField] GameObject[] explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            screenPosition = new Vector2(mousePos.x, mousePos.y);
        }
       else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            screenPosition = Input.GetTouch(0).position;
        else return;
        

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
           
            if (hit.collider.name.Contains("(Balloon)"))
            {
               
                OnTouchBalloon(hit);
            }
                

        }
    }


    private void OnTouchBalloon(RaycastHit2D hit)
    {
       
     
        string ballonName = hit.collider.transform.name.Replace("(Balloon)", "").Trim();
        foreach (GameObject explosion in explosionPrefab)
        {
            string ExploName = explosion.transform.name.Replace("Explosion", "").Trim();


            if (ExploName == ballonName)
            {
                soundManager.PlaySound("balloonPop");
               
                Instantiate(explosion, hit.collider.transform.position, Quaternion.identity);
                break;
            }
        }


        Destroy(hit.collider.gameObject);

    }
}
