using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Task_1
{
    public partial class MainWindow
    {
        private Func<double, double> currentFunction;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem)e.AddedItems[0]).Content)
            {
                case "sin(x)":
                    currentFunction = Math.Sin;
                    break;

                case "x^2":
                    currentFunction = x => x * x;
                    break;
            }
            DrawPlot(currentFunction);
        }
        
        private void Canvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawPlot(currentFunction);
        }
        
        private Point Transform(Point point, double left, double right, double bottom, double top)
        {
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;

            double x = (point.X - left) / (right - left) * width;
            double y = height - (point.Y - bottom) / (top - bottom) * height;

            return new Point(x, y);
        }

        private void DrawPlot(Func<double, double> function)
        {
            if (!double.TryParse(leftTextBox.Text, out var left))
                return;

            if (!double.TryParse(rightTextBox.Text, out var right))
                return;

            if (!double.TryParse(bottomTextBox.Text, out var bottom))
                return;

            if (!double.TryParse(topTextBox.Text, out var top))
                return;
            
            int width = (int)canvas.ActualWidth;

            if (function == null)
                return;

            PointCollection points = new PointCollection(Enumerable.Range(0, width).Select(x => left + x * (right - left) / width).Select(x => new Point(x, function(x))).Select(point => Transform(point, left, right, bottom, top)));

            plotPolyline.Points = points;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentFunction == null)
                return;

            DrawPlot(currentFunction);
        }
    }
}