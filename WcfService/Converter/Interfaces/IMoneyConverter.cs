namespace WcfService.Converter.Interfaces
{
    public interface IMoneyConverter
    {
        string ConvertNumbericMoneyToText(string amount);
    }

    public enum MoneyParts
    {
        DollarIndex = 0,
        CentIndex = 1 
    }

    public enum DollarParts
    {
        Hundred = 0,
        Thousand = 1,
        Million = 2
    }
}
