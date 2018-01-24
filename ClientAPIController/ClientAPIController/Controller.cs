//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace ClientAPIController
//{
//    class Controller
//    {
//        the shit we might use
//        public static string clientApiUrl { get; set; }
//        public static async Task<bool> CheckClientStatus()
//        {
//            clientApiUrl = "http://localhost:63796/api/values";
//            using (var client = new HttpClient())
//            {
//                HttpResponseMessage response = await client.GetAsync(clientApiUrl);
//                if (response.IsSuccessStatusCode)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }

//        [HttpPost]
//        public static async Task<bool> PushEmployeeUpdate(string newEmployee)
//        {
//            var employeesToUpdate = JsonConvert.DeserializeObject<List<Employee>>(newEmployee);

//            update db with employee details in list


//            add updated employees to update notification



//            send status of success / failure
//        }

//        [HttpGet]
//        public static async Task<C> GetEmployees()
//        {
//            using (var client = new HttpClient())
//            {
//                HttpResponseMessage response = await client.GetAsync(clientApiUrl);
//                if (response.IsSuccessStatusCode)
//                {
//                    string result = await response.Content.ReadAsStringAsync();
//                    var rootresult = JsonConvert.DeserializeObject<List<Test>>(result);
//                    return rootresult;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }

//        public static void SyncEmployees()
//        {

//        }
//    }
//}
