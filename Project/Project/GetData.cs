using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using System.Text;
using System.Data.Entity.Validation;

namespace Project
{

    /// <summary>  
    ///  This class use to  optimize work GetLocation method .  
    /// </summary>  

    public class Pair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


    /// <summary>  
    ///  This class contains methods of  transform rows from file add make a LogModel instance.  
    /// </summary>  
    static class GetData
    {

        static Dictionary<string, string> iplocal = new Dictionary<string, string>();



        /// <param name="Unparsed">Used to form and return instance of LogModel</param>
        static public LogModel ParseToModel(string unparsed)
        {


            LogModel log = new LogModel();
            log.IpOrHost = unparsed.Substring(0, unparsed.IndexOf(' '));//IpOrHost   
            unparsed = unparsed.Remove(0, log.IpOrHost.Length + 6);
            if (!iplocal.Keys.Contains(log.IpOrHost))
                iplocal.Add(log.IpOrHost, GetLocation(log.IpOrHost));

            log.RequestTime = (unparsed.Substring(0, unparsed.IndexOf(']')));// DateTime
            unparsed = unparsed.Remove(0, log.RequestTime.Length + 3);

            log.RequestType = unparsed.Substring(0, unparsed.IndexOf(' '));
            unparsed = unparsed.Remove(0, log.RequestType.Length + 1);

            log.Routing = unparsed.Substring(0, unparsed.IndexOf('"') - 9);//routing 
            unparsed = unparsed.Remove(0, log.Routing.Length + 11);

            log.AdditionalParams = GetAddInfo(log);//Additional params

            log.FileName = GetFileName(log.AdditionalParams);


            if (iplocal.Keys.Contains(log.IpOrHost))
            {

                log.Location = iplocal.First(x => x.Key == log.IpOrHost).Value;
            }

            // iplocal.Keys.where =log.IpOrHost;

            log.Result = unparsed.Substring(0, unparsed.IndexOf(' '));
            unparsed = unparsed.Remove(0, log.Result.Length + 1);


            log.Size = int.Parse((unparsed.Substring(0, unparsed.IndexOf(' '))));//Size
            unparsed = unparsed.Remove(0, log.Size.ToString().Length);


            if (log.Location.Split(',') == null)
            {
                log.Isvalid = false;
            }
            return log;
        }
    

        /// <param name="IpOrHost">Used to find location</param>
        public static string GetLocation(string ipOrHost)
        {

            string value;
            string locationResponse;
            string Query = @"https://www.whoisxmlapi.com/whoisserver/WhoisService?apiKey=at_CvO8ofyO9wCpB3k9sdcbNSwO3fyxH&domainName=" + ipOrHost;

            
                locationResponse = new WebClient().DownloadString(Query);       



         


            var responseXml = XDocument.Parse(locationResponse)
                .Element("WhoisRecord").Element("registrant");
            if (responseXml == null)
            {
                responseXml = XDocument.Parse(locationResponse)
               .Element("WhoisRecord").Element("registryData").Element("registrant");
                
            }
            try
            {
                value = ((responseXml.Element("organization").Value != null) ? responseXml.Element("organization").Value : "No organization") + "," + ((responseXml.Element("country").Value != null) ? responseXml.Element("country").Value : "no country");
            }
            catch (Exception ex)
            {
                value = "invalid format";
            }


                return value;//_lastIp.Value;

        }

        /// <param name = "RequestTime" > Used to convert DateTime to right format </ param >
        public static string DateParse(string requestTime)
        {
            #region Dictionary With Month
            Dictionary<string, string> Month = new Dictionary<string, string>();
            Month.Add("Jan", "01");
            Month.Add("Feb", "02");
            Month.Add("Mar", "03");
            Month.Add("Apr", "04");
            Month.Add("May", "05");
            Month.Add("Jun", " 06");
            Month.Add("Jul", "07");
            Month.Add("Aug", "08");
            Month.Add("Sept", "09");
            Month.Add("Oct", "10");
            Month.Add("Nov", "11");
            Month.Add("Dec", "12");


            #endregion

            char[] Date = requestTime.ToCharArray();
            string Tempcontainer = String.Empty;
            int Maxindex, Minindex = 0;
            for (int i = 0; i < Date.Length; i++)
            {
                if (Date[i] == '/')
                {
                    i++;
                    Minindex = i;
                    do
                    {
                        Tempcontainer += Date[i];
                        i++;
                    }
                    while (Date[i] != '/');
                    Maxindex = --i;
                    break;
                }

            }
            var month = Month.FirstOrDefault(x => x.Key == Tempcontainer);
            var kek = requestTime.Replace("/" + Tempcontainer + "/", "/" + month.Value.ToString() + "/");
            // DateTime Parsed = DateTime.ParseExact(kek, "dd/MM/yyyy:HH:mm:ss", CultureInfo.CreateSpecificCulture("en-us"));
            return kek;
        }

        /// <param name = "log" > Used to find additional params an validate it  </ param >

        private static string GetAddInfo(LogModel log)
        {
            string ReturnValue = null;
            string Routing = log.Routing;
            if (Routing.Contains(".gif") || Routing.Contains(".css") || Routing.Contains(".img") || Routing.Contains(".png")
                || log.RequestType == "PUT" || log.RequestType == "UPDATE")
            {
                log.AdditionalParams = " ";
                log.Isvalid = false;
                return log.AdditionalParams;
            }
            else
            {
                string[] ADdinfo = Routing.Split('?');
                if (ADdinfo.Length == 1)
                {
                    log.Routing = ADdinfo[0];
                    log.Isvalid = true;
                    return string.Empty;

                }
                else
                {
                    if (ADdinfo[1] == " ")
                    {
                        log.Isvalid = true;
                        return " ";
                    }
                    else
                    {
                        for (int i = 1; i < ADdinfo.Length; i++)
                        {
                            ReturnValue += ADdinfo[i] + " ";
                        }
                        log.Isvalid = true;
                    }

                }
                log.Routing = log.Routing.Replace(ADdinfo[1], "");
                log.Isvalid = true;
            }


            return ReturnValue;
        }


        public static void Save(List<string> krk)
        {

            List<LogModel> log = new List<LogModel>();

            foreach (var item in krk)
            {
                log.Add(ParseToModel(item));
            }
            log.Clear();



            // MessageBox.Show("Stop");
            // Debugger.Break();
        }


        /// <param name = "logModel" > Used to save instance of model in DB  </ param >
        public static void LoadToDB(LogModel n)
        {

            SoftPiEntities1 piEntities2 = new SoftPiEntities1();
            IP ip = new IP()
            {
                Ip1 = Encoding.ASCII.GetBytes(n.IpOrHost),
                Company = n.Location.Split(',')[0],
                Country = n.Location.Split(',')[1]

            };
            File file = new File()
            {
                Size = n.Size,
                Name = n.FileName,
                Path = n.Routing
            };

            if (!piEntities2.IP.Any(x => x.Ip1 == ip.Ip1))
            {
                piEntities2.IP.Add(ip);
                piEntities2.SaveChanges();
            }
            /////
            piEntities2.IP.Add(ip);
            piEntities2.SaveChanges();
            ////
            piEntities2.File.Add(file);
            piEntities2.SaveChanges();
            var y = Encoding.ASCII.GetBytes(n.IpOrHost);
            //  var u = piEntities2.IP.First(x => x.Ip1.SequenceEqual(y));
            var log = new Log();

            log.File = piEntities2.File.FirstOrDefault(x => x.Id == file.Id);
            log.IP = piEntities2.IP.FirstOrDefault(x => x.Id == ip.Id);

            log.File_path_id = log.File.Id;
            log.Ip_id = piEntities2.IP.First(x => x.Ip1 == ip.Ip1).Id;
            log.requestTime = n.RequestTime.ToString();
            log.requestType = n.RequestType.ToString();
            log.result = int.Parse(n.Result);

            piEntities2.Log.Add(log);
           
           
                piEntities2.SaveChanges();      






        }
        public static string GetFileName(string additionalInfo)
        {
            if (additionalInfo == string.Empty)
            {
                return "No File";
            }
            else
            {
                if (additionalInfo.LastIndexOf('/') < 0)
                {
                    return "No FIle";
                }
                else
                {
                    return additionalInfo.Substring(additionalInfo.LastIndexOf('/'), additionalInfo.Length);
                }
            }

        }


    }
}










