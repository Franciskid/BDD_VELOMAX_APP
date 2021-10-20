using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Spécifie les classes qui implémentent des tables dans la base de donnée velomax
    /// </summary>
    public interface IMySQL
    {
        /// <summary>
        /// Id
        /// </summary>
        object ID { get; }

        /// <summary>
        /// String à sauvegarder dans la base de donnée : les propriétés séparées par une ','
        /// </summary>
        /// <returns></returns>
        string SaveStr();
    }
}
