using System;
using BusinessLayer.Exceptions;
using System.Collections.Generic;

namespace BusinessLayer.Validators
{
    public class VINValidator
    {
        private int _VINLength = 17; // VIN has a length of 17 (used to be 11 before 1981 but that's not the case anymore)
        // https://en.wikibooks.org/wiki/Vehicle_Identification_Numbers_(VIN_codes)/Check_digit refernce to the wikipedia page used for the validation of vins

        public bool IsValid(string vin)
        {
            int sum = 0;
            int checkDigit = 0;
            string allowedLetters = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789";
            if (string.IsNullOrWhiteSpace(vin)) throw new VINValidatorException("VINValidator - VIN is empty");
            if (vin.Length != _VINLength) throw new VINValidatorException($"VINValidator - VIN length isn't equal to {_VINLength}");

            foreach (char c in vin)
            {
                if (!allowedLetters.Contains(c))
                {
                    throw new VINValidatorException("VINValidator - VIN contains an invalid character (I/O/Q/...)");
                }
            }

            vin.ToUpper();

            Dictionary<char, int> charReplacerList = new Dictionary<char, int>();
            Dictionary<char, int> charReplacerListCheckDigit = new Dictionary<char, int>();

            #region Dictionary normal replacer insertion
            charReplacerList.Add('A', 1);
            charReplacerList.Add('B', 2);
            charReplacerList.Add('C', 3);
            charReplacerList.Add('D', 4);
            charReplacerList.Add('E', 5);
            charReplacerList.Add('F', 6);
            charReplacerList.Add('G', 7);
            charReplacerList.Add('H', 8);
            charReplacerList.Add('J', 1);
            charReplacerList.Add('K', 2);
            charReplacerList.Add('L', 3);
            charReplacerList.Add('M', 4);
            charReplacerList.Add('N', 5);
            charReplacerList.Add('P', 7);
            charReplacerList.Add('R', 9);
            charReplacerList.Add('S', 2);
            charReplacerList.Add('T', 3);
            charReplacerList.Add('U', 4);
            charReplacerList.Add('V', 5);
            charReplacerList.Add('W', 6);
            charReplacerList.Add('X', 7);
            charReplacerList.Add('Y', 8);
            charReplacerList.Add('Z', 9);
            charReplacerList.Add('1', 1);
            charReplacerList.Add('2', 2);
            charReplacerList.Add('3', 3);
            charReplacerList.Add('4', 4);
            charReplacerList.Add('5', 5);
            charReplacerList.Add('6', 6);
            charReplacerList.Add('7', 7);
            charReplacerList.Add('8', 8);
            charReplacerList.Add('9', 9);
            charReplacerList.Add('0', 0);
            #endregion
            #region Dictionary replacer checkDigit insertion
            charReplacerListCheckDigit.Add('0', 0);
            charReplacerListCheckDigit.Add('1', 1);
            charReplacerListCheckDigit.Add('2', 2);
            charReplacerListCheckDigit.Add('3', 3);
            charReplacerListCheckDigit.Add('4', 4);
            charReplacerListCheckDigit.Add('5', 5);
            charReplacerListCheckDigit.Add('6', 6);
            charReplacerListCheckDigit.Add('7', 7);
            charReplacerListCheckDigit.Add('8', 8);
            charReplacerListCheckDigit.Add('9', 9);
            charReplacerListCheckDigit.Add('X', 10);
            #endregion


            for (int i = 0; i < vin.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        sum += charReplacerList[vin[i]] * 8;
                        continue;

                    case 1:
                        sum += charReplacerList[vin[i]] * 7;
                        continue;

                    case 2:
                        sum += charReplacerList[vin[i]] * 6;
                        continue;

                    case 3:
                        sum += charReplacerList[vin[i]] * 5;
                        continue;

                    case 4:
                        sum += charReplacerList[vin[i]] * 4;
                        continue;

                    case 5:
                        sum += charReplacerList[vin[i]] * 3;
                        continue;

                    case 6:
                        sum += charReplacerList[vin[i]] * 2;
                        continue;

                    case 7:
                        sum += charReplacerList[vin[i]] * 10;
                        continue;

                    case 8:
                        checkDigit = charReplacerListCheckDigit[vin[i]];
                        continue;

                    case 9:
                        sum += charReplacerList[vin[i]] * 9;
                        continue;

                    case 10:
                        sum += charReplacerList[vin[i]] * 8;
                        continue;

                    case 11:
                        sum += charReplacerList[vin[i]] * 7;
                        continue;

                    case 12:
                        sum += charReplacerList[vin[i]] * 6;
                        continue;

                    case 13:
                        sum += charReplacerList[vin[i]] * 5;
                        continue;

                    case 14:
                        sum += charReplacerList[vin[i]] * 4;
                        continue;

                    case 15:
                        sum += charReplacerList[vin[i]] * 3;
                        continue;

                    case 16:
                        sum += charReplacerList[vin[i]] * 2;
                        continue;
                }
            }

            if (sum % 11 != checkDigit)
            {
                return false;
            }

            return true;
        }
    }
}
