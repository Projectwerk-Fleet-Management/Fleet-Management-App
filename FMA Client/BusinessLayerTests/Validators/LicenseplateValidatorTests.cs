using BusinessLayer.Validators;
using Xunit;

namespace BusinessLayerTests.Validators
{
    public class LicenseplateValidatorTests
    {
        private string validLicenseplate_extraChars = "1-xxx-000";
        private string validLicenseplate = "1xxx000";
        private string invalidLicenseplate_extraChars = "1-x2x-0x0";
        private string invalidLicenseplate = "1x2x0x0";

        private LicenseplateValidator Validator = new LicenseplateValidator();
        [Fact]
        public void isValid_Licenseplate_nochars()
        {
           Assert.True(Validator.isValid(validLicenseplate));
        }
        [Fact]
        public void isValid_Licenseplate_withchars()
        {
            Assert.True(Validator.isValid(validLicenseplate_extraChars));
        }

        [Fact]
        void isInValid_Licenseplate_nochars()
        {
            Assert.False(Validator.isValid(invalidLicenseplate));
        }
        [Fact]
        public void isInValid_Licenseplate_withchars()
        {
            Assert.False(Validator.isValid(invalidLicenseplate_extraChars));
        }
    }
}