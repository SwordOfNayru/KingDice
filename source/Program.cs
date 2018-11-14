using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingDice
{
    public class JeuDeDes
    {
        /*
            ##########################
            #  Piste d'amelioration  #
            ##########################   
        */

        //Variable Global
        private static int nbparti = 0;
        private static string[] menu = new string[] { "Jeu de Base", "Jeu ajouté", "Statistique", "Options", "Sorti" };
        private static string[] sousMenuJeuBase = new string[] { "Jeu : Jet Simple", "Jeu : Jet avec Relance", "Jeu : Suite Hit or stop ?", "Retour au menu principal" };
        private static string[] sousMenuJeuAjoute = new string[] { "Jeu : ", "Retour au menu principal" };
        private static string[] sousMenuOptiontxt = new string[] { "Choissir nom", "Activer le Bot", "Retour au menu principal" };
        private static int[] curseurPosDJA = new int[2], curseurPosDJB = new int[2]; //variable de l'affichage du dé d'un des deux joueur
        private static int[] curseurOrigine;
        private static ConsoleColor[] CouleurTitre = new ConsoleColor[]
        {
            ConsoleColor.Red,
            ConsoleColor.Blue
        };
        private static TableJoueur joueurs = new TableJoueur(2); //Construction des joueurs

        // Main
        public static void Main()
        {
            //Déclaration des variables
            int choix;
            Console.SetWindowSize(161, 40);
            Console.CursorVisible = false;

            Animation.AnimationTitre(CouleurTitre, 10, 300);
            curseurOrigine = new int[] { Console.CursorLeft, Console.CursorTop };
            ModifNomDebut();
            //boucle menu
            do
            {
                Console.Clear();
                Animation.titre(CouleurTitre);
                Console.CursorVisible = true;
                choix = Menu.MenuChoix(menu, "", Animation.TailleTitre); //Affichage du menu
                Console.CursorVisible = false;
                switch (choix)
                {
                    default: break;
                    case 1: sousMenuBase(); break;
                    case 2: sousMenuAjoute(); break;
                    case 3: OptStat(); break;
                    case 4: sousMenuOption(); break;
                }


            } while (choix != 5);
        }
        //Fonction des menus
        private static void sousMenuBase()
        {
            int choix;
            do
            {
                CREPosOri();
                Console.CursorVisible = true;
                choix = Menu.MenuChoix(sousMenuJeuBase, "Jeu de Base");
                Console.CursorVisible = false;
                switch (choix)
                {
                    default: break;
                    case 1: nbparti++; OptJeu1(); break;
                    case 2: nbparti++; OptJeu2(); break;
                    case 3: nbparti++; OptJeu3(); break;
                }
            } while (choix != 4);
        }

        private static void sousMenuAjoute()
        {
            int choix;
            do
            {
                CREPosOri();
                Console.CursorVisible = true;
                choix = Menu.MenuChoix(sousMenuJeuAjoute, "Jeu Ajouté");
                Console.CursorVisible = false;
                switch (choix)
                {
                    default: break;
                    case 1: nbparti++; OptJeu4(); break;
                }
            } while (choix != 2);
        }

        private static void sousMenuOption()
        {

            int choix;
            do
            {
                CREPosOri();
                Console.CursorVisible = true;
                choix = Menu.MenuChoix(sousMenuOptiontxt, "Options");
                Console.CursorVisible = false;

                switch (choix)
                {
                    default: break;
                    case 1: sousMenuNom(); break;
                    case 2: OptBot(); break;
                }
            } while (choix != 3);
        }

        private static void sousMenuNom()
        {
            //Variable
            int choix;
            string[] menuNom = new string[3] { "", "", "Retour au menu Option" };
            //Fonction
            do
            {
                menuNom[0] = "Changer le nom du joueur " + joueurs.Table[0].Nom;
                menuNom[1] = "Changer le nom du joueur " + joueurs.Table[1].Nom;
                CREPosOri();
                choix = Menu.MenuChoix(menuNom, "Change un nom");
                switch (choix)
                {
                    case 1: OptNomModif(joueurs.Table[0]); break;
                    case 2: OptNomModif(joueurs.Table[1]); break;
                    case 3: break;
                }
            } while (choix != 3);
        }
        //Fonction des Option/Jeu

        /// <summary>
        /// Fonction du déroulement du jeu 1
        /// </summary>
        private static void OptJeu1() //Jeu ou le JA et JB tire Chacun leur tour un de, Le premier a 50pts gagne la parti
        {
            joueurs.QuiCommence();
            do
            {
                Debut123();
                joueurs.Actuel.Point += joueurs.Actuel.DernierDe;
                Console.WriteLine("{0} a {1} points", joueurs.Actuel.Nom, joueurs.Actuel.Point);
                if (joueurs.Actuel.Bot)
                    System.Threading.Thread.Sleep(600);
                else
                    if (ContinueOuQuitter() == ConsoleKey.Escape) goto end;
                joueurs.TourSuivant();
            } while (!(joueurs.TesteLimite(50)));
            Victoire(joueurs.QuiAGagner(50));
        end:;
        }

        /// <summary>
        /// Fonction du déroulement du jeu 2
        /// </summary>
        private static void OptJeu2() //Jeu ou le JA et JB tire Chacun leur tour un de si celui ci affiche 6 alors le joueur peut relancé un de, Le premier a 50pts gagne la parti
        {
            joueurs.QuiCommence();
            do
            {
                Debut123();
                joueurs.Actuel.Point += joueurs.Actuel.DernierDe;
                Console.WriteLine("{0} a {1} points", joueurs.Actuel.Nom, joueurs.Actuel.Point);
                if (joueurs.Actuel.DernierDe == 6) Console.WriteLine("Chouette un 6 vous rejouer");
                else joueurs.TourSuivant();
                if (joueurs.Actuel.Bot)
                    System.Threading.Thread.Sleep(600);
                else
                    if (ContinueOuQuitter() == ConsoleKey.Escape) goto end;

            } while (!(joueurs.TesteLimite(50)));
            Victoire(joueurs.QuiAGagner(50));
        end:;
        }


        /*Jeu ou le JA et JB chacun leur tour vont effectué une suite de lancé. Le joueur peut arréter a tout moment et gagnera ses pts
        Cependant si un 1 est sorti alors le joueur perd tout les points de la suite.
        Le premier arrivé a 100 gagne*/
        /// <summary>
        /// Fonction du déroulement du jeu 3
        /// </summary>
        private static void OptJeu3() //Gros problème de compression du code necessite une fonction pour compresser
        {
            //Variable
            List<int> suite;
            bool fin;
            double addsuite, esp;
            const double esppart = 3.33333; //
            ConsoleKey saisi;
            //Fonction
            joueurs.QuiCommence();
            do
            {
                suite = new List<int> { };
                fin = true;
                do
                {
                    Debut123();
                    suite.Add(joueurs.Actuel.DernierDe);
                    if (suite.Last() == 1) //PARTI DU JEU SI LE TIRAGE == 1
                    {
                        fin = false;
                        Console.WriteLine("{0} a {1} points\nDommage la suite est perdu...", joueurs.Actuel.Nom, joueurs.Actuel.Point);
                        if (joueurs.Actuel.Bot) System.Threading.Thread.Sleep(600);
                        else
                            if (ContinueOuQuitter() == ConsoleKey.Escape) goto end;
                    }
                    else //PARTI DU JEU OU LE JOUEUR CHOISI
                    {
                        Animation.LigneDeDes(suite);
                        if (joueurs.Actuel.Bot) //Le bot regarde l'espérence de gain et si l'espérence est négatif il arrète la suite.
                        {
                            addsuite = (double)suite.Sum();
                            esp = -addsuite * 0.16666 + esppart;
                            if (esp < 1)
                            {
                                fin = false;
                                joueurs.Actuel.Point = suite.Sum();
                            }
                            System.Threading.Thread.Sleep(300);
                        }
                        else
                        {
                            Console.WriteLine("Si vous voulez continuer la suite appuyer sur O sinon N\nEchap pour retourner au menu principale.");
                            do
                            {
                                saisi = Console.ReadKey().Key;
                                if (saisi == ConsoleKey.Escape) goto end;
                            } while (saisi != ConsoleKey.O && saisi != ConsoleKey.N);
                            if (saisi == ConsoleKey.N)
                            {
                                fin = false;
                                joueurs.Actuel.Point = suite.Sum();
                            }
                        }
                    }
                } while (fin);
                joueurs.TourSuivant();
            } while (!(joueurs.TesteLimite(100)));
            Victoire(joueurs.QuiAGagner(100));
        end:;
        }

        /// <summary>
        /// Affiche le début du tour commun au 3 premiers jeux
        /// </summary>
        private static void Debut123()
        {
            CREPosOri();
            Animation.EnteteTour(joueurs.Tour, joueurs.Actuel.Nom);
            Console.SetCursorPosition(Console.CursorLeft + 5, Console.CursorTop + 2);
            Animation.AnimationDe(joueurs.Actuel.JetDeDé());
            Console.SetCursorPosition(0, Console.CursorTop + 2);
        }
        /*Un jeu ou le joueur lance 5 dès pour peut selectionner jusqu'à 3 dès pour les relancer*/
        /// <summary>
        /// Fonction de déroulement du jeu 4 comporte la gestion d'un curseur de selection
        /// </summary>
        private static void OptJeu4()
        {
            ConsoleKey saisi = ConsoleKey.O;
            int[] posLigneCur;
            List<int> suite = new List<int> { };
            int[][] CurseurSel = new int[2][]; //Table qui gère la position du curseur et ce qu'il à sélectionner
            int CurPos = 0;
            do { //Loop du tour
                CREPosOri();
                Animation.EnteteTour(joueurs.Tour, joueurs.Actuel.Nom);
                for (int i = 0; i < 5; i++)
                {
                    suite.Add(joueurs.Actuel.JetDeDé());
                }

                Console.WriteLine("");
                posLigneCur = new int[] { Console.CursorLeft, Console.CursorTop };
                Console.WriteLine("");
                Animation.LigneDeDes(suite);
                Console.WriteLine("Utiliser les flêche < et > pour bouger le Curseur, Entrez pour selectionner, O pour continuer, Echap pour quitter");
                CurseurSel[0] = new int[] { 1, 0, 0, 0, 0 };
                CurseurSel[1] = new int[] { 0, 0, 0, 0, 0 };
                do //Gère le curseur et la sélection des dès a relancer.
                {
                    Console.SetCursorPosition(posLigneCur[0], posLigneCur[1]);
                    OptJeu4Selecteur(CurseurSel);
                    saisi = Console.ReadKey(true).Key;
                    CurPos = Pos1IntTable(CurseurSel[0]);

                    if (saisi == ConsoleKey.RightArrow)
                    {
                        if (CurPos != 5)
                        {
                            CurseurSel[0][CurPos] = 0;
                            CurseurSel[0][CurPos + 1] = 1;
                        }
                    }
                    else if (saisi == ConsoleKey.LeftArrow)
                    {
                        if (CurPos != 0)
                        {
                            CurseurSel[0][CurPos] = 0;
                            CurseurSel[0][CurPos - 1] = 1;
                        }
                    }
                    else if (saisi == ConsoleKey.Enter)
                    {
                        if (CurseurSel[1][CurPos] == 1)
                            CurseurSel[1][CurPos] = 0;
                        else
                        {
                            if (CurseurSel[1].Sum() < 3)
                                CurseurSel[1][CurPos] = 1;
                        }
                    }
                    else if (saisi == ConsoleKey.Escape)
                        goto end;
                } while (saisi != ConsoleKey.O);
                for (int i = 0; i < 5; i++)
                {
                    if (CurseurSel[1][i] == 1)
                        suite[i] = joueurs.Actuel.JetDeDé();
                }
                joueurs.Actuel.Point += suite.Sum();
                CREPosOri();
                Console.WriteLine("{0} votre nouvelle ligne de dés : \n", joueurs.Actuel.Nom);
                Animation.LigneDeDes(suite);
                Console.WriteLine("Vous avez {0} points", joueurs.Actuel.Point);
                if (ConsoleKey.Escape == ContinueOuQuitter())
                    goto end;
            } while (joueurs.TesteLimite(50));//Fin loop du tour
        end:;
        }

        /// <summary>
        /// Affiche la barre de selection des dés
        /// </summary>
        /// <param name="Curseur"></param>
        private static void OptJeu4Selecteur(int[][] Curseur)
        {
            for(int i = 0; i<5; i++)
            {
                Console.ResetColor();
                Console.Write(" ");
                if (Curseur[1][i] == 1)
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                else
                    Console.ResetColor();
                if (Curseur[0][i] == 1)
                    Console.Write(" V ");
                else
                    Console.Write("   ");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Retourne la position du 1 dans une table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static int Pos1IntTable(int[] table)
        {
            int count = 0;
            while(table[count] != 1)
                count++;
            return count;
        }

        /// <summary>
        /// Affiche la question pour le changement du nom
        /// </summary>
        /// <param name="joueur"></param>
        private static void OptNomModif(Joueur joueur)
        {
            CREPosOri();
            joueur.Nom = Menu.MenuChaine("Quel nom pour " + joueur.Nom + " ? (max 25 char)", 60, 25);
            CREPosOri();
            Console.WriteLine("Le pseudo à bien était changé");
            System.Threading.Thread.Sleep(600);
        }

        /// <summary>
        /// Demande les noms en début de programme
        /// </summary>
        private static void ModifNomDebut()
        {
            CREPosOri();
            joueurs.Table[0].Nom = Menu.MenuChaine("Quel pseudo pour le Joueur A ? (max 25)", 0, 25);
            CREPosOri();
            joueurs.Table[1].Nom = Menu.MenuChaine("Quel pseudo pour le Joueur B ? (max 25)", 0, 25);
        }

        /// <summary>
        /// Active ou déactive le bot en fonction de l'état
        /// </summary>
        private static void OptBot()
        {
            CREPosOri();
            if(joueurs.Table[1].SetBot())
            {
                Console.WriteLine("Le Bot est Activé");
                sousMenuOptiontxt[1] = "Déactiver le bot";
            }
            else
            {
                Console.WriteLine("Le bot est Déactivé");
                sousMenuOptiontxt[1] = "Activer le bot";
            }
            System.Threading.Thread.Sleep(750);
        }

        private static void OptStat() //Option qui permet d'afficher les statistique.
        {

        }
        /*
        private static void test()
        {
            int i;
            int[] ensemble = new int[10000];
            double[] count = new double[] { 0, 0, 0, 0, 0, 0 };
            for(i = 0; i < 10000; i++)
            {
                ensemble[i] = joueurs.Actuel.JetDeDé();
                count[ensemble[i] - 1]++;
            }
            CREPosOri();
            for(i = 0; i < 6; i++)
            {
                Console.WriteLine("{0} : {1} %", i + 1, (count[i] / 10000) * 100);
            }
            Console.ReadKey();

        }
        */

    //fonction Commune
        
         /// <summary>
         /// Supprime toute l'écran après le point dit curseurOrigine. Permet d'éfface tout sauf le titre.
         /// </summary>
        private static void CREPosOri() //Efface tout l'écran sauf le titre
        {
            Console.SetCursorPosition(curseurOrigine[0], curseurOrigine[1]);
            Animation.ClearResteEcran();
            Console.SetCursorPosition(curseurOrigine[0], curseurOrigine[1]);
        }

        /// <summary>
        /// Affiche la victoire
        /// </summary>
        /// <param name="gagnant"></param>
        private static void Victoire(Joueur gagnant)//Affiche une petite animation de victoire, Elle est placé dans Program.Cs pour eviter le liens Menu.dll > Animation.dll
        {
            ConsoleColor[] couleurs = new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow };
            string chaine = "Bravo à " + gagnant.Nom + "!!!";
            
            for (int i = 0; i< 6; i++)
            {
                CREPosOri();
                Console.ForegroundColor = couleurs[i % 3];
                Menu.LigneSimple("♠", '-', chaine.Length, "♠");
                Console.ResetColor();
                Console.WriteLine(" " + chaine + " ");
                Console.ForegroundColor = couleurs[i % 3];
                Menu.LigneSimple("♠", '-', 43, "♠");
                System.Threading.Thread.Sleep(250);
            }
            Console.ResetColor();

        }

        /// <summary>
        /// Affiche une phrase pour demandé au joueur si il veut quitter le jeu en appuyant sur echap et récupère l'info touche du joueur?
        /// </summary>
        /// <returns></returns>
        private static ConsoleKey ContinueOuQuitter()
        {
            Console.WriteLine("Appuyer sur un touche pour continuer ou sur Echap pour retourner au menu principale ...");
            return Console.ReadKey(true).Key;
        }
    }
}
