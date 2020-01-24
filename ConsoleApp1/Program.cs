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
    class Program
    {
        static void Main(string[] args)
        {
            TransitoryManager tr = new TransitoryManager();
            tr.Create();
            tr.changeCity("New York");
            Console.WriteLine(tr.readToString());
            Console.WriteLine(tr.getFilesNames());
            Console.ReadKey();
        }


    }
}
