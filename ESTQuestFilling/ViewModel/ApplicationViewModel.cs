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

        private CompanyViewModel _currentInstitution;
        public CompanyViewModel CurrentInstitution
        {
            get => _currentInstitution;
            set
            {
                _currentInstitution = value;
                CurrentInstitutionName = value.Name;
                OnPropertyChanged(nameof(CurrentInstitution));
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

        public string CurrentInstitutionName { get; set; } = "No institution read";

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
            ReadInstitutionCommand = new DelegateCommand((object parameter) => ReadCompany((string)parameter));
            WriteInstitutionCheckpointsToFilesCommand = new DelegateCommand((object parameter) => CurrentInstitution.WriteCheckpointsToFiles());
            ReadDatabaseCommand = new DelegateCommand((object parameter) =>
            {
                ReadDatebase();
            });
            CloseAppCommand = new DelegateCommand((object n) => Application.Current.Shutdown());
            
            DatabaseName = "Not read";
            DatabasePath = "Not read";
        }

        public DelegateCommand ReadInstitutionCommand { get; }
        public DelegateCommand WriteInstitutionCheckpointsToFilesCommand { get; }
        public DelegateCommand ReadDatabaseCommand { get; }
        public DelegateCommand CloseAppCommand { get; }

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

        private void ReadCompany(string name)
        {
            if (name == String.Empty || name == null)
            {
                MessageBox.Show("ReadCompany command error");
            }

            if (DatabasePath != null)
            {
                string connectionString =
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                    + DatabasePath
                    + ";User Id=admin;Password=;";

                string queryString =
                    $"SELECT " +
                    $"[{name} - Pytania].[Punkt kontrolny], " +
                    $"[Punkty kontrolne].[Punkt kontrolny], " +
                    $"[{name} - Punkty kontrolne].Nazwa, " +
                    $"[{name} - Pytania].Pytanie, " +
                    $"[Kwantyfikowanie].Kwantyfikator, " +
                    $"[{name} - Pytania].Ocena, " +
                    $"[Numer punktu].[Numer punktu], " +
                    $"[{name} - Pytania].Identyfikator " + 
                    $"FROM (((([{name} - Pytania] INNER JOIN [{name} - Punkty kontrolne] ON [{name} - Pytania].[Punkt kontrolny] = [{name} - Punkty kontrolne].[Punkt kontrolny]) " +
                    $"INNER JOIN [Kwantyfikowanie] ON [{name} - Pytania].[Kwantyfikowanie] = [Kwantyfikowanie].Identyfikator) " +
                    $"INNER JOIN [Punkty kontrolne] ON [{name} - Pytania].[Punkt kontrolny] = [Punkty kontrolne].Identyfikator) " +
                    $"INNER JOIN [Numer punktu] ON [{name} - Pytania].[Numer] = [Numer punktu].Identyfikator) " +
                    $"ORDER BY [{name} - Pytania].Numer;";


                Company company = new Company(name);
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    try
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        reader.Read();
                        Checkpoint chp = new Checkpoint((int)reader[0], (string)reader[1] + " - " + (string)reader[2]);
                        chp.AddQuestion(new Question((int)reader[7], reader[3].ToString(), reader[5].ToString(), (string)reader[4], reader[6].ToString()));
                        company.AddCheckpoint(chp);

                        while (reader.Read())
                        {
                            if ((int)reader[0] != company.CheckpointsList.Last().Id)
                            {
                                chp = new Checkpoint((int)reader[0], (string)reader[1] + " - " + (string)reader[2]);
                                company.AddCheckpoint(chp);
                            }

                            chp.AddQuestion(new Question((int)reader[7], reader[3].ToString(), reader[5].ToString(), (string)reader[4], reader[6].ToString()));
                        }

                        reader.Close();
                        CurrentInstitution = new CompanyViewModel(company);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Reading DB error\n\n" + ex.Message);
                    }

                }
            }
        }
    }
}
