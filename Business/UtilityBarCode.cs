using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    class UtilityBarCode
    {

        public string ValidateMod43(string barcode)
        {
            int subtotal = 0;
            const string charSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

            foreach (char c in barcode)

                subtotal += charSet.IndexOf(c);

            return charSet[subtotal % 43].ToString();

        }

    }
}
