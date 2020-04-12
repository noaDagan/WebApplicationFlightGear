using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.IO;


namespace Exersice3.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
 
        //The function set the display view according to the parameter
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            //Split the string and check if it is a valid ip
            string[] isIp = ip.Split('.');
            Info c = Info.Instance;
            if (isIp.Length != 4)
            {
                //Set To read data from file 
                Session["read"] = true;
                Session["time"] = port;
                readFile(ip);
            }
            else
            {
                //Connrct to client ro read the data.
                c.connect(ip, port);
                Session["time"] = time;
                Session["read"] = false;
            }
            return View();
        }

        //The function read the data and turn it to xml
        [HttpPost]
        public string GetValue()
        {
            Info info = Info.Instance;
            var readFromFile = @Session["read"];
            bool read = Convert.ToBoolean(readFromFile);
            //If dont neet to read from file read the data with the client
            if (!read)
            {
                info.readData();
            }
            else
            {
                //Get the data from the value model
                string[] linesValue = info.flightValueP.StringValue;
                //If there is no more data return null
                if (linesValue.Length == 0|| linesValue[0] == "")
                {
                    return null;
                }
                //Split the string and set Lon and Lat
                string[] tempLine = linesValue[0].Split(',');
                info.flightValueP.Lat = Convert.ToDouble(tempLine[0]);
                info.flightValueP.Lon = Convert.ToDouble(tempLine[1]);
                //Skip the row and contunu to the next values
                info.flightValueP.StringValue = info.flightValueP.StringValue.Skip(1).ToArray();

            }
            var emp = info.flightValueP;
            return ToXml(emp);
        }

        //The function convert the data to xml
        private string ToXml(Models.FlightValue flightValue)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Flight");
            flightValue.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        //The function set the save view to save the data to file
        [HttpGet]
        public ActionResult save(string ip, int port, int time, int timeOut, string name)
        {
            //Connect to server
            Info c = Info.Instance;
            c.connect(ip, port);
            Session["time"] = time;
            Session["timeOut"] = timeOut;
            Session["read"] = false;
            //Set the file name
            c.flightValueP.FileName = name;
            return View();
        }

        //Thr function save the values to a file and return xml
        [HttpPost]
        public string saveValue()
        {
            Info.Instance.readData();
            Info.Instance.readForSave();
            var emp = Info.Instance.flightValueP;
            toFile(emp.FileName);
            return ToXml(emp);
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        //Thr function save the values to a file

        public void toFile(string fileName)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            //If the file dose not exist create the file
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Dispose();
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path,true))
            {
                //Write the data to the file
                Models.FlightValue flight = Info.Instance.flightValueP;
                file.Write(flight.Lat + ",");
                file.Write(flight.Lon + ",");
                file.Write(flight.Rudder + ",");
                file.WriteLine(flight.Throttle + ",");

            }
        }

        //The function read the data from the file
        public void readFile(string fileName)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            string[] lines = System.IO.File.ReadAllLines(path);
            //Set the data from the file to the model
            Info.Instance.flightValueP.StringValue = lines;
        }

    }

}