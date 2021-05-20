using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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


        public string FileName { get; set; }

        public ExportData(ExportType type, List<T> toExport)
        {
            this.TypeExport = type;
            this.ToExport = toExport;
        }

        public bool Export(string path)
        {
            this.FileName = path;

            return this.TypeExport == ExportType.JSON ? ExportToJSON(path) : ExportToXML(path);
        }

        private bool ExportToJSON(string filename)
        {
            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Create, FileAccess.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializer.Serialize(jw, ToExport);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ExportToXML(string filename)
        {
            try
            {
                using (var sw = new StreamWriter(filename))
                {
                    var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
                    writer.Serialize(sw, ToExport);
                }
                 
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
