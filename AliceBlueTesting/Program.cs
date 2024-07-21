using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceBlueTesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SmartApi connect = new SmartApi("lr4liHKm");
            var json = connect.GetCandleData();
        }
    }
}
