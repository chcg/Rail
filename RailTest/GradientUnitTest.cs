using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rail.Misc;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace RailTest
{
    [TestClass]
    public class GradientUnitTest
    {
        [TestMethod]
        public void AngleToPercentTestMethod()
        {
            double d0 = Math.Round(Gradient.AngleToPercent(0.0), 2);
            double d1 = Math.Round(Gradient.AngleToPercent(1.0), 2);
            double d2 = Math.Round(Gradient.AngleToPercent(2.0), 2);
            double d7 = Math.Round(Gradient.AngleToPercent(7.5), 2);
            double d15 = Math.Round(Gradient.AngleToPercent(15.0), 2);
            double d30 = Math.Round(Gradient.AngleToPercent(30.0), 2);
            double d45 = Math.Round(Gradient.AngleToPercent(45.0), 2);
            Assert.AreEqual(0, d0, "d0");
            Assert.AreEqual(1.75, d1, "d1");
            Assert.AreEqual(3.49, d2, "d2");
            Assert.AreEqual(13.17, d7, "d7");
            Assert.AreEqual(26.79, d15, "d15");
            Assert.AreEqual(57.74, d30, "d30");
            Assert.AreEqual(100.0, d45, "d45");
        }

        [TestMethod]
        public void PercentToAngleTestMethod()
        {
            double a0 = Math.Round(Gradient.PercentToAngle(0), 2);
            double a1 = Math.Round(Gradient.PercentToAngle(1), 2);
            double a2 = Math.Round(Gradient.PercentToAngle(2), 2);
            double a4 = Math.Round(Gradient.PercentToAngle(4), 2);
            double a8 = Math.Round(Gradient.PercentToAngle(8), 2);
            double a50 = Math.Round(Gradient.PercentToAngle(50), 2);
            double a100 = Math.Round(Gradient.PercentToAngle(100), 2);
            Assert.AreEqual(0, a0, "a0");
            Assert.AreEqual(0.57, a1, "a1");
            Assert.AreEqual(1.15, a2, "a2");
            Assert.AreEqual(2.29, a4, "a4");
            Assert.AreEqual(4.57, a8, "a8");
            Assert.AreEqual(26.57, a50, "a50");
            Assert.AreEqual(45.0, a100, "a100");
        }
    }
}
