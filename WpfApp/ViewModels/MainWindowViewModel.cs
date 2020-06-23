using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Services;
using WpfApp.Utils;
using WpfApp.Utils.Exceptions;
using WpfApp.Utils.Interfaces;

namespace WpfApp.ViewModels
{
    public class MainWindowViewModel : MainViewModel, IDisposable
    {
        private const int minValue = 0;
        private const int maxValue = 1000000000;

        private readonly IConverterService converterService;

        private string originalMoney;
        private string convertedMoney;
        private bool isInProgress;
        private CancellationTokenSource cancellationTokenSource;
        private string conversionResultStatusMessage;

        public MainWindowViewModel(IConverterService converterService)
        {
            cancellationTokenSource = new CancellationTokenSource();
            this.converterService = converterService;
        }

        public bool IsInProgress
        {
            get => isInProgress;
            set
            {
                isInProgress = value;
                OnPropertyChanged();
            }
        }

        public string OriginalMoney
        {
            get => originalMoney;
            set
            {
                originalMoney = value;
                OnPropertyChanged();
            }
        }

        public string ConversionResultStatusMessage
        {
            get => conversionResultStatusMessage;
            set
            {
                conversionResultStatusMessage = value;
                OnPropertyChanged();
            }
        }

        public string ConvertedMoney
        {
            get => convertedMoney;
            set
            {
                convertedMoney = value;
                OnPropertyChanged();
            }
        }

        public IAsyncCommand Send => new AsyncCommand(() => SendNumberForConvertingDollar(cancellationTokenSource.Token),
            _ => IsMoneyValidNumber, continueOnCapturedContext: false, onException: exc => HandleErrorMessage(exc));

        public ICommand Cancel => new Command(_ => cancellationTokenSource.Cancel(), _ => IsInProgress);

        public ICommand Exit => new Command(_ => CloseApplication(), _ => true);

        public bool IsMoneyValidNumber => string.IsNullOrEmpty(originalMoney) == false && float.TryParse(originalMoney, out _) && originalMoney.Contains(".") == false;

        public void Dispose() => cancellationTokenSource.Dispose();

        public async Task SendNumberForConvertingDollar(CancellationToken cancellationToken)
        {
            CheckMoneyValueBeforeConversion();     

            using (converterService.CreateConverterClientDisposable())
            using (cancellationToken.Register(() => converterService.AbortConversion()))
            {
                try
                {
                    IsInProgress = true;
                    ConversionResultStatusMessage = "Converting...";
                    ConvertedMoney = await converterService.ConvertNumericValueAsync(originalMoney);
                    ConversionResultStatusMessage = "The result: ";
                }
                catch(ConversionException)
                {
                    throw;
                }
                catch(FaultException ex)
                {
                    throw new ConversionException(ConversionFaultReason.ServiceError, ex);
                }
                catch(CommunicationException ex)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = new CancellationTokenSource();
                    throw new ConversionException(ConversionFaultReason.Aborted, ex);
                }
                catch (Exception ex)
                {
                    throw new ConversionException(ConversionFaultReason.UnknownError, ex);
                }
                finally
                {
                    IsInProgress = false;
                }
            }
        }

        private void CheckMoneyValueBeforeConversion()
        {
            originalMoney = originalMoney.Replace(" ", "");
            var values = originalMoney.Split(',');
            var realMoneyValue = int.Parse(values[0]);
            if (realMoneyValue < minValue || realMoneyValue > maxValue) throw new ConversionException(ConversionFaultReason.ValueOutOfRange);

            if (values.Length == 1) return;

            var centPart = values[1];

            if(centPart.Length > 2) throw new ConversionException(ConversionFaultReason.InvalidCentValue);
        }

        private void HandleErrorMessage(Exception exception)
        {
            ConvertedMoney = string.Empty;
            if (exception is ConversionException conversionException)
            { 
                switch (conversionException.Reason)
                {
                    case ConversionFaultReason.Aborted:
                        ConversionResultStatusMessage = "The conversion has been aborted!";
                        break;
                    case ConversionFaultReason.ServiceError:
                        ConversionResultStatusMessage = "A service error occured during the conversion!";
                        break;
                    case ConversionFaultReason.UnknownError:
                        ConversionResultStatusMessage = "Something went wrong!";
                        break;
                    case ConversionFaultReason.ValueOutOfRange:
                        ConversionResultStatusMessage = $"Money must be equals or greater then {minValue} and less then {maxValue}";
                        break;
                    case ConversionFaultReason.InvalidCentValue:
                        ConversionResultStatusMessage = $"Invalid cent value!";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid conversion error ({conversionException.Reason}) reason!");
                };
            }
            else if (exception is OverflowException) ConversionResultStatusMessage = "You must be very rich...";
            else ConversionResultStatusMessage = "Something went wrong!";
        }
        
        private void CloseApplication()
        {
            Dispose();
            Environment.Exit(0);
        }
    }
}
