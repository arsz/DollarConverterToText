using NUnit.Framework;
using WcfService.Converter;

namespace Tests
{
    public class DollarConverterTests
    {
        DollarConverter dollarConverter = new DollarConverter();

        [TestCase("0")]
        [TestCase("00")]
        [TestCase("0000000000")]
        public void ConvertNumbericMoneyToText_ShouldReturnTheWordPresentationForZeroDollarsNumericInput(string numericInput)
        {
            //Arrange
            var expectedResult = "zero dollars";

            //Act
            var result = dollarConverter.ConvertNumbericMoneyToText(numericInput);

            //Assert
            Assert.That(expectedResult == result);
        }

        [TestCase("1")]
        [TestCase("01")]
        [TestCase("00000000001")]
        public void ConvertNumbericMoneyToText_ShouldReturnTheWordPresentationForOneDollarNumericInput(string numericInput)
        {
            //Arrange
            var expectedResult = "one dollar";

            //Act
            var result = dollarConverter.ConvertNumbericMoneyToText(numericInput);

            //Assert
            Assert.That(expectedResult == result);
        }

        [TestCase("25,1")]
        [TestCase("025,10")]
        [TestCase("00000025,1")]
        public void ConvertNumbericMoneyToText_ShouldReturnTheWordPresentationForTwentyFiveDollarsAndTenCentsNumericInput(string numericInput)
        {
            //Arrange
            var expectedResult = "twenty-five dollars and ten cents";

            //Act
            var result = dollarConverter.ConvertNumbericMoneyToText(numericInput);

            //Assert
            Assert.That(expectedResult == result);
        }


        [TestCase("0,01")]
        [TestCase("0000,01")]
        public void ConvertNumbericMoneyToText_ShouldReturnTheWordPresentationForZeroDollarsAndOneCentNumericInput(string numericInput)
        {
            //Arrange
            var expectedResult = "zero dollars and one cent";

            //Act
            var result = dollarConverter.ConvertNumbericMoneyToText(numericInput);

            //Assert
            Assert.That(expectedResult == result);
        }



        [TestCase("45100")]
        [TestCase("000000045100")]
        public void ConvertNumbericMoneyToText_ShouldReturnTheWordPresentationForFortyFiveThousandOneHundredDollarsNumericInput(string numericInput)
        {
            //Arrange
            var expectedResult = "forty-five thousand one hundred dollars";

            //Act
            var result = dollarConverter.ConvertNumbericMoneyToText(numericInput);

            //Assert
            Assert.That(expectedResult == result);
        }


        [TestCase("999999999,99")]
        public void ConvertNumbericMoneyToText_ShouldReturnTheWordPresentationForMaxValueDollarsNumericInput(string numericInput)
        {
            //Arrange
            var expectedResult = "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents";

            //Act
            var result = dollarConverter.ConvertNumbericMoneyToText(numericInput);

            //Assert
            Assert.That(expectedResult == result);
        }

        [TestCase("0,9999")]
        [TestCase("00,100")]
        public void ConvertNumbericMoneyToText_ShouldThrowArgumentOutOfRangeExceptionForGreaterCentInput(string numericInput)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => dollarConverter.ConvertNumbericMoneyToText(numericInput));
        }

        [TestCase("1000000000")]
        [TestCase("-1")]
        [TestCase("-1,99")]
        [TestCase("-999999999,99")]
        public void ConvertNumbericMoneyToText_ShouldThrowArgumentOutOfRangeExceptionForOutOfRangeDollarInput(string numericInput)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => dollarConverter.ConvertNumbericMoneyToText(numericInput));
        }

        [TestCase("99999999999,99")]
        public void ConvertNumbericMoneyToText_ShouldThrowOverflowExceptionForOutOfRangeDollarInput(string numericInput)
        {
            Assert.Throws<System.OverflowException>(() => dollarConverter.ConvertNumbericMoneyToText(numericInput));
        }

    }
}