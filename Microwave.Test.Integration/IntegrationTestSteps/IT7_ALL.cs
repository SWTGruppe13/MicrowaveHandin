using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT7_ALL
    {
        private Output _uut;

        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Door _door;
        private UserInterface _ui;
        private CookController _cookController;
        private Light _light;
        private Timer _timer;
        private Display _display;
        private PowerTube _powerTube;

        [SetUp]
        public void SetUp()
        {
            _uut = new Output();

            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _timer = new Timer();

            _display = new Display(_uut);
            _light = new Light(_uut);
            _powerTube = new PowerTube(_uut);

            _cookController = new CookController(_timer, _display, _powerTube);
            _ui = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _ui;
        }

        [Test]
        public void test()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

        }
    }
}