using System;
using System.Collections.Generic;

public class TableJoueur
{
    //Champs
    public List<Joueur> Table;
    public Random rng;

    //Constructeur
    public TableJoueur(int nbJoueur)
    {
        int charac = 65; //65 pour le A necessitera une verification pour modifié les noms au dela de Z 
        char c;
        Table = new List<Joueur> { };
        for(int i = 0; i < nbJoueur;i++)
        {
            c = Convert.ToChar(charac);
            Table.Add(new Joueur(c.ToString()));
            System.Threading.Thread.Sleep(5); //Permet d'avoir deux seeds différent d'aléatoire
            charac++;
        }
        rng = new Random();
        Tour = 0;
    }

    //Proprieté
    public int Tour { get; private set; }
    public int NbJoueur //Permet de raccoucir l'appel du nombre de joueur
    {
        get { return Table.Count; }
    }
    public Joueur Actuel //permet de simplifier l'appel du joueur a qui c'est le tour
    {
        get { return Table[Tour]; }
    }

    //Methode
    public void QuiCommence() //Decide de qui commence le jeu; Ce base sur des taux de chance qui change en fonction de si le joueur a commencé avant et/ou si il perd
    {
        int totalChance = AddChance();
        int palier = 0;
        int chancePalier = Table[0].ChanceStart;
        int jet = rng.Next(totalChance + 1);
        this.Reset();
        while(jet > chancePalier)
        {
            palier++;
            chancePalier += Table[0].ChanceStart;
        }
        if(Table[palier].Acommencer)
        {
            Table[palier].ChanceStart -= 10;
        }
        else
        {
            Table[palier].ChanceStart = 50;
            Commence(palier);
        }
        Tour = palier;
    }

    private int AddChance()
    {
        int Total = 0;
        for(int i = 0; i < Table.Count; i++)
        {
            Total += Table[i].ChanceStart;
        }
        return Total;
    }

    public void TourSuivant()
    {
        ++Tour;
        if (Tour >= Table.Count) Tour = 0;
    }

    private void Commence(int index) //Permet de mettre a false toute les personne que ne commence pas et a true ce qui commence
    {
        for(int i = 0;i < Table.Count;i++)
        {
            if (i == index) Table[i].Acommencer = true;
            else Table[i].Acommencer = false;
        }
    }

    /// <summary>
    /// Renvoie vrai si un joueur dépasse la limite de point
    /// </summary>
    /// <param name="limite"></param>
    /// <returns></returns>
    public bool TesteLimite(int limite)
    {
        for(int i = 0; i < NbJoueur; i++)
        {
            if (Table[i].Point >= limite) return true;
        }
        return false;
    }

    ///Retourne le joueurs qui a gagner en fonction de la limite de points
    public Joueur QuiAGagner(int pts)
    {
        for(int i = 0; i < NbJoueur; i++)
        {
            if (Table[i].Point >= pts)
            {
                Table[i].Nbvic++;
                return Table[i];
            }
        }
        return Table[0];
    }

    public void Reset()
    {
        foreach(Joueur joueur in Table)
        {
            joueur.Point = 0;
        }
    }
}
public class Joueur
{
    //Champs
    private Random _alea;
    private int _chanceStart;
    private string _nom;

    //Constructeur
    public Joueur(string nom) : this(nom, false) { }

    public Joueur(string nom, bool bot)
    {
        _nom = nom;
        Bot = bot;
        StatD = new int[] { 0, 0, 0, 0, 0, 0 };
        Nbvic = 0;
        Point = 0;
        ChanceStart = 50;
        DernierDe = 0;
        _alea = new Random(Guid.NewGuid().GetHashCode()); //une autre methode pour avoir de l'aléatoire permet de généré vraiment deux seed différente.
    }

    //Propriété
    public int Point { get; set; }
    public int DernierDe { get; private set; }
    public int Nbvic { get; set; }
    public int[] StatD { get; set; }
    public string Nom
    {
        get
        {
            if (Bot) return "[bot] " + _nom;
            else return _nom;
        }
        set
        {
            _nom = value;
            if(_nom.Length > 25)
                _nom = _nom.Remove(0, 25);
        }
    }
    public bool Bot { get; set; }
    public bool Acommencer { get; set; }
    public int ChanceStart
    {
        get { return _chanceStart; }
        set
        {
            if(value < 0) _chanceStart = 0;
            else _chanceStart = value;
        }
    }


    //Methode
    public int JetDeDé()
    {
        int jet = _alea.Next(1, 7);
        DernierDe = jet;
        StatD[jet - 1]++;       
        return jet;
    }

    public bool SetBot() //pour simplier l'OptBot
    {
        Bot = !Bot;
        return Bot;
    }

}
