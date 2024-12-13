using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/* 
    Configuration File For Everything Related To Cards 
*/
namespace Config
{
    public enum CardGrade
    {
        Normal,
        Rare,
        Epic,
        Legendary
    }
    public enum Card
    {
        Knights_Move, // 0
        Arrow_Of_Madness, // 1
        Rush, // 2
        Heavy_Armor, // 3
        Teleport, // 4
        Morale_Boost, // 5
        Clairvoyant, // 6
        King_Power, // 7
        Double_Shot, // 8
        Penetrating_Arrow, // 9
        Rage_Attack, // 10
        Swap, // 11
        Disarm, // 12
        Thunder_Bolt, // 13
        Disguise, // 12
        Testudo, // 13
        Royal_Dagger, // 14
        Losing_Sacrifice, // 15
        Last_Ditch_Effor, // 16
        Demotion_Punishment, // 17
        Diagonal_Shot, // 18
        Longbow_Shot, // 20
        Spear_Throw, // 21
        Shield, // 22
    };

    public delegate bool CardFunction(GameObject piece, GameObject card);

    public class CardConfig
    {
        public static Dictionary<Card, (CardFunction, Piece, CardGrade)> card_dict = new Dictionary<Card, (CardFunction, Piece, CardGrade)>()
        {
            {   // Knight's Move Start
                Card.Knights_Move, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Knights_Move(나이트 빙의): 체스 나이트 움직임으로 공격 및 이동
                        if (!GameManager.executing)
                        {
                            int[,] move_list_core = { { 1, 2 }, { 2, 1 } };
                            List<int[]> move_list = new List<int[]>();
                            List<GameObject> dots = new List<GameObject>();

                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    for (int k = 0; k < move_list_core.GetLength(0); k++)
                                    {
                                        int diffX = move_list_core[k, 0];
                                        int diffY = move_list_core[k, 1];
                                        if (i == 1)
                                        {
                                            diffX *= -1;
                                        }
                                        if (j == 1)
                                        {
                                            diffY *= -1;
                                        }
                                        move_list.Add(new int[] { diffX, diffY });
                                    }
                                }
                            }
                            piece.GetComponent<ChessPiece>().createDots(move_list, card);
                        }
                        return true; // later to be changed 
                    }, 
                    Piece.Mage, // Card Target 
                    CardGrade.Normal // Card Grade
                )
            },  // Knight's Move End
            {   // Arrow Of Madness Start
                Card.Arrow_Of_Madness, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //Arrow_Of_Madness(광기의 화살): 가로, 세로, 대각선 모든 방향으로 1발씩 쏨
                        if (!GameManager.executing) 
                        {
                            List<GameObject> dots = new List<GameObject>();
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 1; j < 8; j++)
                                {
                                    int newX_strike = piece.GetComponent<ChessPiece>().indexX + (PieceConfig.move_list_surround[i, 0] * j);
                                    int newY_strike = piece.GetComponent<ChessPiece>().indexY + (PieceConfig.move_list_surround[i, 1] * j);

                                    // Check the location is out of bound 
                                    if (newX_strike > 4 || newX_strike < 0)
                                    {
                                        continue;
                                    }
                                    if (newY_strike > 7 || newY_strike < 0)
                                    {
                                        continue;
                                    }

                                    GameObject newCell_Stirke = PieceConfig.cells[newY_strike, newX_strike];
                                    if (newCell_Stirke.gameObject.transform.childCount > 0)
                                    {
                                        // The enemy's Piece is located within the range of archer's attack, then create a strike dot    
                                        dots.Add(piece.GetComponent<ChessPiece>().createStrike(newCell_Stirke, newCell_Stirke.transform.GetChild(0).gameObject, card));

                                        break; // If there is any blocking piece, then further iteration is no needed (the piece further than the blocking piece cannot be attacked)
                                    }
                                }
                            }
                            GameManager.dots = dots;
                        }
                        else
                        {
                            GameManager.dots[0].GetComponent<StrikeController>().deleteCard();
                            foreach (GameObject strike_dot in GameManager.dots)
                            {
                                GameObject destoryPiece = strike_dot.transform.parent.GetChild(0).gameObject;
                                int st_self = piece.GetComponent<ChessPiece>().offensePower;
                                int hp_enemy = destoryPiece.GetComponent<ChessPiece>().defensePower;
                                if (st_self < hp_enemy)
                                {
                                    destoryPiece.GetComponent<ChessPiece>().defensePower = hp_enemy - st_self;
                                    continue;
                                }
                                UnityEngine.Object.Destroy(destoryPiece);
                            }
                            GameManager.destroyAlldots();
                            GameManager.destroyAllIndicators();
                            GameManager.endTurn();
                        }
                        return true; 
                    }, 
                    Piece.Archer, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // Arrow_Of_Madness End
            {   // Rush Start
                Card.Rush, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //Rush(돌진): 3칸 앞으로 전진하면서 경로에 있는 말 모두 제거

                        bool move_available = false; 
                        List<GameObject> dots = new List<GameObject>();
                        if (!GameManager.executing)
                        {
                            for (int i = 1; i < 4; i++)
                            {
                                int newX_strike = piece.GetComponent<ChessPiece>().indexX;
                                int newY_strike = piece.GetComponent<ChessPiece>().indexY + i * piece.GetComponent<ChessPiece>().move_dir;

                                // Check the location is out of bound 
                                if (newX_strike > 4 || newX_strike < 0)
                                {
                                    continue;
                                }
                                if (newY_strike > 7 || newY_strike < 0)
                                {
                                    continue;
                                }
                                move_available = true; 
                                GameObject newCell = PieceConfig.cells[newY_strike, newX_strike];

                                if (newCell.gameObject.transform.childCount > 0)
                                {
                                    // If a piece of any side is located within the range of archer's attack, then create a strike dot    
                                    dots.Add(piece.GetComponent<ChessPiece>().createStrike(newCell, newCell.transform.GetChild(0).gameObject, card));
                                }
                                else {
                                    dots.Add(piece.GetComponent<ChessPiece>().createDot(newCell, card));
                                }
                            }
                            GameManager.dots = dots;
                            return move_available; 
                        }
                        else
                        {
                            if (GameManager.dots.Count > 0)
                            {
                                GameManager.dots[0].GetComponent<StrikeController>()?.deleteCard();
                                GameManager.dots[0].GetComponent<DotController>()?.deleteCard();
                                foreach (GameObject dot in GameManager.dots)
                                {

                                    dot.GetComponent<StrikeController>()?.moveParent();
                                    dot.GetComponent<DotController>()?.moveParent();
                                    UnityEngine.Object.Destroy(dot.transform.parent.GetChild(0).gameObject);
                                }
                            }
                            GameManager.destroyAlldots();
                            GameManager.destroyAllIndicators();
                            GameManager.endTurn(); 
                            return true; 
                        }
                    }, 
                    Piece.Warrior, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // Rush End
            {   // Heavy_Armor Start
                Card.Heavy_Armor, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Heavy Arm(중무장): 방패+1, 공격+1
                        if (GameManager.executing)
                        {
                            piece.GetComponent<ChessPiece>().offensePower++;
                            piece.GetComponent<ChessPiece>().defensePower++;
                            GameManager.destroyAllIndicators();
                            card.GetComponent<DragDrop>().destoryCard();
                            GameManager.endTurn();                        
                        }
                        return true; 
                    }, 
                    Piece.Warrior, // Card Target 
                    CardGrade.Epic // Card Grade
                )
            },  // Heavy_Armor End
            {   // Teleport Start
                Card.Teleport, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Teleport(순간이동): 아무 위치로 순간이동
                        bool move_available = false;
                        List<GameObject> dots = new List<GameObject>();
                        if (!GameManager.executing)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    GameObject cell = PieceConfig.cells[j, i];
                                    if (cell.transform.childCount > 0)
                                    {
                                        continue;
                                    }
                                    move_available = true; 
                                    GameObject dot = Helper.prefabNameToGameObject(Prefab.Dot_Move.ToString()); 
                                    dot.transform.SetParent(cell.transform, false);
                                    dot.transform.position = cell.transform.position;
                                    dot.GetComponent<DotController>().parent = piece;
                                    dot.GetComponent<DotController>().card = card;
                                    dots.Add(dot);
                                }
                            }
                            GameManager.dots = dots;
                            return move_available; 
                        }
                        return true; 
                    }, 
                    Piece.Mage, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // Teleport End
            {   // Thunder_Bolt Start
                Card.Thunder_Bolt, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Thunder Bolt(썬더볼트): 인접 대상에 발동 가능, 피격 대상 공격 및 인접 대상을 2개까지 공격
                        bool move_available = false; 
                        List<GameObject> dots = new List<GameObject>();
                        if (!GameManager.executing)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                int newX_strike = piece.GetComponent<ChessPiece>().indexX + (PieceConfig.move_list_surround[i, 0]);
                                int newY_strike = piece.GetComponent<ChessPiece>().indexY + (PieceConfig.move_list_surround[i, 1]);
                                // Check the location is out of bound 
                                if (newX_strike > 4 || newX_strike < 0)
                                {
                                    continue;
                                }
                                if (newY_strike > 7 || newY_strike < 0)
                                {
                                    continue;
                                }
                                move_available = true; 

                                GameObject newCell_Strike = PieceConfig.cells[newY_strike, newX_strike];
                                if (newCell_Strike.transform.childCount > 0)
                                {
                                    GameObject enemyPiece = newCell_Strike.transform.GetChild(0).gameObject;
                                    if (enemyPiece.GetComponent<ChessPiece>().player != piece.GetComponent<ChessPiece>().player)
                                    {
                                        GameObject dot = Helper.prefabNameToGameObject(Prefab.Thunder_Bolt.ToString()); 
                                        dot.transform.SetParent(newCell_Strike.transform, false);
                                        dot.transform.position = newCell_Strike.transform.position;
                                        dot.GetComponent<ThunderBoltController>().parent = piece;
                                        dot.GetComponent<ThunderBoltController>().card = card;
                                        dots.Add(dot);
                                    }
                                }
                            }
                            GameManager.dots = dots;
                            return move_available;
                        }
                        return true; 
                    }, 
                    Piece.Mage, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // Teleport End
            {   // Rage_Attack Start
                Card.Rage_Attack, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Rage Attack(분노의 일격): 방어력을 무시하고 공격
                        if (!GameManager.executing)
                        {
                            bool move_available = false; 
                            List<GameObject> dots = new List<GameObject>();
                            int newIndexX = piece.GetComponent<ChessPiece>().indexX;  
                            int newIndexY = piece.GetComponent<ChessPiece>().indexY + piece.GetComponent<ChessPiece>().move_dir;
                                if(newIndexX > 4 || newIndexX < 0) {
                                    goto GOTO1;
                                }
                                if(newIndexY > 7 || newIndexY < 0 ) {
                                    goto GOTO1;
                                }
                                GameObject newCell = PieceConfig.cells[newIndexY, newIndexX];
                                if (newCell.gameObject.transform.childCount > 0)
                                {
                                    // The enemy's Piece is located within the range of archer's attack, then create a strike dot    
                                    dots.Add(piece.GetComponent<ChessPiece>().createStrike(newCell, newCell.transform.GetChild(0).gameObject, card));
                                    move_available = true; 
                                }
                            GameManager.dots = dots; 
                            GOTO1: return move_available; 
                        } else {
                            if (GameManager.dots.Count > 0)
                            {
                                GameManager.dots[0].GetComponent<StrikeController>().deleteCard();
                                foreach (GameObject strike_dot in GameManager.dots)
                                {
                                    strike_dot.GetComponent<StrikeController>().moveParent();
                                    UnityEngine.Object.Destroy(strike_dot.transform.parent.GetChild(0).gameObject);
                                }
                                GameManager.destroyAlldots();
                                GameManager.destroyAllIndicators();
                                GameManager.endTurn(); 
                                return true; 
                            }
                            return false;
                        }
                    }, 
                    Piece.Warrior, // Card Target 
                    CardGrade.Epic // Card Grade
                )
            },  // Rage_Attack End
            {   // Morale_Boost Start
                Card.Morale_Boost, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //Morale Boost(사기증진): 자신 주변 3x3 범위안에 있는 아군한테 모두 공격력 +1 부여
                        if(GameManager.executing) {
                            int currX = piece.GetComponent<ChessPiece>().indexX;
                            int currY = piece.GetComponent<ChessPiece>().indexY;

                            for(int i=0; i<8; i++) {
                                int newX = currX + (PieceConfig.move_list_surround[i, 0]);
                                int newY = currY + (PieceConfig.move_list_surround[i, 1]);

                                // Check the location is out of bound 
                                if(newX > 4 || newX < 0) {
                                    continue;
                                }
                                if(newY > 7 || newY < 0 ) {
                                    continue;
                                }
                                GameObject newCell = PieceConfig.cells[newY, newX];
                                if(newCell.transform.childCount > 0) {
                                    GameObject friendlyPiece = newCell.transform.GetChild(0).gameObject;
                                    if(piece.GetComponent<ChessPiece>().player == friendlyPiece.GetComponent<ChessPiece>().player) {
                                        friendlyPiece.GetComponent<ChessPiece>().offensePower++;
                                    }
                                }
                            }
                            GameManager.destroyAllIndicators();
                            card.GetComponent<DragDrop>().destoryCard();
                            GameManager.endTurn();                            
                        }
                        return true; 
                    }, 
                    Piece.King, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // Morale_Boost End
            {   // Clairvoyant Start
                Card.Clairvoyant, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //Clairvoyant(천리안): 장착효과, 사거리 무한대로 증가
                        if(GameManager.executing) {
                            piece.GetComponent<Archer>().attackRange = 8;
                            GameManager.destroyAllIndicators();
                            card.GetComponent<DragDrop>().destoryCard();
                            GameManager.endTurn();
                        }
                        return true;  
                    }, 
                    Piece.Archer, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // Clairvoyant End
            {   // King_Power Start
                Card.King_Power, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //King's Power(왕의 일격): 범위에 있는 적 모두 섬멸
                        //     x     x     x
                        //       x   x   x
                        //         x x x
                        //           k
                        bool move_available = false;
                        List<GameObject> dots = new List<GameObject>();
                        if(!GameManager.executing){
                            for(int i=1; i<4; i++) {
                                
                                for(int j = -i; j<i+1; j+=i){
                                    int newX_strike = piece.GetComponent<ChessPiece>().indexX + (PieceConfig.move_list_surround[2, 0]) + j;
                                    int newY_strike = piece.GetComponent<ChessPiece>().indexY + (PieceConfig.move_list_surround[2, 1])*i;

                                    // Check the location is out of bound 
                                    if(newX_strike > 4 || newX_strike < 0) {
                                        continue;
                                    }
                                    if(newY_strike > 7 || newY_strike < 0 ) {
                                        continue;
                                    }

                                    GameObject newCell_Strike = PieceConfig.cells[newY_strike, newX_strike];
                                    if(newCell_Strike.gameObject.transform.childCount > 0) {
                                        GameObject enemyPiece = newCell_Strike.transform.GetChild(0).gameObject;
                                        if( Helper.isEnemy(piece, enemyPiece) ) { 
                                            // The enemy's Piece is located within the range of archer's attack, then create a strike dot    
                                            dots.Add(piece.GetComponent<ChessPiece>().createStrike(newCell_Strike, newCell_Strike.transform.GetChild(0).gameObject, card));
                                            move_available = true;
                                        }
                                    }
                                }
                            }
                            GameManager.dots = dots;
                        }else {
                            foreach(GameObject strike_dot in GameManager.dots) {
                                GameObject destoryPiece = strike_dot.transform.parent.GetChild(0).gameObject;
                                int st_self = piece.GetComponent<ChessPiece>().offensePower;
                                int hp_enemy = destoryPiece.GetComponent<ChessPiece>().defensePower;
                                if(st_self < hp_enemy) {
                                    destoryPiece.GetComponent<ChessPiece>().defensePower = hp_enemy - st_self;
                                    continue;
                                }
                                UnityEngine.Object.Destroy(destoryPiece);
                            }   
                            
                            GameManager.lastDotClicked(true);
                            return true;
                        }
                        return move_available;
                    }, 
                    Piece.King, // Card Target 
                    CardGrade.Legendary // Card Grade
                )
            },  // King_Power End
            {   // Double_Shot Start
                Card.Double_Shot, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //Double Shot(더블 샷): 가로세로 방향중 두 방향으로 한발씩 쏨. 
                        if(GameManager.executing) {
                            piece.GetComponent<Archer>().numEnemy = 0;
                            List<int[]> basic_moves = piece.GetComponent<ChessPiece>().basic_moves;
                            piece.GetComponent<Archer>().onlyAttack = true;
                            piece.GetComponent<Archer>().createArrowArcher(basic_moves, card, 2);
                            //ArrowController.numAttack = 2;
                        }
                        return ArrowController.numAttack>0;
                    }, 
                    Piece.Archer, // Card Target 
                    CardGrade.Epic // Card Grade
                )
            },  // Double_Shot End
            {   // Penetrating_Arrow Start
                Card.Penetrating_Arrow, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        //Penetrating Arrow(관통 화살): 공격력+1로 화살을 발사. 
                        if(GameManager.executing){
                            piece.GetComponent<ChessPiece>().offensePower++;
                            GameManager.destroyAllIndicators();
                            card.GetComponent<DragDrop>().destoryCard();
                            GameManager.endTurn();
                        }
                        return true;
                    }, 
                    Piece.Archer, // Card Target 
                    CardGrade.Epic // Card Grade
                )
            },  // Penetrating_Arrow End
            {   // Switch_Teleport Start
                Card.Swap, // Key 
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Switch Teleport(위치교환): 아군 말과 자신의 위치를 바꾼다.                         
                        if(!GameManager.executing) {
                            List<GameObject> dots = new List<GameObject>();
                            for(int i=0; i<8; i++) {
                                for(int j=0; j<5; j++) {
                                    GameObject cell = PieceConfig.cells[i, j];
                                    if(cell.transform.childCount <= 0 ) {
                                        continue;
                                    }  

                                    if(piece.GetComponent<ChessPiece>().indexX == j && piece.GetComponent<ChessPiece>().indexY == i) continue;

                                    GameObject otherPiece = cell.transform.GetChild(0).gameObject;     
                                    if(!Helper.isEnemy(piece, otherPiece)) {
                                        GameObject swap = Helper.prefabNameToGameObject(Prefab.Swap.ToString());
                                        swap.transform.SetParent(cell.transform, false);
                                        swap.transform.position = cell.transform.position;
                                        swap.GetComponent<SwapController>().mage = piece;
                                        swap.GetComponent<SwapController>().target = otherPiece;
                                        swap.GetComponent<SwapController>().card = card;
                                        dots.Add(swap);
                                    }                           
                                }
                            }
                            GameManager.dots = dots;
                        }
                        return true;
                    }, 
                    Piece.Mage, // Card Target 
                    CardGrade.Epic // Card Grade
                )
            },  // Switch_Teleport End
            {
                Card.Disarm,
                (
                    (GameObject piece, GameObject card) => { // Card Logic
                        // Disarm(무장해제): 상대방 말 한개의 모든 강화효과를 제거한다 (어느 위치에 있든).
                        if(!GameManager.executing) {
                            List<GameObject> dots = new List<GameObject>();
                            for(int i=0; i<8; i++) {
                                for(int j=0; j<5; j++) {
                                    GameObject cell = PieceConfig.cells[i, j];
                                    if(cell.transform.childCount <= 0 ) {
                                        continue;
                                    }  

                                    if(piece.GetComponent<ChessPiece>().indexX == j && piece.GetComponent<ChessPiece>().indexY == i) continue;

                                    GameObject otherPiece = cell.transform.GetChild(0).gameObject;     
                                    if(Helper.isEnemy(piece, otherPiece)) {
                                        GameObject disarm = Helper.prefabNameToGameObject(Prefab.Disarm.ToString());
                                        disarm.transform.SetParent(cell.transform, false);
                                        disarm.transform.position = cell.transform.position;
                                        disarm.GetComponent<DisarmController>().target = otherPiece;
                                        disarm.GetComponent<DisarmController>().card = card;
                                        dots.Add(disarm);
                                    }                           
                                }
                            }
                            GameManager.dots = dots;
                        }
                        return true;
                    }, 
                    Piece.Mage, // Card Target 
                    CardGrade.Epic // Card Grade
                )
            }
        };
    }

}