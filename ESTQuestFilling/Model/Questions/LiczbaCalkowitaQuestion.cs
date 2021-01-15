using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESTQuestFilling.Model.Questions
{
    class LiczbaCalkowitaQuestion : QuestionBase
    {
        public override string GetXmlCode()
        {
            string marksDefinitionXmlCode = "";
            string defaultMarksDefinitionXmlCode =
                "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n";

            if (Comment.StartsWith("[ZAKRES]"))
            {
                try
                {
                    var removedCommentTypeAndSplitToRanges = Comment.Substring(Comment.IndexOf(']') + 1)
                        .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToArray();
                    if (!removedCommentTypeAndSplitToRanges.Any())
                        throw new ArgumentException("No values to convert after [...] prefix.");
                    foreach (var rangeAndMark in removedCommentTypeAndSplitToRanges)
                    {
                        var rangeAndMarkArray = rangeAndMark
                            .Split(new char[] { '<', '>', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                    marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;

                    MessageBox.Show(e.Message + "\n\n" +
                                    "Unable to convert comment due to mark abbreviation error.\n" +
                                    $"Default marks and remainder assigned to question ID: {DataBaseId}");
                }
                catch (Exception e)
                {
                    marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;

                    MessageBox.Show(e.Message + "\n\n" +
                                    $"Unable to convert comment. Default marks and remainder assigned to question ID: {DataBaseId}");
                }
            }
            else
            {
                marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;
            }

            return $"<InputInteger defaultValue=\"0\" required=\"{required}\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
                            $"\t\t<Definition initialMark = \"warning\" refusalMark = \"{refusalMark}\">\n" +
                                marksDefinitionXmlCode +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputInteger>";
        }
    }
}
