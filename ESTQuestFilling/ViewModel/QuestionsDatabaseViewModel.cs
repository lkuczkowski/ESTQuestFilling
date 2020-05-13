using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ESTQuestFilling.Model;

namespace ESTQuestFilling.ViewModel
{
    class QuestionsDatabaseViewModel : BindableBase
    {
        private ObservableCollection<QuestionViewModel> _searchQuestionCollection;

        public ObservableCollection<QuestionViewModel> SearchQuestionCollection
        {
            get => _searchQuestionCollection;
            set
            {
                _searchQuestionCollection = value;
                OnPropertyChanged(nameof(SearchQuestionCollection));
            }
        }

        private void GetSearchedQuestions(object tag)
        {
            string tagString = tag.ToString();
            if (tagString == " " || tagString == string.Empty)
                SearchQuestionCollection = QuestionsCollection;
            else
            {
                SearchQuestionCollection = new ObservableCollection<QuestionViewModel>(QuestionsCollection.Where(n => n.Tag.Contains(tagString)));
            }
        }

        public static ObservableCollection<QuestionViewModel> QuestionsCollection { get; } = new ObservableCollection<QuestionViewModel>();

        public DelegateCommand GetSearchedQuestionsCommand { get; }
        public DelegateCommand ReadQuestionDatabaseCommand { get; }

        private readonly string _databasePath;
        public QuestionsDatabaseViewModel(string databasePath)
        {
            _databasePath = databasePath;
            SearchQuestionCollection = QuestionsCollection;
            GetSearchedQuestionsCommand = new DelegateCommand(GetSearchedQuestions);
            ReadQuestionDatabaseCommand = new DelegateCommand(ReadQuestionDatabase, o => QuestionsCollection.Count == 0);
        }

        private void ReadQuestionDatabase(object parameter)
        {
            if (_databasePath != null)
            {
                string connectionString =
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                    + _databasePath
                    + ";User Id=admin;Password=;";

                string queryString =
                    $"SELECT " +
                    $"[Baza pytań].Pytanie, " +
                    $"[Kwantyfikowanie].Kwantyfikator, " +
                    $"[Baza pytań].Ocena, " +
                    $"[Baza pytań].Tag, " +
                    $"[Baza pytań].Identyfikator " + 
                    $"FROM " +
                    $"[Baza pytań] " +
                    $"INNER JOIN [Kwantyfikowanie] ON [Baza pytań].[Kwantyfikowanie] = [Kwantyfikowanie].Identyfikator " +
                    $"ORDER BY [Baza pytań].Identyfikator;";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    try
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        QuestionsCollection.Clear();
                        Question qu;
                        while (reader.Read())
                        {
                            qu = new Question((int) reader[4], reader[0].ToString(), reader[2].ToString(), reader[1].ToString())
                            {
                                Tag = reader[3].ToString()
                            };
                            QuestionsCollection.Add(new QuestionViewModel(qu));
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
