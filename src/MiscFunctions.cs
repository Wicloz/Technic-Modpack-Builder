using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technic_Modpack_Creator
{
    class MiscFunctions
    {
        public static string CleanName(string originalName)
        {
            return originalName
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
                    .Replace(".txt", "")
                    .Replace("-", "")
                    .Replace("_", "")
                    .Replace(" ", "")
                    .Replace(".", "");
        }

        public static string RemoveLetters(string originalString)
        {
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

            foreach (char c in charArray)
            {
                if (allowedList.Contains(c))
                {
                    returnString += c;
                    pointMade = false;
                }
                else if (!pointMade)
                {
                    returnString += ".";
                    pointMade = true;
                }
            }

            returnString += "#";
            return returnString.Replace(".#", "").Replace("#.", "").Replace("#", "").Replace("..", ".").Replace("..", ".").Replace("..", ".");
        }

        public static string ExtractSection(string s, char[] endChars, char[] startChars)
        {
            string returnString = "";
            bool foundChars = false;
            char[] charArray = s.Replace(" ", "").ToCharArray();
            int checkChar = 0;

            foreach (char c in charArray)
            {
                if (startChars[checkChar] == c)
                {
                    checkChar++;
                }
                else
                {
                    checkChar = 0;
                }

                if (foundChars && endChars.Contains(c))
                {
                    break;
                }
                else if (foundChars)
                {
                    returnString += c;
                }

                if (startChars.Length == checkChar)
                {
                    foundChars = true;
                    checkChar = 0;
                }
            }

            return returnString;
        }
    }
}
