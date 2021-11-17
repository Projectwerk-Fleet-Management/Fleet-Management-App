using System;
using BusinessLayer.Exceptions;
using Microsoft.VisualBasic.CompilerServices;

namespace BusinessLayer.Validators
{
    public class NINValidator
    {
        
        private int _NINLenght = 11; //The national identifiation number is 11 characters

        public bool isValid(string userNIN)
        {
            try
            {
                if (userNIN == null) return false;
                string newNIN = "";

                foreach (char c in userNIN)
                {
                    if (c != '.' && c != '-' && c != ' ')
                    {
                        newNIN = newNIN + c;
                    }
                }
                System.Diagnostics.Debug.WriteLine(newNIN);
                if (newNIN.Length != _NINLenght) return false;
                int left;
                int right;
                try
                {
                    left = int.Parse(newNIN.Substring(0, 9));
                    right = int.Parse(newNIN.Substring(9));
                }
                catch (Exception ex)
                {
                    throw new NINValidatorException("Could not correctly seperate left and right", ex);
                }


                //na 2000
                int year = DateTime.Today.Year - 2000;
                if (left < year * 10000000)
                {
                    left += 2000000000;
                }

                int modulo = left % 97;
                int compareTo = 97 - modulo;
                return compareTo == right;
            }
            catch (Exception ex)
            {
                throw new NINValidatorException("Something went wrong validting the National Idendifaction Number");
            }
            
        }
    }
}