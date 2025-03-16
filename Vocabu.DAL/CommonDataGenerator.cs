using System.Text.Json.Nodes;
using System.Text;
using System;
using Vocabu.DAL.Entities;
using Newtonsoft.Json;

namespace Vocabu.DAL;

internal record ResultCountryNativeNameEng
{
    public string? Official { get; set; }
    public string? Common { get; set; }
}

internal record ResultCountryNativeName
{
    public ResultCountryNativeNameEng? NativeName { get; set; }
}

internal record ResultCountryName
{
    public string? Common { get; set; }
    public string? official { get; set; }
    public ResultCountryNativeName? NativeName { get; set; }
}

internal record ResultCountry
{
    public required ResultCountryName Name { get; set; }
    public required string[] Continents { get; set; }
}

public static class CommonDataGenerator
{
    public static async Task<Country[]> GetCountriesAsync()
    {
        var url = "https://restcountries.com/v3.1/all?fields=name,continents";
        var countryList = new List<Country>();

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
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
                                Continent = c.Continents[0]
                            });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when running {nameof(CommonDataGenerator)}.{nameof(GetCountriesAsync)}: {ex.Message}");
        }

        return countryList.ToArray();
    }
}
