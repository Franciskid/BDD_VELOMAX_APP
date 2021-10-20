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
    public partial class StatistiquesPage : UserControl
    {
        public StatistiquesPage()
        {

            int nbrpiecesvendu = 0;
            InitializeComponent();

            this.DataContext = this;

            var piece = BDDReader.Read<Piece>();
            var commande = BDDReader.Read<Commande>();
            var indiv = BDDReader.Read<ClientIndividuel>();
            var bout = BDDReader.Read<ClientBoutique>();
            var assemb = BDDReader.ReadQuery($"SELECT nom, cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier  FROM velomax.assemblages");

            ///quantité
            List<SQuantite> statsquantites = new List<SQuantite>();

            foreach (Piece a in piece)
            {
                nbrpiecesvendu = 0;
                foreach (Commande c in commande)
                {

                    if (c.Piece != null)
                    {

                        if (a.ID.ToString() == c.Piece.ID.ToString())
                        {
                            nbrpiecesvendu += c.Quantité;
                        }

                    }
                    if (c.Modele != null)
                    {

                        var r = assemb.Where(x => x[0].ToString() == c.Modele.Nom.ToString()).FirstOrDefault();
                        foreach (object i in r)
                        {
                            if (a.ID.ToString() == i.ToString())
                            {
                                nbrpiecesvendu += c.Quantité;
                            }
                        }
                    }

                }
                statsquantites.Add(new SQuantite(a.ID.ToString(), a.Nom, a.Prix, a.DelaiApprovisionnementJour, nbrpiecesvendu));
            }

            statsquantite.ItemsSource = statsquantites;

            //fidelité
            var Sfidels = indiv.Where(x => x.ProgrammeFidélité != null).Select(a => new Sfidel("individuel", a.Nom, a.Telephone, a.AdresseMail, a.DateAdhésionProgramme != null ? a.DateAdhésionProgramme : null, a.ProgrammeFidélité)).ToList();

            statsfidelite.ItemsSource = Sfidels;




            ///meilleurclients
            List<Smeilleur> Smeilleurs = new List<Smeilleur>();

            foreach (Commande a in commande)
            {
                foreach (ClientIndividuel b in indiv)
                {
                    float totalclientencour = 0;

                    if (a.Client.ID.Equals(b.ID))
                    {
                        if (a.Piece != null)
                        {
                            var r = BDDReader.ReadQuery($"select sum(pieces.prix * commandes.quantité) from pieces, commandes, clients where commandes.clientid = clients.idClient and pieces.idPiece = commandes.pieceid and clients.nom='{b.Nom}';").FirstOrDefault();

                            totalclientencour += Convert.ToInt32(r[0]);
                        }
                        if (a.Modele != null)
                        {
                            var s = BDDReader.ReadQuery($"select sum(modeles.prix * commandes.quantité) from modeles, commandes,clients where commandes.clientid=clients.idClient and commandes.modeleid=modeles.idModele and clients.nom='{b.Nom}';").FirstOrDefault();
                            totalclientencour += Convert.ToInt32(s[0]);
                        }

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
                                    Smeilleurs.Add(new Smeilleur((float)totalclientencour, "individuel", b.Nom, b.Telephone, b.AdresseMail));
                                }
                            }
                        }
                    }

                }
                foreach (ClientBoutique b in bout)
                {
                    float totalclientencour = 0;
                    if (a.Client.ID.Equals(b.ID))
                    {
                        if (a.Piece != null)
                        {
                            var r = BDDReader.ReadQuery($"select sum(pieces.prix * commandes.quantité) from pieces, commandes, clients where commandes.clientid = clients.idClient and pieces.idPiece = commandes.pieceid and clients.nom='{b.NomEntreprise}';").FirstOrDefault();

                            totalclientencour += Convert.ToInt32(r[0]);
                        }
                        if (a.Modele != null)
                        {
                            var s = BDDReader.ReadQuery($"select sum(modeles.prix * commandes.quantité) from modeles, commandes,clients where commandes.clientid=clients.idClient and commandes.modeleid=modeles.idModele and clients.nom='{b.NomEntreprise}';").FirstOrDefault();
                            totalclientencour += Convert.ToInt32(s[0]);
                        }
                    }
                    if (totalclientencour != 0)
                    {
                        if (Smeilleurs.Contains(new Smeilleur((float)totalclientencour, "Boutique", b.NomEntreprise, b.Telephone, b.AdresseMail)) == false)
                        {

                            bool test = true;
                            foreach (Smeilleur J in Smeilleurs)
                            {
                                if (J.Nom == b.NomEntreprise)
                                    test = false;
                            }
                            if (test == true)
                            {
                                Smeilleurs.Add(new Smeilleur((float)totalclientencour, "Boutique", b.NomEntreprise, b.Telephone, b.AdresseMail));
                            }
                        }
                    }
                }
            }

            smeilleurclient.ItemsSource = Smeilleurs;

            float prixcommandetotal = commande.Aggregate(0f, (x, y) => x += ((y.Modele?.Prix ?? 0) + (y.Piece?.Prix ?? 0)) * y.Quantité);


            float Chiffredaffaire = (int)prixcommandetotal;   //chiffre d'affaire

            float prixmoyendescommandes = prixcommandetotal / commande.GroupBy(x => x.IDCommande).Select(x => x.First()).Count(); //prix moyen

            float nombrepiecevenduparclients = ((float)nbrpiecesvendu / (bout.Count + indiv.Count)); // nombre de piece vendu en moyenne 

            moyenne.Text = prixmoyendescommandes.ToString() + " € ";
            chiffredaffaires.Text = Chiffredaffaire.ToString() + " € ";
            moyenepieces.Text = nombrepiecevenduparclients.ToString();



        }
        
    }
    public class SQuantite
    {
        public string ID { get; set; }
        public string Nom { get; set; }

        public float Prix { get; set; }
        public int Quantité_vendue { get; set; }

        public int? DelaiApprovisionnement { get; set; }

        public string Details
        {
            get
            {
                return $"Il faudra attendre jusqu'au {DateTime.Now.AddDays((double)DelaiApprovisionnement):dd/MM/yyyy} pour recevoir la pièce";
            }
        }

        public SQuantite(string ID, string nom, float prix, int? delaiApprovisionnement, int quantite)
        {
            this.ID = ID;
            this.Nom = nom;
            this.Prix = prix;
            this.DelaiApprovisionnement = delaiApprovisionnement;
            this.Quantité_vendue = quantite;


        }
        public SQuantite() { }
    }
    public class Sfidel
    {
        public string TypeClient { get; set; }

        public DateTime? Datedebut { get; set; }
        public string Nom { get; set; }

        public string Telephone { get; set; }

        public string Courriel { get; set; }

        public int Temps { get; set; }

        public string Details
        {
            get
            {
                return Datedebut == null ? null : String.Format("{0} est la date d'adhesion au programme {1}", this.Datedebut, ((DateTime)(this.Datedebut)).AddYears(this.Temps));
            }
        }
        public Sfidel(string typeClient, string nom, string telephone, string courriel, DateTime? datadebut, Fidelio programme)
        {
            this.TypeClient = typeClient;
            this.Nom = nom;
            this.Telephone = telephone;
            this.Courriel = courriel;
            this.Datedebut = datadebut;
            this.Temps = (int)programme.Duree_annee;

        }

        public Sfidel() { }
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