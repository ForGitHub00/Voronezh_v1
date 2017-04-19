using CalculateDLL;
using Controls;
using LaserDLL;
using RSI_DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Voronezh_v1 {
    public class Worker {
        #region ctor
        public Worker(bool map2d = true, bool map3d = true, bool viewer2d = true) {
            Map2D = map2d;
            Map3D = map3D;
            Viewer2D = viewer2d;

            _LV = new LViewer();
            _map2d = new Map2D();
            _map2d_win = new LaserViewer2D();
            _map2d_win.DataContext = _map2d;
            _map3d = new LaserViewer3D();

            if (map3d) {
                _map3d.Show();
            }
        }
        #endregion
        #region prop
        private bool map2D;
        public bool Map2D {
            get { return map2D; }
            set { map2D = value; }
        }
        private bool map3D;
        public bool Map3D {
            get { return map3D; }
            set { map3D = value; }
        }
        private bool viewer2D;
        public bool Viewer2D {
            get { return viewer2D; }
            set { viewer2D = value; }
        }



        #endregion
        #region Data
        Robot _R;
        LViewer _LV;
        Map2D _map2d;
        LaserViewer2D _map2d_win;
        LaserViewer3D _map3d;
        #endregion
        #region Work 
        public void RobotStart(int port = 6008) {
            _R = new Robot(port);
            _R.StartListening();
        }
        public void RobotStop() {
            _R.exit = true;
            _R.StopListening();
        }
        [STAThread]
        public void LaserStart() {
            Laser.Init();

            #region vars
            int temp_x = 0;
            #endregion

            Thread laser_thrd = new Thread(new ThreadStart(LaserThread));
            laser_thrd.Start();





            void LaserThread()
            {
                while (true) {
                    Dispatcher.CurrentDispatcher.Invoke(()=> {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);
                        LPoint res = LVoronej.Type1_1point(data);

                        RPoint findPoint = Transform.Trans(new RPoint(temp_x, 0, 0, 0, 0, 0), res);
                        if (Viewer2D) {
                            _LV.SetData(data);
                            _LV.SetPoint(res);
                        }
                        if (Map2D) {
                            _map2d.AddLaserPoint(findPoint);
                        }
                        if (Map3D) {
                            _map3d.AddPoint(findPoint.ToDoubleMas());
                        }
                    });
                    Thread.Sleep(36);
                }
                void InvokerFun()
                {



                }
            }

        }

        #endregion
    }
}
