using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using Config;

public class GameManager
{
    public static float timer = 30;
    public static int turn = 1;
    public static bool executing = false;
    public static int init_deck_count = 14;
    public static void destroyAllIndicators()
    {

        foreach (GameObject indicator in indicators)
        {
            indicator.transform.SetParent(null);
            Object.Destroy(indicator);
        }

        indicators = new List<GameObject>();
    }

    public static void destroyAlldots()
    {
        foreach (GameObject dot in dots)
        {
            if (dot == null) continue;
            dot.transform.SetParent(null);
            Object.Destroy(dot);
        }
        dots = new List<GameObject>();
    }
    public static void endTurn() {
        switchTurn(); 
        GameManager player_data;
        
        if (GameManager.turn == 1)
        {
            player_data = GameManager.player1;
        }
        else
        {
            player_data = GameManager.player2;
        }

        if(player_data.hand.Count < 4 && player_data.deck.Count > 0 ) {
            addCardToHand();
        }
        
    }
    public static void switchTurn()
    {
        turn = (turn == 1 ? 2 : 1);
        timer = 30;
    }
    public static void addCardToHand()
    {
        GameManager player_data;
        GameObject hand;

        if (GameManager.turn == 1)
        {
            player_data = GameManager.player1;
            hand = GameObject.Find("Hand_P1");
        }
        else
        {
            player_data = GameManager.player2;
            hand = GameObject.Find("Hand_P2");
        }
        Card card = player_data.drawCard();

        GameObject card_object = Config.Helper.cardToGameObject(card);

        card_object.transform.SetParent(hand.transform, true);
        card_object.GetComponent<DragDrop>().player = GameManager.turn;
        card_object.GetComponent<DragDrop>().init(GameManager.turn, player_data.hand.Count - 1, card);
    }
    public static GameManager player1 = new GameManager();
    public static GameManager player2 = new GameManager();    
    public List<GameObject> piecesOnBoard = new List<GameObject>();

    public List<GameObject> filterList(Piece type)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject piece in piecesOnBoard)
        {
            if (piece == null)
            {
                continue;
            }
            if (piece.GetComponent<ChessPiece>().chessPieceType == type)
            {
                temp.Add(piece);
            }
        }
        return temp;
    }
    public static List<GameObject> dots = new List<GameObject>();
    public static List<GameObject> indicators = new List<GameObject>();
    public List<int> card_ingame = new List<int>();
    public List<GameObject> strike = new List<GameObject>();
    public GameObject selected_card;
    public List<Card> hand = new List<Card>();
    public List<Card> deck = new List<Card>();

    public GameManager()
    {
        // Initializes Each Player's Deck
        int index = 0;
        foreach (Card card in System.Enum.GetValues(typeof(Card)))
        {
            index++;
            deck.Insert(Random.Range(0, index), card);
            if (index == init_deck_count) break;
        }
    }

    // Draw A Card From The Deck
    public Card drawCard()
    {
        Card card = deck[0];
        deck.RemoveAt(0);
        hand.Add(card);
        return card;
    }
    public Card replaceCard(int index)
    {
        deck.Insert(Random.Range(0, deck.Count + 1), hand[index]);
        hand.RemoveAt(index);
        int index_deck = Random.Range(0, deck.Count);
        Card card = deck[index_deck];
        deck.RemoveAt(index_deck);

        hand.Insert(index, card);
        return card;
    }

    public static void lastDotClicked(bool has_card) {
        if(has_card) {
            GameManager.dots[0].GetComponent<SwapController>()?.deleteCard();
            GameManager.dots[0].GetComponent<DotController>()?.deleteCard();
            GameManager.dots[0].GetComponent<StrikeController>()?.deleteCard();
            GameManager.dots[0].GetComponent<ThunderBoltController>()?.deleteCard();
            GameManager.dots[0].GetComponent<DisarmController>()?.deleteCard();
        }
        GameManager.destroyAlldots();
        GameManager.destroyAllIndicators();
        GameManager.endTurn();
    }
}
