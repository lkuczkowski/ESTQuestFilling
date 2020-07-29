using System;
using ESTQuestFilling.Model;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Windows;

namespace ESTQuestFilling.ViewModel
{
    class CompanyViewModel : BindableBase
    {
        private Company _company;
        private readonly string _databasePath;

        public CompanyViewModel(string name, string databasePath)
        {
            _databasePath = databasePath;
            BuildCompany(name);
            CheckpointViewModelsList = new ObservableCollection<CheckpointViewModel>(_company.CheckpointsList.Select(n=> new CheckpointViewModel(n)));
        }

        public string Name => _company.Name;

        public ObservableCollection<CheckpointViewModel> CheckpointViewModelsList { get; set; }

        public void WriteCheckpointsToFiles() => _company?.WriteInstitutionCodeFiles();

        private void BuildCompany(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("BuildCompany command error");
                return;
            }

            if (_databasePath != null)
            {
                string connectionString =
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                    + _databasePath
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
                    $"ORDER BY [{name} - Pytania].Numer, [{name} - Pytania].Identyfikator;";

                _company = new Company(name);
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
                        _company.AddCheckpoint(chp);

                        while (reader.Read())
                        {
                            if ((int)reader[0] != _company.CheckpointsList.Last().Id)
                            {
                                chp = new Checkpoint((int)reader[0], (string)reader[1] + " - " + (string)reader[2]);
                                _company.AddCheckpoint(chp);
                            }

                            chp.AddQuestion(new Question((int)reader[7], reader[3].ToString(), reader[5].ToString(), (string)reader[4], reader[6].ToString()));
                        }

                        reader.Close();
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
