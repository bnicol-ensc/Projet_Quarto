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
        public static int tourJoueur = -1; // numéro du joueur dont c'est le tour de jouer. Initialisé à -1 pour savoir s'il faut tirer au sort ou si on a récupéré le numéro dans le fichier de sauvegarde (il vaut 0 ou 1 s'il a été initialisé par le fichier)
        public struct Piece
        {
            public bool pieceNulle;
            public ConsoleColor couleur;
            public int hauteur; // 1 ou 2
            public char forme; // '+' ou '*'
            public bool remplie;
        }
        public static Piece[,] plateau = new Piece[4,4];
        public static Piece[] pioche = new Piece[16];


        static void Main(string[] args)
        {
            Console.WriteLine("Projet QUARTO --- NICOL - MORELLE\n\n");

            bool reprendrePartie = Sauvegarde.RecupererPartie();

            if (!reprendrePartie)
            {
                Initialisation.InitialiserPlateau();
                Initialisation.InitialiserPioche();
            }
            Jeu.Jouer();

            Console.ReadLine();
        }
    }
}