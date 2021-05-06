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
    interface ISQL
    {
        /// <summary>
        /// Id
        /// </summary>
        object ID { get; }

        /// <summary>
        /// Nom de l'id
        /// </summary>
        /// <returns></returns>
        string IdToString();
    }
}
