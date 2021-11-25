using Xunit;
using BusinessLayer.Validators;
using System;
using BusinessLayer.Exceptions;
using System.Collections.Generic;
using BusinessLayer;
using BusinessLayer.Model;

namespace BusinessLayerTests.Validators
{
    public class VINValidatorTests
    {
        //valid strings
        private string isValidVIN1 = "1J4FA24127L200905";
        private string isValidVIN2 = "5NPE24AF5FH147002";
        private string isValidVIN3 = "1G2ZG57N984137853";

        //invalid strings
        private string VINIsEmptey = null;
        private string VINIsSpace = "";
        private string VINIsToLong = "1G2ZG57N9841378534";
        private string VINContainsChar = "1G2ZG57@984137853";

        private VINValidator toTest = new VINValidator();

        [Fact]
        public void isValid_true_VIN1()
        {
            Assert.True(toTest.IsValid(isValidVIN1));
        }
        [Fact]
        public void isValid_true_VIN2()
        {
            Assert.True(toTest.IsValid(isValidVIN2));
        }
        [Fact]
        public void isValid_true_VIN3()
        {
            Assert.True(toTest.IsValid(isValidVIN3));
        }
        [Fact]
        public void isInValid_true_VINIsEmpty()
        {
            var ex = Assert.Throws<VINValidatorException>(() => toTest.IsValid(VINIsEmptey));
            Assert.Equal("VINValidator - VIN is empty", ex.Message);
        }
        [Fact]
        public void isInValid_true_VINIsToLong()
        {
            var ex = Assert.Throws<VINValidatorException>(() => toTest.IsValid(VINIsToLong));
            Assert.Equal($"VINValidator - VIN length isn't equal to 17", ex.Message);
        }
        [Fact]
        public void isInValid_true_VINIsSpace()
        {
            var ex = Assert.Throws<VINValidatorException>(() => toTest.IsValid(VINIsSpace));
            Assert.Equal("VINValidator - VIN is empty", ex.Message);
        }
        [Fact]
        public void isInValid_true_VINContainsChar()
        {
            var ex = Assert.Throws<VINValidatorException>(() => toTest.IsValid(VINContainsChar));
            Assert.Equal("VINValidator - VIN contains an invalid character (I/O/Q/...)", ex.Message);
        }
    }
}

