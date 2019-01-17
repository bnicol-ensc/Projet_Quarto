using System;
using System.Linq;

namespace ProjetQuarto
{
    class Saisie
    {
        public static string SaisirJoueur()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string message = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            return message;
        }


        public static bool DemanderRecupererPartie()
        {
            Affichage.AfficherMessage("Voulez-vous reprendre la dernière partie en cours ?", ConsoleColor.DarkCyan);
            Affichage.AfficherMessage(" [O/N]\n");
            return SaisirJoueur().ToUpper() == "O";
        }


        public static string SaisirDimension() // la dimension doit être "ligne", "colonne", "diag1" ou "diag2"
        {
            Affichage.AfficherMessage("Sur quelle dimension voyez-vous le quarto ?", ConsoleColor.DarkCyan);
            Affichage.AfficherMessage(" [ligne/colonne/diag1/diag2] (diag1 = diagonale de gauche à droite, diag2 = diagonale de droite à gauche)\n");
            return SaisirDans(new string[] { "ligne", "colonne", "diag1", "diag2" });
        }

        public static string SaisirCritere() // le critère doit être dans [couleur/forme/hauteur/remplie]
        {
            Affichage.AfficherMessage("Sur quel critère ?", ConsoleColor.DarkCyan);
            Affichage.AfficherMessage(" [couleur/forme/hauteur/remplissage]\n");
            return SaisirDans(new string[] { "couleur", "forme", "hauteur", "remplissage" });
        }
        
        public static string SaisirDans(string[] listePossibilites) // Demande au joueur de saisir une valeur contenue dans la liste passée en paramètre
        {
            string valeur = SaisirJoueur().ToLower();
            while (!listePossibilites.Contains(valeur))
            {
                Affichage.AfficherMessage("Erreur, veuillez saisir une valeur parmi les suivantes : ", ConsoleColor.Red);
                Console.Write("[");
                for (int i = 0; i < listePossibilites.Length; i++)
                {
                    Console.Write(listePossibilites[i]);
                    if (i < listePossibilites.Length - 1)
                        Console.Write("/");
                }
                Console.Write("]\n");
                valeur = SaisirJoueur().ToLower();
            }

            return valeur;
        }

        public static int[] SaisirEmplacementJoueur()
        {
            Affichage.AfficherMessage("Où voulez-vous placer la pièce ?\n", ConsoleColor.DarkCyan);
            Affichage.AfficherMessage("X [1;" + Program.TAILLE + "] : ");
            int x = int.Parse(SaisirJoueur()) - 1;
            Affichage.AfficherMessage("Y [1;" + Program.TAILLE + "] : ");
            int y = int.Parse(SaisirJoueur()) - 1;
            while (!Program.plateau[x, y].pieceNulle)
            {
                Affichage.AfficherMessage("Cette case est déjà prise. Entrez une nouvelle case.\n", ConsoleColor.Red);
                Console.Write("X : ");
                x = int.Parse(SaisirJoueur()) - 1;
                Console.Write("Y : ");
                y = int.Parse(SaisirJoueur()) - 1;
            }

            return new int[] { x, y };
        }

        public static int SaisirPieceJoueur()
        {
            int piece = int.Parse(SaisirJoueur());
            while (piece < 0 || piece > 15 || Program.pioche[piece].pieceNulle)
            {
                Affichage.AfficherMessage("La pièce que vous avez choisie n'est pas dans le tableau. Entrez un numéro de pièce valide.\n", ConsoleColor.Red);
                piece = int.Parse(SaisirJoueur());
            }
            return piece;
        }

        public static bool DemanderSiQuarto()
        {
            Affichage.AfficherMessage("Voyez-vous un QUARTO ", ConsoleColor.DarkCyan);
            Affichage.AfficherMessage("(alignement de 4 pièces ayant toutes une caractéristique en commun, en ligne, colonne ou diagonale) "); // gris clair par défaut
            Affichage.AfficherMessage("? ", ConsoleColor.DarkCyan);
            Affichage.AfficherMessage("[O/N]\n");
            return ((SaisirJoueur()).ToUpper() == "O");
        }
    }
}
