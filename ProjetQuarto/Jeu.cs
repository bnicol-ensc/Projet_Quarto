using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * ATTENTION
 * L'ORDI N'A PASDETECTE LE QUARTO A SON TOUR DE JEU SUR LA DIAGONALE DE GAUCHE A DROITE
 * CREER UNE SAUVEGARDE POUR DEBUG
 **/

namespace ProjetQuarto
{
    class Jeu
    {
        public static int DemanderPieceAuJoueur()
        {
            Affichage.AfficherPioche();
            Console.WriteLine("Quelle pièce voulez-vous donner à l'ordinateur ? Veuillez entrer le numéro de la pièce.");
            int piece = int.Parse(Saisie.SaisieJoueur());
            while (piece < 0 || piece > 15 || Program.pioche[piece].pieceNulle)
            {
                Console.WriteLine("La pièce que vous avez choisie n'est pas dans le tableau. Entrez un numéro de pièce valide.");
                piece = int.Parse(Saisie.SaisieJoueur()); // TODO on pourrait éventuellement mettre cette vérification dans Saisie.cs
            }
            return piece;
        }
        public static int DonnerPieceAuJoueur()
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



        public static int[] PlacerPieceJoueur(int numPiece)
        {
            Console.WriteLine("Où voulez-vous placer la pièce ?");
            Console.Write("X [1;" + Program.TAILLE + "] : ");
            int x = int.Parse(Saisie.SaisieJoueur()) - 1;
            Console.Write("Y [1;" + Program.TAILLE + "] : ");
            int y = int.Parse(Saisie.SaisieJoueur()) - 1;
            while (!Program.plateau[x, y].pieceNulle)
            {
                Console.WriteLine("Cette case est déjà prise. Entrez une nouvelle case.");
                Console.Write("X : ");
                x = int.Parse(Saisie.SaisieJoueur()) - 1;
                Console.Write("Y : ");
                y = int.Parse(Saisie.SaisieJoueur()) - 1;
            }

            // Une fois que le joueur a donné un x et un y correspondant à une case vide, on place la pièce sur cette case et on enlève la pièce de la pioche
            Program.plateau[x, y] = Program.pioche[numPiece];
            Program.pioche[numPiece].pieceNulle = true;

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
            while (Outils.TesterLignePleine(x)); // tant que la ligne sur laquelle on veut placer le pion est déjà pleine, on cherche une autre ligne

            int y = 0;
            do
            {
                y = rand.Next(4);
            }
            while (!Program.plateau[x, y].pieceNulle);

            // Une fois qu'on a trouvé un x et un y correspondant à une case vide, on place la pièce sur cette case et on enlève la pièce de la pioche
            Program.plateau[x, y] = Program.pioche[numPiece];
            Program.pioche[numPiece].pieceNulle = true;

            return new int[] { x, y };
        }



        public static bool DemanderSiQuarto()
        {
            Console.WriteLine("Voyez-vous un QUARTO (alignement de 4 pièces ayant toutes une caractéristique en commun, en ligne, colonne ou diagonale) ? [O/N]");
            return ((Saisie.SaisieJoueur()).ToUpper() == "O");
        }
        
        public static bool VerifierQuartoJoueur(int x, int y)
        {
            string dimension = Saisie.SaisieDimension();
            string critere = Saisie.SaisieCritere();

            bool quartoOrdi = Outils.ChercherQuartoOrientation(x, y, dimension, critere);

            if (quartoOrdi)
            {
                Console.WriteLine("Bravo, il y a bien un quarto sur le critère " + critere + " sur la " + dimension + " " + (dimension == "ligne" ? (x + 1) : (y + 1)) + ", vous avez gagné !");
                Program.finPartie = true;
            }
            else
            {
                Console.WriteLine("Désolé, il n'y a pas de quarto sur le critère " + critere + " sur la " + dimension + " " + (dimension == "ligne" ? (x + 1) : (y + 1)) + ".");
                quartoOrdi = Outils.ChercherQuarto(x, y);
                if (quartoOrdi)
                {
                    Console.WriteLine("L'ordinateur a trouvé un quarto que vous n'avez pas vu, il a donc gagné !");
                    Program.finPartie = true;
                }
            }

            return quartoOrdi;
        }

        public static void JouerOrdi()
        {
            int numPiece = DemanderPieceAuJoueur();
            int[] emplacement = PlacerPieceOrdi(numPiece);
            Affichage.AfficherPlateau();
            bool quarto = Outils.ChercherQuarto(emplacement[0], emplacement[1]);
            if (quarto)
            {
                Console.WriteLine("L'ordinateur a gagné !");
                Program.finPartie = true;
            }
        }
        public static void JouerJoueur()
        {
            Affichage.AfficherPlateau();

            int numPiece = DonnerPieceAuJoueur();
            Console.WriteLine("L'ordinateur vous demande de jouer la pièce suivante :");
            Affichage.AfficherPiece(numPiece);

            int[] emplacement = PlacerPieceJoueur(numPiece);

            Affichage.AfficherPlateau();
            bool quartoJoueur = DemanderSiQuarto();

            if (quartoJoueur)
                VerifierQuartoJoueur(emplacement[0], emplacement[1]);
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
            string nomJoueur = Affichage.RecupEntreeJoueur();*/

            if (Program.tourJoueur == -1)
            {
                Console.WriteLine("Tirage au sort du joueur qui va commencer la partie...");
                Random rand = new Random();
                Program.tourJoueur = rand.Next(2);
            }

            do
            {
                DonnerTourDeJeu(Program.tourJoueur);
                Program.tourJoueur = (Program.tourJoueur + 1) % 2;
                if (!Outils.TesterPlateauPlein() && !Program.finPartie)
                    Sauvegarde.SauvegarderPartie(); // On sauvegarde la partie à chaque tour de jeu TODO modifier le fichier de sauvegarde au lieu de le refaire à chaque fois
            }
            while (!Outils.TesterPlateauPlein() && !Program.finPartie);

            // Si on arrive ici, la partie est finie (si le joueur quitte la partie en cours, c'est qu'il a arrêté le programme avant d'arriver ici)
            // Donc on supprime ici le fichier de sauvegarde, car on ne souhaite pas sauvegarder une partie déjà terminer
            Sauvegarde.SupprimerFichierSauvegarde();
        }
    }
}
