using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class EasyredmienWrapper
    {
        private HttpClient client = new HttpClient();
        private readonly string APIKey = "";
        private readonly string url = "";
        private JsonSerializerSettings jsonIgnoreNull = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public EasyredmienWrapper()
        {

            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Redmine-API-Key", APIKey);


        }


        public async Task<List<User>> GetUsersAsync()
        {
            return await Task.Run(() => GetUsers());
        }

        public List<User> GetUsers(CancellationToken cancelToken = new CancellationToken())
        {
            cancelToken.ThrowIfCancellationRequested();
            int pageCount = 1;
            UsersRootObject root = null;
            string stringResult = null;
            List<User> users = new List<User>();
            do
            {
                HttpResponseMessage response = null;

                response = client.GetAsync("users.json" + $"?limit=100&page={pageCount}", cancelToken).Result;
                //response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    stringResult = response.Content.ReadAsStringAsync().Result;
                    root = JsonConvert.DeserializeObject<UsersRootObject>(stringResult);
                    users.AddRange(root.users);
                }
                else
                {

                    return users;
                }
                pageCount++;
            } while (root != null && root.offset + root.users.Count() < root.total_count);

            return users;
        }
    }
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mail { get; set; }
        public List<CustomField> custom_fields { get; set; }

        public string Initials
        {
            get
            {
                if (custom_fields != null && custom_fields.Any(c => c.name == "Initials"))
                {
                    return custom_fields.First(c => c.name.Equals("Initials")).value != null ? (string)custom_fields.First(c => c.name.Equals("Initials")).value : string.Empty;
                }
                return string.Empty;
            }
        }
    }

    public class UserRootObject
    {
        public User user { get; set; }
    }

    public class UsersRootObject
    {
        public List<User> users { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }

    public class CustomField
    {
        public int id { get; set; }
        public string name { get; set; }
        public string internal_name { get; set; }
        public string field_format { get; set; }
        public object value { get; set; }
    }
}
