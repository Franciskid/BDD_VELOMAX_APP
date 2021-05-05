using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    class MyConstants
    {
        public const string CONNEXION_STRING = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=velomax;" +
                                         "UID=root;PASSWORD='root'";

        public const string ADRESSE = "adresse";
        public const string ASSEMBLAGES = "assemblages";
        public const string CLIENTS = "clients";
        public const string COMMANDES = "commandes";
        public const string COMPTES = "comptes";
        public const string FIDELIO = "fidelio";
        public const string FOURNISSEURS = "fournisseurs";
        public const string MODELES = "modeles";
        public const string PIECES = "pieces";

        public static readonly Dictionary<string, List<string>> DICOVALUES = InitializeDic();

        private static Dictionary<string, List<string>> InitializeDic()
        {
            Dictionary<string, List<string>> dico = new Dictionary<string, List<string>>()
            {
                { ADRESSE, new List<string>() { "idAdresse", "rue", "ville", "codePostal", "pays" } },
                { ASSEMBLAGES, new List<string>() { "idAssemblage", "nom", "grandeur", "cadre", "guidon", "freins", "selle", "derailleur_avant", "derailleur_arriere", "roue_avant", "roue_arriere", "reflecteurs", "pedalier", "ordinateur", "panier", } },
                { CLIENTS, new List<string>() { "idClient", "typeClient", "nom", "prenom", "idAdresse", "telephone", "courriel", "nomContact", "remise", "fidelio", "idFidelio", "dateAdhesionFidelio" } },
                { COMMANDES, new List<string>() { "idCommande", "dateCommande", "dateLivraison"} },
                { COMPTES, new List<string>() { "idCompte", "pseudo", "motdepasse"} },
                { FIDELIO, new List<string>() { "idFidelio", "nom", "prix", "duree_annee", "rabais" } },
                { FOURNISSEURS, new List<string>() { "siret", "nom", "contact", "idAdresse", "score" } },
                { MODELES, new List<string>() { "idModele", "nom", "prix", "ligne", "dateIntroduction", "dateDiscontinuation" } },
                { PIECES, new List<string>() { "idPiece", "nom", "nomFournisseur", "numProduit", "prix", "dateIntroduction", "dateDiscontinuation", "delaiApprovisionnement" } },
            };

            return dico;
        }


        public static string TypeToTable(Type type)
        {
            if (type == typeof(Adresse))
            {
                return CLIENTS;
            }
            if (type == typeof(Fidelio))
            {
                return FIDELIO;
            }
            if (type == typeof(Client))
            {
                return CLIENTS;
            }
            if (type == typeof(Client))
            {
                return CLIENTS;
            }
            if (type == typeof(Client))
            {
                return CLIENTS;
            }

            return null;
        }
    }
}
