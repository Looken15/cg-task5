using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;

namespace task5
{
    public partial class Form4 : Form
    {
        Graphics gr;
        double pointSearchRadius = 10.0;
        int pointRadius = 5;
        List<Point> points = new List<Point>();
        Brush basePointsColor = Brushes.Blue;
        Color baseLineColor = Color.Blue;
        int pointToMove = -1;
        List<Point> curvesPoints = new List<Point>();
        Color curveLineColor = Color.Red;

        int countPrimLines = 100;

        private void Form4_Load(object sender, EventArgs e)
        {
            pb.Image = new Bitmap(pb.Width, pb.Height);
            gr = Graphics.FromImage(pb.Image);
            RedrawScene();
        }
        public Form4()
        {
            InitializeComponent();
        }

        //возвращает индекс ближайшей точки points
        private int GetPointIndexInRadius(Point p)
        {
            int res = -1;
            double min = int.MaxValue;
            for (int i = 0; i < points.Count(); i++)
            {
                var r = Sqrt((points[i].X - p.X) * (points[i].X - p.X) + (points[i].Y - p.Y) * (points[i].Y - p.Y));
                if (r < pointSearchRadius && r < min)
                {
                    res = i;
                    min = r;
                }
            }
            return res;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var m = (MouseEventArgs)e;
            var nearest_point = GetPointIndexInRadius(m.Location);
            if (nearest_point == -1)
            {
                AddPoint(m.Location);
                return;
            }
        }

        private void AddPoint(Point p)
        {
            points.Add(p);
            MakeCurves();
            RedrawScene();
        }

        private void DeletePoint(int index)
        {
            points.RemoveAt(index);
            MakeCurves();
            RedrawScene();
        }


        private void RedrawScene()
        {
            gr.Clear(Color.White);
            DrawPoints();
            DrawCurvesPoints();
            pb.Refresh();
        }

        private void DrawPoints()
        {
            if (points.Count == 0)
                return;
            foreach (var p in points)
                gr.DrawEllipse(new Pen(basePointsColor), p.X - pointRadius, p.Y - pointRadius, pointRadius * 2, pointRadius * 2);
            if (points.Count() > 1)
                gr.DrawLines(new Pen(baseLineColor), points.ToArray());
        }

        private void DrawCurvesPoints()
        {
            if (curvesPoints.Count > 1)
                gr.DrawLines(new Pen(curveLineColor), curvesPoints.ToArray());
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            points.Clear();
            curvesPoints.Clear();
            pointToMove = -1;
            RedrawScene();
        }

        private void pb_DoubleClick(object sender, EventArgs e)
        {
            var m = (MouseEventArgs)e;
            var nearest_point = GetPointIndexInRadius(m.Location);
            if (nearest_point != -1)
                DeletePoint(nearest_point);
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            var m = (MouseEventArgs)e;
            var nearest_point = GetPointIndexInRadius(m.Location);
            if (nearest_point == -1)
                return;
            pointToMove = nearest_point;
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            pointToMove = -1;
            MakeCurves();
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if (pointToMove == -1)
                return;
            var m = (MouseEventArgs)e;
            points[pointToMove] = m.Location;
            MakeCurves();
            RedrawScene();
        }

        private void MakeCurves()
        {
            curvesPoints.Clear();
            if (points.Count() < 3)
                return;
            if (points.Count() == 3)
            {
                MakeCurve(points[0], points[1], points[1], points[2]);
                return; 
            }
            MakeCurve(points[0], points[1], points[2], points[3]);
            for (int i = 3; i < points.Count(); i += 3)
            {
                if (i + 3 == points.Count())
                    MakeCurve(points[i], points[i + 1], points[i + 1], points[i + 2]);
                if (i + 3 < points.Count())
                    MakeCurve(points[i], points[i + 1], points[i + 2], points[i + 3]);
            }
        }

        private void MakeCurve(Point p0, Point p1, Point p2, Point p3)
        {
            double step = 1.0 / countPrimLines;
            for (double t = 0.0; t <= 1; t += step)
            {
                double xb = t * t * t * (3 * p1.X - p0.X - 3 * p2.X + p3.X) + t * t * (3 * p0.X - 6 * p1.X + 3 * p2.X) + t * (3 * p1.X - 3 * p0.X) + p0.X;
                double yb = t * t * t * (3 * p1.Y - p0.Y - 3 * p2.Y + p3.Y) + t * t * (3 * p0.Y - 6 * p1.Y + 3 * p2.Y) + t * (3 * p1.Y - 3 * p0.Y) + p0.Y;
                curvesPoints.Add(new Point((int)xb, (int)yb));
            }
        }
    }
}
