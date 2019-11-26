using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;


namespace Microwave.Test.Integration
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
            _uut.StartCooking(50,1);

            Thread.Sleep(2000);

            _fakeOutput.Received().OutputLine($"PowerTube turned off");
        }

        [Test]
        public void OnTimerExpiredUiIsCalled()
        {
            _uut.StartCooking(80,2);

            Thread.Sleep(3000);

            _fakeUserInterface.Received().CookingIsDone();
        }

        [Test]
        public void CookingStarted_ThenStopped_OutputCalledOnce_AfterTimeExpired()
        {
            _uut.StartCooking(80, 2);

            _uut.Stop();

            Thread.Sleep(3000);

            _fakeOutput.Received(1).OutputLine($"PowerTube turned off");
        }

        [Test]
        public void OnTimerTickOutputCalledCorrectTime()
        {
            _uut.StartCooking(80, 60);

            Thread.Sleep(2000);

            _fakeOutput.Received().OutputLine($"Display shows: 00:59");
        }
    }
}


