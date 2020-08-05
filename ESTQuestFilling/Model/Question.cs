using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Windows;

namespace ESTQuestFilling.Model
{
    [Table]
    public class Question
    {
        /// <summary>
        /// Question ID form database
        /// </summary>
        public int Id { get; private set; }

        private string _analyticsLink = "";
        private string _evaluation;
        public string Answer { get; }
        public string Number { get; }
        public string Tag { get; set; }
        public string Page { get; } = "";
        public string Comment { get; }

        public int[][] EvaluationTable { get; private set; }

        private void SetEvaluation(string marks)
        {
            string[] noAnalyticsIndicataors = {"", "brak", "brak;", "brak; "};
            _evaluation = marks;
            if (noAnalyticsIndicataors.Contains(marks))
            {
                EvaluationTable = new int[1][] { new int[] { 0, 0 } };
                _analyticsLink = "";
            }
            else
            {
                CreateEvaluateTable();
                _analyticsLink = "\t\t<AnalyticsLink>\n" +
                                 EvaluationTable.Aggregate("",
                                     (n, s) => n + $"\t\t\t<Factor refWeight = \"{s[1]}\" refId = \"{s[0]}\"/>\n") +
                                 "\t\t</AnalyticsLink>\n";
            }
        }

        public string QuestionText { get; }

        public Question(int id, string question)
        {
            Id = id;
            QuestionText = question;
        }

        public Question(int id, string question, string marks)
            : this(id, question)
        {
            SetEvaluation(marks);
        }

        public Question(int id, string question, string marks, string answer)
        : this(id, question, marks)
        {
            Answer = answer;
        }

        public Question(int id, string question, string marks, string answer, string number)
            : this(id, question, marks, answer)
        {
            Number = number;
        }

        public Question(int id, string question, string marks, string answer, string number, string page)
            : this(id, question, marks, answer, number)
        {
            Page = page;
        }

        public Question(int id, string question, string marks, string answer, string number, string page,
            string comment)
            : this(id, question, marks, answer, number, page)
        {
            Comment = comment;
        }

        private void CreateEvaluateTable()
        {
            var splitMarksStrings = _evaluation.Split(new char[] { ' ', })
                .Select(n => n.Trim(new char[] { ' ', ';' }).Split(new char[] { '-' })).ToArray();

            EvaluationTable = splitMarksStrings.Select(n => n.Select(Int32.Parse).ToArray()).ToArray();
        }

        // TODO - zastosować wzorzec lub dziedziczenie???
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
                            _analyticsLink +
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
                            _analyticsLink +
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
                            _analyticsLink +
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
                            _analyticsLink +
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
                                _analyticsLink +
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
                                    "\t\t\t\t<Title>Zrób zdjęcie</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputImage>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
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
                            _analyticsLink +
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
                            _analyticsLink +
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
                            _analyticsLink +
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
                                _analyticsLink +
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
                            _analyticsLink +
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
            return "<InputText required=\"false\">\n" +
                         $"\t<Title>{QuestionText}</Title>\n" +
                         "\t<Inner>\n" +
                             "\t\t<OnValue/>\n" +
                             "\t\t<InnerInputs>\n" +
                                 "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\">\n" +
                                     "\t\t\t\t<Title>Wykonaj zdjęcie dotyczące uwag (fakultatywne).</Title>\n" +
                                     "\t\t\t\t<NotEdited/>\n" +
                                 "\t\t\t</InputImage>\n" +
                                 "\t\t\t<InputImage allowCamera = \"true\" allowFile = \"false\">\n" +
                                     "\t\t\t\t<Title>Wykonaj dodatkowe zdjęcie dotyczące uwag (fakultatywne).</Title>\n" +
                                     "\t\t\t\t<NotEdited/>\n" +
                                 "\t\t\t</InputImage>\n" +
                             "\t\t</InnerInputs>\n" +
                         "\t</Inner>\n" +
                         "\t<Mark>\n" +
                             "\t\t<ByOperator expiredMark = \"normal\" initialMark = \"normal\" refusalMark = \"normal\"/>\n" +
                         "\t</Mark>\n" +
                         "\t<NotEdited/>\n" +
                 "</InputText>";
        }

        private string GetTekstCode()
        {
            return "<InputText required = \"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
                            "\t\t<ByOperator expiredMark = \"normal\" initialMark = \"normal\" refusalMark = \"alarm\"/>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
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
                            _analyticsLink +
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

        private string GetTAKZdjecie_nie()
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
                            _analyticsLink +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string GetLiczbaCalkowita()
        {
            return "<!--___________________WPROWADŹ WARTOŚCI: MIN, MAX, DEFAULT________________-->\n" +
                    "<InputInteger defaultValue=\"0\" required=\"true\">\n" +
                            $"\t<Title>{QuestionText}</Title>\n" +
                            "\t<Mark>\n" +
                                _analyticsLink +
                                "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                    "\t\t\t<!--___________________UZUPEŁNIJ PRZEDZIAŁY________________-->\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"alarm\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"warning\"/>\n" +
                                    "\t\t\t<MarkDef rangeMin = \"\" rangeMax = \"\" mark = \"normal\"/>\n" +
                                "\t\t</Definition>\n" +
                            "\t</Mark>\n" +
                            "\t<NotEdited/>\n" +
                       "</InputInteger>";
        }

        private string GetCode(string answerType)
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
                case "[Fakultatywne --> Tekst + zdjęcie]":
                    return GetFakultatywneTekstZdjecieCode();
                case "[Tekst]":
                    return GetTekstCode();
                case "[Lista rozwijana]":
                    return GetListaRozwijanaCode();
                case "[TAK --> Zdjęcie / nie]":
                    return GetTAKZdjecie_nie();
                case "[Liczba całkowita]":
                    return GetLiczbaCalkowita();
                default:
                    throw new InvalidOperationException("Switch statement error: no match expression");
            }
        }

        public string GetQuestionCode()
        {
            return GetCode(Answer);
        }
    }
}
