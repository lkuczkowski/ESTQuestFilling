using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESTQuestFilling.Model
{
    class CheckpointWithPages
    {
        public int Id { get; }

        public string Name { get; }

        public List<QuestionWithPage> QuestionsList { get; }

        public CheckpointWithPages(int id, string name = "Unknown")
        {
            Id = id;
            Name = name;
            QuestionsList = new List<QuestionWithPage>();
        }

        public void AddQuestion(QuestionWithPage question)
        {
            QuestionsList.Add(question);
        }

        public string GetCheckpointCode()
        {
            string firstPart = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                               "<Form xmlns = \"http://tempuri.org/FormSchema.1.5.xsd\" creationDate = \"2019-09-23T10:00:00.178Z\" creationUser = \"lkuczkowski\">\n" +
                               "<Page>\n" +
                               $"<Title>{Name}</Title>\n\n";
            string lastPart = "</Page>\n" +
                              "</Form>";

            StringBuilder sb = new StringBuilder();

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
            return sb.ToString();
        }
    }
}
