using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public static int scoreOfPlayer1, scoreOfPlayer2;
    public static string nameOfPlayer1, nameOfPlayer2;
    public  bool PlayerDied = false ;
    public TMP_Text text;
    public static  bool Respwan = false;
    public static bool gameComplete;
  
    public int playerAlive = 1;

    // Start is called before the first frame update
    private void Awake()
    {
       
            instance = this;
            
       
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
        text.text = nameOfPlayer1 + " : " + scoreOfPlayer1.ToString() + "\n" + nameOfPlayer2 + " : " + scoreOfPlayer2.ToString();

        if(scoreOfPlayer1 + scoreOfPlayer2 >= 50)
        {
            gameComplete = true;
        }
        else
        {
            gameComplete = false;
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if(scoreOfPlayer1>scoreOfPlayer2)
            {
                text.text = nameOfPlayer1 + " : " + scoreOfPlayer1.ToString() + " is the winner";
            }
            if (scoreOfPlayer1 < scoreOfPlayer2)
            {
                text.text = nameOfPlayer2 + " : " + scoreOfPlayer2.ToString() + " is the winner";
            }
            if (scoreOfPlayer1 == scoreOfPlayer2)
            {

                text.text = nameOfPlayer1 + " : " + scoreOfPlayer1.ToString() + "\n" + nameOfPlayer2 + " : " + scoreOfPlayer2.ToString() +"DRAW";

            }
        }

       
    }

    public void Reswpan()
    {
       scoreOfPlayer1 = 0;
       scoreOfPlayer2 = 0;
        PlayerDied = false;
       SceneManager.LoadScene(1);
    }
    public void Reset()
    {
        scoreOfPlayer1 = 0;
        scoreOfPlayer2 = 0;
        PlayerDied = false;
    }
}
