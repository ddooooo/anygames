using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Config;
public class DisarmController : MonoBehaviour, IPointerDownHandler
{
    public GameObject target;
    public GameObject card;
    public void deleteCard()
    {
        if (card) {
            int hand_index = card.GetComponent<DragDrop>().hand_index;
            card.GetComponent<DragDrop>().player_data.hand.RemoveAt(hand_index);
            Destroy(card);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        target.GetComponent<ChessPiece>().offensePower = 1;
        target.GetComponent<ChessPiece>().defensePower = 1;

        if(target.GetComponent<ChessPiece>().chessPieceType == Piece.Archer) {
            target.GetComponent<Archer>().attackRange = 4;
        }
        GameManager.lastDotClicked(true);
    }
}
