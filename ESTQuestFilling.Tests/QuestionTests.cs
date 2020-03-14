using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ESTQuestFilling.Model;

namespace ESTQuestFilling.Tests
{
    class QuestionTests
    {
        [Test]
        public void CreateEvaluateTable_TwoElementsTable_Create2ElementsArray()
        {
            var question = new Question("", "27-10; 65-8;");

            Assert.AreEqual(question.EvaluationTable, new int[2][] { new [] {27, 10}, new [] {65, 8}});
        }
    }
}
