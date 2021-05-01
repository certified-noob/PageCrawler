using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectX
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class PartialEntry
    {
        public int id { get; set; }
        public string transactionType { get; set; }
        public bool adding { get; set; }
        public object tradeDate { get; set; }
        public int shares { get; set; }
        public double price { get; set; }
        public int sharesRemaining { get; set; }
        public bool @short { get; set; }
        public string comments { get; set; }
    }

    public class Root
    {
        public List<PartialEntry> partialEntries { get; set; }
    }


}
