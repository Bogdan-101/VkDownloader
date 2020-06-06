using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            Console.WriteLine("Hello World!");
            VK vk = new VK();

            Console.WriteLine("Инициализация...");
            Thread.Sleep(1000);
            vk.Auth("+375333525625", "asd23WEasd");
            Console.WriteLine("Бот запущен и готов к работе...");

            var pathWithEnv = @"%USERPROFILE%\Desktop\VkDownloader";
            var filePath = Environment.ExpandEnvironmentVariables(pathWithEnv);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);      
            }



            while (true)
            {
                int choice;
                Console.WriteLine("1 - из профиля\n2 - популярные");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Попробуйте ещё раз! неправильный ввод!");
                    continue;
                };
                if (choice == 1)
                {
                    Directory.CreateDirectory(filePath + "\\ByID");
                    Console.WriteLine("Удалить ли все предыдущие файлы из папки? ");
                    bool agree = vk.Agreement();

                    if (agree)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(filePath + "\\ByID");

                        foreach (FileInfo file in dirInfo.GetFiles())
                        {
                            file.Delete();
                        }
                    }

                    Console.WriteLine("Введеите ваш ID: ");
                    int userID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введеите кол-во песен: ");
                    int count = Convert.ToInt32(Console.ReadLine());

                    uint total = Convert.ToUInt32(count);
                    int countsongs = 1;

                    List<AudioFile> audio = vk.GetById(userID, count);
                    

                    // сохранение данных
                    string json_name;
                    using (var stream = File.Open(filePath + "\\usersongs.json", FileMode.CreateNew)) 
                    {
                        json_name = stream.Name;
                    }
                    using (StreamWriter sw = new StreamWriter(json_name, true, System.Text.Encoding.Default))
                    {
                        foreach (var item in audio)
                        {
                            var url = item.url;
                            string save_path = filePath + "\\ByID\\";
                            string name = item.title + " - " + item.performer + ".mp3";

                            try
                            {
                                wc.DownloadFile(url, save_path + name);
                            }
                            catch
                            {
                                Console.WriteLine($"Error during loading {name}");
                            }
                            vk.drawTextProgressBar(countsongs, total);
                            countsongs++;

                            string json = JsonConvert.SerializeObject(item);
                            sw.WriteLine(json);

                            Console.WriteLine("Data has been saved to file");
                        }

                    }

                    
                }

                if (choice == 2)
                {
                    Directory.CreateDirectory(filePath + "\\Popular");
                    Console.WriteLine("Удалить ли все предыдущие файлы из папки? ");
                    bool agree = vk.Agreement();

                    if (agree)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(filePath + "\\Popular");

                        foreach (FileInfo file in dirInfo.GetFiles())
                        {
                            file.Delete();
                        }
                    }

                    Console.WriteLine("Выводим популярные: ");
                    Console.WriteLine("Введеите кол-во песен: ");
                    uint count = Convert.ToUInt32(Console.ReadLine());

                    List<AudioFile> audio = vk.GetPopular(count);
                            //TODO: проверить, пустая она или нет

                    int countsongs = 1;
                    uint total = count;

                    //сохранение данных
                    string json_name;
                    using (var stream = File.Open(filePath + "\\popularsongs.json", FileMode.CreateNew))
                    {
                        json_name = stream.Name;
                    }

                    using (StreamWriter sw = new StreamWriter(json_name, true, System.Text.Encoding.Default))
                    {
                        foreach (var item in audio)
                        {
                            var url = item.url;
                            string save_path = filePath + "\\Popular\\";
                            string name = item.title + " - " + item.performer + ".mp3";

                            try
                            {
                                wc.DownloadFile(url, save_path + name);
                            }
                            catch
                            {
                                Console.WriteLine($"Error during loading {name}");
                            }

                            vk.drawTextProgressBar(countsongs, total);
                            countsongs++;

                            string json = JsonConvert.SerializeObject(item);
                            sw.WriteLine(json);

                            Console.WriteLine("Data has been saved to file");
                        }
                    }
                }
            }
        }
        //vk.smth();
    }
}

