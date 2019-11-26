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
            _output = Substitute.For<Output>();
            _light = Substitute.For<Light>(_output);
            _powerButton = Substitute.For<Button>();
            _timeButton = Substitute.For<Button>();
            _startCancelButton = Substitute.For<Button>();
            _Door = Substitute.For<Door>();

            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _uut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _Door, _display, _light, _cookController);
        }

        [Test]
        public void OnPowerPressedFromReadyOutput()
        {
            _powerButton.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void OnPowerPressedFromSetPowerOutput()
        {
            _powerButton.Press();
            _powerButton.Press();
            _output.Received().OutputLine("Display shows: 100 W");
        }

        [Test]
        public void OnTimePressed_From_SetPower_Output()
        {
            _powerButton.Press(); // Sets state to SetPower
            _timeButton.Press();
            _output.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void OnTimePressed_From_SetTime_Output()
        {
            _powerButton.Press(); // Sets state to SetPower
            _timeButton.Press(); // Sets state to SetTime
            _timeButton.Press();
            _output.Received().OutputLine("Display shows: 02:00");
        }

        [Test]
        public void OnStartCancelPressed_From_SetPower_ClearDisplay_LightOff()
        {
            _powerButton.Press(); // Sets state to SetPower
            _startCancelButton.Press();
            _output.Received().OutputLine("Display cleared");
            _light.Received().TurnOff();
        }

        [Test]
        public void OnStartCancelPressed_From_SetTime_Display_Clear()
        {
            _powerButton.Press(); // Sets state to SetPower
            _timeButton.Press(); // Sets state to SetTime
            _startCancelButton.Press();
            _output.Received().OutputLine("Display cleared");
            _light.Received().TurnOn();
            // Called from cook controller
            _output.Received().OutputLine("PowerTube turned off");
            _timer.
        }

        [Test]
        public void OnStartCancelPressed_From_SetPower_Display_Clear()
        {
            _powerButton.Press(); // Sets state to SetPower
            _startCancelButton.Press();
            _output.Received().OutputLine("Display cleared");
            _light.Received().TurnOff();
        }
    }
}