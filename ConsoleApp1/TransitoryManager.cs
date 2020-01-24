using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    public class TransitoryManager
    {
        public const string FILE_NAME = "PurchaseOrder.xml";
        public void Create()
        {
            PurchaseOrder po = new PurchaseOrder();

            // Creates an address to ship and bill to.
            Address billAddress = new Address();
            billAddress.Name = "Teresa Atkinson";
            billAddress.Line1 = "1 Main St.";
            billAddress.City = "AnyTown";
            billAddress.State = "WA";
            billAddress.Zip = "00000";
            // Sets ShipTo and BillTo to the same addressee.
            po.ShipTo = billAddress;
            po.OrderDate = System.DateTime.Now.ToLongDateString();

            // Creates an OrderedItem.
            OrderedItem i1 = new OrderedItem();
            i1.ItemName = "Widget S";
            i1.Description = "Small widget";
            i1.UnitPrice = (decimal)5.23;
            i1.Quantity = 3;
            i1.Calculate();

            // Inserts the item into the array.
            OrderedItem[] items = { i1 };
            po.OrderedItems = items;
            // Calculate the total cost.
            decimal subTotal = new decimal();
            foreach (OrderedItem oi in items)
            {
                subTotal += oi.LineTotal;
            }
            po.SubTotal = subTotal;
            po.ShipCost = (decimal)12.51;
            po.TotalCost = po.SubTotal + po.ShipCost;

            save<PurchaseOrder>(po);

        }

        public void changeCity(string pCity)
        {
            PurchaseOrder po = read<PurchaseOrder>();
            po.ShipTo.City = pCity;
            save<PurchaseOrder>(po);
        }

        public string getFilesNames()
        {
            StringBuilder str = new StringBuilder();
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            {
                foreach (string s in f.GetFileNames("*.*"))
                {
                    str.AppendLine(s); 
                }
            }
            return str.ToString();
        }

        public bool delete()
        {
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            {
                f.DeleteFile(FILE_NAME);               
            }
            return true;
        }

        internal void save<T>(T pObj)
        {
            // Creates an instance of the XmlSerializer class;
            // specifies the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // IsolatedStorage classes live in System.IO.IsolatedStorage
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            {
                using (var s = new IsolatedStorageFileStream(FILE_NAME, FileMode.Create, f))
                {
                    using (var writer = new StreamWriter(s))
                    {
                        // Serializes the purchase order, and closes the TextWriter.
                        serializer.Serialize(writer, pObj);
                        writer.Close();
                    }
                    s.Close();
                }
            }
        }


        public string readToString()
        {
            string str = string.Empty;
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            {
                using (var s = new IsolatedStorageFileStream(FILE_NAME, FileMode.Open, f))
                {
                    using (var reader = new StreamReader(s))
                    {
                        str = reader.ReadToEnd();
                        reader.Close();
                    }
                    s.Close();
                }
            }
            return str;
        }

        protected T read<T>()
        {
            // Creates an instance of the XmlSerializer class;
            // specifies the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // If the XML document has been altered with unknown
            // nodes or attributes, handles them with the
            // UnknownNode and UnknownAttribute events.
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            // Declares an object variable of the type to be deserialized.
            T obj;
            // Read it back:
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            {
                using (var s = new IsolatedStorageFileStream(FILE_NAME, FileMode.Open, f))
                {
                    using (var reader = new StreamReader(s))
                    {
//#if DEBUG
//                        Console.WriteLine(reader.ReadToEnd());
//                        Console.ReadKey();
//#endif
                        obj = (T)serializer.Deserialize(reader);
                        s.Close();
                    }
                    s.Close();
                }
            }

            return obj;
        }

        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }

    }
}
