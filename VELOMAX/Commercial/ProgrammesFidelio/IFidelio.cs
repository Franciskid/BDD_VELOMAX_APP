using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    interface IFidelio
    {
        float Prix { get; }

        float Rabais { get; }

        float Duree_annee { get; }

        string Description { get; }
    }
}
