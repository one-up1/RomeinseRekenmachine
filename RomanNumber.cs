using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeinseRekenmachine
{
    /**
     * Ik moet eerlijk toegeven dat ik dit niet zelf bedacht heb.
     * Als voorbeeld heb ik dit script gebruikt: https://www.romeinsecijfer.nl/romeinscijfer.js
     */
    public static class RomanNumber
    {
        private static readonly Dictionary<char, int> romans = new Dictionary<char, int>();
        private static readonly List<char> subs = new List<char>();

        private static readonly Dictionary<int, string> getChar = new Dictionary<int, string>();

        static RomanNumber()
        {
            romans.Add('I', 1);
            romans.Add('V', 5);
            romans.Add('X', 10);
            romans.Add('L', 50);
            romans.Add('C', 100);
            romans.Add('D', 500);
            romans.Add('M', 1000);

            subs.Add('I');
            subs.Add('X');
            subs.Add('C');
            subs.Add('M');

            getChar.Add(0, "I");
            getChar.Add(1, "V");
            getChar.Add(2, "X");
            getChar.Add(3, "L");
            getChar.Add(4, "C");
            getChar.Add(5, "D");
            getChar.Add(6, "M");
        }

        public static int Parse(String s)
        {
            Dictionary<char, int> counter = new Dictionary<char, int>();
            counter.Add('I', 0);
            counter.Add('V', 2);
            counter.Add('X', 0);
            counter.Add('L', 2);
            counter.Add('C', 0);
            counter.Add('D', 2);
            counter.Add('M', int.MinValue);

            int intNumb = 0;
            int lastNumb = int.MaxValue;
            int thisNumb = 0;
            int lastSub = int.MaxValue;

            s = s.ToUpper();
            for (int i = 0; i < s.Length; i++)
            {
                char currentR = s[i];
                char nextR = i < s.Length - 1 ? s[i + 1] : char.MinValue;
                char prevR = i > 0 ? s[i - 1] : char.MinValue;

                if (!romans.ContainsKey(currentR) ||
                    (!romans.ContainsKey(nextR) && i + 1 < s.Length))
                {
                    throw new FormatException();
                }

                if (TestSub(currentR, nextR, prevR))
                {
                    thisNumb = romans[nextR] - romans[currentR];
                    i++;
                    lastSub = romans[currentR];
                }
                else if (counter[currentR] < 3)
                {
                    thisNumb = romans[currentR];
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
                    intNumb += thisNumb;
                    lastNumb = thisNumb;
                }
            }
            return intNumb;
        }

        public static string Format(int num)
        {
            if (num < 0)
                throw new FormatException();
            string s = num.ToString();

            string ret = "";
            for (int i = 0; i < s.Length; i++)
            {
                ret = Ints(i, int.Parse(s[s.Length - (i + 1)].ToString())) + ret;
            }
            return ret;
        }

        private static bool TestSub(char cR, char nR, char pR)
        {
            if (cR != char.MinValue && nR != char.MinValue && romans[cR] < romans[nR])
            {
                if ((romans[pR] == romans[nR]) && !subs.Contains(nR))
                {
                    throw new FormatException();
                }
                else if (subs.Contains(cR) && 10 * romans[cR] >= romans[nR])
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

        private static string Ints(int pos, int iValue)
        {
            string charValue = "";
            int s = 2 * pos;
            if (pos > 2)
            {
                for (int i = 0; i < iValue * Math.Pow(10, (pos - 3)); i++)
                {
                    charValue += "M";
                }
            }
            else if (iValue < 4)
            {
                for (int i = 0; i < iValue; i++)
                {
                    charValue += getChar[s];
                }
            }
            else if (iValue == 4)
            {
                charValue = getChar[s] + getChar[s + 1];
            }
            else if (iValue < 9)
            {
                charValue = getChar[s + 1];
                for (var i = 0; i < iValue - 5; i++)
                {
                    charValue += getChar[s];
                }
            }
            else if (iValue == 9)
            {
                charValue = getChar[s] + getChar[s + 2];
            }
            return charValue;
        }
    }
}