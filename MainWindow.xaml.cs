using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vector_graphic_viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private fields
        // Creating lists storing data from json
        List<Model> primitives = new List<Model>();
        List<double> colors = new List<double>();
        List<Point> points = new List<Point>();
        #endregion
        #region Ctors
        public MainWindow()
        {
            InitializeComponent();
            ReadJSON();
        }
        #endregion
        #region Custom methods
        public void Line(string color)
        {
            Line line = new Line();
            line.X1 = points[0].X;
            line.Y1 = points[0].Y;
            line.X2 = points[1].X;
            line.Y2 = points[1].Y;
            line.Stroke = ColorToARGB(color);
            Canvas.SetLeft(line, Main.ActualWidth / 2);
            Canvas.SetTop(line, Main.ActualHeight / 2);
            Main.Children.Add(line);
        }
        public void Circle(string color, double radius, bool filled)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius;
            ellipse.Height = radius;
            ellipse.Stroke = ColorToARGB(color);
            if (filled)
            {
                ellipse.Fill = ColorToARGB(color);
            }
            Canvas.SetLeft(ellipse, (Main.ActualWidth / 2 - ellipse.Width / 2) + points[0].X);
            Canvas.SetTop(ellipse, (Main.ActualHeight / 2 - ellipse.Height / 2) + points[0].Y);
            Main.Children.Add(ellipse);
        }
        public void Triangle(string color, bool filled)
        {
            Polygon triangle = new Polygon();
            PointCollection polygonPoints = new PointCollection
            {
                points[0],
                points[1],
                points[2]
            };
            triangle.Points = polygonPoints;
            triangle.Stroke = ColorToARGB(color);
            if (filled)
            {
                triangle.Fill = ColorToARGB(color);
            }
            Canvas.SetLeft(triangle, Main.ActualWidth / 2);
            Canvas.SetTop(triangle, Main.ActualHeight / 2);
            Main.Children.Add(triangle);
        }
        // Retrive point from JSON string
        public void PointToPoint(string point)
        {
            string[] xy = point.Trim().Split(';');
            double x = double.Parse(xy[0]);
            double y = double.Parse(xy[1]);
            points.Add(new Point(x, -y)); // minus on Y as the number of pixels increases while pointing down - opposite to the carthesian
        }
        // Retrive color from JSON string
        public SolidColorBrush ColorToARGB(string color)
        {
            foreach (var c in color.Trim().Split(';'))
            {
                colors.Add(double.Parse(c));
            }
            return new SolidColorBrush(Color.FromArgb((byte)colors[0], (byte)colors[1], (byte)colors[2], (byte)colors[3]));
        }
        public void ReadJSON()
        {
            try
            {
                using (StreamReader r = new StreamReader("../../../data.json"))
                {
                    string json = r.ReadToEnd();
                    primitives = JsonConvert.DeserializeObject<List<Model>>(json); //using Newtonsoft package
                }
            }
            catch (FileNotFoundException)
            {
                Exception ex = new Exception("File data.json not found");
                throw(ex);            
            }
           
        }
        public void ClearLists()
        {
            points.Clear();
            colors.Clear();
        }
        public void Draw()
        {
            foreach (var primitive in primitives)
            {
                switch (primitive.Type)
                {
                    case Type.line:
                        ClearLists();
                        PointToPoint(primitive.a);
                        PointToPoint(primitive.b);
                        Line(primitive.Color);
                        break;
                    case Type.circle:
                        ClearLists();
                        PointToPoint(primitive.Center);
                        Circle(primitive.Color, primitive.Radius, primitive.Filled);
                        break;
                    case Type.triangle:
                        ClearLists();
                        PointToPoint(primitive.a);
                        PointToPoint(primitive.b);
                        PointToPoint(primitive.c);
                        Triangle(primitive.Color, primitive.Filled);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Event handlers
        // After canvas has loaded in order to get its size
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
        }
        #endregion
    }
}
