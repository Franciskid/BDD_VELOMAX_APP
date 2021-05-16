using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class Pagestastique : UserControl
    {
        public Pagestastique()
        {

            int nbrpiecesvendu = 0;
            int nombrepiecevenduparclients = 0;
            float prixmoyendescommandes = 0;
            int n = 0;
            float Chiffredaffaire = 0;
            float totalclientencour = 0;
            int j = 0;
            int f = 0;
            InitializeComponent();

            this.DataContext = this;

            ///quantité
            List<Squantite> statsquantites = new List<Squantite>();

            foreach (Pieces a in BDDReader.Read<Pieces>())
            {
                nbrpiecesvendu = 0;
                foreach (Commande c in BDDReader.Read<Commande>())
                {

                    if (c.Piece != null)
                    {

                        if (a.ID.ToString() == c.Piece.ID.ToString())
                        {
                            nbrpiecesvendu++;
                            nombrepiecevenduparclients++;
                        }

                    }
                    if (c.Modele != null)
                    {

                        var r = BDDReader.ReadQuery($"SELECT cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier  FROM velomax.assemblages where nom='{c.Modele.Nom}';").FirstOrDefault();
                        foreach (object i in r)
                        {
                            if (a.ID.ToString() == i.ToString())
                            {
                                nbrpiecesvendu++;
                                nombrepiecevenduparclients++;
                            }
                        }
                    }

                }
                statsquantites.Add(new Squantite(a.ID.ToString(), a.Nom, a.Prix, a.DelaiApprovisionnement, nbrpiecesvendu));
            }

            statsquantite.ItemsSource = statsquantites;

            ///fidelité
            List<Sfidel> Sfidels = new List<Sfidel>();

            foreach (ClientIndividuel a in BDDReader.Read<ClientIndividuel>())
            {
                if (a.ProgrammeFidélité != null)
                {
                    Sfidels.Add(new Sfidel("individuel", a.Nom, a.Telephone, a.AdresseMail, a.DateAdhésionProgramme, a.ProgrammeFidélité));
                }
            }
            statsfidelite.ItemsSource = Sfidels;



            
            ///meilleurclients
            List<Smeilleur> Smeilleurs = new List<Smeilleur>();
  
            foreach (Commande a in BDDReader.Read<Commande>())
            {
                foreach (ClientIndividuel b in BDDReader.Read<ClientIndividuel>())
                {
                    j = 0;
                    f = 0;
                    if  (a.Client.ID.Equals(b.ID))
                    {
                        if (a.Piece != null)
                        {
                            var r = BDDReader.ReadQuery($"select sum(pieces.prix) from pieces, commandes, clients where commandes.clientid = clients.idClient and pieces.idPiece = commandes.pieceid and clients.nom='{b.Nom}';").FirstOrDefault();

                            j = Convert.ToInt32(r[0]);
                        }
                        if (a.Modele != null)
                        {
                            var s = BDDReader.ReadQuery($"select sum(modeles.prix) from modeles, commandes,clients where commandes.clientid=clients.idClient and commandes.modeleid=modeles.idModele and clients.nom='{b.Nom}';").FirstOrDefault();
                            f = Convert.ToInt32(s[0]);
                        }
                        totalclientencour = j + f;
                        if (totalclientencour != 0)
                        {
                            if (Smeilleurs.Contains(new Smeilleur((float)totalclientencour, "individuel", b.Nom, b.Telephone, b.AdresseMail)) == false)
                            {
                                bool test = true;
                                foreach (Smeilleur J in Smeilleurs)
                                {
                                    if (J.Nom == b.Nom)
                                        test = false;
                                }
                                if (test == true)
                                {
                                    Smeilleurs.Add(new Smeilleur((float)totalclientencour,"individuel", b.Nom, b.Telephone, b.AdresseMail));
                                }
                            }
                        }
                    }
                  
                }
            }

            foreach (Commande a in BDDReader.Read<Commande>())
            {
                foreach (ClientBoutique b in BDDReader.Read<ClientBoutique>())
                {
                    j = 0;
                    f = 0;
                    if (a.Client.ID.Equals(b.ID))
                    {
                        if (a.Piece!=null)
                        {
                            var r = BDDReader.ReadQuery($"select sum(pieces.prix) from pieces, commandes, clients where commandes.clientid = clients.idClient and pieces.idPiece = commandes.pieceid and clients.nom='{b.NomEntreprise}';").FirstOrDefault();
                                                          
                            j = Convert.ToInt32(r[0]);
                        }
                        if (a.Modele != null)
                        {
                            var s = BDDReader.ReadQuery($"select sum(modeles.prix) from modeles, commandes,clients where commandes.clientid=clients.idClient and commandes.modeleid=modeles.idModele and clients.nom='{b.NomEntreprise}';").FirstOrDefault();
                            f = Convert.ToInt32(s[0]);
                        }
                    }
                    totalclientencour = j + f;
                    if (totalclientencour != 0)
                    {
                        if (Smeilleurs.Contains(new Smeilleur((float)totalclientencour,"Boutique", b.NomEntreprise, b.Telephone, b.AdresseMail)) == false)
                        {

                            bool test = true;
                            foreach (Smeilleur J in Smeilleurs)
                            {
                                if (J.Nom == b.NomEntreprise)
                                    test = false;
                            }
                            if (test == true)
                            {
                                Smeilleurs.Add(new Smeilleur((float)totalclientencour,"Boutique", b.NomEntreprise, b.Telephone, b.AdresseMail));
                            }
                        }
                    }
                }
            }
            smeilleurclient.ItemsSource = Smeilleurs;
            ///plusieurmoyenne
            foreach (Commande c in BDDReader.Read<Commande>())
            {
                if (c.Modele != null)
                {
                    prixmoyendescommandes += c.Modele.Prix;
                }
                if (c.Piece != null)
                {
                    prixmoyendescommandes += c.Piece.Prix;
                }
                n++;
            }



            
            Chiffredaffaire = (int)prixmoyendescommandes;   ///chiffre d'affaire

            prixmoyendescommandes = (prixmoyendescommandes / n); ///prix moyen

            nombrepiecevenduparclients = (int)nombrepiecevenduparclients / (BDDReader.Read<ClientBoutique>().Count+ BDDReader.Read<ClientIndividuel>().Count); /// nombre de piece vendu en moyenne 

            moyenne.Text = prixmoyendescommandes.ToString() + " € ";
            chiffredaffaires.Text = Chiffredaffaire.ToString()+" € ";
            moyenepieces.Text = nombrepiecevenduparclients.ToString();



        }
        public class Squantite
        {
            public string ID { get; set; }
            public string Nom { get; set; }

            public float Prix { get; set; }
            public int Quantité_vendue { get; set; }

            public DateTime DelaiApprovisionnement { get; set; }


            public string Details
            {
                get
                {
                    return String.Format("{0} vaut {1} il faut attendre le {2} avant de se faire livrer.", this.Nom, this.Prix, DateTime.Now.AddMonths(this.DelaiApprovisionnement.Month));
                }
            }
            public Squantite(string ID, string nom, float prix, DateTime delaiApprovisionnement, int quantite)
            {
                this.ID = ID;
                this.Nom = nom;
                this.Prix = prix;
                this.DelaiApprovisionnement = delaiApprovisionnement;
                this.Quantité_vendue = quantite;


            }
        }
        public class Sfidel
        {
            public string TypeClient { get; set; }

            public DateTime Datedebut { get; set; }
            public string Nom { get; set; }

            public string Telephone { get; set; }

            public string Courriel { get; set; }

            public int Temps { get; set; }

            public string Details
            {
                get
                {
                    return String.Format("{0} est la date d'adhesion au programme {1}", this.Datedebut, this.Datedebut.AddYears(this.Temps));
                }
            }
            public Sfidel(string typeClient, string nom, string telephone, string courriel, DateTime datadebut, Fidelio programme)
            {
                this.TypeClient = typeClient;
                this.Nom = nom;
                this.Telephone = telephone;
                this.Courriel = courriel;
                this.Temps = (int)programme.Duree_annee;

            }
        }


        public class Smeilleur
        {
            public float Total_acheter { get; set; }
            public string TypeClient { get; set; }

            public string Nom { get; set; }

            public string Telephone { get; set; }

            public string Courriel { get; set; }

            

            public Smeilleur(float score, string typeClient, string nom, string telephone, string courriel)
            {
                this.Total_acheter = score;
                this.TypeClient = typeClient;
                this.Nom = nom;
                this.Telephone = telephone;
                this.Courriel = courriel;

            }
        }
    }
}