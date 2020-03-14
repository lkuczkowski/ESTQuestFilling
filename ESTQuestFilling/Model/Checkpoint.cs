using System.Collections.Generic;
using System.Text;

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
            string firstPart = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                               "<Form xmlns = \"http://tempuri.org/FormSchema.1.5.xsd\" creationDate = \"2019-09-23T10:00:00.178Z\" creationUser = \"lkuczkowski\">\n" +
                               "<Page>\n" +
                               $"<Title>{Name}</Title>\n\n";
            string lastPart = "</Page>\n" +
                              "</Form>";

            StringBuilder sb = new StringBuilder();

            sb.Append(firstPart);

            foreach (var question in QuestionsList)
            {
                sb.Append(question.GetQuestionCode());
                sb.AppendLine("\n");
            }

            sb.Append(lastPart);
            return sb.ToString();
        }
    }
}
