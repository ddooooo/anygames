using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Config; 
public class cardGenerator_InGame : MonoBehaviour
{

    void Start()
    {
        
        GameObject handsPlayer1 = GameObject.Find("Hand_P1");
        GameObject handsPlayer2 = GameObject.Find("Hand_P2");

        // Player1
        for(int i=0; i<GameManager.player1.hand.Count; i++) {
            Card card = GameManager.player1.hand[i]; 
            GameObject card_object = Config.Helper.cardToGameObject(card);
            

            card_object.transform.SetParent(handsPlayer1.transform, true);

            card_object.GetComponent<DragDrop>().init(player: 1, hand_index: i, card: card); 
        }

        // Player2
        for(int i=0; i<GameManager.player2.hand.Count; i++) {
            Card card = GameManager.player2.hand[i]; 
            GameObject card_object = Config.Helper.cardToGameObject(card);
            

            card_object.transform.SetParent(handsPlayer2.transform, true);

            card_object.GetComponent<DragDrop>().init(player: 2, hand_index: i, card: card); 
        }
        GameManager.turn = PlayerPrefs.GetInt("result");

    }

}
