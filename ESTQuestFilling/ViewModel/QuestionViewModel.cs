using ESTQuestFilling.Model;
using System.Collections.Generic;
using System.Linq;

namespace ESTQuestFilling.ViewModel
{
    class QuestionViewModel : BindableBase
    {
        private readonly Question _question;

        public string QuestionText => _question.QuestionText;

        public string Evaluation => string.Join("\n", _question.EvaluationTable.Select(n => RiskFactors[n[0]] + $" - {n[1]}"));

        public string Answer => _question.Answer;
        public string Number => _question.Number;
        public string Tag => _question.Tag;
        public string ID => _question.Id.ToString();
        public string Page => _question.Page;
        public string Comment => _question.Comment.Replace(";", "\n").Trim();

        public static Dictionary<int, string> RiskFactors { get; set; }

        public QuestionViewModel(Question question)
        {
            _question = question;
        }
    }
}
