using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using Config;
public class ChessPiece : MonoBehaviour, IPointerDownHandler
{
    public int offensePower;
    public int defensePower;
    public int indexX;
    public int indexY;
    public Piece chessPieceType;
    public int player;
    public GameManager player_data;
    public List<GameObject> indicators = null;
    public bool activated = false; // either selected by card or clicking the piece itself
    public List<int[]> basic_moves = new List<int[]>(); 
    public int move_dir; 

    void Start()
    {

        offensePower = 1;
        defensePower = 1;

        basic_moves.Add(new int[]{-1, 0}); 
        basic_moves.Add(new int[]{1, 0}); 
        if (player == 1)
        {
            player_data = GameManager.player1;
            basic_moves.Add(new int[]{0, 1}); 
            move_dir = 1; 

        }
        else
        {
            player_data = GameManager.player2;
            basic_moves.Add(new int[]{0, -1}); 
            move_dir = -1; 
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // exits if not piece owner's turn 
        if (player != GameManager.turn) return;
        // remove all indicators and dots
        GameManager.destroyAllIndicators(); 
        GameManager.destroyAlldots(); 
        bool activated = this.activated; 

        // If the piece is active //
        if (activated)
        {
            activated = false; 
            return;
        }
        activated = true; 

        // create indicator around the piece itself
        addIndicator(); 
        if(chessPieceType == Piece.King) {
            GetComponent<King>().createDots(); // Speical move for King
        } else if(chessPieceType == Piece.Archer){
            GetComponent<Archer>().numEnemy = 0;
            GetComponent<Archer>().createArrowArcher(basic_moves);
        }else {
            createDots(basic_moves); 
        }
    }

    public GameObject createDot(GameObject cell, GameObject card) {
        GameObject dot = Helper.prefabNameToGameObject(Prefab.Dot_Move.ToString());
        dot.transform.SetParent(cell.transform, false);
        dot.transform.position = cell.transform.position;
        dot.GetComponent<DotController>().parent = gameObject; 
        dot.GetComponent<DotController>().card = card;
        return dot; 
    }

    public void createDots(List<int[]> move_list, GameObject card = null) {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < move_list.Count; i++) {
            int[] coordinates = move_list[i]; 
            int newIndexX = GetComponent<ChessPiece>().indexX + (move_list[i][0]);
            int newIndexY = GetComponent<ChessPiece>().indexY + (move_list[i][1]);
            if(newIndexX > 4 || newIndexX < 0) {
                continue;
            }
            if(newIndexY > 7 || newIndexY < 0 ) {
                continue;
            }
            GameObject newCell = PieceConfig.cells[newIndexY, newIndexX];
            
            if(newCell.gameObject.transform.childCount > 0) {
                //if(newCell.gameObject.transform.GetChild(0).name == "dot_move(Clone)" ) continue;
                // 말이 적일 경우
                if(newCell.transform.GetChild(0).GetComponent<ChessPiece>().player != GetComponent<ChessPiece>().player ) {
                    dots.Add(createStrike(newCell, newCell.transform.GetChild(0).gameObject, card));
                }
                continue;
            }

            dots.Add(createDot(newCell, card));
        }
        GameManager.dots = dots; 
    }

    public void addIndicator()
    {
        GameObject selected_indicator = Helper.prefabNameToGameObject(Prefab.Selected_Indicator.ToString());
        selected_indicator.transform.SetParent(gameObject.transform);
        selected_indicator.transform.position = transform.position;
        GameManager.indicators.Add(selected_indicator); 
    }

    public GameObject createStrike(GameObject cell, GameObject enemy, GameObject card) {
        GameObject attacking = Helper.prefabNameToGameObject(Prefab.Attacking.ToString());
        attacking.transform.SetParent(cell.transform, false); // Parent is Cell GameObject
        attacking.transform.position = cell.transform.position;
        attacking.GetComponent<StrikeController>().enemy = enemy;
        attacking.GetComponent<StrikeController>().card = card;
        attacking.GetComponent<StrikeController>().parent = gameObject;

        return attacking;
    }

    public void destroyIndicator()
    {
        if (this.transform.childCount != 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
}
