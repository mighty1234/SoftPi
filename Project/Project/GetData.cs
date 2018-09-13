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

namespace Project
{

    /// <summary>  
    ///  This class use to  optimize work GetLocation method .  
    /// </summary>  
    
    public class Pair
    {
        public string Key { get; set; }
        public string Value { get; set;}
    }

   
    /// <summary>  
    ///  This class contains methods of  transform rows from file add make a LogModel instance.  
    /// </summary>  
    static class GetData
    {
       static  Dictionary<string, string> iplocal = new Dictionary<string, string>();

        static  public Pair _lastIp = new Pair();
        /// <param name="Unparsed">Used to form and return instance of LogModel</param>
        static public LogModel ParseToModel(string unparsed)
        {
              
            LogModel log = new LogModel();

            if (!iplocal.Keys.Contains(unparsed.Substring(0, unparsed.IndexOf(' '))))
            {
                iplocal.Add(unparsed.Substring(0, unparsed.IndexOf(' ')), GetLocation(unparsed.Substring(0, unparsed.IndexOf(' '))));
            }
            log.IpOrHost = unparsed.Substring(0, unparsed.IndexOf(' '));//IpOrHost          

            log.RequestTime = (DateParse((unparsed.Substring(unparsed.IndexOf('[') + 1, unparsed.IndexOf(']') - unparsed.IndexOf('[') - 7))));// DateTime

            log.Routing = unparsed.Substring(unparsed.IndexOf('"') + 1, unparsed.LastIndexOf('"') - unparsed.IndexOf('"'));//routing 

            log.AdditionalParams = GetAddInfo(log);//Additional params


            if (log.IpOrHost.Contains(iplocal.Keys.ToString()))
            {

                log.Location = iplocal.Single(x => x.Key == log.IpOrHost).Value;
            }
            log.Location = _lastIp.Key == log.IpOrHost ? _lastIp.Value :  GetLocation(log.IpOrHost);
           _lastIp.Key =log.IpOrHost;
         
            log.Result = unparsed.Substring(unparsed.LastIndexOf('"') + 1, unparsed.LastIndexOf(' ') - unparsed.LastIndexOf('"'));//Result

            log.Size = int.TryParse(unparsed.Substring(unparsed.LastIndexOf(' ') + 1), out int Result) ? int.Parse(unparsed.Substring(unparsed.LastIndexOf(' ') + 1)) : 0;//Size

            return log;
        }

        /// <param name="IpOrHost">Used to find location</param>
             public static string GetLocation(string ipOrHost)
        {

            string locationResponse;
            string Query = @"https://www.whoisxmlapi.com/whoisserver/WhoisService?apiKey=at_faKGbxo5VlpRVZ0y8TVRcaqbfBpEP&domainName=" + ipOrHost;
            try
            {
                locationResponse = new WebClient().DownloadString(Query);
            }
            catch (WebException)
            {

                _lastIp.Value = "Invalid IP or Host";
                return _lastIp.Value;
            }

            var responseXml = XDocument.Parse(locationResponse)
                .Element("WhoisRecord").Element("registrant");

         
              _lastIp.Value = responseXml.Element("organization").Value + "," + responseXml.Element("country").Value;

            return _lastIp.Value;

        }

        /// <param name = "RequestTime" > Used to convert DateTime to right format </ param >
        public static DateTime DateParse(string requestTime)
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
            DateTime Parsed = DateTime.ParseExact(kek, "dd/MM/yyyy:HH:mm:ss", CultureInfo.CreateSpecificCulture("en-us"));
            return Parsed;
        }

        /// <param name = "log" > Used to find additional params an validate it  </ param >
       
        private static string GetAddInfo(LogModel log)
        {
            string ReturnValue = null;
            string Routing = log.Routing;
            if (Routing.Contains(".gif") || Routing.Contains(".css") || Routing.Contains(".img") || Routing.Contains(".png"))
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
                log.Routing =log.Routing+ ADdinfo[1].Substring(0, ADdinfo[0].LastIndexOf(' '));
                log.Isvalid = true;
            }
           
        
            return ReturnValue;
            }

        

        /// <param name = "logModel" > Used to save instance of model in DB  </ param >
      
    }
}









