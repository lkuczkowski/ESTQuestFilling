using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ESTQuestFilling.Model
{
    [Table(Name = "Czynniki ryzyka")]
    class RiskFactor
    {
        private EntityRef<RiskCategory> _riskCategoryEntityRef;

        public RiskFactor()
        {
            _riskCategoryEntityRef = new EntityRef<RiskCategory>();
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "Identyfikator")]
        public int ID { get; set; }

        [Column(Name = "Numer identyfikacyjne w systemie")]
        public int EST_ID { get; set; }

        [Column(Name = "Kategoria ryzyka")]
        public int RiskCategoryID { get; set; }

        [Column(Name = "Czynnik ryzyka")]
        public string RiskFactorName { get; set; }

        [Column(Name = "Numeracja")]
        public string Number { get; set; }

        [Association(Storage = nameof(_riskCategoryEntityRef), ThisKey = nameof(RiskCategoryID))]
        public RiskCategory RiskCategoryAssociation
        {
            get { return _riskCategoryEntityRef.Entity;}
            set { _riskCategoryEntityRef.Entity = value; }
        }

        public string RiskCategory
        {
            get { return RiskCategoryAssociation.riskCategory; }
        }

        public string RiskField
        {
            get { return _riskCategoryEntityRef.Entity.RiskFieldAssociation.riskField; }
        }
    }
}
