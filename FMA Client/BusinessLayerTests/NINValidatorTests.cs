using System;
using BusinessLayer.Validators;
using Xunit;

namespace BusinessLayerTests
{
    public class NINValidatorTests
    {

        //Valid strings
        private string valid20thcenturyPunct = "93.11.23-283.87";
        private string valid20thcenturyNoPunct = "93112328387";

        private string valid21stcenturyNoPunct = "00012556777";
        private string valid21stcenturyPunct = "00.01.25.567-77";

        //Invalid strings
        private string invalid21stcenturyNoPunct = "00012556778";
        private string invalid21stcenturyPunct = "00.01.25.567-78";

        private string invalid20thcenturyPunct = "93.11.23-283.88";
        private string invalid20thcenturyNoPunct = "93112328388";



        private NINValidator toTest = new NINValidator();

        [Fact]
        public void isValid_true_20thPunct()
        {
            Assert.True(toTest.isValid(valid20thcenturyPunct));
        }

        [Fact]
        public void isValid_true_20thNoPunct()
        {
            Assert.True(toTest.isValid(valid20thcenturyNoPunct));
        }


        [Fact]
        public void isValid_true_21stNoPunct()
        {
            Assert.True(toTest.isValid(valid21stcenturyNoPunct));
        }

        [Fact]
        public void isValid_true_21stPunct()
        {
            Assert.True(toTest.isValid(valid21stcenturyPunct));
        }

        [Fact]
        public void isValid_false_21stNoPunct()
        {
            Assert.False(toTest.isValid(invalid21stcenturyNoPunct));
        }
        [Fact]
        public void isValid_false_21stPunct()
        {
            Assert.False(toTest.isValid(invalid21stcenturyPunct));
        }

        [Fact]
        public void isValid_false_20thNoPunct()
        {
            Assert.False(toTest.isValid(invalid20thcenturyNoPunct));
        }
        [Fact]
        public void isValid_false_20thPunct()
        {
            Assert.False(toTest.isValid(invalid20thcenturyPunct));
        }
    }
}
