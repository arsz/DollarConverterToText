using System;
using System.Threading.Tasks;
using WpfApp.Utils.Exceptions;

namespace WpfApp.Services
{
    public class ConverterService : IConverterService
    {
        private ServiceReference.ConverterServiceClient converterServiceClient;

        public IDisposable CreateConverterClientDisposable()
        {
            converterServiceClient = new ServiceReference.ConverterServiceClient();
            return converterServiceClient;
        }

        public async Task<string> ConvertNumericValueAsync(string value)
        {
            if (converterServiceClient == null) throw new ConversionException(ConversionFaultReason.ServiceError,new ArgumentNullException("Client should be created before starting the conversion!"));

           return await converterServiceClient.ConvertAsync(value).ConfigureAwait(false);
        }


        public void AbortConversion()
        {
            if (converterServiceClient == null) throw new ConversionException(ConversionFaultReason.ServiceError, new ArgumentNullException("Client should be created before aborting the conversion!"));

            converterServiceClient.Abort();
        }

    }
}
