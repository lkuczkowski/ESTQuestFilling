using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESTQuestFilling.Model;

namespace ESTQuestFilling.ViewModel
{
    class RiskFactorsViewModel : BindableBase
    {
        private string _databasePath;

        public DelegateCommand ReadRiskFactorsCommand { get; set; }

        public RiskFactorsViewModel(string databasePath)
        {
            _databasePath = databasePath;
            ReadRiskFactorsCommand = new DelegateCommand(ReadRiskFactors, (object parameter) => RiskFactors == null);
        }

        private ObservableCollection<RiskFactor> _riskFactors;

        public ObservableCollection<RiskFactor> RiskFactors
        {
            get { return _riskFactors; }
            set
            {
                _riskFactors = value;
                OnPropertyChanged(nameof(RiskFactors));
            }
        }

        private void ReadRiskFactors(object parameter)
        {
            string connectionString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                + _databasePath
                + ";User Id=admin;Password=;";

            OleDbConnection L2QConnection = new OleDbConnection(connectionString);
            DataContext dbContext = new DataContext(L2QConnection);
            Table<RiskFactor> trf = dbContext.GetTable<RiskFactor>();
            RiskFactors = new ObservableCollection<RiskFactor>(trf);
        }

        //private Task<ObservableCollection<RiskFactor>> GetRiskFactorsAsync()
        //{
        //    string connectionString =
        //        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
        //        + _databasePath
        //        + ";User Id=admin;Password=;";

        //    OleDbConnection L2QConnection = new OleDbConnection(connectionString);
        //    DataContext dbContext = new DataContext(L2QConnection);
        //    //Table<RiskFactor> trf = dbContext.GetTable<RiskFactor>();
        //    return Task.Run(() => new ObservableCollection<RiskFactor>(dbContext.GetTable<RiskFactor>()));
        //}


    }
}
