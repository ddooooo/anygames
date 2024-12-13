using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{
    [SerializeField] public int player;
    GameManager player_data;
    // Start is called before the first frame update
    void Start()
    {
        if(player == 1) {
            player_data = GameManager.player1;
        }else {
            player_data = GameManager.player2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // gameObject.GetComponent<Text>().text = player_data.myDeckCount.ToString();
    }
}
