using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Exersice3.Models
{
    public class FlightValue
    {
        private string[] fileValue;
        private double rudder;
        private double throttle;
        private double longitude;
        private double latitude;
        private string fileName;
        public double Lon
        {
            get {
                return longitude; }
            set
            {
                if (this.longitude != value && this.longitude != 0)
                {

                    Console.WriteLine(" ");
                }
                this.longitude = value;

            }
        }
        public double Lat
        {
            get {
                return latitude; }
            set {
                if (this.latitude != value && this.latitude != 0)
                {

                   Console.WriteLine(" ");
                }
                this.latitude = value; }
        }

        public double Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                this.rudder = value;

            }
        }

        public double Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                this.throttle = value;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                this.fileName = value;

            }
        }
        public string[] StringValue
        {
            get
            {
                return this.fileValue;
            }
            set
            {
                this.fileValue = value;

            }
        }

        //The function write the lan and lat to xml
        public void ToXml(XmlWriter writer)
        {
            Console.WriteLine(longitude);
            writer.WriteStartElement("Values");
            writer.WriteElementString("Lon", this.longitude.ToString());
            writer.WriteElementString("Lat", this.latitude.ToString());
            writer.WriteEndElement();

        }
    }
}