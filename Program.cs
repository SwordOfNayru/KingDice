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
        private static string[] menu = new string[] { "Jeu de jet", "Jeu de jet avec relance", "Jeu \"Hit or Stop\"", "Choissir nom", "Activer le bot", "Statistique", "Sorti", "test" };
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
                    case 1: nbparti++; OptJeu1(); break;
                    case 2: nbparti++; OptJeu2(); break;
                    case 3: nbparti++; OptJeu3(); break;
                    case 4: OptNom(); break;
                    case 5: OptBot(); break; //OptBot
                    case 8: test(); break;

                }


            } while (choix != 7);
        }

        //Fonction des Option/Jeu
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
        private static void OptJeu3() //Gros problème de compression du code necessite une fonction pour compresser
        {
            //Variable
            List<int> suite;
            bool fin;
            double addsuite; esp;
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
                            if (esp < 0)
                            {
                                fin = true;
                                joueur.Actuel.Point = suite.Sum();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Si vous voulez continuer la suite appuyer sur O sinon N\nEchap pour retourner au menu principale.");
                            do
                            {
                                saisi = Console.ReadKey(true).Key;
                                if (saisi == ConsoleKey.Escape) goto end;
                            } while (saisi != ConsoleKey.O && saisi != ConsoleKey.N);
                            if(saisi == ConsoleKey.N)
                            {
                                fin = true;
                                joueurs.Actuel.Point = suite.Sum();
                            }
                        }
                    }
                } while (fin);
                joueurs.TourSuivant();
            } while (!(joueurs.TesteLimite(100)));
        end:;
        }

        private static void Debut123()
        {
            CREPosOri();
            Animation.EnteteTour(joueurs.Tour, joueurs.Actuel.Nom);
            Console.SetCursorPosition(Console.CursorLeft + 5, Console.CursorTop + 2);
            Animation.AnimationDe(joueurs.Actuel.JetDeDé());
            Console.SetCursorPosition(0, Console.CursorTop + 2);
        }
        /*Un jeu ou le joueur lance 5 dès pour peut selectionner jusqu'à 3 dès pour les relancer*/
        private static void OptJeu4()
        {

        }

        private static void OptNom()
        {
            //Variable
            int choix;
            string[] menuNom = new string[3] {"","","Retour au menu principal" };
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

        private static void OptNomModif(Joueur joueur)
        {
            CREPosOri();
            joueur.Nom = Menu.MenuChaine("Quel nom pour " + joueur.Nom + " ? (max 25 char)", 60, 25);
            CREPosOri();
            Console.WriteLine("Le pseudo à bien était changé");
            System.Threading.Thread.Sleep(600);
        }

        private static void ModifNomDebut()
        {
            CREPosOri();
            joueurs.Table[0].Nom = Menu.MenuChaine("Quel pseudo pour le Joueur A ? (max 25)", 0, 25);
            CREPosOri();
            joueurs.Table[1].Nom = Menu.MenuChaine("Quel pseudo pour le Joueur B ? (max 25)", 0, 25);
        }

        private static void OptBot()
        {
            CREPosOri();
            if(joueurs.Table[1].SetBot())
            {
                Console.WriteLine("Le Bot est Activé");
                menu[4] = "Déactiver le bot";
            }
            else
            {
                Console.WriteLine("Le bot est Déactivé");
                menu[4] = "Activer le bot";
            }
            System.Threading.Thread.Sleep(1000);
        }

        private static void OptStat() //Option qui permet d'afficher les statistique.
        {

        }

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


    //fonction Commune

        private static void CREPosOri() //Efface tout l'écran sauf le titre
        {
            Console.SetCursorPosition(curseurOrigine[0], curseurOrigine[1]);
            Animation.ClearResteEcran();
            Console.SetCursorPosition(curseurOrigine[0], curseurOrigine[1]);
        }

        private static void Victoire(Joueur gagnant)
        {
            Console.Write("Vous avez gagné lol J'ai pas envie de terminer ce programme à est c'est {0} qui gagne", gagnant.Nom);
            Console.ReadKey();
        }
        private static ConsoleKey ContinueOuQuitter()
        {
            Console.WriteLine("Appuyer sur un touche pour continuer ou sur Echap pour retourner au menu principale ...");
            return Console.ReadKey(true).Key;
        }
    }
}
