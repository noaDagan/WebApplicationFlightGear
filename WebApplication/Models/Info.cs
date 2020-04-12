using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Exersice3
{
    public class Info
    {
    
        private string ip;
        private int port;
        private IPEndPoint ep;
        private TcpClient client;
        private bool isConnect = false;
        private static Info instance = null;
        private Models.FlightValue flightValue;

        private Info() {        
            flightValue = new Models.FlightValue();
        }

        //Singletone
        public static Info Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Info();
                }
                return instance;
            }
        }

        public Models.FlightValue flightValueP
        {
            get
            {
                return this.flightValue;
            }
            set
            {
                this.flightValue = value;
            }
        }

           //The function connect to server  
            public void connect(string ip, int port)
             {
            this.ip = ip;

            this.port = port;
            ep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
            client = new TcpClient();
            if (!isConnect)
            {
                try
                {
                    {
                        //Connect to server
                        client.Connect(ep);
                        Console.WriteLine("Command - You are connected");
                        isConnect = true;
                    }
                }
                catch (System.Exception) { }
            }
        }

        // The function return true if client connect and false otherwise
        public bool isConnection()
        {
            return isConnect;
        }
        // The function stop the connection of client 
        public void disconnect()
        {
            if (isConnect)
            {
                client.Close();
                isConnect = false;
            }
        }

        // The function sent values to flight gear
        public void readData()
        {
            string result = "";
            if (isConnection())
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);
                        //Get the lon value from the server
                        string commandWrite = "get /position/longitude-deg";
                        writer.WriteLine(commandWrite);
                        writer.Flush();
                        result = reader.ReadLine();
                        string[] splitValues = result.Split('\'');
                        //Update the model value
                        flightValue.Lon = Convert.ToDouble(splitValues[1]);
                        //Get the lan value from the server
                        commandWrite = "get /position/latitude-deg";
                        writer.WriteLine(commandWrite);
                        writer.Flush();
                        result = reader.ReadLine();
                        splitValues = result.Split('\'');
                        //Update the model value
                        flightValue.Lat = Convert.ToDouble(splitValues[1]);

                }
                catch (System.Exception)
                {
                }
            }
        }

        //The function read the throttle and rudder values
        public void readForSave()
        {
            string result = "";
            if (isConnection())
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);
                    //Get the rudder value from the server
                    string commandWrite = "get /controls/flight/rudder";
                    writer.WriteLine(commandWrite);
                    writer.Flush();
                    result = reader.ReadLine();
                    string[] splitValues = result.Split('\'');
                    //Update the model value
                    flightValue.Rudder = Convert.ToDouble(splitValues[1]);
                    //Get the throttle value from the server
                    commandWrite = "get /controls/engines/engine/throttle";
                    writer.WriteLine(commandWrite);
                    writer.Flush();
                    result = reader.ReadLine();
                    splitValues = result.Split('\'');
                    //Update the model value
                    flightValue.Throttle = Convert.ToDouble(splitValues[1]);
                }
                catch (System.Exception)
                {
                }
            }

        }


    }
}