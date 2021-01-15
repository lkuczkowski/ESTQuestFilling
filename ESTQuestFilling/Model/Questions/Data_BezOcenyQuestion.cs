using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model.Questions
{
    class Data_BezOcenyQuestion : QuestionBase
    {
        public Data_BezOcenyQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {
            
        }

        public override string GetXmlCode()
        {
            return $"<InputDate required = \"{required}\">\n" +
                       $"\t<Title>{QuestionText}</Title>\n" +
                       "\t<NotEdited/>\n" +
                  "</InputDate>";
        }
    }
}
