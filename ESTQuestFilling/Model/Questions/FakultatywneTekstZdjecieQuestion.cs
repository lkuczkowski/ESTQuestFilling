using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model.Questions
{
    class FakultatywneTekstZdjecieQuestion : QuestionBase
    {
        public FakultatywneTekstZdjecieQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {
            
        }

        public override string GetXmlCode()
        {
            return "<InputText required=\"false\">\n" +
                         $"\t<Title>{QuestionText}</Title>\n" +
                         "\t<Inner>\n" +
                             "\t\t<OnValue operand=\"exists\"/>\n" +
                             "\t\t<InnerInputs>\n" +
                                 "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\">\n" +
                                     "\t\t\t\t<Title>Wykonaj zdjęcie dotyczące uwag (fakultatywne).</Title>\n" +
                                     "\t\t\t\t<NotEdited/>\n" +
                                 "\t\t\t</InputImage>\n" +
                                 "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\">\n" +
                                     "\t\t\t\t<Title>Wykonaj dodatkowe zdjęcie dotyczące uwag (fakultatywne).</Title>\n" +
                                     "\t\t\t\t<NotEdited/>\n" +
                                 "\t\t\t</InputImage>\n" +
                             "\t\t</InnerInputs>\n" +
                         "\t</Inner>\n" +
                         "\t<Mark>\n" +
                             "\t\t<ByOperator expiredMark = \"normal\" initialMark = \"normal\" refusalMark = \"normal\"/>\n" +
                         "\t</Mark>\n" +
                         "\t<NotEdited/>\n" +
                 "</InputText>";
        }
    }
}
