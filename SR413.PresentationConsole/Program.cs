using SR413.DataLayer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SR413.PresentationConsole
{
    class Program
    {
        static readonly HttpClient client = new();
        static void Main()
        {
            client.BaseAddress = new Uri("http://localhost:38581/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("====Starting=====");
            // Post data
            var brand = new Brand
            {
                BrandName = "Testing",
                Description = "test"
            };
            PostBrandAsync(brand).Wait();
            //Call Get Brand
            GetBrandAsync().Wait();
            Console.WriteLine("=========Finished========");
            Console.ReadKey();
        }
        static async Task GetBrandAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/brands");
            if (response.IsSuccessStatusCode)
            {
                var brands = await response.Content.ReadAsAsync<List<Brand>>();
                if (brands == null) Console.WriteLine("No record");
                foreach(var item in brands)
                {
                    Console.WriteLine("Brand Name:{0}", item.BrandName);
                }
            }
            else
            {
                var errormsg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine("Error Message:{0}", errormsg);
            }
        }
        static async Task PostBrandAsync(Brand brand)
        {
            var response = await client.PostAsJsonAsync<Brand>("api/brands", brand);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("=====Record was saved====");
            }
            else
            {
                var error = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(error);
            }

        }
    }
}
