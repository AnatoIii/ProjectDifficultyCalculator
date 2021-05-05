using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDifficultyCalculator.Logic;

namespace ProjectDifficultyCalculator.Tests
{
    [TestClass]
    public class IntervalTests
    {
        [TestMethod]
        [DataRow(-5.0, 3.0, 1.0)]
        [DataRow(-5.0, 3.0, -5.0)]
        [DataRow(-5.0, 3.0, 3.0)]
        [DataRow(0.0, 0.0, 0.0)]
        [DataRow(-double.Epsilon, double.Epsilon, 0.0)]
        [DataRow(double.NegativeInfinity, double.PositiveInfinity, 0.0)]
        public void IncludingContains_DoubleValueBelongs_True(double from, double to, double value)
        {
            //Arrange
            var interval = new Interval<double>(from, to);

            //Act
            var result = interval.Contains(value);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow(-5.0, 3.0, 1.0)]
        [DataRow(-5.0, 3.0, -4.0)]
        [DataRow(-5.0, 3.0, 2.0)]
        [DataRow(-double.Epsilon, double.Epsilon, 0.0)]
        [DataRow(double.NegativeInfinity, double.PositiveInfinity, 0.0)]
        public void ExcludingContains_DoubleValueBelongs_True(double from, double to, double value)
        {
            //Arrange
            var interval = new Interval<double>(from, to, false);

            //Act
            var result = interval.Contains(value);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow(-5.0, 3.0, 8.0)]
        [DataRow(-5.0, 3.0, -8.0)]
        [DataRow(0.0, 0.0, double.Epsilon)]
        [DataRow(0.0, 0.0, -double.Epsilon)]
        [DataRow(-1.0, 1.0, double.NaN)]
        [DataRow(-1.0, 1.0, double.NegativeInfinity)]
        [DataRow(-1.0, 1.0, double.PositiveInfinity)]
        public void IncludingContains_DoubleValueBelongs_False(double from, double to, double value)
        {
            //Arrange
            var interval = new Interval<double>(from, to);

            //Act
            var result = interval.Contains(value);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(-5.0, 3.0, -5.0)]
        [DataRow(-5.0, 3.0, 3.0)]
        [DataRow(0.0, 0.0, 0.0)]
        [DataRow(-1.0, 1.0, double.NaN)]
        [DataRow(-1.0, 1.0, double.NegativeInfinity)]
        [DataRow(-1.0, 1.0, double.PositiveInfinity)]
        public void ExcludingContains_DoubleValueBelongs_False(double from, double to, double value)
        {
            //Arrange
            var interval = new Interval<double>(from, to, false);

            //Act
            var result = interval.Contains(value);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))] //Assert
        [DataRow(1.0, -1.0)]
        [DataRow(5.0, 0.0)]
        [DataRow(double.PositiveInfinity, double.NegativeInfinity)]
        [DataRow(0.0, double.NaN)]
        public void IncludingDoubleIntervalCreation_WrongEndpoints_Exception(double from, double to)
        {
            //Arrange
            var interval = new Interval<double>(from, to); //Act
        }
    }
}
