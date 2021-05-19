using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP
{
    /// <summary>
    /// Classe qui permet d'exporter des données au format JSON ou XML
    /// </summary>
    public class ExportData<T>
    {
        public enum ExportType
        {
            JSON = 0,
            XML = 1
        }

        public ExportType TypeExport { get; set; }


        public List<T> ToExport { get; set; }


        public ExportData(ExportType type, List<T> toExport)
        {
            this.TypeExport = type;
            this.ToExport = toExport;
        }

        public bool Export(string path)
        {
            string filename = (Directory.Exists(path) ? path + $"\\" : "../") + $"{(TypeExport == ExportType.JSON ? "JSONExport.json" : "XMLExport.xml")}";

            return this.TypeExport == ExportType.JSON ? ExportToJSON(path) : ExportToXML(path);
        }

        private bool ExportToJSON(string filename)
        {
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ToExport);
                StreamWriter sw = new StreamWriter(filename);
                sw.Write(json);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ExportToXML(string filename)
        {
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            writer.Serialize(new StreamWriter(filename), ToExport);

            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
