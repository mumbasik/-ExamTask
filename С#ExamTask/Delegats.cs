using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;


namespace С_ExamTask
{
    internal class Delegats
    {
        public delegate void VocAdded(string message);
        public delegate void WordAdded(string message);
        public  delegate void WordsPrint(string message);
        public delegate void WordRemoved(string message);
        public delegate void SearchWord(string message);
        public delegate void Change(string message);

        public static void Adding(string message) { 
        Console.WriteLine(message);
        }
        public static void WordAdding(string message) {
            Console.WriteLine(message);
        }
        public static void WordPrinting(string message) { 
            
            Console.WriteLine(message);
        
        }
        public static void Removeing(string message) { 
            Console.WriteLine(message); 
        }
        public static void WordSearching(string message)
        {
            Console.WriteLine(message);
        }
        public static void WordChanhing(string message) {
            Console.WriteLine(message);
        }

    }
}
