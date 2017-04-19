﻿using CalculateDLL;
using Controls;
using LaserDLL;
using RSI_DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Voronezh_v1 {
    /// <summary>
    /// Логика взаимодействия для SeamTracking.xaml
    /// </summary>
    public partial class SeamTracking : Window {
        public SeamTracking() {
            InitializeComponent();
            //Dispatcher.Invoke(()=> {
            //    Worker w = new Worker();
            //    w.RobotStart();
            //    w.LaserStart();
            //});                             
            Map2D = true;
            Map3D = true;
            Viewer2D = true;

            _LV = new LViewer();
            _map2d = new Map2D();
            _map2d_win = new LaserViewer2D();
            _map2d_win.DataContext = _map2d;
            _map3d = new LaserViewer3D();

            if (Map2D) {
                _map2d_win.Show();
            }
            if (Map3D) {
                _map3d.Show();
            }
            if (Viewer2D) {
                _LV.Show();
            }

            Start();
        }
        #region Start
        private void Start() {
            RobotStart();
            LaserStart();
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
                    Dispatcher.Invoke(InvokerFun);
                    temp_x++;
                    Thread.Sleep(360);
                }
                void InvokerFun()
                {
                    Laser.GetProfile(out double[] X, out double[] Z);
                    List<LPoint> data = Helper.GetLaserData(X, Z, true);
                    if (data.Count != 0) {
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

                    }


                }
            }

        }

        #endregion
    }
}
