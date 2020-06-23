using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utils.Exceptions
{
    public class ConversionException : Exception
    {
        public ConversionFaultReason Reason { get; }

        public ConversionException(ConversionFaultReason reason)
        {
            Reason = reason;
        }

        public ConversionException(ConversionFaultReason reason, Exception exception) : base(exception.Message)
        {
            Reason = reason;
        }
    }

    public enum ConversionFaultReason
    {
        Aborted,
        InvalidCentValue,
        ValueOutOfRange,
        ServiceError,
        UnknownError
    }
}
