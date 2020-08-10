using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NUnit.Framework;
using ESTQuestFilling.Model;

namespace ESTQuestFilling.Tests
{
    class QuestionTests
    {
        [Test]
        public void CreateEvaluateTable_TwoElementsTable_Create2ElementsArray()
        {
            var question = new Question(0, "", "27-10; 65-8;");
            var answer = new int[2][] {new[] {27, 10}, new[] {65, 8}};

            Assert.AreEqual(answer, question.EvaluationTable);
        }

        [Test]
        public void ReturnStringFromGetLiczbaCalkowitaCode_ValidCommentWithAllPossibleMarkValues_CreateProperXmlCode()
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", "[ZAKRES] <4, 4> N; <3, 3> W; <2, 2> A;");

            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            $"\t<Title></Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<MarkDef rangeMin = \"4\" rangeMax = \"4\" mark = \"normal\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"3\" rangeMax = \"3\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"2\" rangeMax = \"2\" mark = \"alarm\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                         "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <4,4> N; <3,3> W; <2,2> A;")]
        [TestCase("[ZAKRES] <4, 4> N; <3, 3> W; <2,2> A;")]
        [TestCase("[ZAKRES] <4, 4> N; <3,3> W; <2,2> A;")]
        [TestCase("[ZAKRES] <4 , 4> N; <3 , 3> W; <2 , 2> A;")]
        [TestCase("[ZAKRES] <4 ,4> N; <3 ,3> W; <2 , 2> A;")]
        [TestCase("[ZAKRES] < 4, 4 > N; < 3, 3> W; <2,2 > A;")]
        [TestCase("[ZAKRES] < 4 , 4 > N; < 3 , 3 > W; < 2 , 2 > A;")]
        [TestCase("[ZAKRES] <    4 , 4 > N; < 3 , 3  > W; < 2 , 2    > A;")]
        public void
            ReturnStringFromGetLiczbaCalkowitaCode_ValidCommentDeferentSpacingBetweenValues_CreateProperXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", comment);
            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            "\t<Title></Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<MarkDef rangeMin = \"4\" rangeMax = \"4\" mark = \"normal\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"3\" rangeMax = \"3\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"2\" rangeMax = \"2\" mark = \"alarm\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                         "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetLiczbaCalkowitaXMLCode_EmptyComment_CreateDefaultXmlCode()
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", "");

            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            "\t<Title></Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                       "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_CommentWithOneValidMark_CreateProperXmlCode()
        {
            var question = new Question(0, "", "","[Liczba całkowita]", "", "", "[ZAKRES] <1, 5> W");

            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                                "\t<Title></Title>\n" +
                                "\t<Mark>\n" +
                                    "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                        "\t\t\t<MarkDef rangeMin = \"1\" rangeMax = \"5\" mark = \"warning\"/>\n" +
                                    "\t\t</Definition>\n" +
                                "\t</Mark>\n" +
                                "\t<NotEdited/>\n" +
                         "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_InvalidComment_CreateDefaultXmlCode()
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", "[ZAKRES] <1, 5> FES");

            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            "\t<Title></Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                       "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <s, a> A")]
        [TestCase("[ZAKRES] <s, 5> A")]
        [TestCase("[ZAKRES] <5, a> A")]
        public void ReturnStringFromLiczbaCalkowitaXmlCode_InvalidCommentNaNInRange_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", comment);

            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            "\t<Title></Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                       "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase(" ")]
        [TestCase("[ZAKRES]")]
        [TestCase("<,>")]
        [TestCase("[ZAKRES] <,> A")]
        [TestCase("aaaaaaaaaaaaaa; aaaaaaaaaaaaaaaa; aaaaaaaaaaaaaaa")]
        [TestCase("[] <123> A")]
        public void ReturnStringFromLiczbaCalkowitaXmlCode_GeneralInvalidComment_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", comment);

            string answer = "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            "\t<Title></Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                       "</InputInteger>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }
    }
}
