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
        private PageViewModelBase _currentView;

        public PageViewModelBase CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
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

        private bool _isDatabaseRead;

        public bool IsDatabaseRead
        {
            get => _isDatabaseRead;
            set
            {
                _isDatabaseRead = value;
                OnPropertyChanged();
            }
        }

        private FormsViewModel _formsViewModel;
        public FormsViewModel FormsViewModel
        {
            get => _formsViewModel;
            set
            {
                _formsViewModel = value;
                OnPropertyChanged();
            }
        }

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
            // TODO - Fix null CurrentCompanyViewModel bug
            ReadDatabaseCommand = new DelegateCommand(parameter => ReadDatebase());
            CloseAppCommand = new DelegateCommand(n => Application.Current.Shutdown());
            IsDatabaseRead = false;
            SetFormsViewCommand = new DelegateCommand(n => CurrentView = FormsViewModel);
            SetQuestionsDatabaseViewCommand = new DelegateCommand(n => CurrentView = QuestionsDatabaseViewModel);
            SetRiskFactorsViewCommand = new DelegateCommand(n => CurrentView = RiskFactorsViewModel);
            WriteToXmlCommand = new DelegateCommand(n => WriteToXmlFiles());
            MinMaxWindowCommand = new DelegateCommand(n => MinMaxWindow());
            DragMoveMainWindowCommand = new DelegateCommand(n => Application.Current.MainWindow.DragMove());

            DatabaseName = "Not read";
            DatabasePath = "Not read";
        }

        public DelegateCommand ReadDatabaseCommand { get; }
        public DelegateCommand CloseAppCommand { get; }
        public DelegateCommand SetRiskFactorsViewCommand { get; }
        public DelegateCommand SetFormsViewCommand { get; }
        public DelegateCommand SetQuestionsDatabaseViewCommand { get; }
        public DelegateCommand WriteToXmlCommand { get; }
        public DelegateCommand MinMaxWindowCommand { get; }
        public DelegateCommand DragMoveMainWindowCommand { get; }
        
        private void MinMaxWindow()
        {
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void WriteToXmlFiles()
        {
            if (CurrentView is FormsViewModel currentView)
                currentView.WriteCurrentCompanyFormsToXml();
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
                    // TODO - scalić z wczytywaniem RiskFactors do karty z RiskFactorsView
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(queryRiskFactors, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    QuestionViewModel.RiskFactors = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        QuestionViewModel.RiskFactors.Add((int)reader[0], $"{reader[1]} - {reader[2]} --> {reader[3]}");
                    }
                    QuestionViewModel.RiskFactors.Add(0, "brak");
                    IsDatabaseRead = true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Application View Model cannot read database from {DatabasePath}\nException message:\n" + ex.Message);
                }
            }
            FormsViewModel = new FormsViewModel(DatabasePath);
            RiskFactorsViewModel = new RiskFactorsViewModel(DatabasePath);
            QuestionsDatabaseViewModel = new QuestionsDatabaseViewModel(DatabasePath);
        }
    }
}
