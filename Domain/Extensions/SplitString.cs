using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public class SplitString
    {
        public const string separator = ")s%ec!r_e-t?^(";
        public List<string> SplitStringForNotify(string input)
        {
            List<string> result = new List<string>();

            string[] parts = input.Split(new string[] { separator }, StringSplitOptions.None);

            foreach (var part in parts)
            {
                result.Add(part);
            }

            return result;
        }

        public List<string> SplitStringAfterForConnectString(string input)
        {
            List<string> result = new List<string>();
            int semicolonCount = 0;
            int startIndex = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ';')
                {
                    semicolonCount++;
                }

                if (semicolonCount == 4)
                {
                    result.Add(input.Substring(startIndex, i - startIndex + 1));
                    startIndex = i + 1;
                    semicolonCount = 0;
                }
            }

            if (startIndex < input.Length)
            {
                result.Add(input.Substring(startIndex));
            }

            return result;
        }
    }
}
