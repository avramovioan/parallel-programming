using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace PriceScraper
{
    /// <summary>
    /// The result of the execution is pasted at the bottom.
    /// </summary>
    class Program
    {
        static List<string> links = new List<string>
            {
                "https://www.emag.bg/konzola-playstation-5-so-9396406/pd/DNKW72MBM/",
                "https://www.emag.bg/televizor-samsung-50q80a-50-125-sm-smart-4k-ultra-hd-qled-klas-g-qe50q80aatxxh/pd/D8ZXSHMBM/",
                "https://www.emag.bg/video-karta-asus-rog-strix-geforcer-rtxtm-3060-ti-oc-v2-8gb-gddr6-256-bit-rog-strix-rtx3060ti-o8g-v2-gaming/pd/D9DX2PMBM/",
                "https://www.emag.bg/smartfon-apple-iphone-13-pro-128gb-5g-sierra-blue-mlvd3rm-a/pd/D7FCMXMBM/",
                "https://www.emag.bg/robot-prahosmukachka-roborock-cleaner-s5-max-wifi-58-w-prahosmukachka-i-mop-smart-top-up-navigacija-lidar-virtualna-stena-zona-no-mop-bjal-s5e02-00-white/pd/D888WWBBM/"
            };
        static Stopwatch stopwatch = new Stopwatch();
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("-------------ASYNC------------");
            stopwatch.Start();
            string[] result = await InitAsync();
            stopwatch.Stop();
            Printer(result);
            Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("-----------------------------");

            // This block is for testing purpose
            List<string> test = new List<string>();
            Console.WriteLine("-------------SYNC------------");
            stopwatch.Reset();
            stopwatch.Start();
            foreach (string link in links)
            {
                test.Add(ParseWorker(link));
            }
            stopwatch.Stop();
            Printer(test.ToArray());
            Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("-----------------------------");
            stopwatch.Reset();
            //------------------------------------
        }
        static async Task<string[]> InitAsync()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            foreach (string link in links)
            {
                tasks.Add(Task.Run(() => ParseWorker(link)));
            }
            var result = await Task.WhenAll(tasks);
            return result;
        }

        static HtmlNode DownloadSingleProductEmag(string link)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(link);
            return doc.DocumentNode;
        }
        static string ParseWorker(string link)
        {
            try
            {
                HtmlNode htmlNode = DownloadSingleProductEmag(link);
                string mainPrice = WebUtility.HtmlDecode(htmlNode.SelectSingleNode(".//p[@class='product-new-price has-deal']/text()").InnerText);
                string coins = htmlNode.SelectSingleNode(".//p[@class='product-new-price has-deal']/sup").InnerText;
                string currency = htmlNode.SelectSingleNode(".//p[@class='product-new-price has-deal']/span").InnerText;
                string productTitle = htmlNode.SelectSingleNode(".//h1[@class='page-title']").InnerText.Trim();
                return $"The price for {productTitle} is: {mainPrice},{coins} {currency}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
        }
        static void Printer(string[] arr)
        {
            foreach (string str in arr)
            {
                Console.WriteLine(str);
            }
        }
    }
}

//https://dev.w3.org/html5/html-author/charref
//-------------ASYNC------------
//    The price for Конзола PlayStation 5 is: 1.440,00 лв.
//    The price for Телевизор Samsung 50Q80A, 50&quot; (125 см), Smart, 4K Ultra HD, QLED, Клас G is: 1.599,99 лв.
//    The price for Видео карта ASUS ROG Strix GeForce® RTX™ 3060 Ti OC V2, 8GB GDDR6, 256-bit is: 2.617,32 лв.          
//    The price for Смартфон Apple iPhone 13 Pro, 128GB, 5G, Sierra Blue is: 2.699,00 лв.
//    The price for Робот прахосмукачка Roborock Cleaner S5 MAX, WiFi, 58 W, Прахосмукачка и моп, Smart top-up, Навигация LiDar, Виртуална стена, Зона no mop, Бял is: 870,12 лв. 
//    Elapsed time: 877
//----------------------------- 
//-------------SYNC------------
//    The price for Конзола PlayStation 5 is: 1.440,00 лв.
//    The price for Телевизор Samsung 50Q80A, 50&quot; (125 см), Smart, 4K Ultra HD, QLED, Клас G is: 1.599,99 лв.
//    The price for Видео карта ASUS ROG Strix GeForce® RTX™ 3060 Ti OC V2, 8GB GDDR6, 256-bit is: 2.617,32 лв.
//    The price for Смартфон Apple iPhone 13 Pro, 128GB, 5G, Sierra Blue is: 2.699,00 лв.                               
//    The price for Робот прахосмукачка Roborock Cleaner S5 MAX, WiFi, 58 W, Прахосмукачка и моп, Smart top-up, Навигация LiDar, Виртуална стена, Зона no mop, Бял is: 870,12 лв.
//    Elapsed time: 3509
//-----------------------------
