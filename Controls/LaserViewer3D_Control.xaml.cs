using SciChart.Charting3D;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.PointMarkers;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для LaserViewer3D_Control.xaml
    /// </summary>
    public partial class LaserViewer3D_Control : UserControl {
        public LaserViewer3D_Control() {
            InitializeComponent();

            PointMarkerCombo.Items.Add(typeof(EllipsePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(QuadPointMarker3D));
            PointMarkerCombo.Items.Add(typeof(TrianglePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(CustomPointMarker3D));

            Loaded += OnLoaded;

            
        }
        XyzDataSeries3D<double> xyzDataSeries3D;
        XyzDataSeries3D<double> xyzDataSeries3D2;
        XyzDataSeries3D<double> xyzDataSeries3D3;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            xyzDataSeries3D = new XyzDataSeries3D<double>();
            xyzDataSeries3D2 = new XyzDataSeries3D<double>();
            xyzDataSeries3D3 = new XyzDataSeries3D<double>();


            xyzDataSeries3D.Append(-100, -100, -100);
            xyzDataSeries3D.Append(1000, 1000, 1000);

            ScatterSeries3D.DataSeries = xyzDataSeries3D;
            ScatterSeries3D2.DataSeries = xyzDataSeries3D2;
            ScatterSeries3D3.DataSeries = xyzDataSeries3D3;

            PointMarkerCombo.SelectedIndex = 0;
        }

        public void AddPoint(double x, double y, double z, int index = 0) {
            if (index == 0) {
                xyzDataSeries3D = (XyzDataSeries3D<double>)ScatterSeries3D.DataSeries;
                xyzDataSeries3D.Append(x, y, z);
                ScatterSeries3D.DataSeries = xyzDataSeries3D;
                //Console.WriteLine($"Count = {xyzDataSeries3D.Count}");
            } else if (index == 1) {
                xyzDataSeries3D = (XyzDataSeries3D<double>)ScatterSeries3D2.DataSeries;
                xyzDataSeries3D.Append(x, y, z);
                ScatterSeries3D2.DataSeries = xyzDataSeries3D;
            } else {
                xyzDataSeries3D = (XyzDataSeries3D<double>)ScatterSeries3D3.DataSeries;
                xyzDataSeries3D.Append(x, y, z);
                ScatterSeries3D3.DataSeries = xyzDataSeries3D;
            }
        }

        private void PointMarkerCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ScatterSeries3D != null && OpacitySlider != null && SizeSlider != null) {
                ScatterSeries3D.PointMarker = (BasePointMarker3D)Activator.CreateInstance((Type)((ComboBox)sender).SelectedItem);
                ScatterSeries3D.PointMarker.Fill = Colors.LimeGreen;
                ScatterSeries3D.PointMarker.Size = (float)SizeSlider.Value;
                ScatterSeries3D.PointMarker.Opacity = OpacitySlider.Value;
            }
        }

        private void SizeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (ScatterSeries3D != null && ScatterSeries3D.PointMarker != null)
                ScatterSeries3D.PointMarker.Size = (float)((Slider)sender).Value;
        }

        private void OpacitySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (ScatterSeries3D != null && ScatterSeries3D.PointMarker != null)
                ScatterSeries3D.PointMarker.Opacity = ((Slider)sender).Value;
        }
    }

    //public class CustomPointMarker3D : BaseTexturePointMarker3D {
    //    private Texture2D _texture;

    //    /// <summary>
    //    /// Initializes the instance of <see cref="TrianglePointMarker3D"/>.
    //    /// </summary>
    //    public CustomPointMarker3D() {
    //        DefaultStyleKey = typeof(CustomPointMarker3D);
    //        _texture = new Texture2D(128, 128, TextureFormat.TEXTUREFORMAT_A8B8G8R8);
    //        uint[] pixelData = new uint[128 * 128];

    //        for (uint i = 0; i < 128; i++) {
    //            for (uint j = 0; j < 128; j++) {
    //                uint i8 = 0;
    //                if (i < 52 || i > 76) {
    //                    if (j > 52 && j < 76) {
    //                        i8 = 0xff;
    //                    }
    //                } else {
    //                    i8 = 0xff;
    //                }

    //                pixelData[i + j * 128] = i8 | i8 << 8 | i8 << 16 | i8 << 24;
    //            }
    //        }
    //        _texture.WritePixels(pixelData);
    //    }

    //    /// <summary>
    //    /// Gets the <see cref="Texture2D" /> instance which is repeated across data-points
    //    /// </summary>
    //    public override Texture2D PointTexture {
    //        get { return _texture; }
    //    }
    //}
}
