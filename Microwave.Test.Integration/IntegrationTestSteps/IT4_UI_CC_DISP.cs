using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT4_UI_CC_DISP
    {
        // Unit under test
        public IUserInterface _uut;

        // Fakes
        public IOutput _output;
        public ILight _light;
        public IButton _powerButton;
        public IButton _timeButton;
        public IButton _startCancelButton;
        public IDoor _Door;

        // Included units
        public ITimer _timer;
        public IDisplay _display;
        public IPowerTube _powerTube;
        public ICookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _light = Substitute.For<ILight>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _Door = Substitute.For<IDoor>();

            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _uut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _Door, _display, _light, _cookController);
        }

        [Test]
        public void PowerButtonPressedOnce()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); 
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void PowerButtonPressedTwice()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty);
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.Received().OutputLine("Display shows: 100 W");
        }

        [Test]
        public void PowerButtonPressedMaxAllowedTimes()
        {
            for (int i = 0; i < 14; i++)
            {
                _uut.OnPowerPressed(_powerButton, EventArgs.Empty);
            }
            _output.Received().OutputLine("Display shows: 700 W");
        }

        [Test]
        public void PowerButtonPressedOverMaxAllowedTimes()
        {
            for (int i = 0; i < 15; i++)
            {
                _uut.OnPowerPressed(_powerButton, EventArgs.Empty);
            }
            _output.DidNotReceive().OutputLine("Display shows: 750 W");
            _output.Received(2).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void TimeButtonPressed_From_Ready_State()
        {
            _uut.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.DidNotReceiveWithAnyArgs().OutputLine("");
        }

        [Test]
        public void TimeButtonPressed_From_SetPower_State()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty);

            _output.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void TimeButtonPressed_Once_From_SetTime_State()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // Sets state to SetTime

            _uut.OnTimePressed(_timeButton, EventArgs.Empty);

            _output.Received().OutputLine("Display shows: 02:00");
        }

        [Test]
        public void TimeButtonPressed_Twice_From_SetTime_State()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // Sets state to SetTime

            for (int i = 0; i < 2; i++)
            {
                _uut.OnTimePressed(_timeButton, EventArgs.Empty);
            }

            _output.Received().OutputLine("Display shows: 03:00");
        }

        [Test]
        public void TimeButtonPressed_9_Times_From_SetTime_State()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // Sets state to SetTime

            for (int i = 0; i < 9; i++)
            {
                _uut.OnTimePressed(_timeButton, EventArgs.Empty);
            }

            _output.Received().OutputLine("Display shows: 10:00");
        }

        [Test]
        public void TimeButtonPressed_99_Times_From_SetTime_State()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // Sets state to SetTime

            for (int i = 0; i < 99; i++)
            {
                _uut.OnTimePressed(_timeButton, EventArgs.Empty);
            }

            _output.Received().OutputLine("Display shows: 100:00");
        }

        [Test]
        public void TimeButtonPressed_100_Times_From_SetTime_State()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // Sets state to SetTime

            for (int i = 0; i < 100; i++)
            {
                _uut.OnTimePressed(_timeButton, EventArgs.Empty);
            }

            _output.Received().OutputLine("Display shows: 101:00");
        }

        [Test]
        public void OnStartCancelPressed_From_SetPower_ClearDisplay_LightOff()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.Received().OutputLine("Display cleared");
            _light.Received().TurnOff();
        }

        [Test]
        public void OnStartCancelPressed_From_SetTime_ClearDisplay()
        {
            _uut.OnPowerPressed(_powerButton, EventArgs.Empty); // Sets state to SetPower
            _uut.OnTimePressed(_timeButton, EventArgs.Empty); // Sets state to SetTime
            _uut.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.Received().OutputLine("Display cleared"); 
        }
    }
}