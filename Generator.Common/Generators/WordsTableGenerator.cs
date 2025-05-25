﻿using Generator.Common.Records;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;
using static Vocabu.Domain.Enums;

namespace BigGenerator.Generators;

public class WordsTableGenerator : BaseGenerator
{
    private const string PathFileToRead = "WordLists/english.txt";

    public override async Task<GeneratorResponse> Run()
    {
        //if (Context.Words.AsNoTracking().Any())
        //    return GeneratorResponse.Ok("Words table is not empty, skipping generator ...");

        try
        {
            // Get all words and insert in table
            var englishWorlds = await GetWordsFromLinesTxt(PathFileToRead);

            if (englishWorlds == null)
                return GeneratorResponse.Error($"file {PathFileToRead} not found!");

            var wordList = new List<Word>();

            foreach (string item in englishWorlds)
                wordList.Add(new Word
                {
                    Text = item
                });

            await Context.Words.AddRangeAsync(wordList);
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return GeneratorResponse.Error($" EXCEPTION! : {ex.Message}");
        }

        return GeneratorResponse.Ok();
    }

    private async Task<string[]> GetWordsFromLinesTxt(string txtPath) =>
        await File.ReadAllLinesAsync(txtPath);
}
