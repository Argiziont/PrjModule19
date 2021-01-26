using System;
using Xunit;

namespace PrimeNumberLib.Tests
{
    public class PrimeTesterTests
    {
        #region snippet_IsPrime_ReturnsTrue_InputIsPrime

        [Fact]
        public void sPrime_ReturnsTrue_InputIsPrime()
        {
            // Arrange
            var primeNumber = 3;

            // Act
            var result = PrimeTester.IsPrime(primeNumber);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region snippet_IsPrime_ReturnsFalse_InputIsComposite

        [Fact]
        public void IsPrime_ReturnsFalse_InputIsComposite()
        {
            // Arrange
            var compositeNumber = 9;

            // Act
            var result = PrimeTester.IsPrime(compositeNumber);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
