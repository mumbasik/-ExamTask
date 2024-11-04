using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;
using С_ExamTask;
using System;
using static С_ExamTask.Delegats;


namespace С_ExamTask
{



    class Vocabluary
    {
        public string Word { get; set; }
        public string Translate { get; set; }
        public string LanguageWord { get; set; }
        public string TranslateLangWord  { get; set; }
        public Dictionary<string, List<string>> AdditionalTranslations { get; set; } = new Dictionary<string, List<string>>();
        bool isRussian { get; set; }

        bool isCreated = false;

        public event VocAdded VocAdd;
        public event  WordAdded WordAdd;
        public event  WordsPrint WordPrint;
        public event  WordRemoved WordRemov;
        public event SearchWord Search;
        public event Change Change;
        string file = "voc.txt";


        public Vocabluary() { 
        
        
        }
        
       
        Dictionary<string, string> words = new Dictionary<string, string>()
        {
            
        };

       

        public void AddVocabluary()
        {
            

            Console.WriteLine("Введите тип словаря ");
            Console.WriteLine("Введите язык слова которого вам нужен перевод");


            LanguageWord = Console.ReadLine();
            
            Console.WriteLine("Введите язык слова на котором будет перевод ");
            TranslateLangWord  = Console.ReadLine();
            if (LanguageWord.ToLower() == "русский" && TranslateLangWord.ToLower() == "english")
            {
                isRussian = true; 
                isCreated = true;
                VocAdd?.Invoke($"Словарь  {LanguageWord} - {TranslateLangWord} был создан");
            }
            else if (LanguageWord.ToLower() == "english" && TranslateLangWord.ToLower() == "русский")
            {
                isRussian = false; 
                isCreated = true;
                VocAdd?.Invoke($"Словарь  {LanguageWord} - {TranslateLangWord} был создан");
            }
            else
            {
                Console.WriteLine("Неподдерживаемый тип словаря.");
                isCreated = false;
                return;

            }

          

        }


       public void AddTranslate(string word)
        {
            int choice;
            var x = 0;
            Console.WriteLine("1) Есть дополнительные варианты перевода этого слова? Введи их:");
            Console.WriteLine("2) Выход");
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    var translations = new List<string>();
                    string count;
                    do
                    {
                        Console.WriteLine("Сколько еще дополнительных переводов?");
                        count = Console.ReadLine();
                    } while (!int.TryParse(count, out x) || x <= 0);
                    

                   
                  for (int i = 0; i < x; i++)
                        {
                  string anotherTranslation;
                  while (true)
                            {
                  anotherTranslation = PromptForTranslation();


                  if (ValidateTranslation(anotherTranslation))
                   {
                      break;
                   }
                            }
                  translations.Add(anotherTranslation);
                        }
                    
                    

                   
                    if (!AdditionalTranslations.ContainsKey(word))
                    {
                        AdditionalTranslations[word] = new List<string>();
                    }
                    AdditionalTranslations[word].AddRange(translations);

                    SaveToFile(file);
                    break;

                case 2:
                    break;
            }
        }
        private string PromptForTranslation()
        {
            Console.WriteLine("Введите дополнительный перевод");
            return Console.ReadLine();
        }

        public bool ValidateTranslation(string translation)
        {
            if (isRussian)
            {
                if (ContainsLatin(translation))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Перевод должен быть на английском (латиница). Попробуйте еще раз.");
                    return false;
                }
            }
            else
            {
                if (ContainsCyrillic(translation))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Перевод должен быть на русском (кириллица). Попробуйте еще раз.");
                    return false;
                }
            }
        }

        public void addWord()
        {
            if (!isCreated)
            {
                Console.WriteLine("Слоаврь пуст");

                return;
            }
            if (isRussian)
            {
                
                while (!ContainsCyrillic(Word = PromptForWord(LanguageWord)))
                {
                    Console.WriteLine($"Слово должно быть на {LanguageWord} (кириллица). Попробуйте еще раз.");
                }
                while (!ContainsLatin(Translate = PromptForWord(TranslateLangWord)))
                {
                    Console.WriteLine($"Перевод должен быть на {TranslateLangWord} (латиница). Попробуйте еще раз.");
                }
            }
            else
            {
              
                while (!ContainsLatin(Word = PromptForWord(LanguageWord)))
                {
                    Console.WriteLine($"Слово должно быть на {LanguageWord} (латиница). Попробуйте еще раз.");
                }
                while (!ContainsCyrillic(Translate = PromptForWord(TranslateLangWord)))
                {
                    Console.WriteLine($"Перевод должен быть на {TranslateLangWord} (кириллица). Попробуйте еще раз.");
                }
            }
            
            if (!words.ContainsKey(Word))
            {
                words.Add(Word, Translate);
                AddTranslate(Word);
                SaveToFile(file);
            }
            else
            {
                Console.WriteLine("Такое слово уже существует.");
                return;
            }
            
            WordAdd?.Invoke($"Слова были успешно записаны в словарь и сохранены в файл {file}");
            
        }



        public void removeWord()
        {
            if (!isExist()) return;

            int choice;
            Console.WriteLine("1) Удалить слово (Со словом удаляются все переводы автоматически)");
            Console.WriteLine("2) Удалить перевод этого слова");
            Console.WriteLine("3) Выход");
            choice = Convert.ToInt32(Console.ReadLine());
            string anotherTranslation;
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Введи слово, которое ты хочешь удалить:");
                    Word = Console.ReadLine();
                    

                    if (words.ContainsKey(Word))
                    {
                        
                        words.Remove(Word);
                        if (AdditionalTranslations.ContainsKey(Word))
                        {
                            AdditionalTranslations.Remove(Word); 
                        }

                        Console.WriteLine($"Слово '{Word}' и его переводы успешно удалены.");
                    }
                    else
                    {
                        Console.WriteLine("Такого слова не существует.");
                        return;
                    }
                    break;

                case 2:
                    Console.WriteLine("Введи слово, перевод которого ты хочешь удалить:");
                    Word = Console.ReadLine();

                    


                    if (words.ContainsKey(Word)) 
                    {
                        Console.WriteLine("Введи перевод, который ты хочешь удалить:");
                        string anotherTranslationn = Console.ReadLine();


                        if (words[Word] == anotherTranslationn && (!AdditionalTranslations.ContainsKey(Word) || AdditionalTranslations[Word].Count == 0))
                        {
                            Console.WriteLine("Нельзя удалить единственный перевод этого слова.");
                            return;
                        }



                        if (words[Word] == anotherTranslationn)
                        {

                            words.Remove(Word);
                            Console.WriteLine($"Основной перевод слова '{Word}' удален.");


                            if (AdditionalTranslations.ContainsKey(Word))
                            {
                                AdditionalTranslations.Remove(Word);
                                Console.WriteLine($"Все дополнительные переводы для слова '{Word}' были удалены.");
                            }
                            WordRemov?.Invoke("Удаление произошло успешно");
                        }

                        else if (AdditionalTranslations.ContainsKey(Word) && AdditionalTranslations[Word].Contains(anotherTranslationn))
                        {

                            AdditionalTranslations[Word].Remove(anotherTranslationn);
                            Console.WriteLine($"Дополнительный перевод '{anotherTranslationn}' удален для слова '{Word}'.");


                            if (AdditionalTranslations[Word].Count == 0)
                            {
                                AdditionalTranslations.Remove(Word);
                            }
                            WordRemov?.Invoke("Удаление произошло успешно");
                        }
                        else
                        {
                            Console.WriteLine("Такого перевода не существует.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Такого слова не существует.");
                    }
                    break;

                case 3:
                    Console.WriteLine("Выход из удаления.");
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
           
        }



        public void changeTranslateWord()
        {
            if (!isExist()) return;

            int choice = 0;
            Console.WriteLine("1) Изменить слово");
            Console.WriteLine("2) Изменить перевод");
            Console.WriteLine("3) Выход");
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    string newWord;
                    Console.WriteLine("Какое слово ты хочешь изменить?");
                    Word = Console.ReadLine();

                    if (words.ContainsKey(Word))
                    {
                        Console.WriteLine("Теперь введи новое слово");
                        newWord = Console.ReadLine();

                        if (words.ContainsKey(newWord))
                        {
                            Console.WriteLine("Такое слово уже существует.");
                            return;
                        }

                        string translation = words[Word];
                        words.Remove(Word);
                        words[newWord] = translation;

                        if (AdditionalTranslations.ContainsKey(Word))
                        {
                            var translations = AdditionalTranslations[Word];
                            AdditionalTranslations.Remove(Word);
                            AdditionalTranslations[newWord] = translations;
                        }
                        Console.WriteLine($"Слово '{Word}' успешно изменено на '{newWord}'.");

                        Console.WriteLine("Ввести новые варианты переводов или оставить старые?");
                        AddTranslate(newWord);
                        SaveToFile(file);
                        Change?.Invoke("Изменения вступили в силу");
                    }
                    else
                    {
                        Console.WriteLine("Такого слова не существует в словаре.");
                        return;
                    }
                    break;

                case 2:
                    string newTranslation;
                    Console.WriteLine("Какого слова перевод тебе нужно изменить?");
                    Word = Console.ReadLine();

                    if (words.ContainsKey(Word))
                    {
                        Console.WriteLine("Введи новый перевод этого слова");
                        newTranslation = Console.ReadLine();

                        words[Word] = newTranslation;

                        Console.WriteLine($"Основной перевод слова '{Word}' успешно изменен на '{newTranslation}'.");
                        SaveToFile(file);
                        Change?.Invoke("Изменения вступили в силу");
                    }
                    else
                    {
                        Console.WriteLine("Такого слова не существует в словаре.");
                        return;
                    }
                    break;

                case 3:
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
        bool isExist()
        {
            if (!isCreated) 
            {
                Console.WriteLine("Словарь не создан.");
                return false;
            }

            if (words.Count == 0) 
            {
                Console.WriteLine("Словарь пуст.");
                return false;
            }

            return true; 
        }

        public void searchTranslate()
        {
             if (!isExist()) return;

            Console.WriteLine("Какое слово тебе нужно перевести?");
            Word = Console.ReadLine();

            
            if (words.ContainsKey(Word))
            {
                
                string translation = words[Word];
                Console.WriteLine($"Перевод слова '{Word}': {translation}");

                
                if (AdditionalTranslations.ContainsKey(Word) && AdditionalTranslations[Word].Count > 0)
                {
                    Console.WriteLine("Дополнительные переводы:");
                    foreach (var additionalTranslation in AdditionalTranslations[Word])
                    {
                        Console.WriteLine($"Для слова '{Word}' дополнительный перевод: {additionalTranslation}");
                    }
                    Search?.Invoke("Поиск произошел");
                }
                else
                {
                    Console.WriteLine("Дополнительных переводов нет.");
                }
            }
            else
            {
                Console.WriteLine("Слово не найдено в словаре.");
                return;
            }
          
        }

        static string PromptForWord(string language)
        {
            Console.WriteLine($"Введите слово на {language}:");
            return Console.ReadLine();
        }



        static bool ContainsCyrillic(string input)
        {
            
            return Regex.IsMatch(input, @"[А-Яа-яЁё]");
        }

        static bool ContainsLatin(string input)
        {
          
            return Regex.IsMatch(input, @"[A-Za-z]");
        }
        public void Print()
        {
             if (!isExist()) return;
            
            foreach (var pair in words)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}"); 

                
                if (AdditionalTranslations.ContainsKey(pair.Key) && AdditionalTranslations[pair.Key].Count > 0)
                {
                    Console.WriteLine("Дополнительные переводы:");
                    foreach (var translation in AdditionalTranslations[pair.Key])
                    {
                        Console.WriteLine($"Для слова '{pair.Key}' дополнительный перевод: {translation}");
                    }
                }
            }
            if (words.Count > 0)
            {
                WordPrint?.Invoke("Слова были выведены");
            }
        }
        public void SaveToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (var wordEntry in words)
                    {
                        string word = wordEntry.Key;
                        string translation = wordEntry.Value;
                        writer.WriteLine($"{word}:{translation}");

                        if (AdditionalTranslations.ContainsKey(word))
                        {
                            foreach (var additionalTranslation in AdditionalTranslations[word])
                            {
                                writer.WriteLine($"Дополнительный перевод для слова '{word}': {additionalTranslation}");
                            }
                        }
                    }
                }
                Console.WriteLine("Файл успешно сохранён.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }
        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Такого файла не существует");
                return;
            }

            words.Clear();
            AdditionalTranslations.Clear();

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(':');
                    if (parts.Length < 2) continue; 

                    string word = parts[0].Trim();
                    string translation = parts[1].Trim();
                    words[word] = translation; 

                    
                    List<string> additionalTranslations = new List<string>();
                    while ((line = reader.ReadLine()) != null && line.StartsWith("Дополнительный перевод"))
                    {
                        var additionalParts = line.Split(new[] { ':' }, 2);
                        if (additionalParts.Length > 1)
                        {
                            additionalTranslations.Add(additionalParts[1].Trim());
                        }
                    }

                   
                    if (additionalTranslations.Count > 0)
                    {
                        AdditionalTranslations[word] = additionalTranslations;
                    }
                }
            }

           
            Print(); 
        }

    }
   
}
