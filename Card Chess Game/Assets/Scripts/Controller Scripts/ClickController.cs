using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void sceneOneButtonController(){
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current+1);
    }

    public void sceneThreeButtonController(){

        int player = PlayerPrefs.GetInt("player");
        int current = SceneManager.GetActiveScene().buildIndex;

        if(player == 1) {
            PlayerPrefs.SetInt("player", 2);
            SceneManager.LoadScene(current);
        }else {
            SceneManager.LoadScene(current+1);
        }

    }
}
