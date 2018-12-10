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
        public static bool partieGagnee = false;
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

        // ***************************
        // FONCTIONS D'INITIALISATION
        // ***************************
        public static void InitialiserPlateau()
        {
            for (int i = 0; i < TAILLE; i++)
            {
                for (int j = 0; j < TAILLE; j++)
                {
                    Piece p = new Piece();
                    p.pieceNulle = true;
                    plateau[i, j] = p;
                }
            }
        }
        public static void InitialiserPioche()
        {
            ConsoleColor c1 = ConsoleColor.DarkYellow;
            ConsoleColor c2 = ConsoleColor.DarkRed;
            int taillePioche = pioche.Length;
            for (int i = 0; i < taillePioche; i++)
            {
                Piece p = new Piece();
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

                pioche[i] = p;
            }
        }


        // ***************************
        //    FONCTIONS D'AFFICHAGE
        // ***************************
        public static void AfficherPlateau()
        {
            string separateurLigne = " ***********************";
            Console.WriteLine(separateurLigne);

            for (int i = 0; i < TAILLE; i++) // pour chaque ligne
            {
                for (int j = 0; j < 3; j++) // pour chacune des 3 lignes d'affichage de chaque ligne du plateau
                {
                    Console.Write("| ");
                    for (int k = 0; k < TAILLE; k++) // pour chaque colonne
                    {
                        Piece p = plateau[i, k];
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
                    Piece p = pioche[j];
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
            AfficherMoitiePioche(0, pioche.Length / 2);
            AfficherMoitiePioche(pioche.Length / 2, pioche.Length);
        }
        public static void AfficherPiece(int numPiece)
        {
            Piece p = pioche[numPiece];
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


        // ***************************
        //     FONCTIONS OUTILS
        // ***************************
        public static bool TesterLignePleine(int x)
        {
            bool pleine = true;
            int i = 0;
            while (pleine && i < TAILLE) // on parcourt les cases de la ligne, dès qu'on en trouve une vide c'est que la ligne n'est pas pleine donc on sort de la boucle
            {
                if (plateau[x, i].pieceNulle)
                    pleine = false;
                i++;
            }
            return pleine;
        }
        public static bool TesterColonnePleine(int y)
        {
            bool pleine = true;
            int i = 0;
            while (pleine && i < TAILLE) // on parcourt les cases de la colonne, dès qu'on en trouve une vide c'est que la colonne n'est pas pleine donc on sort de la boucle
            {
                if (plateau[i, y].pieceNulle)
                    pleine = false;
                i++;
            }
            return pleine;
        }
        public static bool TesterDiag1Pleine() // Diagonale de gauche à droite
        {
            bool pleine = true;
            int i = 0;
            int j = 0;
            while (pleine && i <TAILLE)
            {
                if (plateau[i, j].pieceNulle)
                    pleine = false;
                i++;
                j++;
            }
            return pleine;
        }
        public static bool TesterDiag2Pleine() // Diagonale de droite à gauche
        {
            bool pleine = true;
            int i = TAILLE - 1;
            int j = 0;
            while (pleine && i < TAILLE)
            {
                if (plateau[i, j].pieceNulle)
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
            while (plein && i < TAILLE) // on parcourt les lignes du plateau, dès qu'on en trouve une vide c'est que le plateau n'est pas plein donc on sort de la boucle
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
            for (int i = 0; i < pioche.Length; i++)
            {
                if (!pioche[i].pieceNulle)
                    nbPiecesRestantes++;
            }
            return nbPiecesRestantes;
        }


        // ***************************
        //      FONCTIONS DE JEU
        // ***************************
        public static int DemanderPieceAuJoueur()
        {
            AfficherPioche();
            Console.WriteLine("Quelle pièce voulez-vous donner à l'ordinateur ? Veuillez entrer le numéro de la pièce.");
            int piece = int.Parse(Console.ReadLine());
            while (piece < 0 || piece > 15 || pioche[piece].pieceNulle)
            {
                Console.WriteLine("La pièce que vous avez choisie n'est pas dans le tableau. Entrez un numéro de pièce valide.");
                piece = int.Parse(Console.ReadLine());
            }
            return piece;
        }
        public static int DonnerPieceAuJoueur()
        {
            Random rand = new Random();
            int nPiece = rand.Next(CompterPiecesRestantesPioche()); // on choisit un numéro dans le nombre de pièces restantes

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



        public static int[] PlacerPieceJoueur(int numPiece)
        {
            Console.WriteLine("Où voulez-vous placer la pièce ?");
            Console.Write("X [1;" + TAILLE + "] : ");
            int x = int.Parse(Console.ReadLine()) - 1;
            Console.Write("Y [1;" + TAILLE + "] : ");
            int y = int.Parse(Console.ReadLine()) - 1;
            while (!plateau[x,y].pieceNulle)
            {
                Console.WriteLine("Cette case est déjà prise. Entrez une nouvelle case.");
                Console.Write("X : ");
                x = int.Parse(Console.ReadLine()) - 1;
                Console.Write("Y : ");
                y = int.Parse(Console.ReadLine()) - 1;
            }

            // Une fois que le joueur a donné un x et un y correspondant à une case vide, on place la pièce sur cette case et on enlève la pièce de la pioche
            plateau[x, y] = pioche[numPiece];
            pioche[numPiece].pieceNulle = true;

            return new int[] { x, y };
        }
        public static int[] PlacerPieceOrdi(int numPiece) // renvoie la position [x,y] de l'emplacement choisi
        {
            Random rand = new Random();
            int x = 0;
            do
            {
                x = rand.Next(4);
            }
            while (TesterLignePleine(x)); // tant que la ligne sur laquelle on veut placer le pion est déjà pleine, on cherche une autre ligne

            int y = 0;
            do
            {
                y = rand.Next(4);
            }
            while (!plateau[x, y].pieceNulle);

            // Une fois qu'on a trouvé un x et un y correspondant à une case vide, on place la pièce sur cette case et on enlève la pièce de la pioche
            plateau[x, y] = pioche[numPiece];
            pioche[numPiece].pieceNulle = true;

            return new int[] { x, y };
        }



        public static bool DemanderSiQuarto()
        {
            Console.WriteLine("Voyez-vous un QUARTO (alignement de 4 pièces ayant toutes une caractéristique en commun, en ligne, colonne ou diagonale) ? [O/N]");
            return (Console.ReadLine() == "O");
        }
        // TODO LES 4 FONCTIONS SUIVANTES NE SONT PAS OPTIMISEES, ON CHERCHE UN QUARTO SUR TOUS LES CRITERES A LA FOIS, on pourrait les chercher les uns après les autres et arrêter dès qu'on trouve un quarto sur un critère
        public static bool ChercherQuartoOrientation(int x, int y, string orientationTestee) // orientationTestee : ligne/colonne/diag1/diag2
        {
            if ( (orientationTestee == "ligne" && !TesterLignePleine(x)) // si on teste la ligne et qu'elle n'est pas pleine, il ne peut pas y avoir Quarto
                 || (orientationTestee == "colonne" && !TesterColonnePleine(y))  // si on teste la colonne et qu'elle n'est pas pleine, il ne peut pas y avoir Quarto
                 || (orientationTestee == "diag1" && !TesterDiag1Pleine()) // si on teste la diagonale de gauche à droite et qu'elle n'est pas pleine, il ne peut pas y avoir Quarto
                 || (orientationTestee == "diag2" && !TesterDiag2Pleine()) ) // si la diagonale de droite à gauche n'est pas pleine
                return false;

            int nbBlanc = 0;
            int nbBas = 0;
            int nbPlein = 0;
            int nbRond = 0;
            for (int i = 0, j = TAILLE - 1; i < TAILLE; i++, j--) // j ne sert que pour la diag2
            {
                Piece p;
                if (orientationTestee == "ligne")
                    p = plateau[x, i];
                else if (orientationTestee == "colonne")
                    p = plateau[i, y];
                else if (orientationTestee == "diag1")
                    p = plateau[i, i]; // dans le cas de la diagonale de gauche à droite, on ne teste que les cases de mêmes x et y
                else
                    p = plateau[i, j];

                if (p.couleur == ConsoleColor.DarkYellow) nbBlanc++;
                if (p.hauteur == 1) nbBas++;
                if (p.remplie) nbPlein++;
                if (p.forme == '*') nbRond++;
            }

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
                || (x != TAILLE - y - 1 && ChercherQuartoOrientation(x, y, "diag2")); // on vérifie qu'on est sur la diagonale de droite à gauche avant de tester cette diagonale TODO idem au-dessus
        }



        public static void JouerOrdi()
        {
            int numPiece = DemanderPieceAuJoueur();
            int[] emplacement = PlacerPieceOrdi(numPiece);
            AfficherPlateau();
            bool quarto = ChercherQuarto(emplacement[0], emplacement[1]);
            if (quarto)
            {
                Console.WriteLine("L'ordinateur a fait un quarto ! TODO préciser sur quels critères");
                partieGagnee = true;
            }
        }
        public static void JouerJoueur()
        {
            AfficherPlateau();

            int numPiece = DonnerPieceAuJoueur();
            Console.WriteLine("L'ordinateur vous demande de jouer la pièce suivante :");
            AfficherPiece(numPiece);

            int[] emplacement = PlacerPieceJoueur(numPiece);

            AfficherPlateau();
            bool quartoJoueur = DemanderSiQuarto();
            bool quartoOrdi = ChercherQuarto(emplacement[0], emplacement[1]);
            if (quartoJoueur && quartoOrdi)
            {
                Console.WriteLine("Bravo, il y a bien un quarto, vous avez gagné !");
                partieGagnee = true;
            }
            else if (quartoOrdi)
            {
                Console.WriteLine("Vous avez déclaré qu'il n'y avait pas de quarto mais l'ordinateur en a trouvé un, donc l'ordinateur a gagné !");
                partieGagnee = true;
            }
            else if (quartoJoueur)
                Console.WriteLine("Désolé, il n'y a pas de quarto ou l'ordinateur ne trouve pas votre quarto. La partie continue.");
            else
                Console.WriteLine("Vous avez déclaré qu'il n'y avait pas de quarto. La partie continue.");
        }
        public static void DonnerTourDeJeu(int numJoueur)
        {
            if (numJoueur == 0)
            {
                Console.WriteLine("\nC'est au tour de l'ordinateur de jouer !");
                JouerOrdi();
            }
            else
            {
                Console.WriteLine("\nC'est à votre tour de jouer !");
                JouerJoueur();
            }
        }
        public static void Jouer()
        {
            /*Console.WriteLine("Quel est votre nom ?");
            string nomJoueur = Console.ReadLine();*/

            Console.WriteLine("Tirage au sort du joueur qui va commencer la partie...");
            Random rand = new Random();
            int numJoueur = rand.Next(2);
                
            do
            {
                DonnerTourDeJeu(numJoueur);
                numJoueur = (numJoueur + 1) % 2;
            }
            while (!TesterPlateauPlein() && !partieGagnee);
        }



        static void Main(string[] args)
        {
            Console.WriteLine("Projet QUARTO --- NICOL - MORELLE\n\n");

            InitialiserPlateau();
            InitialiserPioche();
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
            Jouer();

            Console.ReadLine();
        }
    }
}