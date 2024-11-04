using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static С_ExamTask.Delegats;

namespace С_ExamTask
{
    internal class main
    {

        static void Main(string[] args)
        {
            Vocabluary obj = new Vocabluary();
            bool running = true;
            int choice;


            while (running)
            {
                Console.WriteLine("Выберите операцию: ");


                Console.WriteLine("1) Создать определённый словник");
                Console.WriteLine("2) Ввести слово и перевод в словник");
                Console.WriteLine("3) Посмотреть все слова и варианты переводов");
                Console.WriteLine("4) Удалить слово и все его переводы");
                Console.WriteLine("5) Искать перевод");
                Console.WriteLine("6) Изменить слово или перевод");
                Console.WriteLine("7) Считать с файла");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        obj.VocAdd += Adding;
                        obj.AddVocabluary();
                        break;
                    case 2:
                        obj.WordAdd += WordAdding;
                        obj.addWord();


                        break;
                    case 3:
                        obj.WordPrint += WordPrinting;
                        obj.Print();
                        break;
                    case 4:
                        obj.WordRemov += Removeing;
                        obj.removeWord();
                        break;
                    case 5:
                        obj.Search += WordSearching;
                        obj.searchTranslate();
                        break;
                    case 6:
                        obj.Change += WordChanhing;
                        obj.changeTranslateWord();
                        break;
                    case 7:
                        string filename;
                        Console.WriteLine("Введите имя файла");
                        filename = Console.ReadLine();
                        obj.LoadFromFile(filename);
                        break;
                        

                    default:
                        Console.WriteLine("Неверная операция!");
                        break;
                    case 0:
                        Console.WriteLine("До свидания!");
                        running = false;
                        break;
                }
            }

        }

    }

}
