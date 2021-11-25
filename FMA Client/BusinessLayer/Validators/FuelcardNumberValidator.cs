namespace BusinessLayer.Validators
{
    public class FuelcardNumberValidator
    {
        public bool isValid(string cardnumber)
        {
            string tocheck = "";
            foreach (char c in cardnumber)
            {
                if (c != '.' && c != '-' && c != ' ')
                {
                    tocheck = tocheck + c;
                }
            }
            if (tocheck.Length == 18)
            { 
                return true;
            }

            return false;
        }
    }
}