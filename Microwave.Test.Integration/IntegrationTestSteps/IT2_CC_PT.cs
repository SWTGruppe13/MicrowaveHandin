using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
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

        [TestCase(50)] // min
        [TestCase(51)] // min +1
        [TestCase(100)] // random
        [TestCase(200)] // random
        [TestCase(699)] // max -1
        [TestCase(700)] // max
        public void PowerTube_turnOn_called_output_right(int power)
        {
            _uut.StartCooking(power,1);
            _fakeOutput.Received().OutputLine($"PowerTube works with {power} W");
        }

        [Test]
        public void PowerTube_turnOff_called_output_right() //husk fejl ved power kun kan være mellem 0 - 100 burde være 50 - 700. - Skulle være fixet nu
        {
            _uut.StartCooking(50,50);
            _uut.Stop();
            _fakeOutput.Received().OutputLine($"PowerTube turned off");
        }


        [TestCase(49)] // min
        [TestCase(-50)] // negative
        [TestCase(1000)] // random
        [TestCase(701)] // max
        public void Throw_OutOfRange_NotAllowed(int power)
        {
            Assert.That(() => _uut.StartCooking(power,50), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Throw_IsAllReadyOn_NotAllowed()
        {
            _uut.StartCooking(50,60);
            Assert.That(() => _uut.StartCooking(50, 50), Throws.TypeOf<ApplicationException>());
        }
    }
}