using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ESTQuestFilling.Model
{
    class QuestionWithPage : Question
    {
        public QuestionWithPage(int id, string question, string marks, string answer, string number, string page)
            : base(id, question, marks, answer, number)
        {
            Page = page;
        }

        public string Page { get; }
    }
}
