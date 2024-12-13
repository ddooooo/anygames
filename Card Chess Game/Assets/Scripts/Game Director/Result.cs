using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Config; 
public class Result : MonoBehaviour
{
    [SerializeField] Text player1;
    [SerializeField] Text result;
    [SerializeField] WheelController wheelScript;

    [SerializeField] private Image resultImage;
    // Start is called before the first frame update

    [SerializeField] private float delay = 10.0f;
    //[SerializeField] private string sceneToLoad;
    private float timeElapsed;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(wheelScript.finish()) {
            resultImage.gameObject.SetActive(true);
            result.text = (wheelScript.result() == (int) Constants.P1_First) ? "P1 First!" : "P2 First!"; 
            timeElapsed += Time.deltaTime;
            if(timeElapsed > delay) {
                PlayerPrefs.SetInt("result", (int) wheelScript.result());
                PlayerPrefs.SetInt("player", 1);
                loadNextScene(); 
            }
        }
    }
    private void loadNextScene() {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current+1);
    }
}
