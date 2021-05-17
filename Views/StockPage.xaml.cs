﻿using System;
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
            List<Pieces> pieces = BDDReader.Read<Pieces>();
            List<Fournisseurs> fournisseurs = BDDReader.Read<Fournisseurs>();
            List<Modele> modeles = BDDReader.Read<Modele>();

            //pieces en stock
            List<Spieces> piecesliste = new List<Spieces>();

            foreach (Pieces a in pieces)
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
                    ///piecesfourniseurs.Add(new Piecesfourniseur(a.Nom, b[0].ToString(), b[1].ToString(), Convert.ToDateTime(b[2].ToString()))) ;
                }
            }
            datagridfourniseur.ItemsSource = piecesfourniseurs;

            
            // Pieces par velo
            List<Piecesparvelo> piecesparvelos = new List<Piecesparvelo>();

            foreach (Modele a in modeles)
            {
                var piecesdufourniseur = BDDReader.ReadQuery($"SELECT cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier  FROM velomax.assemblages, modeles where modeles.nom='{a.Nom}' and modeles.nom=assemblages.nom ;").FirstOrDefault();
                foreach(object b in piecesdufourniseur)
                {
                    piecesparvelos.Add(new Piecesparvelo(a.Nom.ToString(), b.ToString()));
                }

            }
            datagridvelo.ItemsSource = piecesparvelos;


        }

        public class Spieces
        {
            public string ID { get; set; }
            public string Nom { get; set; }

            public float Prix { get; set; }
            public int Quantite{ get; set; }

            public DateTime DelaiApprovisionnement { get; set; }
            public string Details
            {
                get
                {
                    return String.Format("{0} vaut {1} il faut attendre le {2} avant de se faire livrer.", this.ID, this.Prix, DateTime.Now.AddMonths(this.DelaiApprovisionnement.Month));
                }
            }

            public Spieces(string ID, string nom, float prix, DateTime delaiApprovisionnement, int quantite)
            {
                this.ID = ID;
                this.Nom = nom;
                this.Prix = prix;
                this.DelaiApprovisionnement = delaiApprovisionnement;
                this.Quantite= quantite;

            }
        }

        public class Piecesfourniseur
        {
            public string Nom_fournisseur { get; set; }
            public string ID { get; set; }
            public string Nom { get; set; }

            public DateTime Date { get; set; }

            public Piecesfourniseur(string Nonf, string Id,string nom,DateTime dateapprivossoment)
            {
                this.Nom_fournisseur = Nonf;
                this.ID = Id;
                this.Nom = nom;
                this.Date = dateapprivossoment;
            }
        }
        public class Piecesparvelo
        {
            public string Nommodele { get; set; }

            public string ID { get; set; }
           

            public Piecesparvelo(string nommodele,string Id)
            {
                this.Nommodele = nommodele;
                this.ID = Id;
            }


        }
    }
}
