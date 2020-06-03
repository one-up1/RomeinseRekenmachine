using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeinseRekenmachine
{
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

        public static int Parse(String rNumb)
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

            rNumb = rNumb.ToUpper();
            for (int i = 0; i < rNumb.Length; i++)
            {
                char currentR = rNumb[i];
                char nextR = i < rNumb.Length - 1 ? rNumb[i + 1] : char.MinValue;
                char prevR = i > 0 ? rNumb[i - 1] : char.MinValue;

                if (!romans.ContainsKey(currentR) ||
                    (!romans.ContainsKey(nextR) && i + 1 < rNumb.Length))
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

        public static string Format(int intNumb)
        {
            //TODO: validation
            string romNumb;
            string romNumbFinal = "";
            string s = intNumb.ToString();
            for (int i = 0; i < s.Length; i++)
            {
                int currentI = int.Parse(s[s.Length - (i + 1)].ToString());
                romNumb = romNumbFinal;
                romNumbFinal = Ints(i, currentI) + romNumb;
            }
            return romNumbFinal;
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