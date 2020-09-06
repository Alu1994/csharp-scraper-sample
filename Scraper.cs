using System;
using System.Linq;
using ScrapySharp.Html;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using Microsoft.Ajax.Utilities;

namespace ConsoleApp1
{
    class Scraper
    {
        static void Main(string[] args)
        {
            GetMovie();

            Console.WriteLine("");
            Console.WriteLine("");

            GetActor();

            Console.ReadLine();
        }

        private static void GetMovie()
        {
            var url = new Uri("https://www.imdb.com/title/tt0434665/?ref_=fn_al_tt_1");
            var browser = new ScrapingBrowser();

            var homePage = browser.NavigateToPage(url);

            var titleName = homePage.Html.CssSelect("div.title_wrapper h1").First().InnerText;
            Console.Write(titleName.Replace("&nbsp;", "").TrimEnd());

            Console.Write(" ");

            var rating = homePage.Find("div", By.Class("ratingValue")).First();
            foreach (var elem in rating.Descendants().ToList())
            {
                if (elem.Name == "span" && elem.Attributes.Any(x => x.Name == "itemprop" && x.Value == "ratingValue"))
                {
                    Console.Write(elem.InnerText);
                    break;
                }
            }
        }

        private static void GetActor()
        {
            var url = new Uri("https://www.imdb.com/name/nm0000206/?ref_=fn_al_nm_1");
            var browser = new ScrapingBrowser();

            var homePage = browser.NavigateToPage(url);

            var actorName = homePage.Html.CssSelect("h1.header").First();

            Console.WriteLine(actorName.InnerText.Trim());
            Console.WriteLine("====== Movies ======");
            Console.WriteLine("");

            var categories = homePage.Html.CssSelect("div.filmo-category-section").ToList();
            foreach (var category in categories)
            {
                var movies = category.CssSelect("div").Where(x => x.OuterHtml.Contains("id=\"actor-")).ToList().CssSelect("b a").OrderBy(x => x.InnerText);
                movies.ForEach(movie =>
                {
                    Console.WriteLine(movie.InnerText);
                });
            }
        }
    }
}
