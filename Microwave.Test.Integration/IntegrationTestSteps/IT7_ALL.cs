using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration.IntegrationTestSteps
{
    [TestFixture]
    public class IT7_ALL
    {
        private Output _uut;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IUserInterface _ui;
        private ICookController _cookController;
        private ILight _light;
        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;

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
        }
    }
}