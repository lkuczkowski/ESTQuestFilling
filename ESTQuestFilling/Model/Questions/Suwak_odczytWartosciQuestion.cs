using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESTQuestFilling.Model.Questions
{
    class Suwak_odczytWartosciQuestion : QuestionBase
    {
        public Suwak_odczytWartosciQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {
            
        }

        public override string GetXmlCode()
        {
            string marksDefinitionXmlCode = "";
            string defaultMarksDefinitionXmlCode =
                "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n";
            string firstTwoLinesOfXmlCode = "";
            string defaultFirstTwoLinesOfXmlCode =
                "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                $"<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"{required}\">\n";

            if (Comment.StartsWith("[ZAKRES]"))
            {
                try
                {
                    var removedCommentTypeAndSplitToRanges = Comment.Substring(Comment.IndexOf(']') + 1).Trim()
                        .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToArray();
                    if (removedCommentTypeAndSplitToRanges.Length < 2)
                        throw new ArgumentException("No values to convert after [...] prefix.");

                    var interval = removedCommentTypeAndSplitToRanges.First().Split(new char[] { '<', '>', ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    if (!interval.All(n => int.TryParse(n, out _)) || interval.Length != 2)
                        throw new ArgumentException("Interval value is NaN or not a interval definition\n");

                    firstTwoLinesOfXmlCode =
                        $"<InputSliderInt minValue=\"{interval[0]}\" maxValue=\"{interval[1]}\" defaultValue=\"{interval[0]}\" required=\"{required}\">\n";

                    foreach (var rangeAndMark in removedCommentTypeAndSplitToRanges.Skip(1))
                    {
                        var rangeAndMarkArray = rangeAndMark
                            .Split(new char[] { '<', '>', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (rangeAndMarkArray.Length != 3)
                            throw new ArgumentException("Comment separators error.");

                        if (!rangeAndMarkArray.Take(2).All(n => int.TryParse(n, out _)))
                        {
                            throw new ArgumentException("Range value is NaN\n");
                        }

                        marksDefinitionXmlCode +=
                            $"\t\t\t<MarkDef rangeMin = \"{rangeAndMarkArray[0]}\" rangeMax = \"{rangeAndMarkArray[1]}\" " +
                            $"mark = \"{_marksAbbreviationsToXmlNamesDictionary[rangeAndMarkArray[2]]}\"/>\n";
                    }
                }
                catch (KeyNotFoundException e)
                {
                    firstTwoLinesOfXmlCode = defaultMarksDefinitionXmlCode;
                    marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;

                    MessageBox.Show(e.Message + "\n\n" +
                                    "Unable to convert comment due to mark abbreviation error.\n" +
                                    $"Default marks and remainder assigned to question ID: {DataBaseId}");
                }
                catch (Exception e)
                {
                    firstTwoLinesOfXmlCode = defaultFirstTwoLinesOfXmlCode;
                    marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;

                    MessageBox.Show(e.Message + "\n\n" +
                                    $"Unable to convert comment. Default marks and remainder assigned to question ID: {DataBaseId}");
                }
            }
            else
            {
                firstTwoLinesOfXmlCode = defaultFirstTwoLinesOfXmlCode;
                marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;
            }


            return firstTwoLinesOfXmlCode +
                    $"\t<Title>{QuestionText}</Title>\n" +
                    "\t<Mark>\n" +
                        _analyticsLink +
                        $"\t\t<Definition initialMark = \"warning\" refusalMark = \"{refusalMark}\">\n" +
                            marksDefinitionXmlCode +
                        "\t\t</Definition>\n" +
                    "\t</Mark>\n" +
                    "\t<NotEdited/>\n" +
                "</InputSliderInt>";
        }
    }
}
