using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrikeController : MonoBehaviour, IPointerDownHandler
{
    public GameObject enemy;
    public GameObject card;
    public bool moveWhenAttack = true;
    public GameObject parent; 
    public void OnPointerDown(PointerEventData eventData) {
        deleteCard();

        {
            ChessPiece piece = enemy.GetComponent<ChessPiece>();
            int hp_enemy = piece.defensePower;
            int st_self = parent.GetComponent<ChessPiece>().offensePower;
            piece.defensePower = hp_enemy - st_self;

            if(piece.defensePower <= 0) {
                List<GameObject> list = piece.player_data.piecesOnBoard;
                int index = -1;
                for (int i=0; i<list.Count; i++) {
                    if(enemy == list[i]){
                        index = i;
                        break;
                    }
                }
                list.RemoveAt(index);
                Destroy(gameObject);
                Destroy(enemy); 
                moveParent();
            }
        }

        if(parent.GetComponent<ChessPiece>().offensePower > 1) {
            parent.GetComponent<ChessPiece>().offensePower--;
        }

        GameManager.destroyAllIndicators();
        GameManager.destroyAlldots();
        // Switch turn after strike
        GameManager.endTurn();
        
    }
    public void deleteCard()
    {
        if (card) {
            int hand_index = card.GetComponent<DragDrop>().hand_index;
            card.GetComponent<DragDrop>().player_data.hand.RemoveAt(hand_index);
            Destroy(card);
        }
    }

    public void moveParent(){
            Transform cellTransform = gameObject.transform.parent; // parent is a Cell gameobject

            parent.transform.SetParent(cellTransform); // selected piece의 부모는 selected piece가 이동할 cell
            parent.transform.position = cellTransform.position; // 위치 조정
            parent.GetComponent<ChessPiece>().indexX = cellTransform.gameObject.GetComponent<CellController>().indexX;
            parent.GetComponent<ChessPiece>().indexY = cellTransform.gameObject.GetComponent<CellController>().indexY;      
    }
}
