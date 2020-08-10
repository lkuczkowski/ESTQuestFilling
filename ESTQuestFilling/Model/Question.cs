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
        public string Page { get; set; } = "";

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

        private void CreateEvaluateTable()
        {
            var splitMarksStrings = _evaluation.Split(new char[] { ' ', })
                .Select(n => n.Trim(new char[] { ' ', ';' }).Split(new char[] { '-' })).ToArray();

            EvaluationTable = splitMarksStrings.Select(n => n.Select(Int32.Parse).ToArray()).ToArray();
        }

        // TODO - zastosować wzorzec lub dziedziczenie???
        // TODO - dodać obsługę pola z komentarzem z bazy danych
        private string GetTAK_nieXmlCode()
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

        private string GetOcenaEksperckaXmlCode()
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

        private string GetBRAKUWAG_uwagiTekstXmlCode()
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

        private string GetCiagloscZachowanaUszkodzenieTekstXmlCode()
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

        private string Get_takZdjecieNIE_XmlCode()
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

        private string GetBRAKUWAG_uwagiTekstZdjecieXmlCode()
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

        private string Get_takNIE_XmlCode()
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
        private string GetTAK_nieZdjecieXmlCode()
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

        private string Get_takTekstNIE_XmlCode()
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

        private string GetSuwak_odczytWartosciXmlCode()
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

        private string GetTAK_nieTekstXmlCode()
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

        private string GetFakultatywneTekstZdjecieXmlCode()
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

        private string GetTekstOcenaEksperckaXmlCode()
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

        private string GetListaRozwijanaXmlCode()
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

        private string GetTAK_Zdjecie_nieXmlCode()
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

        private string GetLiczbaCalkowitaXmlCode()
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

        private string GetTAK_nieND_TekstXmlCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                            "\t\t<Selection>N/D</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue operand=\"exists\"/>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"false\">\n" +
                                    "\t\t\t\t<Title>Podaj uwagi (fakultatywne).</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"normal\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">NIE</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">N/D</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string Get_takNIE_ND_TekstXmlCode()
        {
            return "<InputRadio required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<SelectionList>\n" +
                            "\t\t<Selection>TAK</Selection>\n" +
                            "\t\t<Selection>NIE</Selection>\n" +
                            "\t\t<Selection>N/D</Selection>\n" +
                        "\t</SelectionList>\n" +
                        "\t<Inner>\n" +
                            "\t\t<OnValue operand=\"exists\"/>\n" +
                            "\t\t<InnerInputs>\n" +
                                "\t\t\t<InputText required = \"false\">\n" +
                                    "\t\t\t\t<Title>Podaj uwagi (fakultatywne).</Title>\n" +
                                    "\t\t\t\t<NotEdited/>\n" +
                                "\t\t\t</InputText>\n" +
                            "\t\t</InnerInputs>\n" +
                        "\t</Inner>\n" +
                        "\t<Mark>\n" +
                            _analyticsLink +
                            "\t\t<Definition initialMark = \"warning\" refusalMark = \"alarm\">\n" +
                                "\t\t\t<MarkDef mark = \"alarm\">TAK</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">NIE</MarkDef>\n" +
                                "\t\t\t<MarkDef mark = \"normal\">N/D</MarkDef>\n" +
                            "\t\t</Definition>\n" +
                        "\t</Mark>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputRadio>";
        }

        private string GetData_bez_ocenyXmlCode()
        {
            return "<InputDate required = \"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputDate>";
        }

        private string GetTekstXmlCode()
        {
            return "<InputText required = \"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputText>";
        }

        private string GetZdjecieXmlCode()
        {
            return "<InputImage allowCamera=\"true\" allowFile=\"false\" required=\"true\">\n" +
                        $"\t<Title>{QuestionText}</Title>\n" +
                        "\t<NotEdited/>\n" +
                   "</InputImage>";
        }

        private string GetCode(string answerType)
        {
            switch (answerType)
            {
                case "[TAK / nie]":
                    return GetTAK_nieXmlCode();
                case "[Ocena ekspercka]":
                    return GetOcenaEksperckaXmlCode();
                case "[BRAK UWAG / uwagi --> Tekst]":
                    return GetBRAKUWAG_uwagiTekstXmlCode();
                case "[Ciągłość zachowana / Uszkodzenie --> Tekst]":
                    return GetCiagloscZachowanaUszkodzenieTekstXmlCode();
                case "[tak --> Zdjęcie / NIE]":
                    return Get_takZdjecieNIE_XmlCode();
                case "[BRAK UWAG / uwagi --> Tekst + zdjęcie]":
                    return GetBRAKUWAG_uwagiTekstZdjecieXmlCode();
                case "[tak / NIE]":
                    return Get_takNIE_XmlCode();
                case "[TAK / nie --> Zdjęcie]":
                    return GetTAK_nieZdjecieXmlCode();
                case "[tak --> Tekst / NIE]":
                    return Get_takTekstNIE_XmlCode();
                case "[Suwak (odczyt wartości)]":
                    return GetSuwak_odczytWartosciXmlCode();
                case "[TAK / nie --> Tekst]":
                    return GetTAK_nieTekstXmlCode();
                case "[Fakultatywne --> Tekst + zdjęcie]":
                    return GetFakultatywneTekstZdjecieXmlCode();
                case "[Tekst (ocena ekspercka)]":
                    return GetTekstOcenaEksperckaXmlCode();
                case "[Lista rozwijana]":
                    return GetListaRozwijanaXmlCode();
                case "[TAK --> Zdjęcie / nie]":
                    return GetTAK_Zdjecie_nieXmlCode();
                case "[Liczba całkowita]":
                    return GetLiczbaCalkowitaXmlCode();
                case "[TAK / nie / ND / --> Tekst]":
                    return GetTAK_nieND_TekstXmlCode();
                case "[tak / NIE / ND / --> Tekst]":
                    return Get_takNIE_ND_TekstXmlCode();
                case "[Data (bez oceny)]":
                    return GetData_bez_ocenyXmlCode();
                case "[Tekst]":
                    return GetTekstXmlCode();
                case "[Zdjęcie]":
                    return GetZdjecieXmlCode();
                default:
                    throw new InvalidOperationException("Unable to get XML code for used answer type.\nSwitch statement error: no match expression");
            }
        }

        public string GetQuestionCode()
        {
            return GetCode(Answer);
        }
    }
}
