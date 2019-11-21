using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;


namespace Microwave.Test.Integration.IntegrationTestSteps
{
    [TestFixture]
    public class IT3_CC_TMD
    {
        private IUserInterface _fakeUserInterface;
        private IOutput _fakeOutput;

        private CookController _uut;
        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;
        

        [SetUp]
        public void SetUp()
        {
            _fakeUserInterface = Substitute.For<IUserInterface>();
            _fakeOutput = Substitute.For<IOutput>();

            _timer = new Timer();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);

            _uut = new CookController(_timer, _display, _powerTube,_fakeUserInterface);

        }

        [Test]
        public void OnTimerExpiredOutputIsCalled()
        {
            _uut.StartCooking(20,1);

            Thread.Sleep(1000);

            _fakeOutput.Received().OutputLine($"PowerTube turned off");
        }

        [Test]
        public void dingdong()
        {
            _uut.StartCooking(20, 1);

            Thread.Sleep(1000);

            _fakeOutput.Received().OutputLine($"PowerTube turned off");
        }

    }
}


