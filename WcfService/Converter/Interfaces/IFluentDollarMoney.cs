namespace WcfService.Converter.Interfaces
{
    public interface IFluentDollarMoney
    {
        IFluentDollarMoney WithHundredValue(string firstPart);

        IFluentDollarMoney WithThousandValue(string secondPart);

        IFluentDollarMoney WithMillionValue(string thirdPart);

        IFluentDollarMoney WithCentValue(string zeroPart);

        string GetConvertedDollarValue();
    }
}
