using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    static class MyConstants
    {
        public const string TABLE_ADRESSE = "adresse";
        public const string TABLE_ASSEMBLAGES = "assemblages";
        public const string TABLE_CLIENTS = "clients";
        public const string TABLE_COMMANDES = "commandes";
        public const string TABLE_COMPTES = "comptes";
        public const string TABLE_FIDELIO = "fidelio";
        public const string TABLE_FOURNISSEURS = "fournisseurs";
        public const string TABLE_MODELES = "modeles";
        public const string TABLE_PIECES = "pieces";

        public const string TABLE_idADRESSE = "idAdresse";
        public const string TABLE_idASSEMBLAGES = "idAssemblage";
        public const string TABLE_idCLIENTS = "idClient";
        public const string TABLE_idCOMMANDES = "idCommande";
        public const string TABLE_idCOMPTES = "idCompte";
        public const string TABLE_idFIDELIO = "idFidelio";
        public const string TABLE_idFOURNISSEURS = "siret";
        public const string TABLE_idMODELES = "idModele";
        public const string TABLE_idPIECES = "idPiece";

        public static Dictionary<string, List<string>> DICOVALUES { get; } = InitializeDic();

        private static Dictionary<string, List<string>> InitializeDic()
        {
            Dictionary<string, List<string>> dico = new Dictionary<string, List<string>>()
            {
                { TABLE_ADRESSE, new List<string>() { TABLE_idADRESSE, "rue", "ville", "codePostal", "pays" } },
                { TABLE_ASSEMBLAGES, new List<string>() { TABLE_idASSEMBLAGES, "nom", "grandeur", "cadre", "guidon", "freins", "selle", "derailleur_avant", "derailleur_arriere", "roue_avant", "roue_arriere", "reflecteurs", "pedalier", "ordinateur", "panier", } },
                { TABLE_CLIENTS, new List<string>() { TABLE_idCLIENTS, "typeClient", "nom", "prenom", "idAdresse", "telephone", "courriel", "nomContact", "remise", "fidelio", "idFidelio", "dateAdhesionFidelio" } },
                { TABLE_COMMANDES, new List<string>() { TABLE_idCOMMANDES, "numCommande", "clientid", "pieceid", "assemblageid", "dateCommande", "dateLivraison"} },
                { TABLE_COMPTES, new List<string>() { TABLE_idCOMPTES, "pseudo", "motdepasse"} },
                { TABLE_FIDELIO, new List<string>() { TABLE_idFIDELIO, "nom", "prix", "duree_annee", "rabais" } },
                { TABLE_FOURNISSEURS, new List<string>() { TABLE_idFOURNISSEURS, "nom", "contact", "idAdresse", "score", "delaidelivraison" } },
                { TABLE_MODELES, new List<string>() { TABLE_idMODELES, "nom", "prix", "ligne", "dateIntroduction", "dateDiscontinuation" } },
                { TABLE_PIECES, new List<string>() { TABLE_idPIECES, "nom", "fournisseurId", "numProduit", "prix", "quantité", "dateIntroduction", "dateDiscontinuation", "delaiApprovisionnement" } },
            };

            return dico;
        }

        public static string UpdateRowSet(IMySQL obj)
            => string.Join(",", DICOVALUES[TypeToTable(obj.GetType())].Select((x, y) => $"{x} = {obj.SaveStr().Split(',')[y++]}"));


        public static string TypeToID(Type type)
        {
            if (type == typeof(Adresse))
            {
                return TABLE_idADRESSE;
            }
            if (type == typeof(Fidelio))
            {
                return TABLE_idFIDELIO;
            }
            if (type == typeof(Client) || type == typeof(ClientBoutique) || type == typeof(ClientIndividuel))
            {
                return TABLE_idCLIENTS;
            }
            if (type == typeof(Modele))
            {
                return TABLE_idMODELES;
            }
            if (type == typeof(Compte))
            {
                return TABLE_idCOMPTES;
            }
            if (type == typeof(Assemblage))
            {
                return TABLE_idASSEMBLAGES;
            }

            if (type == typeof(Commande))
            {
                return TABLE_idCOMMANDES;
            }
            if (type == typeof(Fournisseurs))
            {
                return TABLE_idFOURNISSEURS;
            }
            if (type == typeof(Piece))
            {
                return TABLE_idPIECES;
            }
            if (type == typeof(Piece))
            {
                return TABLE_idPIECES;
            }

            return null;
        }

        public static string TypeToTable(Type type)
        {
            if (type == typeof(Adresse))
            {
                return TABLE_ADRESSE;
            }
            if (type == typeof(Fidelio))
            {
                return TABLE_FIDELIO;
            }
            if (type == typeof(Client) || type == typeof(ClientBoutique) || type == typeof(ClientIndividuel))
            {
                return TABLE_CLIENTS;
            }
            if (type == typeof(Compte))
            {
                return TABLE_COMPTES;
            }
            if (type == typeof(Modele))
            {
                return TABLE_MODELES;
            }
            if (type == typeof(Assemblage))
            {
                return TABLE_ASSEMBLAGES;
            }
            if (type == typeof(Fournisseurs))
            {
                return TABLE_FOURNISSEURS;
            }
            if (type == typeof(Commande))
            {
                return TABLE_COMMANDES;
            }
            if (type == typeof(Piece))
            {
                return TABLE_PIECES;
            }



            return null;
        }
    }
}
