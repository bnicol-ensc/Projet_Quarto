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
            // On essaye d'appliquer d'abord la meilleure stratégie possible, puis la seconde, et si aucune des deux ne permet de trouver un emplacement (pas possible de faire un quarto ou de bloquer le joueur), on joue au hasard
            int[] emplacement = ChercherEmplacementFaireQuarto(numPiece);
            if (emplacement[0] == -1)
            {
                emplacement = ChercherEmplacementEmpecherQuarto(numPiece);
                if (emplacement[0] == -1)
                    emplacement = ChercherEmplacementHasard();
            }

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
            return new int[] { -1, -1 }; // TODO compléter la fonction
        }

        public static int[] ChercherEmplacementEmpecherQuarto(int numPiece) // cherche un emplacement qui empêcherait le joueur de faire un quarto
        {
            return new int[] { -1, -1 }; // TODO compléter la fonction
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