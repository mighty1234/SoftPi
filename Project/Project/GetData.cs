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
using System.Diagnostics;

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

      
        /// <param name="Unparsed">Used to form and return instance of LogModel</param>
        static public LogModel ParseToModel(string unparsed)
        {
           
              
            LogModel log = new LogModel();

            //if (!iplocal.Keys.Contains(unparsed.Substring(0, unparsed.IndexOf(' '))))
            //{
            //    iplocal.Add(unparsed.Substring(0, unparsed.IndexOf(' ')), GetLocation(unparsed.Substring(0, unparsed.IndexOf(' '))));
            //}
            log.IpOrHost = unparsed.Substring(0, unparsed.IndexOf(' '));//IpOrHost    
            unparsed = unparsed.Remove(0,log.IpOrHost.Length+5);

            log.RequestTime = (unparsed.Substring(unparsed.IndexOf('[') + 1, unparsed.IndexOf(']') - unparsed.IndexOf('[')-1));// DateTime
            unparsed = unparsed.Remove(0,log.RequestTime.Length+4);

            log.RequestType = unparsed.Substring(0 , unparsed.IndexOf(' '));
            unparsed = unparsed.Remove(0, log.RequestType.Length);

            log.Routing = unparsed.Substring(0,unparsed.IndexOf('"')); //routing 
            unparsed = unparsed.Remove(0, log.Routing.Length+2);

            log.AdditionalParams = GetAddInfo(log);//Additional params
            unparsed = unparsed.Remove(0, log.AdditionalParams.Length==1?0: log.AdditionalParams.Length);
            
            log.Location = iplocal.Keys.Contains(log.IpOrHost)? iplocal.First(x=>x.Key==log.IpOrHost).Value :  GetLocation(log.IpOrHost);
          

            log.Result = unparsed.Substring(0, unparsed.IndexOf(' '));//Result
            unparsed = unparsed.Remove(0, log.Result.Length+1);

            log.Size =int.Parse(unparsed.Substring(0, unparsed.IndexOf(' ')));//Size

            return log;
        }

        /// <param name="IpOrHost">Used to find location</param>
             public static string GetLocation(string ipOrHost)
        {
            string value;
            string locationResponse;
            string Query = @"https://www.whoisxmlapi.com/whoisserver/WhoisService?apiKey=at_3SvWV0IqqGrZkV7ybmW1So3wwgHjF&domainName=" + ipOrHost;
            try
            {
                locationResponse = new WebClient().DownloadString(Query);
            }
            catch (WebException)
            {

                value = "Invalid IP or Host";
                return value;
            }

            
            var responseXml = XDocument.Parse(locationResponse)
                .Element("WhoisRecord").Element("registrant");
            if (responseXml == null)
            {
                responseXml = XDocument.Parse(locationResponse)
               .Element("WhoisRecord").Element("registryData").Element("registrant");
            }

            
            if (responseXml.Element("organization").Value ==null||responseXml.Element("country").Value != null)
            {
                              
                value = responseXml.Element("organization").Value + "," + responseXml.Element("country").Value;


               
            }
            else
                value = "INVALID_TLD ";



            iplocal.Add(ipOrHost, value);
            return value;

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
            if (Routing.Contains(".gif") || Routing.Contains(".css") || Routing.Contains(".img") || Routing.Contains(".png")||log.RequestType!="GET"
                ||log.RequestType != "POST"|| log.RequestType != "HEAD"||log.Result!="200"|| log.Result != "404")
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
                    ReturnValue = " ";
                }
                else {
                    ReturnValue = ADdinfo[1];
                    log.Routing = log.Routing.Replace(ReturnValue[1],' ');
                }              
              
                log.Isvalid = true;
            }
           
        
            return ReturnValue;
            }

        public static  void Save(List<string> krk) {
       
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
      
    }
}









