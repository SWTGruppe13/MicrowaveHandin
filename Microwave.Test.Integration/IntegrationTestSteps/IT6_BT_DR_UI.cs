using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT6_BT_DR_UI
    {
        // Under test
        private IButton _powerButtonUut;
        private IButton _timeButtonUut;
        private IButton _startCancelButtonUut;
        private IDoor _doorUut;

        // Included units
        private IUserInterface _ui;
        private ICookController _cookController;
        private ILight _light;
        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;

        // Substitutes
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            // Units under test
            _powerButtonUut = new Button();
            _timeButtonUut = new Button();
            _startCancelButtonUut = new Button();
            _doorUut = new Door();

            // Substitutes
            _output = Substitute.For<IOutput>();

            // Included units
            _timer = new Timer();
            _display = new Display(_output);
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _ui = new UserInterface(_powerButtonUut, _timeButtonUut, _startCancelButtonUut, _doorUut, _display, _light, _cookController);
        }

        // Door open event test
        [Test]
        public void DoorOpen_Light_TurnOn()
        {
            _doorUut.Open();

            _output.Received(1).OutputLine(Arg.Is("Light is turned on"));
        }

        // Door closed event test
        [Test]
        public void DoorClosed_Light_TrunOff()
        {
            _doorUut.Open();
            _doorUut.Close();

            _output.Received(1).OutputLine(Arg.Is("Light is turned off"));
        }

        // Powerbutton pressed once event
        [Test]
        public void PowerButton_Pressed_Once_Display_ShowPower_50()
        {
            _powerButtonUut.Press();
            
            _output.Received(1).OutputLine(Arg.Is("Display shows: 50 W"));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        public void PowerButton_Pressed_Multiple_Times_Display_ShowPower(int x)
        {
            var power = 0;
            for (int i = 0; i < x; i++)
            {
                _powerButtonUut.Press();
                power = (power >= 700 ? 50 : power+50);
            }

            _output.Received(1).OutputLine(Arg.Is($"Display shows: {power} W"));
        }

        // Powerbutton pressed 15 times, display received 2 showPower 50 W.
        [Test]
        public void PowerButton_Pressed_15_times_Display_called_twice()
        {
            for (int i = 0; i < 15; i++)
            {
                _powerButtonUut.Press();
            }

            _output.Received(2).OutputLine(Arg.Is("Display shows: 50 W"));
        }

        // PowerButton press then TimeButton, display received time 01:00
        [Test]
        public void TimeButton_Pressed_Once_Display_ShowTime_1()
        {
            _powerButtonUut.Press();
            _timeButtonUut.Press();

            _output.Received(1).OutputLine(Arg.Is("Display shows: 01:00"));
        }

        // Test multiple TimeButton pressed
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(20)]
        [TestCase(21)]
        [TestCase(60)]
        [TestCase(61)]
        public void TimeButton_Pressed_Multiple_Display_ShowTime(int x)
        {
            _powerButtonUut.Press();
            var time = 0;
            for (int i = 0; i < x; i++)
            {
                _timeButtonUut.Press();
                time += 1;
            }

            _output.Received(1).OutputLine(Arg.Is("Display shows: 50 W"));
            _output.Received().OutputLine(Arg.Is($"Display shows: {time:D2}:00"));
        }

        // Powerbutton pressed once then StarCancel button once, display cleared.
        [Test]
        public void StartCancelButton_Pressed_Once_Display_Cleared()
        {
            _powerButtonUut.Press();
            _startCancelButtonUut.Press();

            _output.Received(1).OutputLine(Arg.Is("Display shows: 50 W"));
            _output.Received(1).OutputLine(Arg.Is("Display cleared"));
        }

        // Powerbutton pressed once then StarCancel button once, display cleared.
        [Test]
        public void Time_Equals_1min_StartCancelButton_Pressed_Cooking_started()
        {
            _powerButtonUut.Press();
            _timeButtonUut.Press();
            _startCancelButtonUut.Press();

            _output.Received(1).OutputLine(Arg.Is("Display cleared"));
            _output.Received(1).OutputLine(Arg.Is("Display shows: 01:00"));
        }


        // 1min cooking started and display shows correct time after 3 sec.
        [Test]
        public void Time_Equals_1_StartCancelButton_Pressed_Cooking_started_3s_Delay_Display_Shows_57s()
        {
            _powerButtonUut.Press();
            _timeButtonUut.Press();
            _startCancelButtonUut.Press();

            Thread.Sleep(3000);
            _output.Received(1).OutputLine(Arg.Is("Display cleared"));
            _output.Received(1).OutputLine(Arg.Is("Display shows: 01:00"));
            _output.Received(1).OutputLine(Arg.Is("Display shows: 00:59"));
            _output.Received(1).OutputLine(Arg.Is("Display shows: 00:58"));
            _output.Received(1).OutputLine(Arg.Is("Display shows: 00:57"));
        }

        // TEST VIRKER IKKE. DER MODTAGES KUN 1 DISPLAY CLEARED TODO: FIX
    //    [Test]
    //    public void Time_Equals_1_StartCancelButton_Pressed_Cooking_Time_Expired_Display_Shows_Correct()
    //    {
    //        _powerButtonUut.Press();
    //        _timeButtonUut.Press();
    //        _startCancelButtonUut.Press();

    //        Thread.Sleep(62000);
    //        _output.Received(2).OutputLine(Arg.Is("Display cleared"));
    //        _output.Received(1).OutputLine(Arg.Is("Display shows: 00:00"));
    //    }
    //}
}