using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetQuarto
{
    class Outils
    {
        public static bool TesterLignePleine(int x)
        {
            bool pleine = true;
            int i = 0;
            while (pleine && i < Program.TAILLE) // on parcourt les cases de la ligne, dès qu'on en trouve une vide c'est que la ligne n'est pas pleine donc on sort de la boucle
            {
                if (Program.plateau[x, i].pieceNulle)
                    pleine = false;
                i++;
            }
            return pleine;
        }
        public static bool TesterColonnePleine(int y)
        {
            bool pleine = true;
            int i = 0;
            while (pleine && i < Program.TAILLE) // on parcourt les cases de la colonne, dès qu'on en trouve une vide c'est que la colonne n'est pas pleine donc on sort de la boucle
            {
                if (Program.plateau[i, y].pieceNulle)
                    pleine = false;
                i++;
            }
            return pleine;
        }
        public static bool TesterDiag1Pleine() // Diagonale de gauche à droite
        {
            bool pleine = true;
            int i = 0;
            while (pleine && i < Program.TAILLE)
            {
                if (Program.plateau[i, i].pieceNulle)
                    pleine = false;
                i++;
            }
            return pleine;
        }
        public static bool TesterDiag2Pleine() // Diagonale de droite à gauche
        {
            bool pleine = true;
            int i = Program.TAILLE - 1;
            int j = 0;
            while (pleine && i < Program.TAILLE)
            {
                if (Program.plateau[i, j].pieceNulle)
                    pleine = false;
                i--;
                j++;
            }
            return pleine;
        }
        public static bool TesterPlateauPlein()
        {
            bool plein = true;
            int i = 0;
            while (plein && i < Program.TAILLE) // on parcourt les lignes du plateau, dès qu'on en trouve une vide c'est que le plateau n'est pas plein donc on sort de la boucle
            {
                if (!TesterLignePleine(i))
                    plein = false;
                i++;
            }
            return plein;
        }
        public static int CompterPiecesRestantesPioche()
        {
            int nbPiecesRestantes = 0;
            for (int i = 0; i < Program.pioche.Length; i++)
            {
                if (!Program.pioche[i].pieceNulle)
                    nbPiecesRestantes++;
            }
            return nbPiecesRestantes;
        }
        public static int CompterNbPiecesLigne(int x)
        {
            int nbPieces = 0;
            for (int y = 0; y < Program.TAILLE; y++)
                if (!Program.plateau[x, y].pieceNulle)
                    nbPieces++;
            return nbPieces;
        }
        public static int CompterNbPiecesColonne(int y)
        {
            int nbPieces = 0;
            for (int x = 0; x < Program.TAILLE; x++)
                if (!Program.plateau[x, y].pieceNulle)
                    nbPieces++;
            return nbPieces;
        }
        public static int CompterNbPiecesDiag1()
        {
            int nbPieces = 0;
            for (int i = 0; i < Program.TAILLE; i++)
                if (!Program.plateau[i, i].pieceNulle) // la diagonale de gauche à droite est composée de toutes les cases qui ont x=y
                    nbPieces++;
            return nbPieces;
        }
        public static int CompterNbPiecesDiag2()
        {
            int nbPieces = 0;
            for (int x = 0, y = Program.TAILLE-1; x < Program.TAILLE; x++, y--)
                if (!Program.plateau[x, y].pieceNulle)
                    nbPieces++;
            return nbPieces;
        }



        // Cherche s'il y a un point commun entre certaines pièces données
        public static bool ComparerPieces(Program.Piece[] pieces)
        {
            int nbBlanc = 0;
            int nbBas = 0;
            int nbPlein = 0;
            int nbRond = 0;
            for (int i = 0; i < pieces.Length; i++)
            {
                Program.Piece p = pieces[i];
                if (p.couleur == ConsoleColor.DarkYellow) nbBlanc++;
                if (p.hauteur == 1) nbBas++;
                if (p.remplie) nbPlein++;
                if (p.forme == 'o') nbRond++;
            }

            return (nbBlanc == 0 || nbBlanc == 4 || nbBas == 0 || nbBas == 4
                    || nbPlein == 0 || nbPlein == 4 || nbRond == 0 || nbRond == 4);
        }



        public static bool ChercherQuartoOrientation(int x, int y, string orientationTestee, string critere = "pasTeste") // orientationTestee : ligne/colonne/diag1/diag2 ; critere : non obligatoire, vaut couleur/forme/remplissage/hauteur
        {
            if ((orientationTestee == "ligne" && !TesterLignePleine(x)) // si on teste la ligne et qu'elle n'est pas pleine, il ne peut pas y avoir Quarto
                 || (orientationTestee == "colonne" && !TesterColonnePleine(y))  // si on teste la colonne et qu'elle n'est pas pleine, il ne peut pas y avoir Quarto
                 || (orientationTestee == "diag1" && !TesterDiag1Pleine()) // si on teste la diagonale de gauche à droite et qu'elle n'est pas pleine, il ne peut pas y avoir Quarto
                 || (orientationTestee == "diag2" && !TesterDiag2Pleine())) // si la diagonale de droite à gauche n'est pas pleine
                return false;

            int nbBlanc = 0;
            int nbBas = 0;
            int nbPlein = 0;
            int nbRond = 0;
            for (int i = 0, j = Program.TAILLE - 1; i < Program.TAILLE; i++, j--) // j ne sert que pour la diag2
            {
                Program.Piece p;
                if (orientationTestee == "ligne")
                    p = Program.plateau[x, i];
                else if (orientationTestee == "colonne")
                    p = Program.plateau[i, y];
                else if (orientationTestee == "diag1")
                    p = Program.plateau[i, i]; // dans le cas de la diagonale de gauche à droite, on ne teste que les cases de mêmes x et y
                else
                    p = Program.plateau[i, j];

                if (p.couleur == ConsoleColor.DarkYellow) nbBlanc++;
                if (p.hauteur == 1) nbBas++;
                if (p.remplie) nbPlein++;
                if (p.forme == 'o') nbRond++;
            }

            // Si le critère est la valeur par défaut, on ne s'en servira pas. S'il est renseigné, c'est qu'on vérifie un quarto déclaré par le joueur, dans ce cas on ne veut pas faire tous les affichages suivants.
            if (critere == "couleur" && (nbBlanc == 0 || nbBlanc == 4))
                return true;
            if (critere == "forme" && (nbRond == 0 || nbRond == 4))
                return true;
            if (critere == "hauteur" && (nbBas == 0 || nbBas == 4))
                return true;
            if (critere == "remplissage" && (nbPlein == 0 || nbPlein == 4))
                return true;
            if (critere != "pasTeste") // Si le critère est renseigné c'est qu'on vérifie un quarto du joueur ; donc si on n'a pas trouvé de quarto sur le critère que le joueur a entré, on renvoie faux.
                return false;
            
            string orientation = orientationTestee;
            if (orientationTestee == "diag1")
                orientation = "diagonale de gauche à droite";
            else if (orientationTestee == "diag2")
                orientation = "diagonale de droite à gauche";
            string ligneAEcrire = "Il y a un quarto sur";

            bool quarto = false;
            if (nbBlanc == 0 || nbBlanc == 4) // si on ne trouve aucun blanc par exemple, c'est que toutes les pièces de la ligne sont noires
            {
                ligneAEcrire += " la couleur ";
                quarto = true;
            }
            else if (nbBas == 0 || nbBas == 4)
            {
                ligneAEcrire += " la hauteur ";
                quarto = true;
            }
            else if (nbPlein == 0 || nbPlein == 4)
            {
                ligneAEcrire += " le remplissage ";
                quarto = true;
            }
            else if (nbRond == 0 || nbRond == 4)
            {
                ligneAEcrire += " la forme ";
                quarto = true;
            }

            if (quarto)
            {
                ligneAEcrire += "sur la " + orientation;
                if (orientationTestee == "ligne" || orientationTestee == "colonne")
                    ligneAEcrire += " " + (x + 1);
                ligneAEcrire += " !";
                Console.WriteLine(ligneAEcrire);
            }

            return quarto;
        }

        public static bool ChercherQuarto(int x, int y) // x et y sont les coordonnées de la dernière pièce posée, par l'ordi ou par le joueur (on regarde si un Quarto n'a pas échappé au joueur)
        {
            return ChercherQuartoOrientation(x, y, "ligne") || ChercherQuartoOrientation(x, y, "colonne")
                || (x != y && ChercherQuartoOrientation(x, y, "diag1")) // on vérifie qu'on est sur la diagonale de gauche à droite avant de tester cette diagonale TODO vérifier l'ordre dans lequel sont testées les conditions
                || (x != Program.TAILLE - y - 1 && ChercherQuartoOrientation(x, y, "diag2")); // on vérifie qu'on est sur la diagonale de droite à gauche avant de tester cette diagonale TODO idem au-dessus
        }
        


        public static Program.Piece ConvertirStringPiece(string[] ligne)
        {
            Program.Piece p = new Program.Piece();
            p.pieceNulle = ligne[0] == "True";
            p.couleur = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ligne[1]);
            p.hauteur = int.Parse(ligne[2]);
            p.forme = ligne[3].ToCharArray()[0];
            p.remplie = ligne[4] == "True";

            return p;
        }
    }
}
