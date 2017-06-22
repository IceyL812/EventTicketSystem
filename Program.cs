using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventTicketSystem.System_Classes;

namespace EventTicketSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Math.Min(115, Console.LargestWindowWidth), Math.Min(35, Console.LargestWindowHeight));

            World mWorld = new World();
            DataGenerator mDataGenerator = new DataGenerator();
            UserInterface mUI = new UserInterface();
        }
    }

}
