using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Vocabu.DAL.Entities;
using static Generator.Common.Records.CountryRec;
using static Vocabu.Domain.Enums;

namespace Generator.Common;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($" -> Running {nameof(RunCountryGenerator)} ...");
        await RunCountryGenerator();
        Console.WriteLine($" -> Finish {nameof(RunCountryGenerator)} !");

        Console.WriteLine(" -> Done all generations. Press Enter to close");
        Console.ReadKey();
    }

    private static async Task RunCountryGenerator()
    {
        var baseUrl = "https://restcountries.com/v3.1/all?fields=name,cca2,cca3,ccn3continents";
        var fields = new List<string> { "name","cca2","cca3","ccn3", "continents" };
        var requestUrl = GetUrl(baseUrl, fields);

        var database = DbContext.GetDataBaseService();

        if (database.Set<Country>().AsNoTracking().Any())
        {
            Console.WriteLine($" -> Cannot run {nameof(RunCountryGenerator)}: country table is not empty!");
            return;
        }

        try
        {
            var countryList = new List<Country>();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(requestUrl);
                if (response != null)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<ResultCountry>>(jsonString);

                    if (result != null)
                        foreach (var c in result)
                            countryList.Add(new Country
                            {
                                Id = Guid.NewGuid(),
                                Name = c.Name.Common ?? string.Empty,
                                Iso31661Alpha2 = c.Cca2 ?? string.Empty,
                                Iso31661Alpha3 = c.Cca3 ?? string.Empty,
                                Iso31661Numeric = c.Ccn3 ?? string.Empty,
                                Continent = GetEnumByName<Continents>(c.Continents[0].Replace(" ", string.Empty))
                            });
                }

                if (countryList.Count > 0)
                {
                    await database.Set<Country>().AddRangeAsync(countryList);
                    await database.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when running {nameof(RunCountryGenerator)}: {ex.Message}");
        }
    }

    private static string GetUrl(string baseUrl, IEnumerable<string>? fields)
    {
        var url = baseUrl;

        if (fields != null)
        {
            url += url.EndsWith('?') ? string.Empty : ' ';

            // Add fields to url
            url += "fields=";
            foreach (var field in fields)
                url += field + ',';

            if (url.EndsWith(','))
                url.Remove(url.Length - 1);
        }

        return url;
    }
}