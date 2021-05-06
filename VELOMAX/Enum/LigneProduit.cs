﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public enum LigneProduit
    {
        VTT,
        Vélo_Course,
        Classique,
        BMX
    }

    public static partial class MyHelper
    {
        public static string Description(this LigneProduit ligne)
        {
            switch (ligne)
            {
                case LigneProduit.VTT:
                    return "VTT";
                case LigneProduit.Vélo_Course:
                    return "Vélo de course";
                case LigneProduit.Classique:
                    return "Classique";
                case LigneProduit.BMX:
                    return "BMX";

                default:
                    return "Erreur ligne produit";
            }
        }

        public static LigneProduit DescriptionToLigneProduit(string str)
        {
            switch (str)
            {
                case "VTT":
                    return LigneProduit.VTT;
                case "Vélo de course":
                    return LigneProduit.Vélo_Course;
                case "Classique":
                    return LigneProduit.Classique;
                case "BMX":
                    return LigneProduit.BMX;

                default:
                    return (LigneProduit)(-1);
            }
        }
    }
}
