using CalculateDLL;
using Controls;
using LaserDLL;
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
using System.Windows.Shapes;

namespace Voronezh_v1 {
    /// <summary>
    /// Логика взаимодействия для SeamTracking.xaml
    /// </summary>
    public partial class SeamTracking : Window {
        public SeamTracking() {
            InitializeComponent();
            LaserViewer3D map3D = new LaserViewer3D();
            map3D.Show();
            map3D.AddPoint(1,4,8);
        }
    }
}
