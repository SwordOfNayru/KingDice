using System;

public class Clavier
{
    public static void Pause()
        ///attente d'une frappe au clavier
    {
        Console.ReadKey();
    }

    public static int LireEntier()
        ///saisie d'une variable de type int
	{
        string s;
        try //détecte une erreur du genre appui direct sur "entrée" ==> renvoie null
        {
            s = Console.ReadLine();
            return int.Parse(s);
        }
        catch (Exception) //gère l'erreur détectée
        {
            Console.Write("Saisir une valeur type entier <> de null");
            return (-1);
        }

	}

    public static float LireFloat()
    ///saisie d'une variable de type float
    {
        string s;
        try
        {
            s = Console.ReadLine(); 
            return float.Parse(s);
        }
        catch (Exception)
        {
            Console.Write("Saisir une valeur type float <> de null");
            return (-1);
        }
    }

    public static double LireDouble()
    ///saisie d'une variable de type double
    {
        string s;
        try
        {
            s = Console.ReadLine();
            return double.Parse(s);
        }
        catch (Exception)
        {
            Console.Write("Saisir une valeur <> de null");
            return (-1);
        }
    }

    /// <summary>
    /// Permet la saisi utilisateur d'une chaine de caractère
    /// </summary>
    /// <param name="erreur">Si vrai affiche les erreurs</param>
    /// <returns></returns>
    public static string LireChaine(bool erreur = false)
    ///saisie d'une variable de type chaine
    {
        string s;
        s = Console.ReadLine();
        try
        {
            return s;
        }
        catch (Exception)
        {

            if (erreur) Console.Write("Saisir une valeur <> de null");
            return ("");
        }
    }

    public static char LireCarac()
    {
        char carac = Console.ReadKey().KeyChar;
        try
        {
        }
        catch (OverflowException )
        {
            Console.WriteLine("{0} Value read = {1}.", carac);
            carac = Char.MinValue;
            Console.WriteLine("trop grand");
        }
        return carac;
    }

    public static void LireCarac2(char[] Tcarac)
    {
        char carac;
        int i=0;
        Tcarac = new char[10];
        do
        {
            carac = Console.ReadKey().KeyChar;
            try
            {
                if (carac != ' ')
                {
                    Tcarac[i] = carac;
                    i++;
                }
            }
            catch (OverflowException )
            {
                Console.WriteLine("caractère non valide");
            }
        } while (carac != '0');
    }

    public static bool LireBool()
    {
        bool flag = true;
        char ch = Console.ReadKey().KeyChar;

        try
        {
            if (Char.IsLetter(ch)) //c'est un caractère alphabétique?
            {
                ch = Char.ToUpper(ch); //convertit en majuscule
                if (ch == 0x56 || ch == 0x54) //le caractère est '=V' pour 'vrai'
                    flag = true;
                else
                    flag = false;
            }
            else
                Console.WriteLine("Erreur de saisie");
        }
        catch (OverflowException ) //gestion d'erreur
        {
            Console.WriteLine("{0} Value read = {1}.",  ch);
        }
        return flag;
    }

}

