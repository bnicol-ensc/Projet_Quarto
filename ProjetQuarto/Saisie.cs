using System;
using System.Linq;

namespace ProjetQuarto
{
    class Saisie
    {
        public static string SaisieJoueur()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string message = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            return message;
        }


        public static bool DemanderRecupererPartie()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Voulez-vous reprendre la dernière partie en cours ?");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [O/N]\n");
            return SaisieJoueur().ToUpper() == "O";
        }


        public static string SaisieDimension() // la dimension doit être "ligne", "colonne", "diag1" ou "diag2"
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Sur quelle dimension voyez-vous le quarto ?");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [ligne/colonne/diag1/diag2] (diag1 = diagonale de gauche à droite, diag2 = diagonale de droite à gauche)\n");
            return SaisieDans(new string[] { "ligne", "colonne", "diag1", "diag2" });
        }

        public static string SaisieCritere() // le critère doit être dans [couleur/forme/hauteur/remplie]
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Sur quel critère ?");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [couleur/forme/hauteur/remplissage]\n");
            return SaisieDans(new string[] { "couleur", "forme", "hauteur", "remplissage" });
        }
        
        public static string SaisieDans(string[] listePossibilites) // Demande au joueur de saisir une valeur contenue dans la liste passée en paramètre
        {
            string valeur = SaisieJoueur().ToLower();
            while (!listePossibilites.Contains(valeur))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Erreur, veuillez saisir une valeur parmi les suivantes : ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[");
                for (int i = 0; i < listePossibilites.Length; i++)
                {
                    Console.Write(listePossibilites[i]);
                    if (i < listePossibilites.Length - 1)
                        Console.Write("/");
                }
                Console.Write("]\n");
                valeur = SaisieJoueur().ToLower();
            }

            return valeur;
        }
    }
}
