using Newtonsoft.Json;
using Vocabu.DAL.Entities;
using static Generator.Common.CountryRec;

namespace Generator.Common;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }

    //TODO: fix this shit
    //private static async Task RunCountryGeneratio()
    //{
    //    var url = "https://restcountries.com/v3.1/all?fields=name,continents";
    //    var countryList = new List<Country>();

    //    try
    //    {
    //        using (HttpClient client = new HttpClient())
    //        {
    //            var response = await client.GetAsync(url);
    //            if (response != null)
    //            {
    //                var jsonString = await response.Content.ReadAsStringAsync();
    //                var result = JsonConvert.DeserializeObject<IEnumerable<ResultCountry>>(jsonString);

    //                if (result != null)
    //                    foreach (var c in result)
    //                        countryList.Add(new Country
    //                        {
    //                            Id = Guid.NewGuid(),
    //                            Name = c.Name.Common ?? string.Empty,
    //                            Continent = c.Continents[0]
    //                        });
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error when running {nameof(CommonDataGenerator)}.{nameof(GetCountriesAsync)}: {ex.Message}");
    //    }

    //    return countryList.ToArray();
    //}
}