using System;
using System.Collections.Generic;

public class Animation
{
    public static string[,] TypoJAB = new string[,] 
    {
        {" ███ ","█████","████ "},
        {"  █  ","█   █","█   █"},
        {"  █  ","█████","████ "},
        {"█ █  ","█   █","█   █"},
        {" ██  ","█   █","████ "}
    };
    public static string[,] TypoTour = new string[,]
    {
        {"███","███","█ █","███"},
        {" █ ","█ █","█ █","█  "},
        {" █ ","███","███","█  " }
    };
    public static string[,] TypoMiniDe = new string[,]
    {
        {"   ","█  ","█  ","█ █","█ █","█ █" },
        {" █ ","   "," █ ","   "," █ ","█ █" },
        {"   ","  █","  █","█ █","█ █","█ █" }
    };
    private static int tailletitre = 156;
    public static int TailleTitre
    {
        get
        {
            return tailletitre;
        }
    }
    public static void titre(ConsoleColor[] couleur)
    {
        string[,] ligne = new string[,]
        {
            {"  ___  ____   "  ,"     _____    "," ____  _____  "  ,"    ______    " ,"  ________    " ,"     _____    ","     ______   "  ,"  _________   " },
            {" |_  ||_  _|  "  ,"    |_   _|   ","|_   \\|_   _| " ,"  .' ___  |   " ," |_   ___ `.  " ,"    |_   _|   ","   .' ___  |  "  ," |_   ___  |  " },
            {"   | |_/ /    "  ,"      | |     ","  |   \\ | |   " ," / .'   \\_|   ","   | |   `. \\ ","      | |     ","  / .'   \\_|  " ,"   | |_  \\_|  " },
            {"   |  __'.    "  ,"      | |     ","  | |\\ \\| |   "," | |    ____  " ,"   | |    | | " ,"      | |     ","  | |         "  ,"   |  _|  _   " },
            {"  _| |  \\ \\_  ","     _| |_    "," _| |_\\   |_  " ," \\ `.___]  _| ","  _| |___.' / " ,"     _| |_    ","  \\ `.___.'\\  ","  _| |___/ |  "},
            {" |____||____| "  ,"    |_____|   ","|_____|\\____| " ,"  `._____.'   " ," |________.'  " ,"    |_____|   ","   `._____.'  "  ," |_________|  " },
            {"              ","              ","              ","              ","              ","              ","              ","              "}
        };

        Console.WriteLine(" .----------------.  .----------------.  .-----------------. .----------------.  .----------------.  .----------------.  .----------------.  .----------------. ");
        Console.WriteLine("| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |");
        
        //Display des milieus
        for (int i = 0; i < ligne.GetLength(0); i++)
        {
            for (int j = 0; j < ligne.GetLength(1); j++)
            {
                Console.Write("| |");
                if (j % 2 == 0)
                    Console.ForegroundColor = couleur[0];
                else
                    Console.ForegroundColor = couleur[1];
                Console.Write(ligne[i, j]);
                Console.ResetColor();
                Console.Write("| |");
            }
            Console.WriteLine("");
        }

        Console.WriteLine("| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |");
        Console.WriteLine(" '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------' ");

    }
    public static void AnimationTitre(ConsoleColor[] couleur, int temps, int vitesse)
    {
        Random rng = new Random();
        ConsoleColor[] couleurinverse = new ConsoleColor[] { couleur[1], couleur[0] };
        for (int i = 0; i < temps; i++) {
            Console.Clear();
            if (i%2 == 0)
            {
                titre(couleur);
            }
            else
            {
                titre(couleurinverse);
            }
            System.Threading.Thread.Sleep(vitesse);
        }
    }
    public static void AffDe(int de)
    {
        try
        {
            string[,] des = new string[,]
            {
                { "█████████","█       █","█       █","█       █","█   █   █","█       █","█       █","█       █","█████████" },
                { "█████████","█       █","█ █     █","█       █","█       █","█       █","█     █ █","█       █","█████████" },
                { "█████████","█       █","█ █     █","█       █","█   █   █","█       █","█     █ █","█       █","█████████" },
                { "█████████","█       █","█ █   █ █","█       █","█       █","█       █","█ █   █ █","█       █","█████████" },
                { "█████████","█       █","█ █   █ █","█       █","█   █   █","█       █","█ █   █ █","█       █","█████████" },
                { "█████████","█       █","█ █   █ █","█       █","█ █   █ █","█       █","█ █   █ █","█       █","█████████" }
            };
            for (int i = 0; i < 9; i++)
            {
                Console.Write(des[de - 1, i]);
                Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop + 1);
            }
            Console.WriteLine("");
        }
        catch(IndexOutOfRangeException)
        {
            Console.Write("Erreur index AffDe()");
        }
    }
    public static void AnimationDe(int de)
    {
        Random rng = new Random();
        int row = Console.CursorTop, col = Console.CursorLeft;
        for (int i = 0; i < rng.Next(3, 5); i++)
        {
            AffDe(rng.Next(1, 6));
            Console.SetCursorPosition(col, row);
            System.Threading.Thread.Sleep(150);
        }
        AffDe(de);
    }
    public static void LigneDeDes(List<int> suitede) //Affiche une suite de petit dé pour le jeu 3
    {
        string ligne;
        int j;
        try
        {
            for (int i = 0; i < 3; i++)
            {
                ligne = " ";
                if (i == 1) Console.ForegroundColor = ConsoleColor.DarkGray;
                else Console.ResetColor();
                for (j = 0; j < suitede.Count; j++)
                {
                    ligne = ligne + TypoMiniDe[i, suitede[j] - 1] + " ";
                }
                Console.WriteLine(ligne);
            }
        }
        catch(IndexOutOfRangeException)
        {
            Console.Write("Erreur index");
        }
    }
    public static void EnteteTour(int JAouJB, string nom) //JAouJB == true tour du JA sinon tour du JB
    {
        int i, j;
        string affichage = "";
        for(i = 0; i < 5; i++)
        {
            for(j = 0; j<4; j++)
            {
                if(i == 0 || i == 1)
                {
                    affichage = affichage + "    ";
                }
                else
                {
                    affichage = affichage + TypoTour[i - 2, j] + " ";
                }
            }
            for(j = 0; j<2;j++)
            {
                affichage = affichage + TypoJAB[i, j == 0 ? j : (JAouJB + 1)] + " ";
            }
            affichage = affichage + "\n";
        }
        affichage = affichage + "C'est au joueur " + nom + " de jouer !";
        Console.WriteLine(affichage);
    }
    public static void ClearLigne(bool cond = true)
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        if(cond)
            Console.SetCursorPosition(0, currentLineCursor);
    }
    public static void ClearResteEcran()
    {
        int currentLineCursor = Console.CursorTop;
        for (int i = 0; i < Console.WindowHeight - currentLineCursor -1; i++)
        {
            ClearLigne(false);
            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }
    }
    public static void AlaLigne()
    {
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
    }
}