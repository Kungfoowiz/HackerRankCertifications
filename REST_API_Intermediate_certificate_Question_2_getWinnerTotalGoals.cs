// HackerRank REST API Intermediate certificate.
// Question 2 solution.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Threading.Tasks;
using System.Text.Json;

class AllCompetitions
{
    public int page { get; set; }

    public int per_page { get; set; }

    public int total { get; set; }

    public int total_pages { get; set; }

    public AllCompetitionsData[] data { get; set; }
}

class AllCompetitionsData
{
    public string name { get; set; }

    public string country { get; set; }

    public int year { get; set; }

    public string winner { get; set; }

    public string runnerup { get; set; }
}

class Matches
{
    public int page { get; set; }

    public int per_page { get; set; }

    public int total { get; set; }

    public int total_pages { get; set; }

    public Competition[] data { get; set; }
}

class Competition
{
    public string competition { get; set; }

    public int year { get; set; }

    public string round { get; set; }

    public string team1 { get; set; }

    public string team2 { get; set; }

    public string team1goals { get; set; }

    public string team2goals { get; set; }
}

class Result
{
    public static async Task<int> getWinnerTotalGoals(string competition, int year)
    {
        HttpClient client = new HttpClient();

        var totalGoals = 0;

        var currentPage = 1;

        var totalPages = 1;

        var winner = "";


        // 1. Get who won the competition?
        while (currentPage <= totalPages)
        {

            // Get API data.
            var apiResult = await client.GetStreamAsync(
                $"https://jsonmock.hackerrank.com/api/football_competitions?year={year}&name={competition}&page={currentPage}"
            );

            // Deserialise into AllCompetitions class.
            var result = await JsonSerializer.DeserializeAsync<AllCompetitions>(apiResult);

            // If on first page, update total pages.
            if (currentPage == 1)
            {
                totalPages = result.total_pages;
            }

            // Get the competition winner?
            winner = result.data.FirstOrDefault().winner;

            // Check next page results.
            currentPage++;
        }


        // -------------------------------


        // Reset pagination.
        currentPage = 1;

        totalPages = 1;

        // 2. Get their home goals for that competition?
        while (currentPage <= totalPages)
        {

            // Get API data.
            var apiResult = await client.GetStreamAsync(
                $"https://jsonmock.hackerrank.com/api/football_matches?competition={competition}&year={year}&team1={winner}&page={currentPage}"
            );

            // Deserialise into Matches class.
            var result = await JsonSerializer.DeserializeAsync<Matches>(apiResult);

            // If on first page, update total pages.
            if (currentPage == 1)
            {
                totalPages = result.total_pages;
            }

            // Append home team goals.
            totalGoals += result.data.Sum(x => Int32.Parse(x.team1goals));

            // Check next page results.
            currentPage++;
        }


        // -------------------------------


        // Reset pagination.
        currentPage = 1;

        totalPages = 1;

        // 3. Get their visiting goals for that competition?
        while (currentPage <= totalPages)
        {

            // Get API data.
            var apiResult = await client.GetStreamAsync(
                $"https://jsonmock.hackerrank.com/api/football_matches?competition={competition}&year={year}&team2={winner}&page={currentPage}"
            );

            // Deserialise into Matches class.
            var result = await JsonSerializer.DeserializeAsync<Matches>(apiResult);

            // If on first page, update total pages.
            if (currentPage == 1)
            {
                totalPages = result.total_pages;
            }

            // Append visiting team goals.
            totalGoals += result.data.Sum(x => Int32.Parse(x.team2goals));

            // Check next page results.
            currentPage++;
        }

        return totalGoals;
    }

}

class Solution
{
    public static async Task Main(string[] args)
    {
        string competition = Console.ReadLine();

        int year = Convert.ToInt32(Console.ReadLine().Trim());

        int result = await Result.getWinnerTotalGoals(competition, year);

        Console.WriteLine(result);
    }
}
