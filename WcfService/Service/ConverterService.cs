
using System.ServiceModel;
using WcfService.Converter;
using WcfService.Converter.Interfaces;

namespace WcfService.Service
{

    [ServiceBehavior(IncludeExceptionDetailInFaults = false)]
    public class ConverterService : IConverterService
    {

        private readonly IMoneyConverter moneyConverter;

        public ConverterService()
        {
            moneyConverter = new DollarConverter();
        }

        public string Convert(string value)
        {
            return moneyConverter.ConvertNumbericMoneyToText(value);
        }
    }
}
