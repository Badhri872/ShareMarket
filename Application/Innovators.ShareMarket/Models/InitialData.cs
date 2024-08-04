using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovators_ShareMarket.Models
{
    public class InitialData
    {
        public string NSEToken { get; set; }
        public string NFOToken { get; set; }
        public List<string> NFOStrike { get; set; }
        public List<string> NFOTokens { get; set; }
    }
}
