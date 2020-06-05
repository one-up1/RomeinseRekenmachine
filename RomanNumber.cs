using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeinseRekenmachine
{
    /**
     * https://www.romeinsecijfer.nl/romeinscijfer.js
     */
    public static class RomanNumber
    {
        private static readonly Dictionary<char, int> ints = new Dictionary<char, int>();
        private static readonly List<char> subs = new List<char>();

        private static readonly Dictionary<int, string> chars = new Dictionary<int, string>();

        static RomanNumber()
        {
            ints.Add('I', 1);
            ints.Add('V', 5);
            ints.Add('X', 10);
            ints.Add('L', 50);
            ints.Add('C', 100);
            ints.Add('D', 500);
            ints.Add('M', 1000);

            subs.Add('I');
            subs.Add('X');
            subs.Add('C');
            subs.Add('M');

            chars.Add(0, "I");
            chars.Add(1, "V");
            chars.Add(2, "X");
            chars.Add(3, "L");
            chars.Add(4, "C");
            chars.Add(5, "D");
            chars.Add(6, "M");
        }

        public static int Parse(String s)
        {
            s = s.ToUpper();

            int ret = 0;
            int lastNumb = int.MaxValue;
            int lastSub = int.MaxValue;

            Dictionary<char, int> counter = new Dictionary<char, int>();
            counter.Add('I', 0);
            counter.Add('V', 2);
            counter.Add('X', 0);
            counter.Add('L', 2);
            counter.Add('C', 0);
            counter.Add('D', 2);
            counter.Add('M', int.MinValue);

            // Voeg ondersteuning toe voor negatieve getallen.
            bool negative = false;
            if (s.StartsWith("-"))
            {
                s = s.Substring(1);
                negative = true;
            }

            for (int i = 0; i < s.Length; i++)
            {
                char currentR = s[i];
                char nextR = i < s.Length - 1 ? s[i + 1] : char.MinValue;
                char prevR = i > 0 ? s[i - 1] : char.MinValue;

                if ((!ints.ContainsKey(currentR) ||
                    !ints.ContainsKey(nextR) && i + 1 < s.Length) ||
                    ints[currentR] >= lastSub)
                {
                    throw new FormatException();
                }

                int thisNumb;
                if (TestSub(currentR, nextR, prevR))
                {
                    thisNumb = ints[nextR] - ints[currentR];
                    lastSub = ints[currentR];
                    i++;
                }
                else if (counter[currentR] < 3)
                {
                    thisNumb = ints[currentR];
                    counter[currentR]++;
                }
                else
                {
                    throw new FormatException();
                }

                if (thisNumb > lastNumb)
                {
                    throw new FormatException();
                }
                else
                {
                    ret += thisNumb;
                    lastNumb = thisNumb;
                }
            }

            if (negative)
            {
                ret *= -1;
            }

            return ret;
        }

        public static string Format(int num)
        {
            string ret = "";

            // Voeg ondersteuning toe voor negatieve getallen.
            bool negative = false;
            if (num < 0)
            {
                num *= -1;
                negative = true;
            }

            string s = num.ToString();
            for (int i = 0; i < s.Length; i++)
            {
                ret = getRomanChar(i, int.Parse(s[s.Length - (i + 1)].ToString())) + ret;
            }

            if (negative)
            {
                ret = "-" + ret;
            }

            return ret;
        }

        private static bool TestSub(char cR, char nR, char pR)
        {
            if (nR != char.MinValue && ints[cR] < ints[nR])
            {
                if (pR != char.MinValue && ints[pR] == ints[nR] && !subs.Contains(nR))
                {
                    throw new FormatException();
                }
                else if (subs.Contains(cR) && 10 * ints[cR] >= ints[nR])
                {
                    return true;
                }
                else
                {
                    throw new FormatException();
                }
            }
            return false;
        }

        private static string getRomanChar(int pos, int iValue)
        {
            string ret = "";
            int s = 2 * pos;

            if (pos > 2)
            {
                for (int i = 0; i < iValue * Math.Pow(10, pos - 3); i++)
                {
                    ret += "M";
                }
            }
            else if (iValue < 4)
            {
                for (int i = 0; i < iValue; i++)
                {
                    ret += chars[s];
                }
            }
            else if (iValue == 4)
            {
                ret = chars[s] + chars[s + 1];
            }
            else if (iValue < 9)
            {
                ret = chars[s + 1];
                for (var i = 0; i < iValue - 5; i++)
                {
                    ret += chars[s];
                }
            }
            else if (iValue == 9)
            {
                ret = chars[s] + chars[s + 2];
            }
            return ret;
        }
    }
}