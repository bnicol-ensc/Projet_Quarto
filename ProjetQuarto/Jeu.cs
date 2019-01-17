using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetQuarto
{
    class Jeu
    {
        public static int DemanderPieceAuJoueur()
        {
            Affichage.AfficherPioche();
            Affichage.AfficherMessage("Quelle pièce voulez-vous donner à l'ordinateur ? Veuillez entrer le numéro de la pièce.\n", ConsoleColor.DarkCyan);
            return Saisie.SaisirPieceJoueur();
        }



        public static int[] PlacerPieceJoueur(int numPiece)
        {
            int[] emplacement = Saisie.SaisirEmplacementJoueur(); // on demande au joueur de choisir un emplacement
            int x = emplacement[0];
            int y = emplacement[1];

            // Une fois que le joueur a donné un x et un y correspondant à une case vide, on place la pièce sur cette case et on enlève la pièce de la pioche
            Program.plateau[x, y] = Program.pioche[numPiece];
            Program.pioche[numPiece].pieceNulle = true;

            return new int[] { x, y };
        }
        

        
        public static bool VerifierQuartoJoueur(int x, int y)
        {
            string dimension = Saisie.SaisirDimension();
            string critere = Saisie.SaisirCritere();

            bool quartoOrdi = Outils.ChercherQuartoOrientation(x, y, dimension, critere);

            if (quartoOrdi)
            {
                Affichage.AfficherMessage("Bravo, il y a bien un quarto ", ConsoleColor.DarkCyan);
                Affichage.AfficherMessage("sur le critère " + critere + " sur la " + dimension + " " + (dimension == "ligne" ? (x + 1) : (y + 1)) + ", ");
                Affichage.AfficherMessage("vous avez gagné !", ConsoleColor.DarkCyan);
                Program.finPartie = true;
            }
            else
            {
                Affichage.AfficherMessage("Désolé, il n'y a pas de quarto ", ConsoleColor.DarkCyan);
                Affichage.AfficherMessage("sur le critère " + critere + " sur la " + dimension + " " + (dimension == "ligne" ? (x + 1) : (y + 1)) + ".");
                quartoOrdi = Outils.ChercherQuarto(x, y);
                if (quartoOrdi)
                {
                    Affichage.AfficherMessage("L'ordinateur a trouvé un quarto que vous n'avez pas vu, il a donc gagné !", ConsoleColor.DarkCyan);
                    Program.finPartie = true;
                }
            }

            return quartoOrdi;
        }
        
        public static void JouerOrdi()
        {
            Affichage.AfficherPlateau();
            int numPiece = Jeu.DemanderPieceAuJoueur();
            int[] emplacement = IA.PlacerPieceOrdiStrategie(numPiece);
            Affichage.AfficherPlateau();
            bool quarto = Outils.ChercherQuarto(emplacement[0], emplacement[1]);
            if (quarto)
            {
                Affichage.AfficherMessage("L'ordinateur a gagné !", ConsoleColor.DarkCyan);
                Program.finPartie = true;
            }
        }
        public static void JouerJoueur()
        {
            Affichage.AfficherPlateau();
            int numPiece = IA.DonnerPieceAuJoueurStrategie();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Affichage.AfficherMessage("L'ordinateur vous demande de jouer la pièce suivante :", ConsoleColor.DarkCyan);
            Affichage.AfficherPiece(numPiece);

            int[] emplacement = PlacerPieceJoueur(numPiece);

            Affichage.AfficherPlateau();
            bool quartoJoueur = Saisie.DemanderSiQuarto();

            if (quartoJoueur)
                VerifierQuartoJoueur(emplacement[0], emplacement[1]);
            else
                Affichage.AfficherMessage("Vous avez déclaré qu'il n'y avait pas de quarto. La partie continue.");
        }
        public static void DonnerTourDeJeu(int numJoueur)
        {
            if (numJoueur == 0)
            {
                Affichage.AfficherMessage("\nC'est au tour de l'ordinateur de jouer !\n");
                JouerOrdi();
            }
            else
            {
                Affichage.AfficherMessage("\nC'est à votre tour de jouer !\n");
                JouerJoueur();
            }
        }

        public static void Jouer()
        {
            if (Program.tourJoueur == -1)
            {
                Affichage.AfficherMessage("Tirage au sort du joueur qui va commencer la partie...\n");
                Random rand = new Random();
                Program.tourJoueur = rand.Next(2);
            }

            do
            {
                DonnerTourDeJeu(Program.tourJoueur);
                Program.tourJoueur = (Program.tourJoueur + 1) % 2;
                if (!Outils.TesterPiocheVide() && !Program.finPartie)
                    Sauvegarde.SauvegarderPartie(); // On sauvegarde la partie à chaque tour de jeu
            }
            while (!Outils.TesterPiocheVide() && !Program.finPartie);

            // Si on arrive ici, la partie est finie (si le joueur quitte la partie en cours, c'est qu'il a arrêté le programme avant d'arriver ici)
            // Donc on supprime ici le fichier de sauvegarde, car on ne souhaite pas sauvegarder une partie déjà terminée
            Sauvegarde.SupprimerFichierSauvegarde();
        }
    }
}
