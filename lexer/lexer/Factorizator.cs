using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lexer
{

    struct DataForFactorization
    {
        public string rule;
        public string commonElement;
        public List<int> indexList;
    }

    class Factorizator
    {
        public Factorizator() { }

        private const string ruleName = "FKTR";
        private int ruleIndex = 1;
        private bool isNeedToRepeatFactorization = true;

        private List<DataForFactorization> dataForFacotization = new List<DataForFactorization>();
        private List<Rule> newGrammarList = new List<Rule>();
        private List<IndexOfTerminal> newIndexOfTerminal = new List<IndexOfTerminal>();
        private List<TerminalList> newTerminalList = new List<TerminalList>();

        public void ApplyFactorization(ref GrammarConverter grammarConverter)
        {

            while (isNeedToRepeatFactorization)
            {
                isNeedToRepeatFactorization = false;

                List<Rule> grammarList = grammarConverter.GetGrammarList();
                List<IndexOfTerminal> indexOfTerminalList = grammarConverter.GetIndexOfTerminalList();

                for (int i = 0; i < indexOfTerminalList.Count(); i++)
                {
                    IndexOfTerminal currList = indexOfTerminalList[i];

                    ToExstractFirstComponents(ref grammarList, ref currList);
                }

                grammarConverter.SetGrammarList(newGrammarList);

                UpdateTerminalIndexList();
                grammarConverter.SetTerminalIndexList(newIndexOfTerminal);

                UpdateTerminalList();
                grammarConverter.SetTerminalList(newTerminalList);


                newGrammarList.Clear();
                newIndexOfTerminal.Clear();
                newTerminalList.Clear();
            }

        }

        private void ToExstractFirstComponents(ref List<Rule> grammarList, ref IndexOfTerminal currList)
        {
            for (int i = 0; i < currList.rowIndex.Count(); i++)
            {
                int currIndex = currList.rowIndex[i];

                Rule currRule = grammarList[currIndex];

                List<string> ruleСomposition = currRule.ruleСomposition;

                string firstComponent = ruleСomposition[0];
                
                bool isExist = dataForFacotization.Exists(x => x.commonElement == firstComponent);

                if (isExist)
                {
                    //поиск
                    DataForFactorization currData = dataForFacotization.Find(x => x.commonElement == firstComponent);
                    currData.indexList.Add(currIndex);
                } else
                {
                    //создание
                    DataForFactorization currData = new DataForFactorization();
                    currData.rule = currRule.ruleName;
                    currData.commonElement = firstComponent;
                    currData.indexList = new List<int> { currIndex };
                    dataForFacotization.Add(currData);
                }
                
                
            }

            ToAnalyze(ref grammarList);

            dataForFacotization.Clear();
        }

        private void ToAnalyze(ref List<Rule> grammarList)
        {

            for (int i = 0; i < dataForFacotization.Count; i++)
            {
                int elementWithCommonElement = dataForFacotization[i].indexList.Count();

                if (elementWithCommonElement == 1)
                {
                    //добавляем в правило
                    int index = dataForFacotization[i].indexList[0];
                    Rule currRule = grammarList[index];
                    newGrammarList.Add(currRule);
                } else
                {
                    //факторизуем
                    Factorization(ref grammarList, i);
                }
            }
        }

        private void Factorization(ref List<Rule> grammarList, int index)
        {
            
            isNeedToRepeatFactorization = true;

            string rule = dataForFacotization[index].rule;
            string commonElement = dataForFacotization[index].commonElement;

            Rule newRule = new Rule();
            newRule.ruleName = rule;
            newRule.guideSet = new List<string>();
            newRule.ruleСomposition = new List<string> { commonElement, "<" + ruleName + ruleIndex + ">" };

            newGrammarList.Add(newRule);

            for (int i = 0; i < dataForFacotization[index].indexList.Count(); i++)
            {

                newRule = new Rule();
                newRule.ruleName = "<" + ruleName + ruleIndex + ">";
                newRule.guideSet = new List<string>();
                newRule.ruleСomposition = new List<string>();

                int currIndex = dataForFacotization[index].indexList[i];
                Rule currRule = grammarList[currIndex];

                if (currRule.ruleСomposition.Count() > 1)
                {
                    for (int j = 1; j < currRule.ruleСomposition.Count(); j++)
                    {
                        newRule.ruleСomposition.Add(currRule.ruleСomposition[j]);
                    }
                } else
                {
                    newRule.ruleСomposition.Add("[уь]");
                }

                newGrammarList.Add(newRule);
            }

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
                            //узнать индекс
                            TerminalList currTerminalList = newTerminalList.Find(x => x.terminal.Contains(element));
                            currTerminalList.index.Add(i);
                        }
                    }
                }

            }
        }
    }
}
