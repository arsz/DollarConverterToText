using System;
using System.Collections.Generic;
using WcfService.Converter.Interfaces;

namespace WcfService.Converter.Model
{
    public class DollarMoney : IFluentDollarMoney
    {
        private const string hundred = " hundred ";
        private const string thousand = " thousand ";
        private const string million = " million ";
        private const string dollar = " dollars";
        private const string cent = " cents";
        private const string and = " and ";
        private const string invalidValue00 = "00";
        private const string invalidValue000 = "000";

        private string convertedHundredValue;
        private string convertedThousandValue;
        private string convertedMillionValue;
        private string convertedCentValue;

        private DollarMoney()
        {
            convertedHundredValue = string.Empty;
            convertedThousandValue = string.Empty;
            convertedMillionValue = string.Empty;
            convertedCentValue = string.Empty;
        }

        public string GetConvertedDollarValue()
        {
            CheckOnes();

            if (string.IsNullOrEmpty(convertedCentValue) == false) convertedCentValue = convertedCentValue.Insert(0, and);

            var fullConvertedDollar = convertedMillionValue + convertedThousandValue + convertedHundredValue + convertedCentValue;

            fullConvertedDollar = fullConvertedDollar.Replace("  ", " ");

            return fullConvertedDollar;
        }

        private void CheckOnes()
        {
            if (convertedHundredValue == GetInvalidSingularDollar && string.IsNullOrEmpty(convertedThousandValue) && string.IsNullOrEmpty(convertedMillionValue)) convertedHundredValue = GetValidSingularDollar;

            if (convertedCentValue == GetInvalidSingularCent) convertedCentValue = GetValidSingularCent;
        }

        public IFluentDollarMoney WithCentValue(string zeroPart)
        {
            convertedCentValue = ConvertRemainingDigits(convertedCentValue,zeroPart);

            convertedCentValue += cent;

            return this;
        }

        public IFluentDollarMoney WithHundredValue(string firstPart)
        {
            convertedHundredValue = ConvertDollarDigits(firstPart);

            convertedHundredValue += dollar;

            return this;
        }

        public IFluentDollarMoney WithThousandValue(string secondPart)
        {
            convertedThousandValue = ConvertDollarDigits(secondPart);

            if(string.IsNullOrEmpty(convertedThousandValue) == false) convertedThousandValue += thousand;

            return this;
        }

        public IFluentDollarMoney WithMillionValue(string thirdPart)
        {
            convertedMillionValue = ConvertDollarDigits(thirdPart);

            if (string.IsNullOrEmpty(convertedMillionValue) == false) convertedMillionValue += million;

            return this;
        }

      
        private string ConvertDollarDigits(string part)
        {
            var convertedValue = string.Empty;
            var realPartValue = int.Parse(part);

            if (part == invalidValue000) return convertedValue;

            if (realPartValue >= 1000 || realPartValue < 0) throw new ArgumentOutOfRangeException();

            var lessThanTen = realPartValue < 10;

            if (lessThanTen) return standardValues[realPartValue.ToString()];

            var hasHundredPart = realPartValue >= 100;

            var valueWithoutHundreds = string.Empty;
            if (hasHundredPart)
            {
                var hundredDigit = part[0];
                var hundredValue = firstDigitValues[hundredDigit];
                convertedValue = hundredValue += hundred;
                valueWithoutHundreds = part.Substring(1, 2);
            }
            else
            {
                valueWithoutHundreds = realPartValue.ToString();
            }

            convertedValue = ConvertRemainingDigits(convertedValue, valueWithoutHundreds);

            return convertedValue;
        }


        private string ConvertRemainingDigits(string valueToConvert,string remainingDigits)
        {
            if (remainingDigits == invalidValue00) return valueToConvert;

            if (standardValues.ContainsKey(remainingDigits))
            {
                valueToConvert += standardValues[remainingDigits];
            }
            else
            {
                var secondDigit = remainingDigits[0];
                var firstDigit = remainingDigits[1];
                valueToConvert += secondDigitValues[secondDigit] + "-" + firstDigitValues[firstDigit];
            }

            return valueToConvert;
        }

        public static DollarMoney CreateDefaultDollar => new DollarMoney();

        private static Dictionary<string, string> standardValues = new Dictionary<string, string>()
        {
            { "0", "zero" }, {"1","one" }, {"2", "two"}, { "3", "three" }, {"4","four" }, {"5", "five"},
            { "6", "six" }, {"7","seven" }, {"8", "eight"}, { "9", "nine" }, {"10","ten" },
            {"11", "eleven"},{ "12", "twelve" }, {"13","thirteen" }, {"14", "fourteen"}, { "15", "fifteen" },
            { "16","sixteen" }, { "17", "seventeen"}, {"18","eighteen" }, {"19", "nineteen"}, { "20", "twenty" },
            { "30","thirty" }, {"40", "forty"}, { "50", "fifty" }, {"60","sixty" }, {"70", "seventy"}, { "80", "eighty" }, {"90","ninety" }
        };

        private static Dictionary<char, string> secondDigitValues = new Dictionary<char, string>()
        {
            { '2', "twenty" },{ '3',"thirty" }, {'4', "forty"}, { '5', "fifty" },
            { '6',"sixty" }, {'7', "seventy"}, { '8', "eighty" }, {'9',"ninety" }
        };

        private static Dictionary<char, string> firstDigitValues = new Dictionary<char, string>()
        {
            {'1',"one" }, {'2', "two"}, { '3', "three" }, {'4',"four" }, {'5', "five"},
            { '6', "six" }, {'7',"seven" }, {'8', "eight"}, { '9', "nine" }
        };

        private string GetInvalidSingularDollar => "one dollars";

        private string GetValidSingularDollar => "one dollar";

        private string GetInvalidSingularCent => "one cents";

        private string GetValidSingularCent => "one cent";

    }
}
