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
    /// Logique d'interaction pour Pagestock.xaml
    /// </summary>
    public partial class Pagestock : UserControl
    {
        public Pagestock()
        {
            InitializeComponent();
            datagridpiece.ItemsSource = BDDReader.Read<Pieces>();
        }
    }
}