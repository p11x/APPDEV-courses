using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    class Geo
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
    class Address
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public Geo Geo { get; set; }
    }
    class CompanyDetails
    {
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        public string Bs { get; set; }
    }
    class UserDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public CompanyDetails Company { get; set; }
    }
    internal class Class56
    {
        public static async void GetUserDetails()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = client.GetAsync("https://jsonplaceholder.typicode.com/users").Result;
                    var response = await result.Content.ReadAsStringAsync();


                    var users = JsonConvert.DeserializeObject<List<UserDetails>>(response);

                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Id + "\t\t" + user.Name + "\t\t" + user.Email + "\t\t" + user.Phone
                            + "\t\t" + user.Website);

                        Console.WriteLine("Address Details............");
                        Console.WriteLine(user.Address.City + "\t" + user.Address.Zipcode);
                        Console.WriteLine(user.Address.Geo.Lat + "\t" + user.Address.Geo.Lng);
                        Console.WriteLine("Company Details.............");
                        Console.WriteLine(user.Company.Name + "\t" + user.Company.CatchPhrase);

                        Console.WriteLine("\n\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Main(string[] args)
        {
            GetUserDetails();
        }
    }
}
