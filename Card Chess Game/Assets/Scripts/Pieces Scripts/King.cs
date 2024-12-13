using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using Config;
public class King : MonoBehaviour {
    public int defensePower = 1;
    public void createDots() {
        List<GameObject> dots = new List<GameObject>();
        // 왕-이동: 상하좌우대각선 1칸
        for (int i = 0; i < 8; i++) {
            //int newIndexX = indexX + (CardSave.position[i,0]);
            //int newIndexY = indexY + (CardSave.position[i,1]);
            int newIndexX = GetComponent<ChessPiece>().indexX + (PieceConfig.move_list_surround[i,0]);
            int newIndexY = GetComponent<ChessPiece>().indexY + (PieceConfig.move_list_surround[i,1]);

            if(newIndexX > 4 || newIndexX < 0) {
                continue;
            }
            if(newIndexY > 7 || newIndexY < 0 ) {
                continue;
            }
            GameObject newCell = PieceConfig.cells[newIndexY, newIndexX];
            
            if(newCell.gameObject.transform.childCount > 0) {
                continue;
            }

            GameObject dot = Helper.prefabNameToGameObject(Prefab.Dot_Move.ToString());
            dot.transform.SetParent(newCell.transform, false);
            dot.transform.position = newCell.transform.position;
            dot.GetComponent<DotController>().parent = gameObject; 
            dots.Add(dot);
        }
        GameManager.dots = dots; 
    }
}
