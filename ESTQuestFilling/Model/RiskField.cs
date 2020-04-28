using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model
{
    [Table(Name = "Obszar ryzyka")]
    class RiskField
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "Identyfikator")]
        public int ID { get; set; }

        [Column(Name = "Obszar")]
        public string riskField { get; set; }
    }
}
