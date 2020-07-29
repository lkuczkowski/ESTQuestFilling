using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.Model
{
    interface ICheckpointable
    {
        int Id { get; }
        string Name { get; }
        void AddQuestion(Question question);
        string GetCheckpointCode();

    }
}
