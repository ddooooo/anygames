using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace Config {
    enum Prefab {
        Dot_Move, 
        Thunder_Bolt, 
        Cell, 
        Card, 
        Attacking,
        Selected_Indicator,
        Mulligan,
        Arrow,
        Swap,
        Disarm,
    }

    enum Constants {
        P1_First = 1, 
        P2_First = 2,
    }

    // Class For Path Strings
    public static class Path {
        public const string prefab_base_path = "Assets/Prefab"; 
        public const string sprite_base_path = "Sprites"; 
        public static class Prefab {
            public const string dot_move = "Dot_Move";
            public const string thunder_bolt = "Thunder_Bolt"; 
            public const string cell = "Board/Cell"; 
            public const string card = "Card"; 
            public const string king = "Chess_Piece/King";
            public const string mage = "Chess_Piece/Mage"; 
            public const string archer = "Chess_Piece/Archer"; 
            public const string warrior = "Chess_Piece/Warrior"; 
            public const string mulligan = "Mulligan";
            public const string attacking = "Attacking";
            public const string selected_indicator = "Selected_Indicator";
            public const string arrow = "Arrow";
            public const string swap = "Swap";
            public const string disarm = "Disarm";
            
        }
    }

    // Class For Helper/Utility Functions
    public static class Helper {
        // Creates And Returns GameObject Based On Prefab Name As String
        public static GameObject prefabNameToGameObject(string prefab_name){
            string path = typeof(Path.Prefab).GetField(prefab_name.ToLower()).GetValue(null) as string;
            Object prefab = AssetDatabase.LoadAssetAtPath($"{Path.prefab_base_path}/{path}.prefab", typeof(GameObject));
            return GameObject.Instantiate(prefab) as GameObject;

        }


        // Generate Card GameObject From Card Enum
        public static GameObject cardToGameObject(Card card) {
            GameObject card_object = prefabNameToGameObject(Prefab.Card.ToString()); 
            Sprite sprite = generateSprite(card.ToString()); 
            card_object.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            card_object.transform.GetChild(1).GetComponent<Text>().text = card.ToString(); 

            card_object.GetComponent<DragDrop>().pieceType = CardConfig.card_dict[card].Item2;
            return card_object; 
        }

        public static Sprite generateSprite(params string[] path_list) {
            string fileLocation = $"{Config.Path.sprite_base_path}/{string.Join("/", path_list)}";
            return Resources.Load<Sprite>(fileLocation);
        }

        public static bool isEnemy(GameObject me, GameObject other)
        {
            return me.GetComponent<ChessPiece>().player != other.GetComponent<ChessPiece>().player;
        }
    }
}

