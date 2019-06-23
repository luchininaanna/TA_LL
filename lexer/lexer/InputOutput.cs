using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace lexer
{
    class InputOutput
    {
        public InputOutput(){}

        public void ExtractData(ref GrammarConverter grammarConverter)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("recursion_test_2.txt");

            string line;
            int rawIndex = 0;
            int wordIndex = 2;

            while ((line = file.ReadLine()) != null)
            {
                rawIndex++;
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<Rule> grammarList = grammarConverter.GetGrammarList();
                List<TerminalList> terminalList = grammarConverter.GetTerminalList();
                List<IndexOfTerminal> indexOfTerminalList = grammarConverter.GetIndexOfTerminalList();

                PutDataToStracture(words, ref grammarList);
                AddComponentsToTerminalList(words, rawIndex, ref terminalList);
                UpdateTerminalIndexList(words[0], wordIndex, rawIndex, ref indexOfTerminalList);
                wordIndex += words.Count() - 2;
            }

        }

        private void PutDataToStracture(string[] words, ref List<Rule> grammarList)
        {
            Rule newRule;
            newRule.ruleName = words[0];

            int amount = words.Length;
            List<string> currComposition = new List<string>();

            for (int i = 2; i < amount; i++)
            {
                currComposition.Add(words[i]);
            }

            newRule.ruleСomposition = currComposition;
            newRule.guideSet = new List<string>();

            grammarList.Add(newRule);
        }

        private void AddComponentsToTerminalList(string[] words, int rawIndex, ref List<TerminalList> terminalList)
        {
            int amount = words.Length;

            for (int i = 1; i < amount; i++)
            {
                string currWord = words[i];

                //проверка на терминал
                if ((currWord[0] == '<') && (currWord[currWord.Length - 1] == '>'))
                {
                    //поиск терминала в существующем списке
                    bool isExist = terminalList.Exists(x => x.terminal.Contains(currWord));

                    if (!isExist)
                    {
                        TerminalList newTerninal;
                        newTerninal.terminal = currWord;
                        newTerninal.index = new List<int>();
                        newTerninal.index.Add(rawIndex + 1);
                        terminalList.Add(newTerninal);
                    } else
                    {
                        //узнать индекс
                        TerminalList currTerminalList = terminalList.Find(x => x.terminal.Contains(currWord));
                        currTerminalList.index.Add(rawIndex + 1);
                    }
                }
            }
        }

        private void UpdateTerminalIndexList(string terminal, int wordIndex, int row, 
            ref List<IndexOfTerminal> indexOfTerminalList)
        {
            bool isExist = indexOfTerminalList.Exists(x => x.terminal == terminal);

            if (!isExist)
            {
                IndexOfTerminal newIndexOfTerminal;
                newIndexOfTerminal.terminal = terminal;

                List<int> startIndex = new List<int>();
                startIndex.Add(wordIndex);
                newIndexOfTerminal.startIndex = startIndex;

                List<int> rowIndex = new List<int>();
                rowIndex.Add(row);
                newIndexOfTerminal.rowIndex = rowIndex;
               
                indexOfTerminalList.Add(newIndexOfTerminal);
            } else
            {
                IndexOfTerminal currIndexOfTerminal = indexOfTerminalList.Find(x => x.terminal == terminal);
                currIndexOfTerminal.startIndex.Add(wordIndex);
                currIndexOfTerminal.rowIndex.Add(row);
            }
        }
    }
}