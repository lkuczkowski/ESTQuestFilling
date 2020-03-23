using System;
using System.Linq;
using System.Windows;

namespace ESTQuestFilling.Model
{
    public class Question
    {
        private string _evaluation;
        public string Answer { get; }
        public string Number { get; }
        public string Tag { get; set; }

        public int[][] EvaluationTable { get; private set; }


        // TODO - sprawdzać inne przypadku np.: "brak;" oraz pusty string 
        private void SetEvaluation(string marks)
        {
            _evaluation = marks;
            if (marks != "brak")
            {
                CreateEvaluateTable();
            }
            else
            {
                EvaluationTable = new int[1][] { new int[] { 0, 0 } };
            }
        }

        public string QuestionText { get; }

        public Question(string question)
        {
            QuestionText = question;
        }

        public Question(string question, string marks)
            : this(question)
        {
            SetEvaluation(marks);
        }

        public Question(string question, string marks, string answer)
        : this(question, marks)
        {
            Answer = answer;
        }

        public Question(string question, string marks, string answer, string number)
            : this(question, marks, answer)
        {
            Number = number;
        }

        private void CreateEvaluateTable()
        {
            var splitMarksStrings = _evaluation.Split(new char[] { ' ', })
                .Select(n => n.Trim(new char[] { ' ', ';' }).Split(new char[] { '-' })).ToArray();

            EvaluationTable = splitMarksStrings.Select(n => n.Select(Int32.Parse).ToArray()).ToArray();
        }


        // TODO - czy można jakoś uprościć teksty z kodem (fragment z oceną)???

        // TODO - dodać obsługę pola z komentarzem z bazy danych
        private string GetTAKnieCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string GetOcenaEksperckaCode()
        {
            return "<InputImage allowCamera=\"true\" allowFile=\"false\" required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<ByOperator expiredMark=\"warning\" initialMark=\"normal\" refusalMark=\"alarm\"/>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputImage>";
        }

        private string GetBRAKUWAG_UwagiTekstCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
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
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("",
                                (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string GetCiagloscZachowanaUszkodzenieOknoZUwagamiCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>Ciągłość zachowana</Selection>\n" +
                            "\t\t<Selection>Uszkodzenie</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>Uszkodzenie</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<Title>Podaj uwagi</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("",
                                (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">Ciągłość zachowana</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">Uszkodzenie</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string Get_takZdjecieNIECode()
        {
            return "<InputRadio required=\"true\">\n" +
                            $"\t<Title>{QuestionText}</Title>\n" +
                            "\t<SelectionList>\n" +
                                "\t\t<Selection>TAK</Selection>\n" +
                                "\t\t<Selection>NIE</Selection>\n" +
                            "\t</SelectionList>\n" +
                            "\t<Inner>\n" +
                                "\t\t<OnValue>TAK</OnValue>\n" +
                                "\t\t<InnerInputs>\n" +
                                    "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                    "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA________________-->\n" +
                                    "\t\t\t<Title></Title>\n" +
                                    "\t\t\t<NotEdited/>\n" +
                                "\t\t</InputImage>\n" +
                                "\t\t</InnerInputs>\n" +
                            "\t</Inner>\n" +
                            "\t<Mark>\n" +
                                "\t\t<AnalyticsLink>\n" +
                                    EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                                "\t\t</AnalyticsLink>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                    "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                          "</InputRadio>";
        }

        private string GetBRAKUWAGuwagiTekstZdjecieCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
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
                                    "<\t\t\t\tTitle>Zrób zdjęcie</Title>\n" +
                                    "<\t\t\t\tNotEdited/>\n" +
                                "\t\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("",
                                (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">BRAK UWAG</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">UWAGI</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string Get_takNIECode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }
        private string GetTAKnieZdjecieCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\" required = \"true\">\n" +
                                "\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA________________-->\n" +
                                "\t\t\t<Title></Title>\n" +
                                "\t\t\t<NotEdited/>\n" +
                            "\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string Get_takTekstNIECode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>TAK</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA________________-->\n" +
                                    "\t\t\t\t<Title></Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string GetSuwak_odczytWartosciCode()
        {
            return "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                    "<InputSliderInt minValue=\"\" maxValue=\"\" defaultValue=\"\" required=\"true\">\n" +
                            $"\t<Title>{QuestionText}</Title>\n" +
                            "\t<Mark>\n" +
                                "\t\t<AnalyticsLink>\n" +
                                    EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                                "\t\t</AnalyticsLink>\n" +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                       "</InputSliderInt>";
        }

        private string GetTAKnieTekstCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue>NIE</OnValue>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"true\">\n" +
                                    "\t\t\t\t<!--___________________WPROWADŹ TYTUŁ POLA________________-->\n" +
                                    "\t\t\t\t<Title></Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string GetFakultatywneTekstZdjecieCode()
        {
           return "<InputText required=\"false\">" +
                        $"\t<Title>{QuestionText}</Title>" +
                        "\t<Inner>" +
                            "\t\t<OnValue/>" +
                            "\t\t<InnerInputs>" +
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\">" +
                                    "\t\t\t\t<Title>Wykonaj zdjęcie dotyczące uwag (fakultatywne).</Title>" +
                                    "\t\t\t\t<NotEdited/>" +
                                "\t\t\t</InputImage>" + 
                                "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\">" +
                                    "\t\t\t\t<Title>Wykonaj dodatkowe zdjęcie dotyczące uwag (fakultatywne).</Title>" +
                                    "\t\t\t\t<NotEdited/>" +
                                "\t\t\t</InputImage>" +
                            "\t\t</InnerInputs>" +
                        "\t</Inner>" +
                        "\t<Mark>" +
                            "\t\t<ByOperator expiredMark = \"normal\" initialMark = \"normal\" refusalMark = \"normal\"/>" +
                        "\t</Mark>" +
                        "\t<NotEdited/>" +
                "</InputText>";
        }

        private string GetTekstCode()
        {
            return "<InputText required = \"true\">" +
                        $"\t<Title>{QuestionText}</Title>" +
                        "\t<Mark>" +
                        "   \t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<ByOperator expiredMark = \"normal\" initialMark = \"normal\" refusalMark = \"alarm\"/>" +
                        "\t</Mark>" +
                        "\t<NotEdited/>" +
                   "</InputText>";
        }

        private string GetListaRozwijanaCode()
        {
            return "<InputCombo required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                        "\t<!--___________________WPROWADŹ POLA WYBORU________________-->\n" +
                            "\t\t<Selection></Selection>\n" +
                            "\t\t<Selection></Selection>\n" +
                            "\t\t<Selection></Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Mark>\n" +
                            "\t\t<AnalyticsLink>\n" +
                                EvaluationTable.Aggregate("", (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                            "\t\t</AnalyticsLink>\n" +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                            "\t\t<!--___________________WPROWADŹ OCENY DLA PÓL WYBORU________________-->\n" +
                                "\t\t\t<MarkDef mark = \"normal\"></MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"warning\"></MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\"></MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputCombo>";
        }

        private string GetCode(string answerType)
        {
            try
            {
                switch (answerType)
                {
                    case "[TAK / nie]":
                        return GetTAKnieCode();
                    case "[Ocena ekspercka]":
                        return GetOcenaEksperckaCode();
                    case "[BRAK UWAG / uwagi --> Tekst]":
                        return GetBRAKUWAG_UwagiTekstCode();
                    case "[Ciągłość zachowana / Uszkodzenie --> Tekst]":
                        return GetCiagloscZachowanaUszkodzenieOknoZUwagamiCode();
                    case "[tak --> Zdjęcie / NIE]":
                        return Get_takZdjecieNIECode();
                    case "[BRAK UWAG / uwagi --> Tekst + zdjęcie]":
                        return GetBRAKUWAGuwagiTekstZdjecieCode();
                    case "[tak / NIE]":
                        return Get_takNIECode();
                    case "[TAK / nie --> Zdjęcie]":
                        return GetTAKnieZdjecieCode();
                    case "[tak --> Tekst / NIE]":
                        return Get_takTekstNIECode();
                    case "[Suwak (odczyt wartości)]":
                        return GetSuwak_odczytWartosciCode();
                    case "[TAK / nie --> Tekst]":
                        return GetTAKnieTekstCode();
                    case "[Fakultatywne --> Tekst + zdjęcie":
                        return GetFakultatywneTekstZdjecieCode();
                    case "[Tekst]":
                        return GetTekstCode();
                    case "[Lista rozwijana]":
                        return GetListaRozwijanaCode();
                    default:
                        throw new NotImplementedException("Switch statement error");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public string GetQuestionCode()
        {
            return GetCode(Answer);
        }
    }
}
