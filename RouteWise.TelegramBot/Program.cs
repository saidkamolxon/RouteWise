using RouteWise.Service.Services;
using RouteWise.Service.Services.FleetLocate;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        string vas = "2575 State Hwy 414, Fort Bridger, WY 82933, USA";
        Console.WriteLine(vas.Substring(vas.Length-13, 2));
    }
}