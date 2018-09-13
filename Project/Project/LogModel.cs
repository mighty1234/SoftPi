using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>  
    ///  This class it's a model.  
    /// </summary>  
    class LogModel
    {
        public DateTime RequestTime { get; set; }

        public string IpOrHost { get; set; }

        public string Routing { get; set; }

        public string AdditionalParams { get; set; }

        public string Result { get; set; }

        public string Location { get; set; }

        public int Size { get; set; }

        public bool Isvalid { get; set; }
     }

}
