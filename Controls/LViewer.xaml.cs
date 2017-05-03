using CalculateDLL;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Controls {
    /// <summary>
    /// Логика взаимодействия для LViewer.xaml
    /// </summary>
    public partial class LViewer : UserControl {
        public LViewer(string Title = "Laser 2D") {
            InitializeComponent();
            Data = new LasViewer();
            DataContext = Data;

            win = new Window() { Title = Title};
            Grid gr = new Grid();
            win.Content = gr;
            gr.Children.Add(this);
        }
        public LasViewer Data;
        public void SetData(double[] X, double[] Z) {
            List<LPoint> data = Helper.GetLaserData(X, Z, true);
            Data.LasData = new ObservableCollection<DataPoint>();
            for (int i = 0; i < data.Count; i++) {
                Data.LasData.Add(new DataPoint(data[i].X, data[i].Z));
            }

        }
        public void SetData(List<LPoint> data) {
            Data.LasData = new ObservableCollection<DataPoint>();
            for (int i = 0; i < data.Count; i++) {
                Data.LasData.Add(new DataPoint(data[i].X, data[i].Z));
            }
        }
        public void SetPoints(List<LPoint> points) {
            Data.Points = new ObservableCollection<DataPoint>();
            if (points.Count == 1) {
                Data.Points.Add(new DataPoint(points[0].X, points[0].Z));
                Data.Points.Add(new DataPoint(points[0].X, points[0].Z));
            } else {
                for (int i = 0; i < points.Count; i++) {
                    Data.Points.Add(new DataPoint(points[i].X, points[i].Z));
                }
            }
        }
        public void SetPoint(LPoint point) {
            Data.Points = new ObservableCollection<DataPoint>();
            Data.Points.Add(new DataPoint(point.X, point.Z));
            Data.Points.Add(new DataPoint(point.X, point.Z));
        }

        Window win;
        public void Show() {
            win.Show();
        }
        public void Hide() {
            win.Hide();
        }
    }
    public class LasViewer : DependencyObject {



        public ObservableCollection<DataPoint> Points {
            get { return (ObservableCollection<DataPoint>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(ObservableCollection<DataPoint>), typeof(LasViewer), new PropertyMetadata(new ObservableCollection<DataPoint>()));



        public ObservableCollection<DataPoint> LasData {
            get { return (ObservableCollection<DataPoint>)GetValue(LasDataProperty); }
            set { SetValue(LasDataProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LasData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LasDataProperty =
            DependencyProperty.Register("LasData", typeof(ObservableCollection<DataPoint>), typeof(LasViewer), new PropertyMetadata(new ObservableCollection<DataPoint>()));
    }
    public static class Helper {
        public static void GetLaserDataFromTXT(string path, out double[] X, out double[] Z) {
            List<string> result = new List<string>();
            try {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        result.Add(line);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            X = new double[result.Count];
            Z = new double[result.Count];

            for (int i = 0; i < result.Count; i++) {
                string[] temp = result[i].Split('|');
                X[i] = Convert.ToDouble(temp[0]);
                Z[i] = Convert.ToDouble(temp[1]);
            }


        }
        public static List<LPoint> GetLaserDataFromTXT(string path, bool zeroZ = false) {
            double[] x;
            double[] z;
            GetLaserDataFromTXT(path, out x, out z);
            return GetLaserData(x, z, zeroZ);
        }
        public static List<LPoint> GetLaserData(double[] X, double[] Z, bool _deleteZeroZ = false) {
            List<LPoint> result = new List<LPoint>();
            for (int i = 0; i < X.Length; i++) {
                if (Z[i] == 0 ^ !_deleteZeroZ) {
                    continue;
                }
                result.Add(new LPoint() {
                    X = X[i],
                    Z = Z[i]
                });
            }
            return result;
        }
    }
}
