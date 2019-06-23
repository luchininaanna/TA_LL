using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lexer
{
    struct Rule
    {
        public string ruleName;
        public List<string> ruleСomposition;
        public List<string> guideSet;
    }

    struct GuideSet
    {
        public string ruleName;
        public List<string> set;
    }

    struct TerminalList
    {
        public string terminal;
        public List<int> index;
    }

    struct IndexOfTerminal
    {
        public string terminal;
        public List<int> startIndex;
        public List<int> rowIndex;
    }

    struct RowInTable
    {
        public string name;
        public bool isEnd;
        public List<string> guideSet;
        public int errorTransit;
        public bool isShift;
        public bool isStack;
        public int goTo;
    }

    class GrammarConverter
    {
        private const string TERMINAL = "terminal";
        private const string NOT_TERMINAL = "not terminal";
        private const string EMPTY = "empty";
        private const string END = "[end]";

        public GrammarConverter() { }

        private List<Rule> grammarList = new List<Rule>();
        private List<GuideSet> guideSet = new List<GuideSet>();
        private List<TerminalList> terminalList = new List<TerminalList>();
        private List<IndexOfTerminal> indexOfTerminalList = new List<IndexOfTerminal>();
        private List<RowInTable> table = new List<RowInTable>();

        public List<Rule> GetGrammarList()
        {
            return grammarList;
        }
        public List<TerminalList> GetTerminalList()
        {
            return terminalList;
        }
        public List<IndexOfTerminal> GetIndexOfTerminalList()
        {
            return indexOfTerminalList;
        }
        public List<RowInTable> GetGrammarTable()
        {
            return table;
        }

        public void SetTerminalIndexList(List<IndexOfTerminal> newIndexOfTerminalList)
        {
            indexOfTerminalList.Clear();
            foreach (IndexOfTerminal element in newIndexOfTerminalList)
            {
                indexOfTerminalList.Add(element);
            }
        }
        public void SetTerminalList(List<TerminalList> newTerminalList)
        {
            terminalList.Clear();
            foreach (TerminalList element in newTerminalList)
            {
                terminalList.Add(element);
            }
        }
        public void SetGrammarList(List<Rule> newGrammarList)
        {
            grammarList.Clear();

            foreach (Rule rule in newGrammarList)
            {
                grammarList.Add(rule);
            }
        }

        public void PrintTerminalList()
        {
            int amount1 = terminalList.Count;
            Console.WriteLine("Printing Terminal List...");
            Console.WriteLine("--benig");

            for (int i = 0; i < amount1; i++)
            {
                TerminalList t1 = terminalList[i];
                Console.Write(t1.terminal);
                Console.Write(" : ");

                int amount2 = t1.index.Count;

                for (int j = 0; j < amount2; j++)
                {
                    Console.Write(t1.index[j]);
                    Console.Write(", ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("--end");
            Console.WriteLine();
        }
        public void PrintGrammarList()
        {
            int amount1 = grammarList.Count;
            Console.WriteLine("Printing Grammar List...");
            Console.WriteLine("--benig");

            for (int i = 0; i < amount1; i++)
            {
                Rule t1 = grammarList[i];
                Console.Write(t1.ruleName);
                Console.Write(" -> ");

                int amount2 = t1.ruleСomposition.Count;

                for (int j = 0; j < amount2; j++)
                {
                    Console.Write(t1.ruleСomposition[j]);
                    Console.Write(" ");
                }
                Console.Write("   | ");

                int amount3 = t1.guideSet.Count;

                for (int k = 0; k < amount3; k++)
                {
                    Console.Write(t1.guideSet[k]);
                    Console.Write("  ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("--end");
            Console.WriteLine();
        }
        public void PrintGuideSet()
        {
            int amount1 = guideSet.Count;
            Console.WriteLine("Printing Guide Set...");
            Console.WriteLine("--benig");

            for (int i = 0; i < amount1; i++)
            {
                GuideSet t1 = guideSet[i];
                Console.Write(t1.ruleName);
                Console.Write(" : ");

                int amount2 = t1.set.Count;

                for (int j = 0; j < amount2; j++)
                {
                    Console.Write(t1.set[j]);
                    Console.Write(", ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("--end");
            Console.WriteLine();
        }
        public void PrintIndex()
        {
            int amount1 = indexOfTerminalList.Count;
            Console.WriteLine("Printing Index...");
            Console.WriteLine("--benig");

            for (int i = 0; i < amount1; i++)
            {
                IndexOfTerminal t1 = indexOfTerminalList[i];
                Console.Write(t1.terminal);
                Console.Write(" : ");

                int amount2 = t1.startIndex.Count;

                for (int j = 0; j < amount2; j++)
                {
                    Console.Write(t1.startIndex[j]);
                    Console.Write(", ");
                }

                Console.Write("  //  ");
                amount2 = t1.rowIndex.Count;

                for (int j = 0; j < amount2; j++)
                {
                    Console.Write(t1.rowIndex[j]);
                    Console.Write(", ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("--end");
            Console.WriteLine();
        }
        public void PrintTable()
        {
            Console.WriteLine(" name | isEnd | guideSet | errorTransit | isShift | isStack | goTo |");
            Console.WriteLine("--------------------------------------------------------------------");

            foreach (RowInTable row in table)
            {
                Console.Write(" " + row.name + "  |");
                Console.Write("  " + row.isEnd + "  |");

                foreach(string el in row.guideSet)
                {
                    Console.Write(el + ", ");
                }
                Console.Write("  |");

                Console.Write("  " + row.errorTransit + "  |");
                Console.Write("  " + row.isShift + "  |");
                Console.Write("  " + row.isStack + "  |");
                Console.WriteLine("  " + row.goTo + "  |");
                Console.WriteLine("--------------------------------------------------------------------");
            }

        }

        private void AddNotTerminalToList(string notTerminal, ref List<string> list)
        {
            if (!list.Contains(notTerminal))
                list.Add(notTerminal);
        }

        public void InitializeStartRule()
        {
            string terminal = grammarList[0].ruleName;

            Rule newRule;
            newRule.ruleName = "<Start>";

            List<string> currComposition = new List<string>();
            currComposition.Add(terminal);
            currComposition.Add("[end]");
            newRule.ruleСomposition = currComposition;
            newRule.guideSet = new List<string>();

            bool isExist = terminalList.Exists(x => x.terminal == terminal);
            if (isExist)
            {
                TerminalList currTerminalList = terminalList.Find(x => x.terminal.Contains(terminal));
                currTerminalList.index.Add(1);
            } else
            {
                TerminalList newTerminalList;
                newTerminalList.terminal = terminal;
                newTerminalList.index = new List<int>() { 1 };     
                terminalList.Add(newTerminalList);
            }

            IndexOfTerminal startIndexOfTerminal = new IndexOfTerminal();
            startIndexOfTerminal.terminal = newRule.ruleName;
            startIndexOfTerminal.rowIndex = new List<int>() { 0 };
            startIndexOfTerminal.startIndex = new List<int>() { 0 };
            indexOfTerminalList.Insert(0, startIndexOfTerminal);

            grammarList.Insert(0, newRule);
        }

        private void FindTerminalSet(string terminal, ref List<string> currSet, ref List<string> guideSetForRule)
        {
            bool isExist = guideSet.Exists(x => x.ruleName == terminal);

            if (isExist)
            {
                GuideSet additionSet = guideSet.Find(x => x.ruleName == terminal);
                int additionSetSize = additionSet.set.Count();

                for (int j = 0; j < additionSetSize; j++)
                {
                    AddNotTerminalToList(additionSet.set[j], ref currSet);
                    AddNotTerminalToList(additionSet.set[j], ref guideSetForRule);
                }
            }
        }

        private string DefineStringType(string currWord)
        {
            if ((currWord[0] == '<') && (currWord[currWord.Length - 1] == '>'))
            {
                return TERMINAL;
            }
            else if (currWord == "[empty]")
            {
                return EMPTY;
            }
            else
            {
                return NOT_TERMINAL;
            }
        }

        private void FindEmptyTerminalSet(string terminal, ref List<string> currSet, 
            ref List<string> guideSetForRule, ref bool isFirstRule)
        {
            TerminalList currTerminalList = terminalList.Find(x => x.terminal.Contains(terminal));
            int indexAmount = currTerminalList.index.Count();

            for (int i = 0; i < indexAmount; i++)
            {
                int currIndex = currTerminalList.index[i];
                Rule currRule = grammarList[currIndex];
                List<string> ruleСomposition = currRule.ruleСomposition;
                int elementIndex = ruleСomposition.FindIndex(x => x == terminal);

                if (elementIndex < ruleСomposition.Count() - 1)
                {
                    string nextElement = ruleСomposition[elementIndex + 1];
                    string newtElementType = DefineStringType(nextElement);

                    switch (newtElementType)
                    {
                        case TERMINAL:
                            FindTerminalSet(nextElement, ref currSet, ref guideSetForRule);
                            break;
                        case NOT_TERMINAL:
                            AddNotTerminalToList(nextElement, ref currSet);
                            AddNotTerminalToList(nextElement, ref guideSetForRule);
                            if (isFirstRule)
                            {
                                AddNotTerminalToList(END, ref currSet);
                                AddNotTerminalToList(END, ref guideSetForRule);
                                isFirstRule = false;
                            }
                            break;
                    }
                }
                else
                {
                    string rule = currRule.ruleName;

                    if (rule != terminal)
                    {
                        FindEmptyTerminalSet(rule, ref currSet, ref guideSetForRule, ref isFirstRule);
                    }
                    else
                    {
                        AddNotTerminalToList(END, ref currSet);
                        AddNotTerminalToList(END, ref guideSetForRule);
                    }
                }
            }
        }

        private void OperateRule(Rule rule, ref bool isFirstRule, ref GuideSet currGuideSet, 
            ref List<string> currSet, string currWord, ref string currName, ref bool isChanges)
        {
            if (rule.ruleName == grammarList[0].ruleName)
                isFirstRule = true;

            List<string> guideSetForRule = new List<string>();
            bool isNeedToSaveSet = (currName != "") && (currName != rule.ruleName);

            if ((isNeedToSaveSet) && (currSet.Count() != 0))
            {
                currGuideSet.set = currSet;
                string ruleName = currGuideSet.ruleName;
                bool isExist = guideSet.Exists(x => x.ruleName == ruleName);
                if (!isExist)
                    guideSet.Add(currGuideSet);
            }

            if (currName != rule.ruleName)
            {
                currName = rule.ruleName;
                currGuideSet.ruleName = currName;
                currSet = new List<string>();
            }

            currWord = rule.ruleСomposition[0];
            string wordType = DefineStringType(currWord);

            switch (wordType)
            {
                case TERMINAL:
                    FindTerminalSet(currWord, ref currSet, ref guideSetForRule);
                    isFirstRule = false;
                    break;
                case EMPTY:
                    FindEmptyTerminalSet(rule.ruleName, ref currSet, ref guideSetForRule, ref isFirstRule);
                    break;
                case NOT_TERMINAL:
                    AddNotTerminalToList(currWord, ref currSet);
                    AddNotTerminalToList(currWord, ref guideSetForRule);
                    isFirstRule = false;
                    break;
            }

            int before = rule.guideSet.Count();
            int guideSetForRuleSize = guideSetForRule.Count();

            for (int i = 0; i < guideSetForRuleSize; i++)
                AddNotTerminalToList(guideSetForRule[i], ref rule.guideSet);

            int after = rule.guideSet.Count();
 
            if (before != after)
                isChanges = true;
        }

        public void DefineGuideSet() {

            string currName = "";
            string currWord = "";
            GuideSet currGuideSet = new GuideSet();
            List<string> currSet = new List<string>();
            bool isFirstRule = false;
            bool isChanges = true;

            while (isChanges)
            {
                isChanges = false;

                foreach (Rule rule in grammarList)
                    OperateRule(rule, ref isFirstRule, ref currGuideSet, ref currSet, currWord, ref currName, ref isChanges);

                if ((currName != "") && (currSet.Count != 0))
                {
                    currGuideSet.set = currSet;
                    string ruleName = currGuideSet.ruleName;
                    bool isExist = guideSet.Exists(x => x.ruleName == ruleName);
                    if (!isExist)
                        guideSet.Add(currGuideSet);
                }
            }
        }


        private void AddNameToRow(ref RowInTable newRow, string element)
        {
            newRow.name = element;
        }

        private void AddIsEndToRow(ref RowInTable newRow, Rule currRule, string element)
        {
            newRow.isEnd = false;

            bool isEndInMainRule = (element == END) && (currRule.ruleName == "<Start>");
            if (isEndInMainRule)
                newRow.isEnd = true;
        }

        private void AddGuideSetToRow(ref RowInTable newRow, Rule currRule, string element)
        {
            string elementType = DefineStringType(element);

            switch (elementType)
            {
                case TERMINAL:
                    newRow.guideSet = guideSet.Find(x => x.ruleName == element).set;
                    break;
                case NOT_TERMINAL:
                    newRow.guideSet = new List<string>() { element };
                    break;
                case EMPTY:
                    newRow.guideSet = currRule.guideSet;
                    break;
            }
        }

        private void AddErrorTransit(ref RowInTable newRow, Rule currRule, string element, int position, int row)
        {
            int elementIndex = currRule.ruleСomposition.FindIndex(x => x == element);
            int elementAmount = currRule.ruleСomposition.Count();
            bool isFirstElement = (elementIndex == 0);

            List<int> currList = indexOfTerminalList.Find(x => x.terminal == currRule.ruleName).rowIndex;
            int indexInTerminalList = currList.FindIndex(x => x == row);
            bool isLastRuleForTerminate = (indexInTerminalList == currList.Count() - 1);

            if (isFirstElement && !isLastRuleForTerminate)
                newRow.errorTransit = position + elementAmount;
        }

        private void AddIsShiftToRow(ref RowInTable newRow, Rule currRule, string element)
        {
            newRow.isShift = false;

            string elementType = DefineStringType(element);
            if (elementType == NOT_TERMINAL)
                newRow.isShift = true;
        }

        private void AddIsStackToRow(ref RowInTable newRow, Rule currRule, string element)
        {
            newRow.isStack = false;

            string elementType = DefineStringType(element);
            int elementIndex = currRule.ruleСomposition.FindIndex(x => x == element);
            int elementAmount = currRule.ruleСomposition.Count();
            bool isEndInComPosition = (elementIndex == (elementAmount - 1));

            if (!isEndInComPosition && (elementType == TERMINAL))
                newRow.isStack = true;
        }

        private void AddGoToToRow(ref RowInTable newRow, Rule currRule, string element, int position)
        {
            string elementType = DefineStringType(element);
            int elementIndex = currRule.ruleСomposition.FindIndex(x => x == element);
            int elementAmount = currRule.ruleСomposition.Count();
            bool isEndInComPosition = (elementIndex == (elementAmount - 1));

            if ((elementType == NOT_TERMINAL) && (!isEndInComPosition))
                newRow.goTo = position + 1;

            if (elementType == TERMINAL)
            {
                List<int> currList = indexOfTerminalList.Find(x => x.terminal == element).startIndex;
                newRow.goTo = currList[0];
            }
        }

        public List<RowInTable> CreateTable()
        {
            int position = 0;
            int row = 0;

            foreach (Rule currRule in grammarList)
            {
                foreach (string element in currRule.ruleСomposition)
                {
                    RowInTable newRow = new RowInTable();
       
                    AddNameToRow(ref newRow, element);
                    AddIsEndToRow(ref newRow, currRule, element);
                    AddGuideSetToRow(ref newRow, currRule, element);
                    AddErrorTransit(ref newRow, currRule, element, position, row);
                    AddIsShiftToRow(ref newRow, currRule, element);
                    AddIsStackToRow(ref newRow, currRule, element);
                    AddGoToToRow(ref newRow, currRule, element, position);
                    position++;

                    table.Add(newRow);
                }
                row++;
            }
            return table;
        }
    }
}