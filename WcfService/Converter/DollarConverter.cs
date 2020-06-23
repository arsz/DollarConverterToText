using System;
using WcfService.Converter.Interfaces;
using WcfService.Converter.Model;

namespace WcfService.Converter
{
    public class DollarConverter : IMoneyConverter
    {
        public string ConvertNumbericMoneyToText(string money)
        {
            string[] dollarAndCents = money.Split(',');
            string convertedMoney = string.Empty;
            var moneyModel = DollarMoney.CreateDefaultDollar;

            var dollarPart = dollarAndCents[(int)MoneyParts.DollarIndex];

            dollarPart = int.Parse(dollarPart).ToString();

            BuildDollarPart(dollarPart, moneyModel);

            if (HasOnlyDollarPart(dollarAndCents)) return moneyModel.GetConvertedDollarValue();

            var centPart = dollarAndCents[(int)MoneyParts.CentIndex];

            BuildCentPart(centPart, moneyModel);

            return moneyModel.GetConvertedDollarValue();
        }


        private void BuildCentPart(string centPart, IFluentDollarMoney centMoney)
        {
            if (centPart.Length > 2) throw new ArgumentOutOfRangeException("Cent part cannot be greater than 99");

            if (centPart.Length == 2 && centPart[0] == '0') centPart = centPart[1].ToString();
            else if (centPart.Length == 1) centPart += "0";

            centMoney.WithCentValue(centPart);
        }

        private void BuildDollarPart(string dollarPart, IFluentDollarMoney dollarMoney)
        {
            if (int.Parse(dollarPart) > 1000000000 || int.Parse(dollarPart) < 0) throw new ArgumentOutOfRangeException("Dollar value out of range!");

            var moneySeparatorInterval = 3;
            var iterator = 0;
            var dollarIndex = dollarPart.Length;
            var hasNextDollarPart = true;

            while (hasNextDollarPart)
            {
                dollarIndex -= moneySeparatorInterval;
                hasNextDollarPart = TryGetNextDollarPart(ref dollarIndex,ref moneySeparatorInterval);
                var nextDollarPartValue = dollarPart.Substring(dollarIndex, moneySeparatorInterval);

                var nextDollarPart = (DollarParts)iterator;
                switch (nextDollarPart)
                {
                    case DollarParts.Hundred:
                        dollarMoney.WithHundredValue(nextDollarPartValue);
                        break;
                    case DollarParts.Thousand:
                        dollarMoney.WithThousandValue(nextDollarPartValue);
                        break;
                    case DollarParts.Million:
                        dollarMoney.WithMillionValue(nextDollarPartValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid {0} value!", nextDollarPart.ToString());
                }

                iterator++;
            }
        }

        private bool TryGetNextDollarPart(ref int index, ref int separatorInterval)
        {
            if (index > 0) return true;

            separatorInterval = index + separatorInterval;

            index = 0;

            return false;
        }


        private bool HasOnlyDollarPart(string[] moneyParts) => moneyParts.Length == 1;

    }
}
