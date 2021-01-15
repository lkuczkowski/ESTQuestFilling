using System;
using System.Collections.Generic;
using System.Linq;

namespace ESTQuestFilling.Model.Questions
{
    public abstract class QuestionBase : IXmlCodeProducer
    {
        public int DataBaseId { get; private set; }

        protected string _analyticsLink;
        protected string _evaluation;
        protected string required = "true";
        protected string refusalMark = "warning";
        public string Answer { get; }
        public string Number { get; }
        public string Tag { get; set; }
        public string Page { get; }
        public string Comment { get; }
        public string QuestionText { get; }
        public int[][] EvaluationTable { get; protected set; }

        public static readonly Dictionary<string, string> _marksAbbreviationsToXmlNamesDictionary = new Dictionary<string, string>()
        {
            { "N", "normal"},
            { "W", "warning"},
            { "A", "alarm"}
        };
        
        protected QuestionBase(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
        {
            DataBaseId = dataBaseId;
            QuestionText = question;
            Answer = answer;
            Number = number;
            Page = page;
            Comment = comment;
            SetEvaluationTableAndAnalyticsLinkString(analytics);
        }

        protected void SetEvaluationTableAndAnalyticsLinkString(string marks)
        {
            string[] noAnalyticIndicators = { "", "brak", "brak;", "brak; " };
            _evaluation = marks;
            if (noAnalyticIndicators.Contains(marks))
            {
                EvaluationTable = new int[1][] { new int[] { 0, 0 } };
                _analyticsLink = "";
            }
            else
            {
                CreateEvaluateTable();
                _analyticsLink = "\t\t<AnalyticsLink>\n" +
                                 EvaluationTable.Aggregate("",
                                     (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                                 "\t\t</AnalyticsLink>\n";
            }
        }

        private void CreateEvaluateTable()
        {
            var splitMarksStrings = _evaluation.Split(new char[] { ' ', })
                .Select(n => n.Trim(new char[] { ' ', ';' }).Split(new char[] { '-' })).ToArray();

            EvaluationTable = splitMarksStrings.Select(n => n.Select(Int32.Parse).ToArray()).ToArray();
        }

        public abstract string GetXmlCode();
    }
}
