using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetQuarto
{
    class Initialisation
    {
        public static void InitialiserPlateau()
        {
            for (int i = 0; i < Program.TAILLE; i++)
            {
                for (int j = 0; j < Program.TAILLE; j++)
                {
                    Program.Piece p = new Program.Piece();
                    // Initialisation de la pièce avec des valeurs quelconques (elle ne sera jamais affichée)
                    p.pieceNulle = true;
                    p.couleur = ConsoleColor.Black;
                    p.hauteur = 0;
                    p.forme = '-';
                    p.remplie = false;

                    Program.plateau[i, j] = p;
                }
            }
        }
        public static void InitialiserPioche()
        {
            ConsoleColor c1 = ConsoleColor.DarkYellow;
            ConsoleColor c2 = ConsoleColor.DarkRed;
            int taillePioche = Program.pioche.Length;
            for (int i = 0; i < taillePioche; i++)
            {
                Program.Piece p = new Program.Piece();
                p.pieceNulle = false;

                // initialisation couleur
                if (i < taillePioche / 2)
                    p.couleur = c1;
                else
                    p.couleur = c2;

                // initialisation hauteur
                if (i % 8 == 0 || i % 8 == 1 || i % 8 == 2 || i % 8 == 3)
                    p.hauteur = 1;
                else
                    p.hauteur = 2;

                // initialisation forme
                if (i % 4 == 0 || i % 4 == 1)
                    p.forme = '*';
                else
                    p.forme = '+';

                // initialisation rempli ou non
                if (i % 2 == 0)
                    p.remplie = true;
                else
                    p.remplie = false;

                Program.pioche[i] = p;
            }
        }
    }
}
