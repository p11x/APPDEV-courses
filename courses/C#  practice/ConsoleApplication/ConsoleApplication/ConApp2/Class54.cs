using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Reading Data from JSON File
    internal class Class54
    {
        static void Main(string[] args)
        {
            string path = "D:\\SprintParkOnsiteBatch\\ConsoleApplication\\ConApp2\\Employees.json";

            try
            {
                StreamReader sr = new StreamReader(path);
                string st= sr.ReadToEnd();
                sr.Close();

                List<JsonEmployee> list = JsonConvert.DeserializeObject<List<JsonEmployee>>(st);

                foreach (JsonEmployee emp in list)
                {
                    Console.WriteLine(emp.Id+"\t"+emp.FirstName+"\t"+emp.LastName+"\t"+emp.Age);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

    class JsonEmployee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
