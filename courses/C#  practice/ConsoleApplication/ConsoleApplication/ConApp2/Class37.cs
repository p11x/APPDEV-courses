using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Anonymous Objects 
    //All properties in anonymous object are by default readonly
    internal class Class37
    {
        static void Main(string[] args)
        {
            var myObject = new {UserId=101, UserName="Sathesh", UserLocation="Hyderabad" };

            //myObject.UserLocation = "Chennai";

            Console.WriteLine(myObject.UserId + "\t" + myObject.UserName + "\t" + myObject.UserLocation);
        }
    }
}
