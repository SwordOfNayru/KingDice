using System;
/*


    ############################
    #                          #
    #   Piste d'amélioration   #
    #                          #
    ############################
    
     
*/
public class Menu
{
    public static void AffMenu(string[] contenu, string titre = "", int taille = 0) //en couleur de base
    {
        //Récupération de la taille maximale
        if(taille == 0 || taille < TailleStringMax(contenu))
            taille = TailleStringMax(contenu);

        
        //Affichage du Titre du tableau
        if (titre != "")
        {
            LigneSimple("╔", '═', titre.Length, "╗");
            Console.WriteLine("║" + titre + "║");
            LigneSimple("╚",'═', titre.Length, "╝");
        }
        //fin Affichage titre

        //Affchage Menu
        LigneSimple("┌─┬",'─', taille,"┐");
        for(int i = 0; i < contenu.Length; i++)
        {

            Console.Write("│{0}│", (i + 1));
            Console.Write("{0}", contenu[i]);
            RepChar(' ', (taille - contenu[i].Length));
            Console.WriteLine("│");


            if( i != (contenu.Length - 1))
            {
                LigneSimple("├─┼", '─', taille, "┤");
            }
            else
            {
                LigneSimple("├─┴",'─', taille,"┤");
            }
        }
        LigneSimple("│", ' ', (taille + 2), "│");
        LigneSimple("└", '─', (taille + 2), "┘");

    }

    public static void TableauDoubleEntree()  //Création d'un objet Tableau en-tête
    {

    }
    //Fonction d'aide à l'affichage
    public static void RepChar(char c, int taille) //Répète un caractère
    {
        for(int i = 0; i < taille; i++)
        {
            Console.Write(c);
        }
    }
    public static void LigneSimple(string debut, char c, int taillemilieu, string fin) //Permet d'afficher une ligne constituer d'un début (||) un milieu d'un caractère répétable (-) et d'une fin pour retourne à la ligne
    {
        Console.Write(debut);
        RepChar(c, taillemilieu);
        Console.WriteLine(fin);
    }
    public static string Centre(string chaine, int taille, char espaceur = ' ') //Permet de centré dans un longueur de chaine donnée avec un espaceur
    {
        if (chaine.Length < taille)
        {
            return chaine;
        }
        else
        {
            for (int i = 0; i <= ((taille - chaine.Length) / 2); i++)
            {
                chaine = espaceur + chaine + espaceur;
            }

            if (chaine.Length == taille)
                return chaine;
            else
                return chaine + espaceur;
        }
    }
    /// <summary>
    /// retourne la taille de la chaine de caractère la plus grande d'une tableau
    /// </summary>
    /// <param name="tableau"></param>
    /// <returns></returns>
    public static int TailleStringMax(string[] tableau)
    {
        int taille = 0;
        for (int i = 0; tableau.Length > i; i++)
        {
            if (tableau[i].Length > taille)
                taille = tableau[i].Length;
        }
        return taille;
    }

    /// <summary>
    /// Fonction de choix du menu
    /// </summary>
    /// <param name="contenu"></param>
    /// <param name="titre"></param>
    /// <param name="taille"></param>
    /// <returns></returns>
    public static int MenuChoix(string[] contenu, string titre = "", int taille = 0)
    {
        int col, row, choix;
        if (taille == 0)
            taille = TailleStringMax(contenu);
        AffMenu(contenu, titre, taille);
        col = Console.CursorLeft;
        row = Console.CursorTop;
        col += 2;
        row -= 2;
        do
        {
            Console.SetCursorPosition(col, row);
            RepChar(' ', (taille + 1)); Console.Write("│");
            Console.SetCursorPosition(col, row);
            Console.Write("Choix : ");
            choix = Clavier.LireEntier();

        } while (choix < 0 || choix > contenu.Length);
        return choix;        
    }

    //Fonction d'un menu d'entre de caractère
    public static string MenuChaine(string question, int taille = 0, int nbcharmax = 0)
    {
        //Variable
        if (taille == 0 || taille < question.Length)
            taille = question.Length;
        string choix = "";

        //Fonction
        LigneSimple("┌", '─', taille, "┐");
        Console.Write("│" + question);
        RepChar(' ', taille - question.Length);
        Console.WriteLine("│");
        LigneSimple("├", '─', taille, "┤");
        Console.WriteLine("");
        LigneSimple("└", '─', taille, "┘");
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 2);
        do
        {
            LigneSimple("│", ' ', taille, "│");
            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop - 1);
            Console.CursorVisible = true;
            choix = Clavier.LireChaine();
            Console.CursorVisible = false;
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
        } while (nbcharmax != 0 && choix.Length > nbcharmax);
        return choix;
    }


}
