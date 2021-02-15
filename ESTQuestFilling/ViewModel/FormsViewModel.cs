using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESTQuestFilling.ViewModel
{
    class FormsViewModel : PageViewModelBase
    {
        private readonly string _databasePath;

        private int _selectedCompanyIndex;

        public int  SelectedCompanyIndex
        {
            get => _selectedCompanyIndex;
            set
            {
                _selectedCompanyIndex = value;
                OnPropertyChanged();
            }
        }

        public FormsViewModel(string databasePath)
        {
            _databasePath = databasePath;
            CompanyListCollection = new ObservableCollection<string>(){"No company selected"};
            ReadCompanyNamesFromDatabase();
            CreateCompanyViewModelFromNameCommand = new DelegateCommand(CreateCompanyViewModelFromName, 
                o => SelectedCompanyIndex != 0);
            SelectedCompanyIndex = 0;
        }

        private void ReadCompanyNamesFromDatabase()
        {
            string connectionString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                + _databasePath
                + ";User Id=admin;Password=;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";
                    
                    connection.Open();
                    DataTable dt = connection.GetSchema("Tables", restrictions);

                    string tableName;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tableName = dt.Rows[i][2].ToString();
                        if (tableName.EndsWith(" - Pytania"))
                            CompanyListCollection.Add(tableName.Replace(" - Pytania", ""));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot read database from {_databasePath}\nException message:\n" + ex.Message);
                }
            }
        }

        public DelegateCommand CreateCompanyViewModelFromNameCommand { get; }

        private ObservableCollection<string> _companyListCollection;
        public ObservableCollection<string> CompanyListCollection
        {
            get => _companyListCollection;
            set
            {
                _companyListCollection = value;
                OnPropertyChanged();
            }
        }

        public void WriteCurrentCompanyFormsToXml()
        {
            CurrentCompanyViewModel?.WriteCheckpointsToFiles();
        }

        private void CreateCompanyViewModelFromName(object parameter)
        {
            CurrentCompanyViewModel = new CompanyViewModel(parameter.ToString(), _databasePath);
        }
        
        private CompanyViewModel _currentCompanyViewModel;
        public CompanyViewModel CurrentCompanyViewModel
        {
            get => _currentCompanyViewModel;
            set
            {
                _currentCompanyViewModel = value;
                OnPropertyChanged(nameof(CurrentCompanyViewModel));
            }
        }


    }
}
