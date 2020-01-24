using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp1
{


    [XmlRoot("PurchaseOrder", Namespace = "http://www.candas.com",IsNullable = false)]
    public class PurchaseOrder
    {
        public Address ShipTo;
        public string OrderDate;
        // The XmlArray attribute changes the XML element name
        // from the default of "OrderedItems" to "Items".
        [XmlArray("Items")]
        public OrderedItem[] OrderedItems;
        public decimal SubTotal;
        public decimal ShipCost;
        public decimal TotalCost;
    }
}
