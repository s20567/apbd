﻿using System.Text.RegularExpressions;


namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentNullException("Website URL", "No argument has been passed");
            }
            
            string websiteUrl = args[0];
            
            if (!Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
            {
                throw new ArgumentException("A given argument is not a valid HTTP URL");
            }
            
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            
            try
            {
                response = await httpClient.GetAsync(websiteUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while downloading page contents");
                throw;
            }
            finally
            {
                httpClient.Dispose();
            }
            
            string content = await response.Content.ReadAsStringAsync();
            Regex regex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            MatchCollection matchCollection = regex.Matches(content);
            
            if (matchCollection.Count == 0)
            {
                Console.WriteLine("No e-mail addresses found");
            }
            else
            {
                foreach (var match in matchCollection.Select(m => m.Groups[0].Value).Distinct())
                {
                    Console.WriteLine(match);
                }
            }
        }
    }
}