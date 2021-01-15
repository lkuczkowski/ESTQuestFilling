using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model.Questions
{
    class TekstQuestion : QuestionBase
    {
        public TekstQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {
            
        }

        public override string GetXmlCode()
        {
            return $"<InputText required = \"{required}\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputText>";
        }
    }
}
