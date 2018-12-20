using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetQuarto
{
    class Sauvegarde
    {
        public static void SauvegarderPartie()
        {
            string nomDossier = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            StreamWriter fichierSauvegarde = new StreamWriter(nomDossier + "\\sauvegarde" + ".csv");
            fichierSauvegarde.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmss"));
            fichierSauvegarde.WriteLine("pioche");
            fichierSauvegarde.WriteLine("pieceNulle;couleur;hauteur;forme;remplie");
            for (int i = 0; i < 16; i++)
            {
                Program.Piece p = Program.pioche[i];
                fichierSauvegarde.Write(p.pieceNulle + ";");
                fichierSauvegarde.Write(p.couleur + ";");
                fichierSauvegarde.Write(p.hauteur + ";");
                fichierSauvegarde.Write(p.forme + ";");
                fichierSauvegarde.Write(p.remplie + "\n");
            }
            fichierSauvegarde.WriteLine("plateau");
            for (int i = 0; i < Program.TAILLE; i++)
            {
                for (int j = 0; j < Program.TAILLE; j++)
                {
                    Program.Piece p = Program.plateau[i, j];
                    fichierSauvegarde.Write(p.pieceNulle + ";");
                    fichierSauvegarde.Write(p.couleur + ";");
                    fichierSauvegarde.Write(p.hauteur + ";");
                    fichierSauvegarde.Write(p.forme + ";");
                    fichierSauvegarde.Write(p.remplie + "\n");
                }
            }
            fichierSauvegarde.WriteLine("tourJoueur");
            fichierSauvegarde.WriteLine(Program.tourJoueur);

            fichierSauvegarde.Close();
        }

        public static bool RecupererPartie()
        {
            string nomDossier = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string nomFichier = nomDossier + "\\sauvegarde.csv";
            string[] fichiers = Directory.GetFiles(nomDossier);
            if (fichiers.Contains(nomFichier) && Saisie.DemanderRecupererPartie()) // On vérifie qu'il y a une dernière partie en cours avant de demander au joueur s'il veut la reprendre
            {
                string fichier = fichiers[0];
                using (StreamReader sr = new StreamReader(nomFichier))
                {
                    string ligne;
                    int cpt = 0;
                    while ((ligne = sr.ReadLine()) != null)
                    {
                        string[] valeurs = ligne.Split(';');
                        if (cpt > 2 && cpt < 19)
                        {
                            Program.pioche[cpt - 3] = Outils.ConvertirStringPiece(valeurs);
                        }

                        else if (cpt > 19 && cpt < 36)
                        {
                            int posPlateau = cpt - 20;
                            Program.plateau[posPlateau / Program.TAILLE, posPlateau % Program.TAILLE] = Outils.ConvertirStringPiece(valeurs);
                        }

                        else if (cpt > 36)
                        {
                            Program.tourJoueur = int.Parse(valeurs[0]);
                        }

                        cpt++;
                    }
                }
                return true;
            }
            return false;
        }

        public static void SupprimerFichierSauvegarde()
        {
            string nomDossier = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string nomFichier = nomDossier + "\\sauvegarde.csv";
            File.Delete(nomFichier);
        }
    }
}
