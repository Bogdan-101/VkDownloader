using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace ConsoleApp1
{
    class VK
    {
        VkApi api = new VkApi();
        public void Auth(string login, string password)
        {

            var services = new ServiceCollection();
            services.AddAudioBypass();
            api = new VkApi(services);
            
            api.Authorize(new ApiAuthParams
            {
                Login = login,
                Password = password,
                Settings = Settings.All,
                ApplicationId = 7467638
            }) ;
        }

        public List<AudioFile> GetById(int userID, int count)
        {
            VkCollection<Audio> audio = api.Audio.Get(new AudioGetParams
            {
                OwnerId = userID,
                Count = count

            });


            List<AudioFile> audioFile = new List<AudioFile>();
            try
            {
                foreach (var item in audio)
                {
                    if (item.Url != null)
                    {
                        audioFile.Add(new AudioFile { performer = item.Artist, title = item.Title, url = AudioFile.DecodeAudioUrl(item.Url) });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            foreach (var item in audioFile)
            {
                Console.WriteLine(item.url + "\n---------------------------------------------------------------------------\n");
            }

            return audioFile;
        }

        public void drawTextProgressBar(int progress, uint total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }

        public List<AudioFile> GetPopular(uint count)
        {
            IEnumerable<Audio> audio = api.Audio.GetPopular(count: count);


            List<AudioFile> audioFile = new List<AudioFile>();
            try
            {
                foreach (var item in audio)
                {
                    if (item.Url != null)
                    {
                        audioFile.Add(new AudioFile { performer = item.Artist, title = item.Title, url = AudioFile.DecodeAudioUrl(item.Url) });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            foreach (var item in audioFile)
            {
                Console.WriteLine(item.url);
            }

            return audioFile;
        }

        public void smth()
        {
            VkCollection<Audio> audio = api.Audio.Get(new AudioGetParams
            {
                OwnerId = 597417716

            });


            List <AudioFile> audioFile = new List<AudioFile>();
            try
            {
                foreach (var item in audio)
                {
                    if (item.Url != null)
                    {
                        audioFile.Add(new AudioFile { performer = item.Artist, title = item.Title, url = AudioFile.DecodeAudioUrl(item.Url) });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            foreach(var item in audioFile)
            {
                Console.WriteLine(item.url);
            }


            IEnumerable<Audio> audioo = api.Audio.GetPopular();


            List<AudioFile> audioFilee = new List<AudioFile>();
            try
            {
                foreach (var item in audioo)
                {
                    if (item.Url != null)
                    {
                        audioFilee.Add(new AudioFile { performer = item.Artist, title = item.Title, url = AudioFile.DecodeAudioUrl(item.Url) });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            foreach (var item in audioFilee)
            {
                Console.WriteLine(item.url);
            }

        }

        public bool Agreement()
        {
            int top = Console.CursorTop;
            int y = top;

            Console.WriteLine("Да");
            Console.WriteLine("Нет");

            int down = Console.CursorTop;

            Console.CursorSize = 100;
            Console.CursorTop = top;

            ConsoleKey key;
            while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
            {
                if (key == ConsoleKey.UpArrow)
                {
                    if (y > top)
                    {
                        y--;
                        Console.CursorTop = y;
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if (y < down - 1)
                    {
                        y++;
                        Console.CursorTop = y;
                    }
                }
            }

            Console.CursorTop = down;

            if (y == top)
            {
                Console.WriteLine("Да");
                return true;
            }    
            else if (y == top + 1)
            {
                Console.WriteLine("Нет");
                return false;
            }

            return false;
        }
    }

    public class AudioFile
    {
        public string performer { get; set; }
        public string title { get; set; }
        public Uri url { get; set; }

        public static Uri DecodeAudioUrl(Uri audioUrl)
        {

            var segments = audioUrl.Segments.ToList();

            segments.RemoveAt((segments.Count - 1) / 2);
            segments.RemoveAt(segments.Count - 1);

            segments[segments.Count - 1] = segments[segments.Count - 1].Replace("/", ".mp3");

            return new Uri($"{audioUrl.Scheme}://{audioUrl.Host}{string.Join("", segments)}{audioUrl.Query}");
        }

    }

}
