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
    /// Logique d'interaction pour StockPage.xaml
    /// </summary>
    public partial class StockPage : UserControl
    {
        public StockPage()
        {
            InitializeComponent();
            List<Piece> pieces = BDDReader.Read<Piece>();
            List<Fournisseurs> fournisseurs = BDDReader.Read<Fournisseurs>();
            List<Modele> modeles = BDDReader.Read<Modele>();
            List<Assemblage> assemblages = BDDReader.Read<Assemblage>();
            //pieces en stock
            List<Spieces> piecesliste = new List<Spieces>();

            foreach (Piece a in pieces)
            {
                if (a.Quantité != 0)
                {
                    piecesliste.Add(new Spieces(a.ID.ToString(), a.Nom, a.Prix, a.DelaiApprovisionnement, a.Quantité));
                }
            }

            Datagridpiece.ItemsSource = piecesliste;


            // pieces par foursineur

            List<Piecesfourniseur> piecesfourniseurs = new List<Piecesfourniseur>();

            foreach (Fournisseurs a in fournisseurs)
            {
                var piecesdufourniseur = BDDReader.ReadQuery($"select pieces.idPiece, pieces.nom, pieces.delaiApprovisionnement from pieces,fournisseurs where fournisseurId=pieces.fournisseurId and fournisseurs.nom='{a.Nom}';");

                foreach (var b in piecesdufourniseur)
                {
                 
                        piecesfourniseurs.Add(new Piecesfourniseur(a.Nom, b[0].ToString(), b[1].ToString(),DateTime.Now.AddDays(a.Delai)));
                  
                    
                }
            }
            datagridfourniseur.ItemsSource = piecesfourniseurs;


            // Pieces par velo
            List<Piecesparvelo> piecesparvelos = new List<Piecesparvelo>();

            foreach (Assemblage a in assemblages)
            {
                piecesparvelos.Add(new Piecesparvelo(a.Nom.ToString(), a.Cadre.ToString(), a.Derailleur_Arriere.ToString(), a.Derailleur_Avant.ToString(), a.Selles.ToString(), a.Freins.ToString(), a.Guidon.ToString(), a.Roue_Avant.ToString(), a.Roue_Arriere.ToString(), a.Reflecteurs.ToString(), a.Pedalier.ToString(), a.Ordinateur.ToString(), a.Panier.ToString()));


            }
            datagridvelo.ItemsSource = piecesparvelos;

            //Pieces par type de velo
            List<Piecespartypedevelo> piecestpartypedervelos = new List<Piecespartypedevelo>();
         
            var f = BDDReader.ReadQuery($"SELECT modeles.ligne,guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier FROM velomax.modeles,assemblages where modeles.nom=assemblages.nom group by ligne;");

            foreach (var b in f)
             {
                    piecestpartypedervelos.Add(new Piecespartypedevelo(b[0].ToString(), b[5].ToString(), b[4].ToString(), b[3].ToString(), b[2].ToString(), b[1].ToString(), b[6].ToString(), b[7].ToString(), b[8].ToString(), b[9].ToString(), b[10].ToString(), b[11].ToString()));
             }
            datagridpiecestypevelo.ItemsSource = piecestpartypedervelos;
        }

        public class Spieces
        {
            public string ID { get; set; }
            public string Nom { get; set; }

            public float Prix { get; set; }
            public int Quantite { get; set; }

            public DateTime DelaiApprovisionnement { get; set; }
            public string Details
            {
                get
                {
                    return String.Format("{0} vaut {1} il faut attendre le {2} avant de se faire livrer.", this.ID, this.Prix, DateTime.Now.AddDays(3));
                }
            }

            public Spieces(string ID, string nom, float prix, DateTime delaiApprovisionnement, int quantite)
            {
                this.ID = ID;
                this.Nom = nom;
                this.Prix = prix;
                this.DelaiApprovisionnement = delaiApprovisionnement;
                this.Quantite = quantite;

            }
        }

        public class Piecesfourniseur
        {
            public string Nom_fournisseur { get; set; }
            public string ID { get; set; }
            public string Nom { get; set; }

            public DateTime Date { get; set; }

            public Piecesfourniseur(string Nonf, string Id, string nom, DateTime dateapprivossoment)
            {
                this.Nom_fournisseur = Nonf;
                this.ID = Id;
                this.Nom = nom;
                this.Date = dateapprivossoment;
            }
        }
        public class Piecesparvelo
        {
            public Piecesparvelo(string nommodele, string cadre, string derailleur_arriere, string derailleur_avant, string selle, string freins, string guidon, string roue_avant, string roue_arriere, string reflecteurs, string pedalier, string ordinateur, string panier)
            {
                Nommodele = nommodele;
                this.cadre = cadre;
                this.derailleur_arriere = derailleur_arriere;
                this.derailleur_avant = derailleur_avant;
                this.selle = selle;
                this.freins = freins;
                this.guidon = guidon;
                this.roue_avant = roue_avant;
                this.roue_arriere = roue_arriere;
                this.reflecteurs = reflecteurs;
                this.pedalier = pedalier;
                this.ordinateur = ordinateur;
                this.panier = panier;
            }

            public string Nommodele { get; set; }

            public string cadre { get; set; }

            public string derailleur_arriere { get; set; }

            public string derailleur_avant { get; set; }

            public string selle { get; set; }

            public string freins { get; set; }

            public string guidon { get; set; }




            public string roue_avant { get; set; }


            public string roue_arriere { get; set; }

            public string reflecteurs { get; set; }

            public string pedalier { get; set; }

            public string ordinateur { get; set; }

            public string panier { get; set; }






        }
        public class Piecespartypedevelo
        {
            public Piecespartypedevelo(string typemodele, string derailleur_arriere, string derailleur_avant, string selle, string freins, string guidon, string roue_avant, string roue_arriere, string reflecteurs, string pedalier, string ordinateur, string panier)
            {
                Typemodele = typemodele;
                this.derailleur_arriere = derailleur_arriere;
                this.derailleur_avant = derailleur_avant;
                this.selle = selle;
                this.freins = freins;
                this.guidon = guidon;
                this.roue_avant = roue_avant;
                this.roue_arriere = roue_arriere;
                this.reflecteurs = reflecteurs;
                this.pedalier = pedalier;
                this.ordinateur = ordinateur;
                this.panier = panier;
            }

            public string Typemodele { get; set; }

           

            public string derailleur_arriere { get; set; }

            public string derailleur_avant { get; set; }

            public string selle { get; set; }

            public string freins { get; set; }

            public string guidon { get; set; }




            public string roue_avant { get; set; }


            public string roue_arriere { get; set; }

            public string reflecteurs { get; set; }

            public string pedalier { get; set; }

            public string ordinateur { get; set; }

            public string panier { get; set; }
        }
    }
}
