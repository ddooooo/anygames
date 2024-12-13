using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using Config;
public class Archer : MonoBehaviour {
    public int offensePower = 1;
    public int defensePower = 1;
    public int attackRange = 4;
    public int numEnemy = 0;
    public bool onlyAttack = false;
    public GameObject createStrikeDot(GameObject cell, GameObject enemy, GameObject card, GameObject parent) {
        GameObject striking = Helper.prefabNameToGameObject(Prefab.Arrow.ToString());
        striking.transform.SetParent(cell.transform, false); // Parent is Cell GameObject
        striking.transform.position = cell.transform.position;
        striking.GetComponent<ArrowController>().enemy = enemy;
        striking.GetComponent<ArrowController>().card = card;
        striking.GetComponent<ArrowController>().moveWhenAttack = false;
        striking.GetComponent<ArrowController>().parent = parent;
        return striking;
    }

    public GameObject createMoveDot(GameObject newCell, GameObject card) {
        GameObject move = Helper.prefabNameToGameObject(Prefab.Dot_Move.ToString());
        move.transform.SetParent(newCell.transform, false);
        move.transform.position = newCell.transform.position;
        move.GetComponent<DotController>().parent = gameObject; 
        move.GetComponent<DotController>().card = card;
        return move;
    }

    public void createArrowArcher(List<int[]> move_list, GameObject card = null, int num_attack = 1)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < 3; i++) {
            for(int j = 1; j<attackRange; j++) {
                int newX_strike = GetComponent<ChessPiece>().indexX + (move_list[i][0]*j);
                int newY_strike = GetComponent<ChessPiece>().indexY + (move_list[i][1]*j);
                
                // Check the location is out of bound 
                if(newX_strike > 4 || newX_strike < 0) {
                    continue;
                }
                if(newY_strike > 7 || newY_strike < 0 ) {
                    continue;
                }

                GameObject newCell_Stirke = PieceConfig.cells[newY_strike, newX_strike ];
                if(newCell_Stirke.gameObject.transform.childCount > 0) {
                    //if(newCell_Stirke.gameObject.transform.GetChild(0).name == "dot_move(Clone)" ) continue;
                    
                    // The enemy's Piece is located within the range of archer's attack, then create a strike dot    
                    if(newCell_Stirke.transform.GetChild(0).GetComponent<ChessPiece>().player != GetComponent<ChessPiece>().player ) { 
                        dots.Add(createStrikeDot(newCell_Stirke, newCell_Stirke.transform.GetChild(0).gameObject, card, this.gameObject));
                        numEnemy++;
                    }

                    // If there is any blocking piece, then further iteration is no needed 
                    // (the piece further than blocking piece cannot be attacked)
                    break; 
                }else if(!onlyAttack){
                    // If there is no blocking piece on the location where archer can move to
                    if(j==1){
                        dots.Add(createMoveDot(newCell_Stirke, card));
                    }
                }
            }
        }
        ArrowController.numAttack = System.Math.Min(num_attack, numEnemy); 
        onlyAttack = false;
        GameManager.dots = dots; 
    }

}
