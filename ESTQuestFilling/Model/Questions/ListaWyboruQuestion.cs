using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model.Questions
{
    class ListaWyboruQuestion : QuestionBase
    {
        public ListaWyboruQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {
            
        }
        public override string GetXmlCode()
        {
            throw new NotImplementedException();
        }
    }
}
