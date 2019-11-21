using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace Microwave.Test.Integration.IntegrationTestSteps
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
    }
}