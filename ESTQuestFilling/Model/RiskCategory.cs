using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model
{
    [Table(Name = "Kategorie ryzyka")]
    class RiskCategory
    {
        private EntityRef<RiskField> _riskFieldEntityRef;

        public RiskCategory()
        {
            _riskFieldEntityRef = new EntityRef<RiskField>();
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "Identyfikator")]
        public int ID { get; set; }

        [Column(Name = "Obszar ryzyka")]
        public int RiskFieldID { get; set; }

        [Column(Name = "Kategoria ryzyka")]
        public string riskCategory { get; set; }

        [Association(Storage = nameof(_riskFieldEntityRef), ThisKey = nameof(RiskFieldID))]
        public RiskField RiskFieldAssociation
        {
            get { return _riskFieldEntityRef.Entity; }
            set { _riskFieldEntityRef.Entity = value; }
        }
    }
}
