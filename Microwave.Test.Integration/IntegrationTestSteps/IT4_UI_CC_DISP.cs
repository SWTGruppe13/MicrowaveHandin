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
            _light = Substitute.For<Light>();
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
        public void Test()
        {
            _powerButton.Press();
            _output.Received().OutputLine("Display shows: 50 W");
        }
    }
}