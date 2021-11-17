using System.Collections;
using System.Linq;

namespace BusinessLayer.Validators
{
    public class LicenseplateValidator
    {
        public bool isValid(string licenseplate)
        {
            string tocheck = "";

            foreach (char c in licenseplate)
            {
                if (c != '.' && c != '-' && c != ' ')
                {
                    tocheck = tocheck + c;
                }
            }

            if (tocheck.Length != 7) return false;
            if (!char.IsDigit(tocheck[0])) return false;
            for (int i = 1; i != 3; i++)
            {
                if (char.IsDigit(tocheck[i])) return false;
            }
            for (int i = 4; i != 6; i++)
            {
                if (!char.IsDigit(tocheck[i])) return false;
            }

            return true;

        }
    }
}