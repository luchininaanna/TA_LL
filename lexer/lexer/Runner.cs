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

        Stack<int> states;
        int currState;
        int wordLength;
        int indexInWord;
        string currWord;
        bool isOk;



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
                    Console.WriteLine(word + " " + isRightSequence);
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

            AnalyzeStateWithSymbol(ref table);

            Console.WriteLine();

            return isOk;
        }

        private void AnalyzeStateWithSymbol(ref List<RowInTable> table)
        {
            bool isEnd = (indexInWord > wordLength - 1);
            
            if (isEnd)
            {
                isOk = (currState == 1);
                Console.WriteLine("currState " + currState);
            }
            //Console.WriteLine("currState " + currState);
            //Console.WriteLine("indexInWord " + indexInWord);
            //Console.WriteLine("stack " + states.Count());
            //Console.WriteLine("symbol  " + currWord[indexInWord]);
            //Console.WriteLine();


            if ((wordLength > indexInWord) && !isEnd)
            {
                RowInTable currRow = table[currState];

                //проверяем входит ли символ в напраляющее множество
                string symbol = "" + currWord[indexInWord];

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
                        indexInWord++;
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
                            Console.WriteLine("ERROR 1");
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
                            Console.WriteLine("ERROR 2");
                        }
                    }
                }

            }
        }
    }

}















































