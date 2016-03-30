using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technic_Modpack_Creator {
    class MiscFunctions {
        public static bool PartialMatch (string localName, string onlineName) {
            string cleanS1 = CleanString(CleanModName(localName)).Replace("mod", "");
            string cleanS2 = CleanString(onlineName).Replace("mod", "");

            if (cleanS1.Contains(cleanS2) || cleanS2.Contains(cleanS1)) {
                return true;
            }
            else {
                return false;
            }
        }

        public static string CleanModName (string originalName) {
            char[] delim = new char[] { '-' };
            string proDash = originalName.Split(delim)[0];
            return CleanString(proDash).Replace("botaniar", "botania");
        }

        public static string CleanString (string originalString) {
            return originalString
                .ToLower()
                    .Replace("0", "")
                    .Replace("1", "")
                    .Replace("2", "")
                    .Replace("3", "")
                    .Replace("4", "")
                    .Replace("5", "")
                    .Replace("6", "")
                    .Replace("7", "")
                    .Replace("8", "")
                    .Replace("9", "")
                    .Replace(".zip", "")
                    .Replace(".jar", "")
                    .Replace(".disabled", "")
                    .Replace(".txt", "")
                    .Replace("-", "")
                    .Replace("_", "")
                    .Replace(" ", "")
                    .Replace(".", "")
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("(", "")
                    .Replace("}", "");
        }

        public static string RemoveLetters (string originalString) {
            string returnString = "";
            char[] charArray = originalString.Replace(" ", "").ToLower().ToCharArray();
            List<char> allowedList = new List<char>();

            allowedList.Add('1');
            allowedList.Add('2');
            allowedList.Add('3');
            allowedList.Add('4');
            allowedList.Add('5');
            allowedList.Add('6');
            allowedList.Add('7');
            allowedList.Add('8');
            allowedList.Add('9');
            allowedList.Add('0');

            returnString += "#";
            bool pointMade = true;

            foreach (char c in charArray) {
                if (allowedList.Contains(c)) {
                    returnString += c;
                    pointMade = false;
                }
                else if (!pointMade) {
                    returnString += ".";
                    pointMade = true;
                }
            }

            returnString += "#";
            return returnString.Replace(".#", "").Replace("#.", "").Replace("#", "").Replace("..", ".").Replace("..", ".").Replace("..", ".");
        }

        public static string ExtractSection (string s, char[] endChars) {
            string _s = "##" + s;
            char[] startCharList = new char[] { '#', '#' };
            return MiscFunctions.ExtractSection(_s, endChars, startCharList);
        }

        public static string ExtractSection (string s, char[] endChars, char[] startChars) {
            if (startChars == null || startChars.Length == 0) {
                s = "##" + s;
                startChars = new char[] { '#', '#' };
            }

            string returnString = "";
            bool foundChars = false;
            char[] charArray = s.ToCharArray();
            int checkChar = 0;

            foreach (char c in charArray) {
                if (startChars[checkChar] == c) {
                    checkChar++;
                }
                else {
                    checkChar = 0;
                }

                if (foundChars && endChars.Contains(c)) {
                    break;
                }
                else if (foundChars) {
                    returnString += c;
                }

                if (startChars.Length == checkChar) {
                    foundChars = true;
                    checkChar = 0;
                }
            }

            return returnString;
        }
    }
}
