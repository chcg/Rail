using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rail.Controls
{
    public static class DrawingContexts
    {
        private static Pen linePen = new Pen(Brushes.Black, 1);
        private static Pen dockPen = new Pen(Brushes.Blue, 1);
        private static Pen positionPen = new Pen(Brushes.Red, 2);

        //public static void DrawDockRect(this DrawingContext drawingContext, Point p)
        //{
        //    drawingContext.DrawRectangle(null, dockPen, new Rect(new Point(p.X - 2, p.Y - 2), new Size(4, 4)));
        //}

        public static void DrawDockPoints(this DrawingContext drawingContext, IEnumerable<DockPoint> points)
        {
            if (points != null)
            {
                foreach (DockPoint point in points)
                {
                    drawingContext.DrawDockRect(point);
                }
            }
        }

        public static void DrawDockRect(this DrawingContext drawingContext, DockPoint p)
        {
            //drawingContext.DrawRectangle(null, dockPen, new Rect(new Point(p.X - 2, p.Y - 2), new Size(4, 4)));
            drawingContext.DrawEllipse(null, dockPen, p, 3.0, 3.0);
            drawingContext.DrawLine(positionPen, p, new Point(
                p.X + (Cos(p.Angle) * 16) - (Sin(p.Angle) * 16),
                p.Y + (Sin(p.Angle) * 16) + (Cos(p.Angle) * 16)));
        }

        public static void DrawPosition(this DrawingContext drawingContext, Point p)
        {
            drawingContext.DrawLine(positionPen, new Point(p.X - 6, p.Y - 6), new Point(p.X + 6, p.Y + 6));
            drawingContext.DrawLine(positionPen, new Point(p.X - 6, p.Y + 6), new Point(p.X + 6, p.Y - 6));
        }

        public static void DrawArc(this DrawingContext drawingContext, Point p1, Point p2, double radius)
        {

            List<PathSegment> segments = new List<PathSegment>(1);
            segments.Add(new ArcSegment(p2, new Size(radius, radius), 0.0, false, SweepDirection.Clockwise, true));
            
            List<PathFigure> figures = new List<PathFigure>(1);
            PathFigure pf = new PathFigure(p1, segments, true);
            pf.IsClosed = false;
            figures.Add(pf);

            Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);
            drawingContext.DrawGeometry(null, linePen, g);
        }

        public static void DrawText(this DrawingContext drawingContext, Point p, double angle, FormattedText text)
        {
            drawingContext.PushTransform(new RotateTransform(angle, p.X, p.Y));
            drawingContext.DrawText(text, p - new Vector(text.Width / 2, text.Height / 2));
            drawingContext.Pop();
        }

        private static double Sin(double value)
        {
            return Math.Sin(value * Math.PI / 180.0);
        }

        private static double Cos(double value)
        {
            return Math.Cos(value * Math.PI / 180.0);
        }
    }
}
