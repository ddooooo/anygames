using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GameManager.timer = GameManager.timer - Time.deltaTime;
        if(GameManager.timer <=0 ){
            GameManager.timer = 30;
            GameManager.endTurn();
        }
        this.GetComponent<Text>().text = Mathf.Floor(GameManager.timer).ToString();
    }
}
