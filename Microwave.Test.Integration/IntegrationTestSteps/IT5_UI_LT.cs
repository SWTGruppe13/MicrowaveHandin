using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT5_UI_LT
    {
        // Unit under test
        public UserInterface _uut;

        // Fakes
        public IOutput _output;
        public IButton _powerButton;
        public IButton _timeButton;
        public IButton _startCancelButton;
        public IDoor _Door;

        // Included units
        public ITimer _timer;
        public IDisplay _display;
        public IPowerTube _powerTube;
        public ICookController _cookController;
        public ILight _light;


        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            
            _powerButton = Substitute.For<Button>();
            _timeButton = Substitute.For<Button>();
            _startCancelButton = Substitute.For<Button>();
            _Door = Substitute.For<Door>();

            _light = new Light(_output);
            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _uut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _Door, _display, _light, _cookController);
        }

        [Test]
        public void OnStartCancelPressed_StateSetPower_LightOffNotCalled_AlreadyOff()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty);
            _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.DidNotReceive().OutputLine("Light is turned off");
        }

        //[Test]
        //public void OnStartCancelPressed_StateSetPower_LightOn()
        //{
        //    _uut.OnPowerPressed(_powerButton, EventArgs.Empty);
        //    _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            
        //    _output.Receive().OutputLine("Light is turned off");
        //}

        [Test]
        public void OnStartCancelPressed_StateSetTime_LightTurnsOn() // works
        {
            _uut.OnPowerPressed(_powerButton,EventArgs.Empty); // Ready -> SetPower
            _uut.OnTimePressed(_timeButton,EventArgs.Empty); // SetPower -> SetTime
            _uut.OnStartCancelPressed(_startCancelButton,EventArgs.Empty);
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnStartCancelPressed_StateCooking_LightTurnsOff() // works
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Ready -> SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // SetPower -> SetTime
            _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty); // SetTime -> Cooking
            _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void OnDoorOpened_StateReady_LightTurnsOn()
        {
            _uut.OnDoorOpened(_Door,EventArgs.Empty);
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorOpened_StateSetPower_LightTurnsOn()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Ready -> SetPower
            _uut.OnDoorOpened(_Door, EventArgs.Empty);
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorOpened_StateSetTime_LightTurnsOn()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Ready -> SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // SetPower -> SetTime
            _uut.OnDoorOpened(_Door, EventArgs.Empty);
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorClosed_StateDoorOpen_LightTurnsOff()
        {
            _uut.OnDoorOpened(_Door, EventArgs.Empty); // Ready -> DoorOpen
            _uut.OnDoorClosed(_Door, EventArgs.Empty);
            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void CookingDone_StateCooking_LightTurnsOff()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Ready -> SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // SetPower -> SetTime
            _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty); // SetTime -> Cooking
            _uut.CookingIsDone();
            _output.Received().OutputLine("Light is turned off");
        }
    }
}


  