using System;

namespace ProjetQuarto
{
    class IA
    {
        /*
         * Fonctions pour faire jouer l'ordinateur (c'est l'ordinateur qui doit poser une pièce)
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
                    // TODO idem pour les diags
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
                if (Outils.CompterNbPiecesLigne(x) == 3)
                {
                    // On remplit un tableau contenant les 3 pièces de la ligne et la pièce qu'on doit placer
                    Program.Piece[] pieces = new Program.Piece[4];
                    pieces[0] = pieceAPlacer;
                    int cpt = 1;
                    int yEmplacementVideLigne = -1; // on en profite pour chercher quel emplacement de la ligne est vide
                    for (int y = 0; y < Program.TAILLE; y++)
                    {
                        Program.Piece piece = Program.plateau[x, y];
                        if (!piece.pieceNulle)
                        {
                            pieces[cpt] = piece;
                            cpt++;
                        }
                        else
                            yEmplacementVideLigne = y;
                    }

                    if (Outils.ComparerPieces(pieces)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun donc on peut faire un quarto en plaçant la pièce sur cette ligne
                    {
                        emplacement[0] = x;
                        emplacement[1] = yEmplacementVideLigne;
                    }
                }
                else
                    x++;
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
                if (Outils.CompterNbPiecesColonne(y) == 3)
                {
                    // On remplit un tableau contenant les 3 pièces de la colonne et la pièce qu'on doit placer
                    Program.Piece[] pieces = new Program.Piece[4];
                    pieces[0] = pieceAPlacer;
                    int cpt = 1;
                    int xEmplacementVideColonne = -1; // on en profite pour chercher quel emplacement de la colonne est vide
                    for (int x = 0; x < Program.TAILLE; x++)
                    {
                        Program.Piece piece = Program.plateau[x, y];
                        if (!piece.pieceNulle)
                        {
                            pieces[cpt] = piece;
                            cpt++;
                        }
                        else
                            xEmplacementVideColonne = x;
                    }

                    if (Outils.ComparerPieces(pieces)) // si cette fonction renvoie vrai, les 4 pièces ont un point commun donc on peut faire un quarto en plaçant la pièce sur cette colonne
                    {
                        emplacement[0] = xEmplacementVideColonne;
                        emplacement[1] = y;
                    }
                }
                else
                    y++;
            }
            return emplacement;
        }




        /*
         * Fonctions pour attribuer une pièce au joueur
         */

        public static int DonnerPieceAuJoueurStrategie()
        {
            // On cherche à donner au joueur une pièce qui ne lui permettra pas de faire un quarto (par exemple, si une ligne est presque pleine et que le point commun entre les pièces est la couleur rouge, on ne donnera pas une pièce rouge)
            int piece = ChoisirPieceJoueurEmpecherQuarto();
            if (piece == -1)
                piece = ChoisirPieceJoueurHasard();
            return piece;
        }

        public static int ChoisirPieceJoueurHasard()
        {
            Random rand = new Random();
            int nPiece = rand.Next(Outils.CompterPiecesRestantesPioche()); // on choisit un numéro dans le nombre de pièces restantes

            bool trouve = false;
            int i = 0;
            int cpt = 0;

            while (!trouve && i < Program.pioche.Length) // on cherche le numéro choisi dans la pioche (par exemple, si on a choisi le numéro 0 mais que la première case de la pioche est vide, ce sera la 2e case de la pioche)
            {
                if (!Program.pioche[i].pieceNulle)
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
            return -1; // TODO compléter la fonction
        }

    }
}