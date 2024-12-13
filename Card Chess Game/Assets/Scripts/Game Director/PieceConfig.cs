using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Configuration File For Everything Related To Pieces and Board
*/
namespace Config {
    public enum Piece // Also Prefab
    {
        Archer,
        Mage,
        Warrior,
        King
    };

    public static class PieceConfig
    {
        public static int[,] move_list_basic = { {1, 0}, {-1,0}, {0,-1}, {0, 1}};
        public static int[,] move_list_surround = { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 } };
        public static int[,] move_list_knight = { { 2, 1 }, { 1, 2 }, { 2, -1 }, { 1, -2 }, { -2, 1 }, { -1, 2 }, { -2, -1 }, { -1, -2 } };
        public static GameObject[,] cells = new GameObject[8, 5];
        public static float[] board_pos = { -320f, -570f, 160 };
        public static Piece[,] pieces_on_board = {
            {Piece.Archer, Piece.Mage, Piece.King, Piece.Mage, Piece.Archer},
            {Piece.Warrior, Piece.Warrior, Piece.Warrior, Piece.Warrior, Piece.Warrior}
        };

    }
}
