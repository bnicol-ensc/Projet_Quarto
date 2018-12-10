using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetQuarto
{
    class Program
    {
        public const int TAILLE = 4;
        public static bool finPartie = false;
        public struct Piece
        {
            public bool pieceNulle;
            public ConsoleColor couleur; // 'B' ou 'N'
            public int hauteur; // 1 ou 2
            public char forme; // '+' ou '*'
            public bool remplie;
        }
        public static Piece[,] plateau = new Piece[4,4];
        public static Piece[] pioche = new Piece[16];


        static void Main(string[] args)
        {
            Console.WriteLine("Projet QUARTO --- NICOL - MORELLE\n\n");

            Initialisation.InitialiserPlateau();
            Initialisation.InitialiserPioche();
            /*int cpt = 0;
            for (int i = 0; i < TAILLE; i ++)
            {
                for (int j = 0; j < TAILLE; j++)
                {
                    plateau[i, j] = pioche[cpt];
                    cpt++;
                }
            }
            AfficherPlateau();*/
            Jeu.Jouer();

            Console.ReadLine();
        }
    }
}