using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    static class BDDConstants
    {
        private const string TABLE_ADRESSES = "adresse";
        private const string TABLE_ASSEMBLAGES = "assemblages";
        private const string TABLE_CLIENTS = "clients";
        private const string TABLE_COMMANDES = "commandes";
        private const string TABLE_COMPTES = "comptes";
        private const string TABLE_FIDELIO = "fidelio";
        private const string TABLE_FOURNISSEURS = "fournisseurs";
        private const string TABLE_MODELES = "modeles";
        private const string TABLE_PIECES = "pieces";


        /// <summary>
        /// Chaque type est associé à une table dans la BDD.
        /// </summary>
        public static Dictionary<Type, string> DICOTYPEVALUES { get; } = InitializeTypeDic();

        /// <summary>
        /// Chaque table dans la BDD est associée à tous les champs qui en composent les colonnes.
        /// </summary>
        public static Dictionary<string, string[]> DICOTABLEVALUES { get; } = InitializeTableDic();


        private static Dictionary<Type, string> InitializeTypeDic()
        {
            Dictionary<Type, string> dico = new Dictionary<Type, string>
            {
                { typeof(Adresse), TABLE_ADRESSES},
                { typeof(Assemblage), TABLE_ASSEMBLAGES},
                { typeof(Client), TABLE_CLIENTS},
                { typeof(ClientBoutique), TABLE_CLIENTS},
                { typeof(ClientIndividuel), TABLE_CLIENTS},
                { typeof(Commande), TABLE_COMMANDES},
                { typeof(Compte), TABLE_COMPTES},
                { typeof(Fidelio), TABLE_FIDELIO},
                { typeof(Fournisseurs), TABLE_FOURNISSEURS},
                { typeof(Modele), TABLE_MODELES},
                { typeof(Piece), TABLE_PIECES},
            };

            return dico;
        }

        private static Dictionary<string, string[]> InitializeTableDic()
        {
            Dictionary<string, string[]> dico = new Dictionary<string, string[]>();

            foreach (var e in DICOTYPEVALUES)
            {
                if (e.Key != typeof(ClientBoutique) && e.Key != typeof(ClientIndividuel))
                {
                    dico.Add(DICOTYPEVALUES[e.Key], BDDColumnNames(e.Key));
                }
            }

            return dico;
        }


        public static string TypeToTable(Type type) => DICOTYPEVALUES[type];

        public static string TypeToID(Type type) => DICOTABLEVALUES[TypeToTable(type)].FirstOrDefault();


        private static string[] BDDColumnNames(Type table)
        {
            try
            {
                using (var c = BDDReader.OpenConnexion())
                using (var command = new MySqlCommand($"SELECT * FROM {TypeToTable(table)}", c))
                using (var reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    return (from DataRow r in reader.GetSchemaTable().Rows select (string)r.ItemArray.FirstOrDefault()).ToArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
