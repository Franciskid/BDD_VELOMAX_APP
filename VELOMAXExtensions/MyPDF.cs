using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BDD_VELOMAX_APP
{
    class MyPDF
    {
        public static PdfDocument Create(Client cli, List<Commande> commande, List<Assemblage> assemblage)
        {
            Document doc = new Document();
            doc.Info.Title = "Facture VéloMax";
            doc.Info.Author = "VéloMax";
            doc.Info.Subject = $"Facture Commande {commande.FirstOrDefault().IDCommande}";


            DefineStyles(doc);

            DefineContent(doc, cli, BDDReader.Get<ClientBoutique>("VELOMAX", "nom"), commande);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = doc
            };
            renderer.RenderDocument();

            renderer.Save("pdfTestSave.pdf");
            Process.Start("pdfTestSave.pdf");

            return renderer.PdfDocument;
        }



        private static void DefineContent(Document doc, Client cli, ClientBoutique velomax, List<Commande> commande)
        {
            //Image
            Section section = doc.AddSection(); 
            section.PageSetup.TopMargin = Unit.FromCentimeter(5);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(3);

            Image image = section.Headers.Primary.AddImage(@"..\..\Images\velomaxSpeedLogoBlack.png");// ".. /../Images/velomaxSpeedLogoBlack.png");
            image.Height = "3cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;

            //Header titre
            Paragraph header = section.Headers.Primary.AddParagraph();
            header.AddText("FACTURE VELOMAX® OFFICIELLE™");
            header.Format.Font.Size = 32;
            header.Format.RightIndent = "2cm";
            header.Format.Alignment = ParagraphAlignment.Left;
            header.Format.Font.Color = Colors.Black;


            // Create footer
            Paragraph footer = section.Footers.Primary.AddParagraph();
            footer.AddText($"{velomax.NomEntreprise} · {velomax.Adresse.Rue} · {velomax.Adresse.Ville}   {velomax.Adresse.CodePostal} · {velomax.Adresse.Province}");
            footer.AddLineBreak();
            footer.AddText($"{velomax.AdresseMail}  -  {velomax.Telephone}");
            footer.Format.Font.Size = 9;
            footer.Format.Alignment = ParagraphAlignment.Center;

            Paragraph footerPage = section.Footers.Primary.AddParagraph();
            footerPage.AddText($"Page ");
            footerPage.AddPageField();
            footerPage.AddText($"/");
            footerPage.AddNumPagesField();
            footerPage.Format.Font.Size = 7;
            footerPage.Format.Alignment = ParagraphAlignment.Right;



            #region Adresse de livraison

            // Adresse de livraison
            var livraisonAdresseTxt = section.AddTextFrame();
            livraisonAdresseTxt.Height = "4.0cm";
            livraisonAdresseTxt.Width = "6cm";
            livraisonAdresseTxt.Left = ShapePosition.Left;
            livraisonAdresseTxt.RelativeHorizontal = RelativeHorizontal.Margin;
            livraisonAdresseTxt.RelativeVertical = RelativeVertical.Margin;
            livraisonAdresseTxt.Top = "0cm";

            var livraisonTexte = livraisonAdresseTxt.AddParagraph();
            livraisonTexte.AddText("Adresse de livraison :");
            livraisonTexte.Format.Alignment = ParagraphAlignment.Left;
            livraisonTexte.Format.Font.Bold = true;
            livraisonTexte.Format.Font.Size = 12;
            livraisonTexte.Format.SpaceAfter = "0.2cm";

            ClientIndividuel ind = cli is ClientIndividuel ? (ClientIndividuel)cli : null;
            ClientBoutique bout = cli is ClientBoutique ? (ClientBoutique)cli : null;

            string name = ind == null ? bout.NomEntreprise : $"{ind.Prénom} {ind.Nom}";

            Paragraph livraisonPar = livraisonAdresseTxt.AddParagraph();
            livraisonPar.Format.Alignment = ParagraphAlignment.Left;
            livraisonPar.Format.LeftIndent = "0.5cm";
            livraisonPar.Format.Font.Name = "Times New Roman";
            livraisonPar.Format.Font.Size = 10;
            livraisonPar.AddText($"{name}");
            livraisonPar.AddLineBreak();
            livraisonPar.AddText($"{cli.Adresse.Rue}");
            livraisonPar.AddLineBreak();
            livraisonPar.AddText($"{cli.Adresse.CodePostal}  ·  {cli.Adresse.Ville}");
            livraisonPar.AddLineBreak();
            livraisonPar.AddText($"{cli.Adresse.Province}");
            livraisonPar.AddLineBreak();

            #endregion

            #region Adresse de facturation

            // Adresse de facturation
            var facturationAdresseTxt = section.AddTextFrame();
            facturationAdresseTxt.Height = "4.0cm";
            facturationAdresseTxt.Width = "6cm";
            facturationAdresseTxt.Left = ShapePosition.Right;
            facturationAdresseTxt.RelativeHorizontal = RelativeHorizontal.Margin;
            facturationAdresseTxt.Top = "5cm";
            facturationAdresseTxt.RelativeVertical = RelativeVertical.Page;


            var facturationTexte = facturationAdresseTxt.AddParagraph();
            facturationTexte.AddText("Adresse de facturation :");
            facturationTexte.Format.Alignment = ParagraphAlignment.Right;
            facturationTexte.Format.Font.Bold = true;
            facturationTexte.Format.Font.Size = 12;
            facturationTexte.Format.SpaceAfter = "0.2cm";

            var facturationpar = facturationAdresseTxt.AddParagraph();
            facturationpar.Format.Alignment = ParagraphAlignment.Left;
            facturationpar.Format.LeftIndent = "0.5cm";
            facturationpar.Format.Font.Size = 10;
            facturationpar.AddText($"À PAYER !");

            #endregion

            #region Infos commande

            // Adresse de facturation
            var infos = section.AddTextFrame();
            infos.Height = "4.0cm";
            infos.Width = "7cm";
            infos.Left = ShapePosition.Left;
            infos.RelativeHorizontal = RelativeHorizontal.Margin;
            infos.Top = "3cm";
            infos.RelativeVertical = RelativeVertical.Paragraph;

            var infosPar = infos.AddParagraph();
            infosPar.Format.Alignment = ParagraphAlignment.Left;
            infosPar.Format.Font.Size = 9;
            infosPar.AddText($"Numéro client : {cli.ID}");
            infosPar.AddLineBreak();
            infosPar.AddText($"Numéro de commande : {commande.FirstOrDefault().IDCommande}");
            infosPar.AddLineBreak();
            infosPar.AddText($"Facture du ");
            infosPar.AddDateField("dd/MM/yyyy ");
            infosPar.AddLineBreak();
            infosPar.AddText($"Expédition le ");
            infosPar.AddDateField("dd/MM/yyyy ");

            #endregion

            //var date = section.AddTextFrame();
            //date.RelativeVertical = RelativeVertical.Paragraph;
            //date.Width = "20cm";
            var datePar = section.AddParagraph();
            datePar.Format.SpaceBefore = "0cm";
            datePar.Style = "Reference";
            datePar.AddFormattedText($"COMMANDE {commande.FirstOrDefault().IDCommande}", TextFormat.Bold);
            datePar.AddTab();
            datePar.AddText($"{velomax.Adresse.Ville}, ");
            datePar.AddDateField("dd/MM/yyyy ");

            #region Table

            var table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = Colors.Gray;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row firstRow = table.AddRow();
            firstRow.HeadingFormat = true;
            firstRow.Format.Alignment = ParagraphAlignment.Center;
            firstRow.Format.Font.Bold = true;
            firstRow.Shading.Color = Colors.AliceBlue;
            firstRow.Cells[0].AddParagraph("Quantité");
            firstRow.Cells[0].Format.Font.Bold = false;
            firstRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            firstRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            firstRow.Cells[1].AddParagraph("Article");
            firstRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            firstRow.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            firstRow.Cells[2].AddParagraph("Reférence");
            firstRow.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            firstRow.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            firstRow.Cells[3].AddParagraph("Prix € Unitaire HT");
            firstRow.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            firstRow.Cells[3].VerticalAlignment = VerticalAlignment.Center;

            firstRow.Cells[4].AddParagraph("Taux TVA");
            firstRow.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            firstRow.Cells[4].VerticalAlignment = VerticalAlignment.Center;

            firstRow.Cells[5].AddParagraph("Prix € Unitaire TTC");
            firstRow.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            firstRow.Cells[5].VerticalAlignment = VerticalAlignment.Center;

            firstRow.Cells[6].AddParagraph("Prix € Total TTC");
            firstRow.Cells[6].Format.Alignment = ParagraphAlignment.Left;
            firstRow.Cells[6].VerticalAlignment = VerticalAlignment.Center;

            table.SetEdge(0, 0, 7, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

            #endregion

            #region Populate table

            bool pair = false;
            foreach (var com in commande.OrderBy(x => x.Modele != null).ThenBy(x => x.Piece?.Nom ?? x.Modele?.Nom.ToString()))
            {
                Row row = table.AddRow();
                row.BottomPadding = "0.2cm";
                row.TopPadding = "0.2cm";
                row.VerticalAlignment = VerticalAlignment.Center;
                row.Format.Alignment = ParagraphAlignment.Left;

                bool isPiece = com.Modele == null;

                row.Shading.Color = (pair = !pair) ? (isPiece ? Color.FromRgb(188, 219, 245) : Color.FromRgb(179, 209, 155)) : //Foncé
                    (isPiece ? Color.FromRgb(201, 225, 245) : Color.FromRgb(213, 245, 188)); //Clair

                var p0 = row.Cells[0].AddParagraph(com.Quantité.ToString());
                p0.Format.Alignment = ParagraphAlignment.Center;
                p0.Format.Font.Bold = true;

                row.Cells[1].AddParagraph(isPiece ? com.Piece.Nom : $"{com.Modele.Ligne} : {com.Modele.Nom}");
                row.Cells[2].AddParagraph(isPiece ? com.Piece.ID.ToString() : com.Modele.ID.ToString());
                row.Cells[3].AddParagraph(((com.Prix / com.Quantité) / 1.2).ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }));
                row.Cells[4].AddParagraph("20%");
                row.Cells[5].AddParagraph((com.Prix / com.Quantité).ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }));

                var p6 = row.Cells[6].AddParagraph(com.Prix.ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }) + "€");
                p6.Format.Alignment = ParagraphAlignment.Right;
                p6.Format.Font.Bold = true;
                p6.Format.Font.Size = 11;
            }

            // ligne invisible
            Row rowEnd = table.AddRow();
            rowEnd.Borders.Visible = false;

            double prixTotal = commande.Aggregate(0d, (x, y) => x += y.Prix);

            // prix total
            rowEnd = table.AddRow();
            rowEnd.KeepWith = 4;
            rowEnd.Format.Font.Size = 11;
            rowEnd.BottomPadding = "0.1cm";
            rowEnd.TopPadding = "0.1cm";
            rowEnd.Cells[0].Borders.Visible = false;
            rowEnd.Cells[0].Format.Font.Bold = true;
            rowEnd.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            rowEnd.Cells[0].MergeRight = 5;
            rowEnd.Cells[0].AddParagraph("Prix total");
            rowEnd.Cells[6].AddParagraph(prixTotal.ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }) + " €");
            rowEnd.Cells[6].Shading.Color = Colors.AliceBlue;

            // tva
            rowEnd = table.AddRow();
            rowEnd.Format.Font.Size = 11;
            rowEnd.BottomPadding = "0.1cm";
            rowEnd.TopPadding = "0.1cm";
            rowEnd.Cells[0].Borders.Visible = false;
            rowEnd.Cells[0].Format.Font.Bold = true;
            rowEnd.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            rowEnd.Cells[0].MergeRight = 5;
            rowEnd.Cells[0].AddParagraph("TVA (20%)");
            rowEnd.Cells[6].AddParagraph((0.2 * prixTotal).ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }) + " €");
            rowEnd.Cells[6].Shading.Color = Colors.AliceBlue;

            // livraison
            rowEnd = table.AddRow();
            rowEnd.Format.Font.Size = 11;
            rowEnd.BottomPadding = "0.1cm";
            rowEnd.TopPadding = "0.1cm";
            rowEnd.Cells[0].Borders.Visible = false;
            rowEnd.Cells[0].Format.Font.Bold = true;
            rowEnd.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            rowEnd.Cells[0].MergeRight = 5;
            rowEnd.Cells[0].AddParagraph("Livraison");
            rowEnd.Cells[6].AddParagraph(0.ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }) + " €");
            rowEnd.Cells[6].Shading.Color = Colors.AliceBlue;

            // remise
            double remise = bout == null ? ind.ProgrammeFidélité?.Rabais ?? 0 : bout.Remise;
            rowEnd = table.AddRow();
            rowEnd.Format.Font.Size = 11;
            rowEnd.BottomPadding = "0.1cm";
            rowEnd.TopPadding = "0.1cm";
            rowEnd.Cells[0].Borders.Visible = false;
            rowEnd.Cells[0].Format.Font.Bold = true;
            rowEnd.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            rowEnd.Cells[0].MergeRight = 5;
            rowEnd.Cells[0].AddParagraph("Remise");
            rowEnd.Cells[6].AddParagraph(remise.ToString("0.0") + " %");
            rowEnd.Cells[6].Shading.Color = Colors.AliceBlue;

            // Aprix total avec remis
            rowEnd = table.AddRow();
            rowEnd.Format.Font.Size = 13;
            rowEnd.BottomPadding = "0.3cm";
            rowEnd.TopPadding = "0.3cm";
            rowEnd.Cells[0].Borders.Visible = false;
            rowEnd.Cells[0].Format.Font.Bold = true;
            rowEnd.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            rowEnd.Cells[0].MergeRight = 5;
            rowEnd.Cells[0].AddParagraph("Total à payer :");
            rowEnd.Cells[6].AddParagraph((prixTotal - (prixTotal * remise / 100)).ToString("n", new NumberFormatInfo { NumberGroupSeparator = " " }) + " €");
            rowEnd.Cells[6].Shading.Color = Colors.AliceBlue;

            // border
            table.SetEdge(6, table.Rows.Count - 5, 1, 4, Edge.Box, BorderStyle.Single, 0.75);

            #endregion


            // dernier paragraphe
            var lastP = doc.LastSection.AddParagraph();
            lastP.Format.SpaceBefore = "1cm";
            lastP.Format.Borders.Width = 0.75;
            lastP.Format.Borders.Distance = 3;
            lastP.Format.Borders.Color = Colors.Gray;
            lastP.Format.Shading.Color = Colors.AliceBlue;
            lastP.Format.Alignment = ParagraphAlignment.Center;
            lastP.AddText($"{velomax.NomEntreprise} et son président M. {velomax.NomContact} vous remercient pour votre achat !");
        }



        private static void DefineStyles(Document doc)
        {
            // Get the predefined style Normal.
            Style style = doc.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = doc.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = doc.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = doc.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = doc.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        public static void CreateTest()
        {
            Client velomax = BDDReader.Get<Client>("VELOMAX", "nom");

            Client cli = BDDReader.Get<Client>(20);

            string s = $"select * from {BDDConstants.TypeToTable(typeof(Commande))} where 'clientid' = {cli.ID};";
            var com = BDDReader.Read<Commande>($"select * from {BDDConstants.TypeToTable(typeof(Commande))} where clientid = {cli.ID};").GroupBy(x => x.IDCommande);

            Create(cli, com.FirstOrDefault().ToList(), null);
        }
    }
}
