using System.ServiceModel;

namespace WcfService.Service
{
    [ServiceContract]
    public interface IConverterService
    {
        /// <summary>
        /// This function is going to convert the numberic value to word presentation.
        /// </summary>
        /// <param name="value">The numeric value of the dollar amount</param>
        /// <returns></returns>
        [OperationContract]
        string Convert(string value);

    }
}

