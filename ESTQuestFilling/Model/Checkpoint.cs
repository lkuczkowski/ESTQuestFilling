using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace ESTQuestFilling.Model
{
    class Checkpoint
    {
        public int Id { get; }

        public string Name { get; }

        public List<Question> QuestionsList { get; }

        public Checkpoint(int id, string name = "Unknown")
        {
            Id = id;
            Name = name;
            QuestionsList = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            QuestionsList.Add(question);
        }

        public string GetCheckpointCode()
        {
            StringBuilder sb = new StringBuilder();

            if (QuestionsList[0].Page == "")
            {
                string firstPart = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                   "<Form xmlns = \"http://tempuri.org/FormSchema.1.5.xsd\" creationDate = \"2019-09-23T10:00:00.178Z\" creationUser = \"lkuczkowski\">\n" +
                                   "<Page>\n" +
                                   $"<Title>{Name}</Title>\n\n";
                string lastPart = "</Page>\n" +
                                  "</Form>";

                sb.Append(firstPart);

                try
                {
                    foreach (var question in QuestionsList)
                    {
                        sb.Append(question.GetQuestionCode());
                        sb.AppendLine("\n");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                sb.Append(lastPart);

            }
            else
            {
                var questionsGropedByPage = QuestionsList.GroupBy(n => n.Page);
                string firstPart = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                   $"<Form xmlns = \"http://tempuri.org/FormSchema.1.5.xsd\" creationDate = \"{DateTime.Now:u}\" creationUser = \"lkuczkowski\">\n\n";

                sb.Append(firstPart);

                foreach (var questionGroup in questionsGropedByPage)
                {
                    sb.Append("\t<Page>\n" + $"\t\t<Title>{questionGroup.Key}</Title>\n\n");
                    foreach (var question in questionGroup)
                    {
                        sb.Append(question.GetQuestionCode());
                        sb.AppendLine(Environment.NewLine);
                    }

                    sb.AppendLine("\t</Page>\n");
                }

                sb.AppendLine("</Form>");
            }
            return sb.ToString();
        }
    }
}
