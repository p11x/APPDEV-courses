using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConApp2
{
    internal class Class55
    {
        string _path = "D:\\SprintParkOnsiteBatch\\ConsoleApplication\\ConApp2\\data.json";

        //Newtonsoft.Json
        public List<StudentTraining> UsingWithNewtonsoftJson()
        {
            using StreamReader reader = new StreamReader(_path);
            var json = reader.ReadToEnd();
            List<StudentTraining> students = JsonConvert.DeserializeObject<List<StudentTraining>>(json);
            return students;
        }


        //using JsonSerializer from NewtonSoft.Json
        public List<StudentTraining> UsingJsonTextReaderInNewtonsoftJson()
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            List<StudentTraining> students = new List<StudentTraining>();

            //StreamReader sr = new StreamReader(_path);
            //JsonTextReader jr = new JsonTextReader(sr);
            //students = serializer.Deserialize<List<StudentTraining>>(jr);
            //jr.Close();
            //sr.Close();


            using (var streamReader = new StreamReader(_path))
            {
                using (var textReader = new JsonTextReader(streamReader))
                {
                    students = serializer.Deserialize<List<StudentTraining>>(textReader);
                }
            }
            return students;
        }


        //Using JArray from NewtonSoft.Json
        public List<StudentTraining> UsingJArrayParseInNewtonsoftJson()
        {
            using StreamReader reader = new(_path);
            var json = reader.ReadToEnd();

            var jarray = JArray.Parse(json);

            List<StudentTraining> students = new List<StudentTraining>();

            foreach (var item in jarray)
            {
                StudentTraining student = item.ToObject<StudentTraining>();
                students.Add(student);
            }
            return students;
        }


        //Using System.Text.Json.JsonSerializer
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public List<StudentTraining> UsingFileReadAllTextWithSystemTextJson()
        {
            var json = File.ReadAllText(_path);
            List<StudentTraining> students = System.Text.Json.JsonSerializer.Deserialize<List<StudentTraining>>(json, _options);
            return students;
        }

        static void Main(string[] args)
        {
            Class55 obj = new Class55();
            var data = obj.UsingWithNewtonsoftJson();
            var data1 = obj.UsingJsonTextReaderInNewtonsoftJson();
            var data2 = obj.UsingJArrayParseInNewtonsoftJson();
            var data3 = obj.UsingFileReadAllTextWithSystemTextJson();


            foreach (var item in data)
            {
                Console.WriteLine("Student Id    :" + item.Id);
                Console.WriteLine("First Name is :" + item.FirstName);
                Console.WriteLine("Last Name is  :" + item.LastName);
                Console.WriteLine("Courses Joined are......");
                foreach (var course in item.Courses)
                {
                    Console.WriteLine("\tCourse Id    :" + course.Id);
                    Console.WriteLine("\tCourse Name  :" + course.CourseName);
                    Console.WriteLine("\tCourse Price :" + course.Price);
                }
                Console.WriteLine("\n");
            }
        }
    }



    class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
    class StudentTraining
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Course> Courses { get; set; }
    }
}
