using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model.Questions
{
    class OcenaEkspercka_ZdjecieQuestion : QuestionBase
    {
        public OcenaEkspercka_ZdjecieQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {

        }

        public override string GetXmlCode()
        {
            return $"<InputImage allowCamera=\"true\" allowFile=\"false\" required=\"{required}\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
                            $"\t\t<ByOperator expiredMark=\"warning\" initialMark=\"normal\" refusalMark=\"{refusalMark}\"/>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputImage>";
        }
    }
}
