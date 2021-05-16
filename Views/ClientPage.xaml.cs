﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace BDD_VELOMAX_APP.Views
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : UserControl
    {
        public float Remise { get; set; } = 0;

        public ObservableCollection<Fidelio> ListeFidelio { get; set; } = new ObservableCollection<Fidelio>() { new Fidelio(-1, "Pas de fidélio", 0, 0, 0) };

        public Fidelio SelectedFidelio { get; set; }

        public IEnumerable<ClientViewModel> ListeClients = DataReader.Read<Client>().Select(x => new ClientViewModel(x));


        public ClientPage()
        {
            InitializeComponent();

            var fidel = DataReader.Read<Fidelio>();

            fidel.ForEach(x => ListeFidelio.Add(x));

            SelectedFidelio = ListeFidelio[0];

            DatagridClients.ItemsSource = ListeClients;

            this.cb_fidelio.ItemsSource = ListeFidelio;
        }
       



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Butt_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Butt_Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Butt_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var o = (ClientViewModel)this.DataContext;

            var val = this.DatagridClients.SelectedItem as ClientViewModel;

            o.Nom = val.Nom;
            o.Prénom = val.Prénom;
            o.Adresse = val.Adresse;
            o.Ville = val.Ville;
            o.CodePostal = val.CodePostal;
            o.Mail = val.Mail;
            o.Téléphone = val.Téléphone;
            o.Province = val.Province;
            o.DateAdhésion = val.DateAdhésion;
            o.DateFin = val.DateFin;
            o.NomContact = val.NomContact;
            o.Remise = val.Remise;
            o.ProgrammeFidélité = val.ProgrammeFidélité;

            var a = TB_CliIndividuel_Telephone.Text;
        }

       
    }
}
