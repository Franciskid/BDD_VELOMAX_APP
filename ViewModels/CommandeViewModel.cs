using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    public class CommandeViewModel
    {
        public int IDCommande { get; set; }

        public string Client { get; set; }

        public string PieceCommandée { get; set; }

        public int Quantité { get; set; }

        public float Prix { get; set; }

        public DateTime DateCommande { get; set; }

        public DateTime DateLivraison { get; set; }


        public CommandeViewModel() { }

        public CommandeViewModel(Commande com, Assemblage ass) 
        {
            this.IDCommande = com.IDCommande;
            this.Client = com.Client.AdresseMail;
            this.PieceCommandée = com.Piece != null ? $"{com.Piece.Nom} : {com.Piece.ID}" : $"{com.Modele.Ligne} : {com.Modele.Nom} ({ass.Grandeurs.ToString().Substring(0, 1)})";
            this.Quantité = com.Quantité;
            this.Prix = com.Prix;
            this.DateCommande = com.DateCommande;
            this.DateLivraison = com.DateLivraison;
        }

    }

    public static partial class MyHelper
    {
        /// <summary>
        /// Envoie un mail
        /// </summary>
        /// <param name="message"></param>
        public static void SendMail(MailMessage message)
        {
            using (SmtpClient cli = new SmtpClient())
            {
                cli.DeliveryMethod = SmtpDeliveryMethod.Network;
                cli.UseDefaultCredentials = false;
                cli.EnableSsl = true;
                cli.Host = "smtp.gmail.com";
                cli.Port = 587;
                cli.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailVelomax"], ConfigurationManager.AppSettings["mailVelomaxMDP"]);

                cli.Send(message);
            }
        }

        /// <summary>
        /// Envoie un mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dest"></param>
        /// <param name="view"></param>
        public static void SendMail(string subject, string dest, AlternateView view, List<Attachment> attach)
        {
            MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["mailVelomax"], dest)
            {
                Subject = subject,
                Priority = MailPriority.Normal,
                IsBodyHtml = true,
            };

            mail.AlternateViews.Add(view);

            attach.ForEach(x => mail.Attachments.Add(x));

            SendMail(mail);
        }

        public static string StringLen(string str, int len)
        {
            return str.Substring(Math.Min(len, str.Length), Math.Max(str.Length - len, 0)).Length <= len ? str + new string(' ', len - str.Length) : str.Substring(Math.Min(len, str.Length), Math.Max(str.Length - len, 0));
        }
    }
}
