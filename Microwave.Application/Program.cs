using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;

namespace Microwave.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            // Comment to test webhook
            Button startCancelButton = new Button();
            Button powerButton = new Button();
            Button timeButton = new Button();

            Door door = new Door();

            Output output = new Output();

            Display display = new Display(output);

            PowerTube powerTube = new PowerTube(output);

            Light light = new Light(output);

            MicrowaveOvenClasses.Boundary.Timer timer = new Timer();

            CookController cooker = new CookController(timer, display, powerTube);

            UserInterface ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);

            // Finish the double association
            cooker.UI = ui;

            // Simulate a simple sequence

            powerButton.Press();

            timeButton.Press();

            startCancelButton.Press();

            // The simple sequence should now run

            System.Console.WriteLine("When you press enter, the program will stop");

            //Output output = new Output();
            //Light light = new Light(output);
            //Display display = new Display(output);
            //PowerTube powerTube = new PowerTube(output);

            //light.TurnOn();
            //light.TurnOff();
            //Console.WriteLine();
            //display.ShowPower(100);
            //display.ShowTime(10, 10);
            //display.Clear();
            //Console.WriteLine();
            //powerTube.TurnOff();
            //powerTube.TurnOn(50);

            // Wait for input
            System.Console.ReadLine();
        }
    }
}
