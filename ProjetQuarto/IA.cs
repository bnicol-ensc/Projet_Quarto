using System;

namespace ProjetQuarto
{
    class IA
    {
        /*
         * Fonctions pour faire jouer l'ordinateur (c'est l'ordinateur qui doit poser une pièce)
         * --> essayer de faire un quarto
         */

        public static int[] PlacerPieceOrdiStrategie(int numPiece) // renvoie la position [x,y] de l'emplacement choisi
        {
            // On essaye d'appliquer d'abord la meilleure stratégie possible, et si elle ne permet de trouver un emplacement (pas possible de faire un quarto), on joue au hasard
            int[] emplacement = ChercherEmplacementFaireQuarto(numPiece);
            if (emplacement[0] == -1)
                emplacement = ChercherEmplacementHasard();

            int x = emplacement[0];
            int y = emplacement[1];

            // Une fois qu'on a trouvé un x et un y correspondant à une case vide, on place la pièce sur cette case et on enlève la pièce de la pioche
            Program.plateau[x, y] = Program.pioche[numPiece];
            Program.pioche[numPiece].pieceNulle = true;

            return emplacement;
        }

        public static int[] ChercherEmplacementHasard()
        {
            Random rand = new Random();
            int x = 0;
            do
            {
                x = rand.Next(4);
            }
            while (Outils.TesterLignePleine(x)); // tant que la ligne sur laquelle on veut placer le pion est déjà pleine, on cherche une autre ligne

            int y = 0;
            do
            {
                y = rand.Next(4);
            }
            while (!Program.plateau[x, y].pieceNulle); // tant que l'emplacement sur lequel on veut mettre la pièce n'est pas vide, on en cherche un autre

            return new int[] { x, y };
        }

        public static int[] ChercherEmplacementFaireQuarto(int numPiece) // cherche un emplacement qui permettrait de faire un quarto, renvoie (-1,-1) si n'en trouve pas
        {
            int[] emplacement = ChercherEmplacementFaireQuartoLigne(numPiece);
            if (emplacement[0] == -1) // si on n'a pas trouvé de quarto possible pour une ligne
            {
                emplacement = ChercherEmplacementFaireQuartoColonne(numPiece);
                if (emplacement[0] == -1) // si on n'a pas trouvé de quarto possible pour une colonne
                {
                    emplacement = ChercherEmplacementFaireQuartoDiag1(numPiece);
                    if (emplacement[0] == -1)
                        emplacement = ChercherEmplacementFaireQuartoDiag2(numPiece);
                }
            }

            return emplacement; // TODO compléter la fonction
        }

        // TODO peut-être rassembler les fonctions suivantes ?
        public static int[] ChercherEmplacementFaireQuartoLigne(int numPiece)
        {
            Program.Piece pieceAPlacer = Program.pioche[numPiece];
            int x = 0;
            int[] emplacement = { -1, -1 };

            while (emplacement[0] == -1 && x < Program.TAILLE) // tant qu'on n'a pas trouvé d'emplacement et qu'on n'a pas parcouru toutes les lignes
            {
                if (Outils.CompterNbPiecesLigne(x) == Program.TAILLE - 1)
                {
                    // On remplit un tableau contenant les 3 pièces de la ligne et la pièce qu'on doit placer
                    Program.Piece[] piecesLigne = Outils.RemplirTableauPiecesLigne(x);
                    piecesLigne[Program.TAILLE - 1] = pieceAPlacer;

                    if (Outils.ComparerPieces(piecesLigne)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun donc on peut faire un quarto en plaçant la pièce sur cette ligne
                    {
                        emplacement[0] = x;
                        emplacement[1] = Outils.TrouverEmplacementYVideLigne(x);
                    }
                    else
                        x++;
                }
                else
                    x++; // on ne passe que dans l'un des 2 "else x++", et on passe forcément dans l'un des deux si on n'a pas trouvé de place où faire un quarto
            }
            return emplacement;
        }

        public static int[] ChercherEmplacementFaireQuartoColonne(int numPiece)
        {
            Program.Piece pieceAPlacer = Program.pioche[numPiece];
            int y = 0;
            int[] emplacement = { -1, -1 };

            while (emplacement[0] == -1 && y < Program.TAILLE) // tant qu'on n'a pas trouvé d'emplacement et qu'on n'a pas parcouru toutes les colonnes
            {
                if (Outils.CompterNbPiecesColonne(y) == Program.TAILLE-1)
                {
                    // On remplit un tableau contenant les 3 pièces de la colonne et la pièce qu'on doit placer
                    Program.Piece[] piecesColonne = Outils.RemplirTableauPiecesColonne(y);
                    piecesColonne[Program.TAILLE-1] = pieceAPlacer;
                    
                    if (Outils.ComparerPieces(piecesColonne)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun donc on peut faire un quarto en plaçant la pièce sur cette colonne
                    {
                        emplacement[0] = Outils.TrouverEmplacementXVideColonne(y);
                        emplacement[1] = y;
                    }
                    else
                        y++;
                }
                else
                    y++;  // on ne passe que dans l'un des 2 "else y++", et on passe forcément dans l'un des deux si on n'a pas trouvé de place où faire un quarto
            }
            return emplacement;
        }

        public static int[] ChercherEmplacementFaireQuartoDiag1(int numPiece)
        {
            Program.Piece pieceAPlacer = Program.pioche[numPiece];
            int[] emplacement = { -1, -1 };

            if (Outils.CompterNbPiecesDiag(1) == 3)
            {
                // On remplit un tableau contenant les 3 pièces de la diagonale et la pièce qu'on doit placer
                Program.Piece[] piecesDiag1 = Outils.RemplirTableauPiecesDiag(1);
                piecesDiag1[Program.TAILLE - 1] = pieceAPlacer;
                
                if (Outils.ComparerPieces(piecesDiag1)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun donc on peut faire un quarto en plaçant la pièce sur cette diagonale
                {
                    emplacement[0] = Outils.TrouverEmplacementXVideDiag(1);
                    emplacement[1] = emplacement[0];
                }
            }
            return emplacement;
        }

        public static int[] ChercherEmplacementFaireQuartoDiag2(int numPiece)
        {
            Program.Piece pieceAPlacer = Program.pioche[numPiece];
            int[] emplacement = { -1, -1 };

            if (Outils.CompterNbPiecesDiag(2) == 3)
            {
                // On remplit un tableau contenant les 3 pièces de la diagonale et la pièce qu'on doit placer
                Program.Piece[] piecesDiag2 = Outils.RemplirTableauPiecesDiag(2);
                piecesDiag2[Program.TAILLE - 1] = pieceAPlacer;
                
                if (Outils.ComparerPieces(piecesDiag2)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun donc on peut faire un quarto en plaçant la pièce sur cette diagonale
                {
                    emplacement[0] = Outils.TrouverEmplacementXVideDiag(2);
                    emplacement[1] = Program.TAILLE - 1 - emplacement[0];
                }
            }
            return emplacement;
        }




        /*
         * Fonctions pour attribuer une pièce au joueur
         * --> essayer de l'empêcher de faire un quarto
         */

        public static int DonnerPieceAuJoueurStrategie()
        {
            // On cherche à donner au joueur une pièce qui ne lui permettra pas de faire un quarto (par exemple, si une ligne est presque pleine et que le point commun entre les pièces est la couleur rouge, on ne donnera pas une pièce rouge)
            int piece = ChoisirPieceJoueurEmpecherQuarto();
            if (piece == -1)
                piece = ChoisirPieceJoueurHasard(Program.pioche);
            return piece;
        }

        public static int ChoisirPieceJoueurHasard(Program.Piece[] pioche)
        {
            Random rand = new Random();
            int nPiece = rand.Next(Outils.CompterPiecesRestantesPioche(pioche)); // on choisit un numéro dans le nombre de pièces restantes

            bool trouve = false;
            int i = 0;
            int cpt = 0;

            while (!trouve && i < pioche.Length) // on cherche le numéro choisi dans la pioche (par exemple, si on a choisi le numéro 0 mais que la première case de la pioche est vide, ce sera la 2e case de la pioche)
            {
                if (!pioche[i].pieceNulle)
                {
                    if (cpt == nPiece)
                        trouve = true;
                    cpt++;
                }
                i++;
            }

            return i - 1;
        }

        public static int ChoisirPieceJoueurEmpecherQuarto()
        {
            int numPiece = -1;

            /* 1. On constitue un tableau qui copie la pioche
             * 2. On cherche toutes les lignes, colonnes et diagonales presque pleines (contenant TAILLE-1 éléments)
             * 3. À chaque fois qu'on en trouve, on enlève du tableau toutes les pièces qui peuvent permettre de faire un quarto à cet endroit
             * 4. On pioche au hasard parmi les pièces restantes dans le tableau (s'il n'y en a aucune, renvoie -1)
             */

            // 1.
            Program.Piece[] pioche2 = Program.pioche;

            // 2. 3.
            ModifierPiocheSelonLignes(ref pioche2);
            ModifierPiocheSelonColonnes(ref pioche2);
            ModifierPiocheSelonDiag(ref pioche2, 1);
            ModifierPiocheSelonDiag(ref pioche2, 2);

            // 4.
            if (Outils.CompterPiecesRestantesPioche(pioche2) > 0)
                numPiece = ChoisirPieceJoueurHasard(pioche2);

            return numPiece;
        }

        public static void ModifierPiocheSelonLignes(ref Program.Piece[] pioche2)
        {
            for (int x = 0; x < Program.TAILLE; x++)
            {
                if (Outils.CompterNbPiecesLigne(x) == Program.TAILLE - 1) // 3.
                {
                    Program.Piece[] piecesLigne = Outils.RemplirTableauPiecesLigne(x);

                    for (int i = 0; i < pioche2.Length; i++)
                    {
                        if (!pioche2[i].pieceNulle)
                        {
                            piecesLigne[Program.TAILLE - 1] = pioche2[i];
                            if (Outils.ComparerPieces(piecesLigne)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun, donc il ne faut pas donner cette pièce au joueur, sinon il peut faire un quarto
                            {
                                pioche2[i].pieceNulle = true; // donc on supprime cette pièce de la pioche, pour que le joueur ne puisse pas faire de quarto
                            }
                        }
                    }
                }
            }
        }

        public static void ModifierPiocheSelonColonnes(ref Program.Piece[] pioche2)
        {
            for (int y = 0; y < Program.TAILLE; y++)
            {
                if (Outils.CompterNbPiecesColonne(y) == Program.TAILLE - 1) // 3.
                {
                    Program.Piece[] piecesColonne = Outils.RemplirTableauPiecesColonne(y);

                    for (int i = 0; i < pioche2.Length; i++)
                    {
                        if (!pioche2[i].pieceNulle)
                        {
                            piecesColonne[Program.TAILLE - 1] = pioche2[i];
                            if (Outils.ComparerPieces(piecesColonne)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun, donc il ne faut pas donner cette pièce au joueur, sinon il peut faire un quarto
                            {
                                pioche2[i].pieceNulle = true; // donc on supprime cette pièce de la pioche, pour que le joueur ne puisse pas faire de quarto
                            }
                        }
                    }
                }
            }
        }

        public static void ModifierPiocheSelonDiag(ref Program.Piece[] pioche2, int numDiag) // 1 : diagonale de gauche à droite, 2 : diagonale de droite à gauche
        {
            for (int i = 0; i < Program.TAILLE; i++)
            {
                if (Outils.CompterNbPiecesDiag(numDiag) == Program.TAILLE - 1) // 3.
                {
                    Program.Piece[] piecesDiag = Outils.RemplirTableauPiecesDiag(numDiag);

                    for (int j = 0; j < pioche2.Length; j++)
                    {
                        if (!pioche2[j].pieceNulle)
                        {
                            piecesDiag[Program.TAILLE - 1] = pioche2[j];
                            if (Outils.ComparerPieces(piecesDiag)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun, donc il ne faut pas donner cette pièce au joueur, sinon il peut faire un quarto
                            {
                                pioche2[j].pieceNulle = true; // donc on supprime cette pièce de la pioche, pour que le joueur ne puisse pas faire de quarto
                            }
                        }
                    }
                }
            }
        }

    }
}