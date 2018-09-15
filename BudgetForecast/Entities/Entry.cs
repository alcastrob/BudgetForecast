using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetForecast.Entities
{
    public class Entry
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Payee { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public DateTime Moment { get; set; }
        public decimal Ammount { get; set; }
        public string Status { get; set; }
        public int Repeats { get; set; }
    }
}
