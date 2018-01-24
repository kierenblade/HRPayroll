using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ClientAPIController
{
    class Program
    {
        //demoBoye
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            var results = GetPosts().GetAwaiter().GetResult();
            //foreach (var item in results)
            //{
            //    Console.WriteLine(item);
            //}
            Console.WriteLine(results); 
            Console.ReadLine();
        }

        public static async Task<string> GetPosts()
        {
            //string URL = "https://jsonplaceholder.typicode.com/posts";
            //using (var client = new HttpClient())
            //{
            //    HttpResponseMessage response = await client.GetAsync(URL);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        string result = await response.Content.ReadAsStringAsync();
            //        var rootresult = JsonConvert.DeserializeObject<List<Test>>(result);
            //        return rootresult;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}

            string url = "http://localhost:63796/api/login";
            using (var client = new HttpClient())
            {
                var mycontent = JsonConvert.SerializeObject("JSON");
                var buffer = System.Text.Encoding.UTF8.GetBytes(mycontent);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(url, bytecontent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    //var rootresult = JsonConvert.DeserializeObject<List<string>>(result);
                    return result;
                }
                else
                {
                    return null;
                }
            }

            //string url = "http://localhost:63796/api/Login/AlternativeGet";
            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetAsync(url);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string result = await response.Content.ReadAsStringAsync();
            //        //var rootresult = JsonConvert.DeserializeObject<List<string>>(result);
            //        return result;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }
        public class Tshief
        {
            public string name { get; set; }
            public string status { get; set; }
        }
        public class Test
        {
            public int Userid { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}
