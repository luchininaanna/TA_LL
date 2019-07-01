using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace lexer
{
    class LeftRecursion
    {
        public LeftRecursion() { }

        private const string RULE_NAME = "LR";
        private int ruleIndex = 1;

        private List<Rule> newGrammarList = new List<Rule>();
        private List<IndexOfTerminal> newIndexOfTerminal = new List<IndexOfTerminal>();
        private List<TerminalList> newTerminalList = new List<TerminalList>();
        private List<string> rulesInNewGrammar = new List<string>();

        public void ApplyLeftRecursion(ref GrammarConverter grammarConverter)
        {
            List<Rule> grammarList = grammarConverter.GetGrammarList();
            for (int i = 0; i < grammarList.Count(); i++)
            {
                Rule rule = grammarList[i];
                string ruleName = rule.ruleName;
                string firstComponent = rule.ruleСomposition[0];

                if (ruleName == firstComponent)
                {
                    rulesInNewGrammar.Add(ruleName);
                    DeleteLeftRecursion(i, ruleName, ref grammarConverter);
                }
            }

            AddRulesWithoutRecursion(ref grammarConverter);
            grammarConverter.SetGrammarList(newGrammarList);
            UpdateTerminalIndexList();
            grammarConverter.SetTerminalIndexList(newIndexOfTerminal);
            UpdateTerminalList();
            grammarConverter.SetTerminalList(newTerminalList);
        }

        private void AddRulesWithoutRecursion(ref GrammarConverter grammarConverter)
        {
            List<IndexOfTerminal> indexOfTerminal = grammarConverter.GetIndexOfTerminalList();
            List<Rule> grammarList = grammarConverter.GetGrammarList();

            foreach (IndexOfTerminal terminalInfo in indexOfTerminal)
            {
                if (!rulesInNewGrammar.Contains(terminalInfo.terminal))
                {
                    List<int> list = terminalInfo.rowIndex;
                    foreach(int index in list)
                    {
                        if (terminalInfo.terminal == "<Start>")
                            newGrammarList.Insert(0, grammarList[index]);
                        else
                            newGrammarList.Add(grammarList[index]);
                    }
                }
            }
        }

        private void DeleteLeftRecursion(int index, string currRuleName, ref GrammarConverter grammarConverter)
        {
            List<Rule> grammarList = grammarConverter.GetGrammarList();
            List <IndexOfTerminal> currIndexOfTerminal = grammarConverter.GetIndexOfTerminalList();
            IndexOfTerminal currRuleList = currIndexOfTerminal.Find(x => x.terminal == currRuleName);
            List<int> rowList = currRuleList.rowIndex;
            string newRule = "<" + RULE_NAME + ruleIndex + ">";

            Rule rule;
            foreach (int row in rowList)
            {
                if (row != index) {
                    rule = new Rule();
                    rule.ruleName = currRuleName;
                    rule.guideSet = new List<string>();
                    rule.ruleСomposition = new List<string>();

                    foreach (string element in grammarList[row].ruleСomposition)
                        if(element != "[empty]")
                            rule.ruleСomposition.Add(element);

                    rule.ruleСomposition.Add(newRule);

                    newGrammarList.Add(rule);
                }
            }

            rule = new Rule();
            rule.ruleName = newRule;
            rule.guideSet = new List<string>();
            rule.ruleСomposition = new List<string> { "[empty]" };
            newGrammarList.Add(rule);

            rule = new Rule();
            rule.ruleName = newRule;
            rule.guideSet = new List<string>();
            rule.ruleСomposition = new List<string> ();

            List<string> composition = grammarList[index].ruleСomposition;
            for (int i = 1; i < composition.Count(); i++)
                rule.ruleСomposition.Add(composition[i]);

            rule.ruleСomposition.Add(newRule);
            newGrammarList.Add(rule);

            ruleIndex++;
        }

        private void UpdateTerminalIndexList()
        {
            int pos = 0;
            for (int i = 0; i < newGrammarList.Count; i++)
            {
                bool isExist = newIndexOfTerminal.Exists(x => x.terminal == newGrammarList[i].ruleName);
                if (!isExist)
                {
                    IndexOfTerminal currIndexOfTerminal;
                    currIndexOfTerminal.terminal = newGrammarList[i].ruleName;

                    List<int> startIndex = new List<int>();
                    startIndex.Add(pos);
                    currIndexOfTerminal.startIndex = startIndex;

                    List<int> rowIndex = new List<int>();
                    rowIndex.Add(i);
                    currIndexOfTerminal.rowIndex = rowIndex;

                    newIndexOfTerminal.Add(currIndexOfTerminal);
                }
                else
                {
                    IndexOfTerminal currIndexOfTerminal = newIndexOfTerminal.Find(x => x.terminal == newGrammarList[i].ruleName);
                    currIndexOfTerminal.startIndex.Add(pos);
                    currIndexOfTerminal.rowIndex.Add(i);
                }

                pos += newGrammarList[i].ruleСomposition.Count();
            }



        }

        private void UpdateTerminalList()
        {
            for (int i = 0; i < newGrammarList.Count; i++)
            {
                Rule currRule = newGrammarList[i];
                List<string> composition = currRule.ruleСomposition;

                foreach (string element in composition)
                {
                    bool isTerminal = (element[0] == '<') && (element[element.Length - 1] == '>');
                    if (isTerminal)
                    {
                        bool isExist = newTerminalList.Exists(x => x.terminal == element);
                        if (!isExist)
                        {
                            TerminalList newTerninal;
                            newTerninal.terminal = element;
                            newTerninal.index = new List<int>();
                            newTerninal.index.Add(i);
                            newTerminalList.Add(newTerninal);
                        }
                        else
                        {
                            TerminalList currTerminalList = newTerminalList.Find(x => x.terminal.Contains(element));
                            currTerminalList.index.Add(i);
                        }
                    }
                }

            }
        }
    }
}

//!Предполагается, что из саморекурсивного нетерминала следует максимум 2 альтернативы