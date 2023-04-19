using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

    public string balloonName;
    [SerializeField] int secondsToWait = 5;
    private int randomSpeed;
    private Rigidbody2D rigidbody2Ds;
   
   
    public  int SecondsToDestroyBalloons =13;



    // Start is called before the first frame update
    void Start()
    {
    
        rigidbody2Ds = GetComponent<Rigidbody2D>();
        randomSpeed = Random.Range(2, 5);
        StartCoroutine(RandomWait());

        StartCoroutine(EndBalloons());

    } 
    

    IEnumerator EndBalloons()
    {

        yield return new WaitForSeconds(SecondsToDestroyBalloons);
        Destroy(gameObject);
    }

    IEnumerator RandomWait()
    {
        secondsToWait = Random.Range(0, secondsToWait);
        yield return new WaitForSeconds(secondsToWait);

        int maxzigzag = 100;

        for (int i = 0; i < maxzigzag; i++)
        {
            float waitDir = Random.Range(0.1f, 0.7f);
            yield return new WaitForSeconds(waitDir);
            int dir = Random.Range(0, 3);
            Direction(dir);
        }



    }

    private void Direction(int dir)
    {
        switch (dir)
        {
            case 0:
                rigidbody2Ds.velocity = new Vector2(0, 1) * randomSpeed; //up
                break;
            case 1:
                rigidbody2Ds.velocity = new Vector2(1, 1) * randomSpeed; // diagonal right
                break;
            case 2:
                rigidbody2Ds.velocity = new Vector2(-1, 1) * randomSpeed; //diagonal left
                break;

        }
    }
}

