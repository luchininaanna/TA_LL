using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace lexer
{
    class Runner
    {
        public Runner() { }

        const string EMPTY_STRING = "&";
        Stack<int> states;
        int currState;
        int wordLength;
        int indexInWord;
        string currWord;
        bool isOk;
        bool withoutReading;

        public void ProcessSequenceFromFile(ref List<RowInTable> table)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("sequence.txt");

            string line;

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    bool isRightSequence = IsRightSecuence(word, ref table);

                    if (word != EMPTY_STRING)
                    {
                        Console.WriteLine(word + " " + isRightSequence);
                    } else
                    {
                        Console.WriteLine("{empty string} " + isRightSequence);
                    }
                    
                }
            }
        }

        private bool IsRightSecuence(string word, ref List<RowInTable> table)
        {
            isOk = false;
            currState = 0;
            indexInWord = 0;
            currWord = word;
            wordLength = currWord.Length;
            states = new Stack<int>();
            withoutReading = false;

            if (word != EMPTY_STRING)
            {
                AnalyzeStateWithSymbol(ref table);
            } else
            {
                AnalyzeEmptyString(ref table);
            }
            

            return isOk;
        }


        private void AnalyzeStateWithSymbol(ref List<RowInTable> table)
        {
            RowInTable currRow;
            bool isEnd = (indexInWord > wordLength - 1);
            
            if (isEnd)
            {
                if (states.Count() == 0)
                {
                    isOk = (currState == 1);
                    return;
                }
                else
                {
                    withoutReading = true;
                }     
            }


            if ((wordLength > indexInWord) || withoutReading)
            {
                currRow = table[currState];

                //проверяем входит ли символ в напраляющее множество
                string symbol = "";
                if (!withoutReading)
                {
                    symbol = "" + currWord[indexInWord];
                } else 
                {
                    symbol = "[end]";
                }

                if (currRow.guideSet.Contains(symbol))
                {
                    //стек
                    if (currRow.isStack)
                    {
                        states.Push(currState + 1);
                    }

                    //чтение нового символа
                    if (currRow.isShift)
                    {
                        if (!withoutReading)
                        {
                            indexInWord++;
                        } else
                        {
                            isOk = false;
                            //return;
                        }
                    }

                    //переход в новое состояние
                    if (currRow.goTo != 0)
                    {
                        currState = currRow.goTo;
                        AnalyzeStateWithSymbol(ref table);
                    }
                    else
                    {
                        if (states.Count() > 0)
                        {
                            currState = states.Pop();
                            AnalyzeStateWithSymbol(ref table);
                        } else
                        {
                            return;
                        }
                    }

                } else
                {
                    //переходим по ErrorTransit
                    if (currRow.errorTransit != 0)
                    {
                        currState = currRow.errorTransit;
                        AnalyzeStateWithSymbol(ref table);
                    } else
                    {
                        if (states.Count() > 0)
                        {
                            currState = states.Pop();
                            AnalyzeStateWithSymbol(ref table);
                        }
                        else
                        {
                            //isOk = false;
                            return;
                        }
                    }
                }

            }
        }

        private void AnalyzeEmptyString(ref List<RowInTable> table)
        {
            RowInTable currRow = table[0];
            isOk = currRow.guideSet.Contains("[end]");
        }
    }

}















































