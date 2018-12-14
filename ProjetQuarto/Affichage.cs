using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetQuarto
{
    class Affichage
    {
        public static void AfficherPlateau()
        {
            string separateurLigne = " ***********************";
            Console.WriteLine(separateurLigne);

            for (int i = 0; i < Program.TAILLE; i++) // pour chaque ligne
            {
                for (int j = 0; j < 3; j++) // pour chacune des 3 lignes d'affichage de chaque ligne du plateau
                {
                    Console.Write("| ");
                    for (int k = 0; k < Program.TAILLE; k++) // pour chaque colonne
                    {
                        Program.Piece p = Program.plateau[i, k];
                        if (!p.pieceNulle) // si la case du plateau n'est pas vide
                        {
                            Console.ForegroundColor = p.couleur;
                            if (p.hauteur == 2 || j != 0) // on ne dessine rien en haut pour la hauteur basse
                            {
                                if (p.remplie || j == 2)
                                    Console.Write(new string(p.forme, 3));
                                else
                                    Console.Write(p.forme + " " + p.forme);
                            }
                            else
                                Console.Write("   ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else // si la case est vide
                        {
                            Console.Write("   ");
                        }
                        Console.Write(" | ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(separateurLigne);
            }
        }
        public static void AfficherMoitiePioche(int deb, int fin)
        {
            string separateurLigne = " -----------------------------------------------";
            Console.WriteLine(separateurLigne);
            for (int i = deb; i < fin; i++)
            {
                Console.Write("|  " + i + " ");
                if (i < 10)
                    Console.Write(" "); // Pour que l'affichage reste propre même avec 2 décimales !
            }
            Console.WriteLine("|\n" + separateurLigne);
            for (int i = 0; i < 3; i++) // pour chacune des 3 lignes d'affichage de chaque pièce
            {
                Console.Write("| ");
                for (int j = deb; j < fin; j++) // pour chaque pièce de la pioche
                {
                    Program.Piece p = Program.pioche[j];
                    if (!p.pieceNulle) // si la case de la pioche n'est pas vide
                    {
                        Console.ForegroundColor = p.couleur;
                        if (p.hauteur == 2 || i != 0) // on ne dessine rien en haut pour la hauteur basse
                        {
                            if (p.remplie || i == 2)
                                Console.Write(new string(p.forme, 3));
                            else
                                Console.Write(p.forme + " " + p.forme);
                        }
                        else
                            Console.Write("   ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else // si la case est vide
                    {
                        Console.Write("   ");
                    }
                    Console.Write(" | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(separateurLigne);
        }
        public static void AfficherPioche()
        {
            AfficherMoitiePioche(0, Program.pioche.Length / 2);
            AfficherMoitiePioche(Program.pioche.Length / 2, Program.pioche.Length);
        }
        public static void AfficherPiece(int numPiece)
        {
            Program.Piece p = Program.pioche[numPiece];
            Console.WriteLine();
            Console.ForegroundColor = p.couleur;
            for (int i = 0; i < 3; i++)
            {
                if (p.hauteur == 2 || i != 0) // on ne dessine rien en haut pour la hauteur basse
                {
                    if (p.remplie || i == 2)
                        Console.Write(new string(p.forme, 3) + "\n");
                    else
                        Console.Write(p.forme + " " + p.forme + "\n");
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }



        public static string RecupEntreeJoueur()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string message = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            return message;
        }
        /*
        public static void ParlerOrdi(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }*/
    }
}
