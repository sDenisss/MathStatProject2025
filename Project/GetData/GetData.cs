using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project
{
    public class GetData
    {
        public static async Task GetLinksSitesWithMathes()
        {
            var url = "https://fbref.com/en/comps/1/schedule/World-Cup-Scores-and-Fixtures";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            var html = await TryGetHtml(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var matchLinks = new HashSet<string>();

            foreach (var link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                var href = link.GetAttributeValue("href", "");
                if (href.Contains("/matches/") && href.Contains("World-Cup"))
                {
                    matchLinks.Add("https://fbref.com" + href);
                }
            }

            int i = 1;
            foreach (var match in matchLinks)
            {
                Console.WriteLine($"{i++}: {match}");
            }

            await PrintAllDataFromAllMatches(matchLinks);
        }

        public static async Task ParseMatchData(string url)
        {
            var httpClient = new HttpClient();
            var html = await TryGetHtml(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Названия команд
            var teams = doc.DocumentNode.SelectNodes("//div[@class='scorebox']//strong/a");
            var team1 = teams[0].InnerText;
            var team2 = teams[1].InnerText;

            // Счёт
            var scores = doc.DocumentNode.SelectNodes("//div[@class='scorebox']//div[@class='score']");
            var score1 = scores[0].InnerText;
            var score2 = scores[1].InnerText;

            // Владение мячом
            var possessionNodes = doc.DocumentNode.SelectNodes("//tr[th[contains(text(),'Possession')]]/following-sibling::tr[1]//strong");
            string possession1 = possessionNodes?[0].InnerText ?? "N/A";
            string possession2 = possessionNodes?[1].InnerText ?? "N/A";


            // Точность пасов
            var passAccRow = doc.DocumentNode.SelectSingleNode("//tr[th[contains(text(),'Passing Accuracy')]]/following-sibling::tr[1]");
            string passAccText1 = HtmlEntity.DeEntitize(passAccRow.SelectSingleNode(".//td[1]//div[1]").InnerText.Trim()); // "377 of 469 — 80%"
            // string passAcc1 = HtmlEntity.DeEntitize(passAccRow.SelectSingleNode(".//td[1]//div[1]/strong")?.InnerText.Trim() ?? "N/A");
            

            string passAccText2 = HtmlEntity.DeEntitize(passAccRow.SelectSingleNode(".//td[2]//div[1]").InnerText.Trim()); // "82% — 430 of 522"
            // string passAcc2 = passAccRow.SelectSingleNode(".//td[2]//div[1]/strong")?.InnerText.Trim() ?? "N/A";


            // // Удары в створ
            var shotsRow = doc.DocumentNode.SelectSingleNode("//tr[th[contains(text(),'Shots on Target')]]/following-sibling::tr[1]");
            string shotsText1 = HtmlEntity.DeEntitize(shotsRow.SelectSingleNode(".//td[1]//div[1]").InnerText.Trim()); // "0 of 5 — 0%"
            // string shots1 = shotsRow.SelectSingleNode(".//td[1]//div[1]/strong")?.InnerText.Trim() ?? "N/A";
            string shotsText2 = HtmlEntity.DeEntitize(shotsRow.SelectSingleNode(".//td[2]//div[1]").InnerText.Trim()); // "40% — 2 of 5"
            // string shots2 = shotsRow.SelectSingleNode(".//td[2]//div[1]/strong")?.InnerText.Trim() ?? "N/A";

            // xG
            var xG = doc.DocumentNode.SelectNodes("//div[@class='score_xg']");
            var xG1 = xG[0].InnerText;
            var xG2 = xG[1].InnerText;


            // Вывод
            Console.WriteLine($"{team1} {score1} - {score2} {team2}");
            Console.WriteLine($"Possession: {possession1} - {possession2}");
            Console.WriteLine($"Passing Accuracy: {passAccText1} - {passAccText2}");
            Console.WriteLine($"Shots on Target: {shotsText1} - {shotsText2}");
            Console.WriteLine($"xG: {xG1} - {xG2}");
            Console.WriteLine("-------------------------------------");

            //             Console.WriteLine($"{team1} {score1} - {score2} {team2}");
            // Console.WriteLine($"Possession: {possession1} - {possession2}");
            // Console.WriteLine($"Passing Accuracy: {passAcc1}/{passAccText1} - {passAcc2}/{passAccText2}");
            // Console.WriteLine($"Shots on Target: {shots1}/{shotsText1} - {shots2}/{shotsText2}");
            // Console.WriteLine("-------------------------------------");
        }

        public static async Task<string> TryGetHtml(string url, int retries = 3)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    return await client.GetStringAsync(url);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка загрузки {url}: {e.Message}");
                    await Task.Delay(2000); // Пауза и повтор
                }
            }

            return null;
        }


        public static async Task PrintAllDataFromAllMatches(HashSet<string> matchLinks)
        {
            // var url = matchLinks.FirstOrDefault();
            // await ParseMatchData("https://fbref.com/en/matches/1b2886ce/Qatar-Ecuador-November-20-2022-World-Cup");
            int i = 0;
            foreach (var url in matchLinks)
            {
                System.Console.WriteLine(++i);;
                await ParseMatchData(url);
                await Task.Delay(5000); // Делаем паузу 2 секунды
            }
        }
    }
}
