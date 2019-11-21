using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration.IntegrationTestSteps
{
    [TestFixture]
    public class IT2_CC_PT
    {
        private IUserInterface _fakeUserInterface;
        private CookController _uut;
        private ITimer _fakeTimer;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IOutput _fakeOutput;


        [SetUp]
        public void SetUp()
        {
            _fakeUserInterface = Substitute.For<IUserInterface>();
            _fakeTimer = Substitute.For<ITimer>();
            _fakeOutput = Substitute.For<IOutput>();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);
            _uut = new CookController(_fakeTimer,_display,_powerTube,_fakeUserInterface);
        }

        [Test]
        public void PowerTube_turnOn_called_output_right()
        {
            _uut.StartCooking(50,1);
            _fakeOutput.Received().OutputLine($"PowerTube works with 50 %");
        }

        [Test]
        public void PowerTube_turnOff_called_output_right() //husk fejl ved power kun kan være mellem 0 - 100 burde være 50 - 700.
        {
            _uut.StartCooking(50,50);
            _uut.Stop();
            _fakeOutput.Received().OutputLine($"PowerTube turned off");
        }

    }
}