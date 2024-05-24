using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

public class Parser
{
    private readonly HtmlWeb _web;

    public Parser(HtmlWeb web)
    {
        _web = web;
    }

    public async Task<string> GetTitleAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);
        var titleNode = document.DocumentNode.SelectSingleNode("//title")?.InnerText.Trim();

        if (titleNode == null)
        {
            throw new Exception("Title not found");
        }

        return titleNode;
    }

    public async Task<string> GetNameAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);
        var nameNode = document.DocumentNode.SelectSingleNode("//*[@id=\"container\"]/div[2]/div/div[2]/div[1]/div/div[4]/h1");

        if (nameNode == null)
        {
            throw new Exception("Name not found");
        }

        return nameNode.InnerText.Trim();
    }

    public async Task<string> GetDescriptionAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);
        var descriptionNode = document.DocumentNode.SelectSingleNode("//*[@id=\"container\"]/div[2]/div/div[2]/div[1]/div/div[4]/div[2]/p");

        if (descriptionNode == null)
        {
            throw new Exception("Description not found");
        }

        return descriptionNode.InnerText.Trim();
    }

    public async Task<string> GetPriceAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);
        var priceNode = document.DocumentNode.SelectSingleNode("//span[@class='salary']");

        if (priceNode == null)
        {
            return "Salary not found";
        }

        return priceNode.InnerText.Trim();
    }
}

public class Job
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Salary { get; set; }

    public override string ToString()
    {
        return $"Title: {Title}\n\nName: {Name}\n\nDescription: {Description}\n\nSalary: {Salary}";
    }
}

public static class Program
{
    private const string JobUrl = "https://jobs.dou.ua/companies/js-dynamics/vacancies/268778/";

    static async Task Main(string[] args)
    {
        var parser = new Parser(new HtmlWeb());

        var job = new Job()
        {
            Title = await parser.GetTitleAsync(JobUrl),
            Name = await parser.GetNameAsync(JobUrl),
            Description = await parser.GetDescriptionAsync(JobUrl),
            Salary = await parser.GetPriceAsync(JobUrl)
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(job);
    }
}
