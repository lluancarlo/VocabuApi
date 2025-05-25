//using BigGenerator.Generators;
//using Generator.Common.Records;
//using Microsoft.EntityFrameworkCore;
//using System.Net.Http.Json;
//using System.Text.Json;
//using Vocabu.DAL.Entities;
//using static Vocabu.Domain.Enums;

//public class WordsAIGenerator : BaseGenerator
//{
//    private const Languages LanguageToProcess = Languages.English;
//    private const string OllamaApiUrl = "http://localhost:11434/api/generate";
//    private const string ModelName = "llama3.3";

//    public override async Task<GeneratorResponse> Run()
//    {
//        try
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var words = await Context.Words
//               .AsNoTracking()
//               .Where(w => w.Language == LanguageToProcess && (w.WordMeanings == null || !w.WordMeanings.Any()))
//               .ToListAsync();

//                if (!words.Any())
//                    return GeneratorResponse.Ok($"No words found on language {nameof(LanguageToProcess)}, skipping...");

//                foreach (var word in words)
//                {
//                    var payload = new
//                    {
//                        stream = false,
//                        model = ModelName,
//                        prompt = @$"
//Please provide a JSON object for the English word '{word.Text}' with the following fields:
//{{
//		Text = ""{word.Text}"",
//		Languages = One of [English, Italian, Portuguese],
//		Meanings[] = A list of up to 5 short definitions, if the word has multiple common meanings. If only one meaning, just include one definition.
//		[
//		{{
//			PartOfSpeech = One of [Noun, Pronoun, Verb, Adjective, Adverb, Preposition, Conjunction, Interjection, Article, Determiner, Particle, ModalVerb].
//			Definition = ""definition description""
//			Example = ""definition examples""
//			Synonyms[] = [ ""word1"", ""word2"", ""word3"", ... ]
//			Antonyms[] = [ ""word1"", ""word2"", ""word3"", ... ]
//		}},
//		...
//		]
//}}

//IMPORTANT: Respond ONLY with JSON and do not add explanations.
//"
//                    };

//                    var response = await client.PostAsJsonAsync(OllamaApiUrl, payload);

//                    if (response != null && response.IsSuccessStatusCode)
//                    {
//                        var jsonString = await response.Content.ReadFromJsonAsync<OllamaResponse>();

//                        if (jsonString.Response.Contains("json"))
//                            jsonString.Response = jsonString.Response.Replace("json", "");

//                        if (jsonString.Response.Contains("```"))
//                            jsonString.Response = jsonString.Response.Replace("```", "");

//                        var wordData = JsonSerializer.Deserialize<WordData>(jsonString.Response);

//                        if (wordData != null && wordData.Meanings != null && wordData.Meanings.Count > 0)
//                        {
//                            foreach (var meaning in wordData.Meanings)
//                            {
//                                Context.WordMeanings.Add(new WordMeaning
//                                {
//                                    WordId = word.Id,
//                                    PartOfSpeech = GetEnumByName<PartOfSpeech>(meaning.PartOfSpeech),
//                                    Definition = meaning.Definition,
//                                    Example = meaning.Example,
//                                    // TODO check this structure
//                                    //Synonyms = meaning.Synonyms != null ? string.Join(",", meaning.Synonyms) : null,
//                                    //Antonyms = meaning.Antonyms != null ? string.Join(",", meaning.Antonyms) : null
//                                });
//                            }
//                        }
//                    }
//                }

//                await Context.SaveChangesAsync();
//            }
//        }
//        catch (Exception ex)
//        {
//            return GeneratorResponse.Error($"EXCEPTION! : {ex.Message}");
//        }
//        return GeneratorResponse.Ok();
//    }

//    public class OllamaResponse
//    {
//        public string Response { get; set; }
//        public string Model { get; set; }
//        public DateTime Created_At { get; set; }
//    }

//    private class WordData
//    {
//        public string Text { get; set; } = string.Empty;
//        public string Languages { get; set; } = string.Empty;
//        public List<MeaningData> Meanings { get; set; }
//    }

//    private class MeaningData
//    {
//        public string PartOfSpeech { get; set; } = string.Empty;
//        public string Definition { get; set; } = string.Empty;
//        public string Example { get; set; } = string.Empty;
//        public List<string>? Synonyms { get; set; }
//        public List<string>? Antonyms { get; set; }
//    }
//}
