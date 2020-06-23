using System;
using System.Threading.Tasks;

namespace WpfApp.Services
{
    public interface IConverterService
    {
        Task<string> ConvertNumericValueAsync(string value);

        void AbortConversion();

        IDisposable CreateConverterClientDisposable();
    }
}
