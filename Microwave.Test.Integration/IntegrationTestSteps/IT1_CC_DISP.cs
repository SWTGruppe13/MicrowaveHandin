using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_CC_DISP
    {
        private IUserInterface _fakeUi;
        private ITimer _fakeTimer;
        private IPowerTube _fakePowerTube;
        private IOutput _fakeOutput;
        private CookController _uut;
        private IDisplay _display;

        [SetUp]
        public void SetUp()
        {
            _fakeUi = Substitute.For<IUserInterface>();
            _fakeTimer = Substitute.For<ITimer>();
            _fakePowerTube = Substitute.For<IPowerTube>();
            _fakeOutput = Substitute.For<IOutput>();
            _display = new Display(_fakeOutput);

            _uut = new CookController(_fakeTimer,_display,_fakePowerTube, _fakeUi);
        }

        [Test]
        public void test123()
        {

        }

    }
}