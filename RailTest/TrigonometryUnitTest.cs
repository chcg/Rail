using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rail.Trigonometry;
using System.Windows;

namespace RailTest
{
    [TestClass]
    public class TrigonometryUnitTest
    {
        [TestMethod]
        public void PointMoveTestMethod()
        {
            Point p1 = new Point(5, 5);
            Point p2 = new Point(-5, -5);


            Point r1 = p1.Move(10, 10);
            Point r2 = p2.Move(10, 10);
            Point r3 = p1.Move(new Vector(10, 10));
            Point r4 = p2.Move(new Vector(10, 10));


            Assert.AreEqual(new Point(15, 15), r1);
            Assert.AreEqual(new Point(5, 5), r2);
            Assert.AreEqual(new Point(15, 15), r3);
            Assert.AreEqual(new Point(5, 5), r4);
        }

        [TestMethod]
        public void PointRotateTestMethod()
        {
            Point p1 = new Point(10, 0);
            Point p2 = new Point(0, 10);

            Point r1 = p1.Rotate(0).Round(8);
            Point r2 = p1.Rotate(90).Round(8);
            Point r3 = p1.Rotate(180).Round(8);
            Point r4 = p1.Rotate(270).Round(8);

            Point r5 = p2.Rotate(0).Round(8);
            Point r6 = p2.Rotate(90).Round(8);
            Point r7 = p2.Rotate(180).Round(8);
            Point r8 = p2.Rotate(270).Round(8);

            Assert.AreEqual(new Point(10, 0), r1);
            Assert.AreEqual(new Point(0, 10), r2);
            Assert.AreEqual(new Point(-10, 0), r3);
            Assert.AreEqual(new Point(0, -10), r4);

            Assert.AreEqual(new Point(0, 10), r5);
            Assert.AreEqual(new Point(-10, 0), r6);
            Assert.AreEqual(new Point(0, -10), r7);
            Assert.AreEqual(new Point(10, 0), r8);
        }

        [TestMethod]
        public void PointRotateCenterTestMethod()
        {
            Point p1 = new Point(30, 20);
            Point p2 = new Point(20, 30);
            Point c = new Point(20, 20);

            Point r1 = p1.Rotate(0, c).Round(8);
            Point r2 = p1.Rotate(90, c).Round(8);
            Point r3 = p1.Rotate(180, c).Round(8);
            Point r4 = p1.Rotate(270, c).Round(8);

            Point r5 = p2.Rotate(0, c).Round(8);
            Point r6 = p2.Rotate(90, c).Round(8);
            Point r7 = p2.Rotate(180, c).Round(8);
            Point r8 = p2.Rotate(270, c).Round(8);

            Assert.AreEqual(new Point(20 + 10, 20 +  0), r1);
            Assert.AreEqual(new Point(20 +  0, 20 + 10), r2);
            Assert.AreEqual(new Point(20 - 10, 20 +  0), r3);
            Assert.AreEqual(new Point(20 +  0, 20 - 10), r4);

            Assert.AreEqual(new Point(20 +  0, 20 + 10), r5);
            Assert.AreEqual(new Point(20 - 10, 20 +  0), r6);
            Assert.AreEqual(new Point(20 +  0, 20 - 10), r7);
            Assert.AreEqual(new Point(20 + 10, 20 +  0), r8);
        }
    }
}
