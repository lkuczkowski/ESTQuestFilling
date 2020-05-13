using ESTQuestFilling.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Linq;
using System.Data.OleDb;
using System.Linq;
using System.Windows;

namespace ESTQuestFilling.ViewModel
{
    class ApplicationViewModel : BindableBase
    {
        public static ObservableCollection<string> CompanyListCollection { get; } = new ObservableCollection<string>()
        {
            "No database read"
        };

        public string CurrentInstitutionName { get; set; } = "No institution read";

        private CompanyViewModel _currentCompanyViewModel;
        public CompanyViewModel CurrentCompanyViewModel
        {
            get => _currentCompanyViewModel;
            set
            {
                _currentCompanyViewModel = value;
                CurrentInstitutionName = value.Name;
                OnPropertyChanged(nameof(CurrentCompanyViewModel));
                OnPropertyChanged(nameof(CurrentInstitutionName));
            }
        }

        private string _databasePath;
        public string DatabasePath
        {
            get => _databasePath;
            set
            {
                _databasePath = value;
                OnPropertyChanged(nameof(DatabasePath));
            }
        }

        private string _databaseName;
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                OnPropertyChanged(nameof(DatabaseName));
            }
        }

        public ObservableCollection<string> DatabaseTableNamesList { get; set; } = new ObservableCollection<string>();

        private RiskFactorsViewModel _riskFactorsViewModel;
        public RiskFactorsViewModel RiskFactorsViewModel
        {
            get { return _riskFactorsViewModel; }
            set
            {
                _riskFactorsViewModel = value;
                OnPropertyChanged(nameof(RiskFactorsViewModel));
            }
        }

        private QuestionsDatabaseViewModel _questionsDatabaseViewModel;

        public QuestionsDatabaseViewModel QuestionsDatabaseViewModel
        {
            get { return _questionsDatabaseViewModel; }
            set
            {
                _questionsDatabaseViewModel = value;
                OnPropertyChanged(nameof(QuestionsDatabaseViewModel));
            }
        }

        public ApplicationViewModel()
        {
            CreateCompanyViewModelFromNameCommand = new DelegateCommand(CreateCompanyViewModelFromName);
            WriteInstitutionCheckpointsToFilesCommand = new DelegateCommand((object parameter) => CurrentCompanyViewModel.WriteCheckpointsToFiles());
            ReadDatabaseCommand = new DelegateCommand((object parameter) =>
            {
                ReadDatebase();
            });
            CloseAppCommand = new DelegateCommand((object n) => Application.Current.Shutdown());
            
            DatabaseName = "Not read";
            DatabasePath = "Not read";
        }

        public DelegateCommand CreateCompanyViewModelFromNameCommand { get; }
        public DelegateCommand WriteInstitutionCheckpointsToFilesCommand { get; }
        public DelegateCommand ReadDatabaseCommand { get; }
        public DelegateCommand CloseAppCommand { get; }

        private void CreateCompanyViewModelFromName(object parameter)
        {
            CurrentCompanyViewModel = new CompanyViewModel(parameter.ToString(), DatabasePath);
        }

        // TODO - Repository pattern
        private void ReadDatebase()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                DatabasePath = fileDialog.FileName;
                DatabaseName = fileDialog.SafeFileName;
            }

            RiskFactorsViewModel = new RiskFactorsViewModel(DatabasePath);
            QuestionsDatabaseViewModel = new QuestionsDatabaseViewModel(DatabasePath);
            
            string connectionString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                + DatabasePath
                + ";User Id=admin;Password=;";

            string queryRiskFactors =
                "SELECT " +
                "[Czynniki ryzyka].[Numer identyfikacyjne w systemie], " +
                "[Czynniki ryzyka].[Numeracja], " +
                "[Kategorie ryzyka].[Kategoria ryzyka], " +
                "[Czynniki ryzyka].[Czynnik ryzyka] " +
                "FROM " +
                "[Czynniki ryzyka] " +
                "INNER JOIN [Kategorie ryzyka] ON [Czynniki ryzyka].[Kategoria ryzyka] = [Kategorie ryzyka].[Identyfikator];";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";
                    DataTable dt = connection.GetSchema("Tables", restrictions);
                    DatabaseTableNamesList.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DatabaseTableNamesList.Add(dt.Rows[i][2].ToString());
                    }

                    CompanyListCollection.Clear();
                    foreach (var companyName in DatabaseTableNamesList.Where((s) => s.EndsWith(" - Pytania")).Select((m) => m.Replace(" - Pytania", "")))
                    {
                        CompanyListCollection.Add(companyName);
                    }

                    // TODO - scalić z wczytywaniem RiskFactors do karty z RiskFactorsView
                    OleDbCommand command = new OleDbCommand(queryRiskFactors, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    QuestionViewModel.RiskFactors = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        QuestionViewModel.RiskFactors.Add((int)reader[0], $"{reader[1]} - {reader[2]} --> {reader[3]}");
                    }
                    QuestionViewModel.RiskFactors.Add(0, "brak");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot read database from {DatabasePath}\nException message:\n" + ex.Message);
                }
            }
        }
    }
}
