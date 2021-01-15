using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESTQuestFilling.Model.Questions
{
    class ListaRozwijanaQuestion : QuestionBase
    {
        public ListaRozwijanaQuestion(int dataBaseId = 0, string question = "", string analytics = "", string answer = "", string number = "", string page = "", string comment = "")
            : base(dataBaseId, question, analytics, answer, number, page, comment)
        {
            
        }

        public override string GetXmlCode()
        {
            string selectionFieldsXmlCode = "";
            string defaultSelectionFieldsXMLCode =
                "\t<!--___________________WPROWADŹ POLA WYBORU________________-->\n" +
                    "\t\t<Selection></Selection>\n" +
                    "\t\t<Selection></Selection>\n" +
                    "\t\t<Selection></Selection>\n";

            string marksDefinitionXmlCode = "";
            string defaultMarksDefinitionXmlCode =
                "\t\t<!--___________________WPROWADŹ OCENY DLA PÓL WYBORU________________-->\n" +
                    "\t\t\t<MarkDef mark = \"normal\"></MarkDef>\n" +
                    "\t\t\t<MarkDef mark = \"warning\"></MarkDef>\n" +
                    "\t\t\t<MarkDef mark = \"alarm\"></MarkDef>\n";

            if (Comment.StartsWith("[LISTA]"))
            {
                try
                {
                    var removedCommentTypeAndSplitToSelections = Comment.Substring(Comment.IndexOf(']') + 1).Trim()
                        .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToArray();

                    foreach (var selectorAndMark in removedCommentTypeAndSplitToSelections)
                    {
                        var selectorAndMarkArray = selectorAndMark.Split(new char[] { '<', '>' },
                            StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToArray();
                        if (selectorAndMarkArray.Length != 2)
                            throw new ArgumentException("Comment separators error.");

                        selectionFieldsXmlCode += $"\t\t<Selection>{selectorAndMarkArray[0]}</Selection>\n";
                        marksDefinitionXmlCode += $"\t\t\t<MarkDef mark = \"{_marksAbbreviationsToXmlNamesDictionary[selectorAndMarkArray[1]]}\">{selectorAndMarkArray[0]}</MarkDef>\n";
                    }
                }
                catch (KeyNotFoundException e)
                {

                    selectionFieldsXmlCode = defaultSelectionFieldsXMLCode;
                    marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;
                    MessageBox.Show(e.Message + "\n\n" +
                                    "Unable to convert comment due to mark abbreviation error.\n" +
                                    $"Default marks and remainder assigned to question ID: {DataBaseId}");
                }
                catch (Exception e)
                {
                    selectionFieldsXmlCode = defaultSelectionFieldsXMLCode;
                    marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;
                    MessageBox.Show(e.Message + "\n\n" +
                                    $"Unable to convert comment. Default marks and remainder assigned to question ID: {DataBaseId}");
                }

            }
            else
            {
                selectionFieldsXmlCode = defaultSelectionFieldsXMLCode;
                marksDefinitionXmlCode = defaultMarksDefinitionXmlCode;
            }

            return $"<InputCombo required=\"{required}\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                         selectionFieldsXmlCode +
                        "\t</SelectionList>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
                            $"\t\t<Definition initialMark = \"warning\" refusalMark = \"{refusalMark}\">\n" +
                                marksDefinitionXmlCode +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputCombo>";
        }
    }
}
