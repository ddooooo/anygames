using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Config;

public class cardGenerator : MonoBehaviour
{
    public int player; 
    public static int result;
    private int num_cards; 
    GameObject card_base;
    void OnEnable()
    {
        player = PlayerPrefs.GetInt("player");
        result = PlayerPrefs.GetInt("result");
    }

    void Start()
    {
        card_base = GameObject.Find("Base");
        int result = PlayerPrefs.GetInt("result");
        if(player == 1 && result == (int)Constants.P1_First || player == 2 && result == (int)Constants.P2_First) {
            num_cards = 3; 
        } else {
            num_cards = 4; 
        }
        createCards(); 
    }

    public void createCards()
    {
        GameManager player = (this.player == 1) ? GameManager.player1 : GameManager.player2; 
        for(int i = 0; i < num_cards; i++) {
            Card card_drawn = player.drawCard(); 
            GameObject card = Helper.prefabNameToGameObject(Prefab.Mulligan.ToString()); 
            DragAndDrop component = card.GetComponent<DragAndDrop>();
            card.transform.SetParent(card_base.transform, true);
            component.init(card_drawn, this.player, i);
            int center_x = 0; 
            float displacement = 250f; 
            card.transform.position = card_base.transform.position + new Vector3(center_x - ((num_cards-0.75f) / 2) * displacement + (i * displacement), 0, 0);
        
        }
    }
}
