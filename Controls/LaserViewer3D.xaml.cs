using SciChart.Charting3D;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.PointMarkers;
using SciChart.Examples.ExternalDependencies.Data;
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

namespace Controls {
    /// <summary>
    /// Логика взаимодействия для LaserViewer3D.xaml
    /// </summary>
    public partial class LaserViewer3D : Window {
        public LaserViewer3D() {
            InitializeComponent();

          
        }


    }

    public class CustomPointMarker3D : BaseTexturePointMarker3D {
        private Texture2D _texture;

        /// <summary>
        /// Initializes the instance of <see cref="TrianglePointMarker3D"/>.
        /// </summary>
        public CustomPointMarker3D() {
            DefaultStyleKey = typeof(CustomPointMarker3D);
            _texture = new Texture2D(128, 128, TextureFormat.TEXTUREFORMAT_A8B8G8R8);
            uint[] pixelData = new uint[128 * 128];

            for (uint i = 0; i < 128; i++) {
                for (uint j = 0; j < 128; j++) {
                    uint i8 = 0;
                    if (i < 52 || i > 76) {
                        if (j > 52 && j < 76) {
                            i8 = 0xff;
                        }
                    } else {
                        i8 = 0xff;
                    }

                    pixelData[i + j * 128] = i8 | i8 << 8 | i8 << 16 | i8 << 24;
                }
            }
            _texture.WritePixels(pixelData);
        }

        /// <summary>
        /// Gets the <see cref="Texture2D" /> instance which is repeated across data-points
        /// </summary>
        public override Texture2D PointTexture {
            get { return _texture; }
        }
    }
}
