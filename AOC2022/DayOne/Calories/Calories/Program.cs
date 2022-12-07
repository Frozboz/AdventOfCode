using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        //int carCount = 0;
        //string json = @"{""Count"":14,""Message"":""Response returned successfully"",""SearchCriteria"":""Make:440"",""Results"":[{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1684,""Model_Name"":""V8 Vantage""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1686,""Model_Name"":""DBS""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1687,""Model_Name"":""DB9""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1688,""Model_Name"":""Rapide""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1695,""Model_Name"":""V12 Vantage""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1697,""Model_Name"":""Virage""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":1701,""Model_Name"":""Vanquish""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":13751,""Model_Name"":""DB11""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":14157,""Model_Name"":""Lagonda""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":14162,""Model_Name"":""Vantage""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":14164,""Model_Name"":""V8""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":19609,""Model_Name"":""Vanquish S""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":19610,""Model_Name"":""Vanquish Zagato""},{""Make_ID"":440,""Make_Name"":""ASTON MARTIN"",""Model_ID"":27591,""Model_Name"":""DBX""}]}";
        //JsonSerializerOptions options = new JsonSerializerOptions
        //{
        //    PropertyNameCaseInsensitive = true
        //};

        //CarRepositoryModel carInfo = JsonSerializer.Deserialize<CarRepositoryModel>(json, options);

        //HttpClient client = new HttpClient(new HttpClientHandler());
        //client.BaseAddress = new Uri(@"https://adventofcode.com/2022/day/1/input");
        //Task<HttpResponseMessage> data = client.GetAsync("/");

        //Task<string> webpage = data.Result.Content.ReadAsStringAsync();

        const Int32 BufferSize = 128;
        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
        {
            List<int> calories = new List<int>();

            string line;
            int highestCalorie = 0;
            int currentCalorie = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                //is it a linebreak or not
                if (string.IsNullOrEmpty(line))
                {
                    //end of elf
                    highestCalorie = HighestCalorie(highestCalorie, currentCalorie);

                    calories.Add(currentCalorie);
                    currentCalorie = 0;
                }
                else
                {
                    int calorie;
                    int.TryParse(line, out calorie);
                    currentCalorie += calorie;
                }
                //Console.WriteLine(line);
            }

            var result = calories.OrderByDescending(x => x).Take(3).ToList();

            foreach (int c in result)
            {
                Console.WriteLine(c);
            }

            highestCalorie = HighestCalorie(highestCalorie, currentCalorie);
            Console.WriteLine($"Highest was {highestCalorie}");
            Console.WriteLine($"Top 3 combined was {result.Sum()}");
        }        

        Console.ReadKey();
    }

    public static int HighestCalorie(int highest, int current)
    {
        if (current > highest)
        {
            Console.WriteLine($"New highest Calorie found!  {current} > {highest}");
            return current;
        }
        else
        {
            //Console.WriteLine($"Checked calories and current is not more than highest. {current} < {highest}");
            return highest;

        }
    }

    public static string ReadPage()
    {
        HttpClient client = new HttpClient(new HttpClientHandler());
        client.BaseAddress = new Uri(@"https://adventofcode.com/2022/day/1/input");
        Task<HttpResponseMessage> data = client.GetAsync("/");

        Task<string> webpage = data.Result.Content.ReadAsStringAsync();

        return webpage.Result;
    }
}
 