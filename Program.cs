// HTTP GET JSON.

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Net.Http;
using System.Net.Http.Headers;

using System.Text.Json;

using System.Threading.Tasks;

using Model.WebAPIClient;

using Shuffle;

class Solution
{

    private static readonly HttpClient client = new HttpClient();



    public static async Task Main(string[] args)
    {
        var repos = await ProcessRepository();

        // LINQ Skip and Take.
        var filteredRepos = repos.Skip(28);
        // var filteredRepos = repos.Take(1);

        // Console.WriteLine($"Found repos [{repos.Count()}]");
        // Console.WriteLine($"Found filtered repos [{filteredRepos.Count()}]");

        // var filter1 = repos.Take(1);
        // var filter2 = repos.Skip(29);

        // var filteredRepos = filter1.InterleaveSequenceWith(filter2);

        // foreach (var repo in repos)
        foreach (var repo in filteredRepos)
        {
            Console.WriteLine(repo.Name);

            Console.WriteLine(repo.GitHubHomeUrl);

            Console.WriteLine(repo.Watchers);

            Console.WriteLine(repo.LastPush);

            Console.WriteLine();
        }
    }

    private static async Task<List<Repository>> ProcessRepository()
    {
        client.DefaultRequestHeaders.Accept.Clear();

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );

        client.DefaultRequestHeaders.Add("User-Agent", "Test project");

        // var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
        // var msg = await stringTask;
        // Console.Write(msg);

        var streamTask = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
        var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(streamTask);

        // foreach (var repo in repositories)
        //     Console.WriteLine(repo.Name);

        return repositories;
    }


}
