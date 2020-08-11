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


        //*************************************GetLiczbaCalkowitaXmlCode() Tests************************************

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
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_InvalidCommentNaNInRange_CreateDefaultXmlCode(string comment)
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
        [TestCase("                            ")]
        [TestCase(" \t\n")]
        [TestCase(" ,./<>?;:''\"[]{}\\\'=+-_)(*&^%$#@!!~`")]
        [TestCase("[ZAKRES]")]
        [TestCase("<,>")]
        [TestCase("[ZAKRES] <,> A")]
        [TestCase("aaaaaaaaaaaaaa; aaaaaaaaaaaaaaaa; aaaaaaaaaaaaaaa")]
        [TestCase("[] <123> A")]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_GeneralInvalidComment_CreateDefaultXmlCode(string comment)
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

        [TestCase("[TEKST] <10, 20> A")]
        [TestCase("[ZDJĘCIE] <10, 20> A")]
        [TestCase("[] <10, 20> A")]
        [TestCase("<10, 20> A")]
        [TestCase(" <10, 20> A")]
        [TestCase("] <10, 20> A")]
        [TestCase("[ <10, 20> A")]
        [TestCase("() <10, 20> A")]
        [TestCase("(ZAKRES) <10, 20> A")]
        [TestCase("ZAKRES <10, 20> A")]
        [TestCase("]TEKST[ <10, 20> A")]
        [TestCase("[TEKST] <10, 20> A")]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_InvalidPrefix_CreateDefaultXmlCode(string comment)
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

        [TestCase("[ZAKRES] <10, 20>; A;")]
        [TestCase("[ZAKRES] ;<10, 20> A <;20, 30> W")]
        [TestCase("[ZAK;RES] <10, 20>; A;")]
        [TestCase("[ZAKRES] <10, 20>; A;;;;;")]
        [TestCase("[ZAKRES] <10; 20>; A;")]
        [TestCase(";[ZAKRES] <10, 20> A;")]
        [TestCase("[ZAKRES] ;<10, 20> A; ;")]
        [TestCase("[ZAKRES] <10, ;20>; A;")]
        [TestCase("[ZAKRES] <10, 20>;; A;")]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_TooManySeparators_CreateDefaultXmlCode(string comment)
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

        [Test]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_CommentWithOneValidMarkNoSpaceAfterPrefix_CreateProperXmlCode()
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", "[ZAKRES]<1, 5> W");

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

        [TestCase("[ZAKRES] <4, 4> N; <3, 3> W; <2, 2> A;")]
        [TestCase("[ZAKRES]<4, 4> N; <3, 3> W; <2, 2> A;")]
        [TestCase("[ZAKRES] <4, 4> N;<3, 3> W; <2, 2> A;")]
        [TestCase("[ZAKRES] <4, 4> N;<3, 3> W;<2, 2> A;")]
        [TestCase("[ZAKRES]<4, 4> N;<3, 3> W;<2, 2> A;")]
        public void ReturnStringFromGetLiczbaCalkowitaCode_ValidCommentNoSpacesAfterSeparator_CreateProperXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Liczba całkowita]", "", "", comment);

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

        [TestCase("[ZAKRES] <10, 20, 30> A")]
        [TestCase("[ZAKRES] <10, 20, 30> A; <1, 2, 3> W")]
        [TestCase("[ZAKRES] <10, 20, 30> A; <1, 2, 3> W; <100, 200> N")]
        public void ReturnStringFromGetLiczbaCalkowitaXmlCode_MoreThenTwoValuesInRange_CreateDefaultXmlCode(string comment)
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

        //*************************************GetSuwak_odczytWartosciXmlCode() Tests************************************

        [Test]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_ValidCommentWithIntervalAndAllPossibleMarks_CreateProperXmlCode()
        {
            string comment = "[ZAKRES] <0, 100>; <0, 30> A; <30, 70> W; <70, 100> N";
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);
            string answer = "<InputSliderInt minValue=\"0\" maxValue=\"100\" defaultValue=\"0\" required=\"true\">\n" +
                                    "\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<MarkDef rangeMin = \"0\" rangeMax = \"30\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"30\" rangeMax = \"70\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"70\" rangeMax = \"100\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <0, 10>; <4,4> N; <3,3> W; <2,2> A;")]
        [TestCase("[ZAKRES]<0,10>;<4,4>N;<3,3>W;<2,2>A;")]
        [TestCase("[ZAKRES] <0,       10>; <4, 4> N; <3, 3> W; <2,2> A;")]
        [TestCase("[ZAKRES] <0,10>; <4, 4> N; <3,3> W; <2,2> A;")]
        [TestCase("[ZAKRES] <0 , 10>; <4 , 4> N; <3 , 3> W; <2 , 2> A;")]
        [TestCase("[ZAKRES] <    0,10>; <4 ,4> N; <3 ,3> W; <2 , 2> A;")]
        [TestCase("[ZAKRES] <0, 10      >; < 4, 4 > N; < 3, 3> W; <2,2 > A;")]
        [TestCase("[ZAKRES] <0     ,      10>; < 4 , 4 > N; < 3 , 3 > W; < 2 , 2 > A;")]
        [TestCase("[ZAKRES] <     0    , 10     > ; <    4 , 4 > N; < 3 , 3  > W; < 2 , 2    > A;")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_ValidCommentWithSpacingCases_CreateProperXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);
            string answer = "<InputSliderInt minValue=\"0\" maxValue=\"10\" defaultValue=\"0\" required=\"true\">\n" +
                                    "\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<MarkDef rangeMin = \"4\" rangeMax = \"4\" mark = \"normal\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"3\" rangeMax = \"3\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"2\" rangeMax = \"2\" mark = \"alarm\"/>\n" +
                                    "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_EmptyComment_CreateDefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_NoMinMaxValue_CreateDefaultXmlCode()
        {
            string comment = "[ZAKRES] <0, 30> A; <30; 70> W; <70, 100> N";
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_CommentWithIntervalAndOneMark_CreateProperXmlCode()
        {
            string comment = "[ZAKRES] <0, 100>; <0, 30> A;";
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<InputSliderInt minValue=\"0\" maxValue=\"100\" defaultValue=\"0\" required=\"true\">\n" +
                                   "\t<Title></Title>\n" +
                                   "\t<Mark>\n" +
                                       "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                           "\t\t\t<MarkDef rangeMin = \"0\" rangeMax = \"30\" mark = \"alarm\"/>\n" +
                                   "\t\t</Definition>\n" +
                                   "\t</Mark>\n" +
                                   "\t<NotEdited/>\n" +
                              "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_NoMarkAfterInterval_CreateDefaultXmlCode()
        {
            string comment = "[ZAKRES] <0, 30>";
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <0, 30> <10, 20> A")]
        [TestCase("[ZAKRES] <0, 30> <10, 20> A;")]
        [TestCase("[ZAKRES] <0, 30>; <10, 20> A <20, 30> W")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_MissingSeparator_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <0, 30>; <10, 20>; A;")]
        [TestCase("[ZAKRES] <0, ;30>; <10, 20> A;")]
        [TestCase("[ZAKRES] <0, 30>; ;<10, 20> A <;20, 30> W")]
        [TestCase("[ZAK;RES] <0, 30>; <10, 20>; A;")]
        [TestCase("[ZAKRES] <0, 30>; <10, 20>; A;;;;;")]
        [TestCase("[ZAKRES] <0; 30>; <10; 20>; A;")]
        [TestCase(";[ZAKRES] <0, 30>; <10, 20> A;")]
        [TestCase("[ZAKRES] <0, 30>; ;<10, 20> A; ;")]
        [TestCase("[ZAKRES] <0, ;30>; <10, ;20>; A;")]
        [TestCase("[ZAKRES] ;<0;, 30>; <10, 20>;; A;")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_TooManySeparators_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[TEKST] <0, 30>; <10, 20> A;")]
        [TestCase("[ZDJĘCIE] <0, 30>; <10, 20> A;")]
        [TestCase("[] <0, 30>; <10, 20> A;")]
        [TestCase("<0, 30>; <10, 20> A;")]
        [TestCase("] <0, 30>; <10, 20> A;")]
        [TestCase("[ <0, 30>; <10, 20> A;")]
        [TestCase("() <0, 30>; <10, 20> A;")]
        [TestCase("(ZAKRES) <0, 30>; <10, 20> A;")]
        [TestCase("ZAKRES <0, 30>; <10, 20> A;")]
        [TestCase("]TEKST[ <0, 30>; <10, 20> A;")]
        [TestCase("[TEKST] <0, 30>; <10, 20> A;")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_InvalidPrefix_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase(" ")]
        [TestCase("                            ")]
        [TestCase(" \t\n")]
        [TestCase(" ,./<>?;:''\"[]{}\\\'=+-_)(*&^%$#@!!~`")]
        [TestCase("[ZAKRES]")]
        [TestCase("<,>")]
        [TestCase("[ZAKRES] <,> A")]
        [TestCase("aaaaaaaaaaaaaa; aaaaaaaaaaaaaaaa; aaaaaaaaaaaaaaa")]
        [TestCase("[] <123> A")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_GeneralInvalidComment_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <0, 10>; <s, a> A")]
        [TestCase("[ZAKRES] <0, 10>; <s, 5> A")]
        [TestCase("[ZAKRES] <0, 10>; <5, a> A")]
        [TestCase("[ZAKRES] <a, 10>; <5, 6> A")]
        [TestCase("[ZAKRES] <0, b>; <5, 8> A")]
        [TestCase("[ZAKRES] <a, b>; <5, 8> A")]
        [TestCase("[ZAKRES] <a, b>; <5, a> A")]
        [TestCase("[ZAKRES] <c, 10>; <5, 7> A")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_MinMaxOrRangeNaN_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [TestCase("[ZAKRES] <1, 2, 3>; <5, 8> A")]
        [TestCase("[ZAKRES] <1, 10>; <5, 6, 7> A")]
        [TestCase("[ZAKRES] <1, 10>; <5, 7, 8> A")]
        public void
            ReturnStringFromSuwak_odczytWartosciXmlCode_MoreThenTwoValuesInRange_CreateDefaultXmlCode(string comment)
        {
            var question = new Question(0, "", "", "[Suwak (odczyt wartości)]", "", "", comment);

            string answer = "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                            "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                                    $"\t<Title></Title>\n" +
                                    "\t<Mark>\n" +
                                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                            "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                            "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                        "\t\t</Definition>\n" +
                                    "\t</Mark>\n" +
                                    "\t<NotEdited/>\n" +
                               "</InputSliderInt>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetBRAKUWAG_UwagiTekstCode_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[BRAK UWAG / uwagi --> Tekst]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>BRAK UWAG</Selection>\n" +
                            "\t\t<Selection>UWAGI</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>UWAGI</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    $"\t\t\t\t<Title>Podaj uwagi</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetBRAKUWAG_UwagiTekstCode_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[ZDJĘCIE] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[BRAK UWAG / uwagi --> Tekst]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>BRAK UWAG</Selection>\n" +
                            "\t\t<Selection>UWAGI</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>UWAGI</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    $"\t\t\t\t<Title>Podaj uwagi</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetBRAKUWAG_UwagiTekstCode_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[TEKST] Poprawny komentarz";
            var question = new Question(0, "", "", "[BRAK UWAG / uwagi --> Tekst]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>BRAK UWAG</Selection>\n" +
                            "\t\t<Selection>UWAGI</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>UWAGI</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    $"\t\t\t\t<Title>Poprawny komentarz</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGet_takZdjecieNIECode_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[tak --> Zdjęcie / NIE]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                            $"\t<Title></Title>\n" +
                            "\t<SelectionList>\n" +
                                "\t\t<Selection>TAK</Selection>\n" +
                                "\t\t<Selection>NIE</Selection>\n" +
                            "\t</SelectionList>\n" +
                            "\t<Inner>\n" +
                                "\t\t<OnValue>TAK</OnValue>\n" +
                                "\t\t<InnerInputs>\n" +
                                    "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                    "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                    $"\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                    "\t\t\t<NotEdited/>\n" +
                                "\t\t</InputImage>\n" +
                                "\t\t</InnerInputs>\n" +
                            "\t</Inner>\n" +
                            "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                    "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                          "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGet_takZdjecieNIECode_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[TEKST] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[tak --> Zdjęcie / NIE]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                            $"\t<Title></Title>\n" +
                            "\t<SelectionList>\n" +
                                "\t\t<Selection>TAK</Selection>\n" +
                                "\t\t<Selection>NIE</Selection>\n" +
                            "\t</SelectionList>\n" +
                            "\t<Inner>\n" +
                                "\t\t<OnValue>TAK</OnValue>\n" +
                                "\t\t<InnerInputs>\n" +
                                    "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                    "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                    $"\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                    "\t\t\t<NotEdited/>\n" +
                                "\t\t</InputImage>\n" +
                                "\t\t</InnerInputs>\n" +
                            "\t</Inner>\n" +
                            "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                    "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                          "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGet_takZdjecieNIECode_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[ZDJĘCIE] Poprawny komentarz";
            var question = new Question(0, "", "", "[tak --> Zdjęcie / NIE]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                            $"\t<Title></Title>\n" +
                            "\t<SelectionList>\n" +
                                "\t\t<Selection>TAK</Selection>\n" +
                                "\t\t<Selection>NIE</Selection>\n" +
                            "\t</SelectionList>\n" +
                            "\t<Inner>\n" +
                                "\t\t<OnValue>TAK</OnValue>\n" +
                                "\t\t<InnerInputs>\n" +
                                    "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                            $"\t\t\t<Title>Poprawny komentarz</Title>\n" +
                                    "\t\t\t<NotEdited/>\n" +
                                "\t\t</InputImage>\n" +
                                "\t\t</InnerInputs>\n" +
                            "\t</Inner>\n" +
                            "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                    "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                          "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetBRAKUWAGuwagiTekstZdjecieCode_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[BRAK UWAG / uwagi --> Tekst + zdjęcie]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>BRAK UWAG</Selection>\n" +
                            "\t\t<Selection>UWAGI</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>UWAGI</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<Title>Podaj uwagi</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                    "\t\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";


            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetBRAKUWAGuwagiTekstZdjecieCode_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[ZDJĘCIE] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[BRAK UWAG / uwagi --> Tekst + zdjęcie]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>BRAK UWAG</Selection>\n" +
                            "\t\t<Selection>UWAGI</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>UWAGI</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<Title>Podaj uwagi</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                    "\t\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetBRAKUWAGuwagiTekstZdjecieCode_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[TEKST] Poprawny komentarz.; [ZDJĘCIE] Druga część poprawnego komentarza.;";
            var question = new Question(0, "", "", "[BRAK UWAG / uwagi --> Tekst + zdjęcie]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>BRAK UWAG</Selection>\n" +
                            "\t\t<Selection>UWAGI</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>UWAGI</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<Title>Poprawny komentarz.</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                    "\t\t\t\t<Title>Druga część poprawnego komentarza.</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKnieZdjecieCode_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[TAK / nie --> Zdjęcie]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        "\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                "\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKnieZdjecieCode_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[TEKST] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[TAK / nie --> Zdjęcie]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        "\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                "\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKnieZdjecieCode_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[ZDJĘCIE] Poprawny komentarz";
            var question = new Question(0, "", "", "[TAK / nie --> Zdjęcie]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        "\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<Title>Poprawny komentarz</Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGet_takTekstNIECode_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[tak --> Tekst / NIE]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        "\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                    "\t\t\t\t<Title>Wprowadź tekst</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGet_takTekstNIECode_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[ZDJĘCIE] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[tak --> Tekst / NIE]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        "\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                    "\t\t\t\t<Title>Wprowadź tekst</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGet_takTekstNIECode_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[TEKST] Poprawny komentarz.";
            var question = new Question(0, "", "", "[tak --> Tekst / NIE]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        "\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<Title>Poprawny komentarz.</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }







        [Test]
        public void ReturnStringFromGetTAKnieTekstCode_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[TAK / nie --> Tekst]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                    $"\t\t\t\t<Title>Wprowadź tekst</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKnieTekstCode_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[ZDJĘCIE] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[TAK / nie --> Tekst]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                    $"\t\t\t\t<Title>Wprowadź tekst</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKnieTekstCode_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[TEKST] Poprawny komentarz";
            var question = new Question(0, "", "", "[TAK / nie --> Tekst]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                $"\t\t\t\t<Title>Poprawny komentarz</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }







        [Test]
        public void ReturnStringFromGetTAKZdjecie_nie_EmptyComment_DefaultXmlCode()
        {
            string comment = String.Empty;
            var question = new Question(0, "", "", "[TAK --> Zdjęcie / nie]", "", "", comment);

            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                "\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKZdjecie_nie_InvalidPrefix_DefaultXmlCode()
        {
            string comment = "[TEKST] Niepoprawny komentarz";
            var question = new Question(0, "", "", "[TAK --> Zdjęcie / nie]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA INNY NIŻ DOMYŚLNY________________-->\n" +
                                "\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }

        [Test]
        public void ReturnStringFromGetTAKZdjecie_nie_ValidComment_ReturnProperXmlCode()
        {
            string comment = "[ZDJĘCIE] Poprawny komentarz";
            var question = new Question(0, "", "", "[TAK --> Zdjęcie / nie]", "", "", comment);
            string answer = "<InputRadio required=\"true\">\n" +
                        $"\t<Title></Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<Title>Poprawny komentarz</Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                        "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";

            Assert.AreEqual(answer, question.GetQuestionCode());
        }
    }
}

