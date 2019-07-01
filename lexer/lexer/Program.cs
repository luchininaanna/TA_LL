using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            InputOutput fileHandler = new InputOutput();
            GrammarConverter grammarConverter = new GrammarConverter();

            fileHandler.ExtractGrammarData(ref grammarConverter);
            grammarConverter.InitializeStartRule();

            Factorizator factorizator = new Factorizator();
            factorizator.ApplyFactorization(ref grammarConverter);

            LeftRecursion leftRecursion = new LeftRecursion();
            leftRecursion.ApplyLeftRecursion(ref grammarConverter);

            grammarConverter.DefineGuideSet();

            //grammarConverter.PrintTerminalList();
            grammarConverter.PrintGuideSet();
            grammarConverter.PrintGrammarList();
            //grammarConverter.PrintIndex();

            List<RowInTable> table = grammarConverter.CreateTable();
            //grammarConverter.PrintTable();

            Runner runner = new Runner();
            runner.ProcessSequenceFromFile(ref table);

            Console.ReadKey();
        }
    }
}
