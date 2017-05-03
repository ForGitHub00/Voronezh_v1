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
            Map2D = false;
            Map3D = true;
            Viewer2D = true;

            test = false;
            l1 = false;
            l2 = false;
            l3 = false;
            l4 = false;
            cal = false;
            one = false;

            _LV = new LViewer();
            _map2d = new Map2D();
            _map2d_win = new LaserViewer2D();
            _map2d_win.DataContext = _map2d;
            _map3d = new LaserViewer3D();
            s = Singleton.GetInstance();

            if (Map2D) {
                _map2d_win.Show();
            }
            if (Map3D) {
                _map3d.Show();

            }
            if (Viewer2D) {
                _LV.Show();
            }

            // Start();
        }
        #region Start
        private void Start() {
            RobotStart();
            //LaserStart();
            Program1();
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

        public bool test;

        public bool l1;
        public bool l2;
        public bool l3;
        public bool l4;
        public bool cal;
        public bool one;




        #endregion
        #region Data
        Robot _R;
        LViewer _LV;
        Map2D _map2d;
        LaserViewer2D _map2d_win;
        LaserViewer3D _map3d;
        Singleton s;
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

        public void Program1() {
            Laser.Init();

            RPoint send_point = new RPoint();
            bool isFirst = true;
            bool start = false;
            bool finish = false;

            //LPoint lp = new LPoint(-11.475, 217.6);
            //RPoint rp = new RPoint(699.44, -66.74, 247.03, -1.99, -1.12, -27.76);
            //var tr = Transform.Trans(rp, lp);
            //Console.WriteLine(tr.ToString());

            List<RPoint> line1 = new List<RPoint>();
            List<RPoint> line2 = new List<RPoint>();

            LViewer aprx_v = new LViewer("Approx");
            aprx_v.Show();

            List<LPoint> aprox_points = new List<LPoint>();
            List<RPoint> seam_points = new List<RPoint>();
            List<RPoint> seam_points2 = new List<RPoint>();

            List<List<LPoint>> lines = new List<List<LPoint>>();

            List<RPoint> show = new List<RPoint>();

            //RPoint s1 = new RPoint(732.26, -226.4, 368.73);
            //RPoint s2 = new RPoint(901.1, -245.6, 369);
            //RPoint f1 = new RPoint(727.0475, -262.422, 370.675);
            //RPoint f2 = new RPoint(894.9, -260.64, 366.07);

            RPoint s1 = new RPoint(700, -100, 300);
            RPoint s2 = new RPoint(800, -130, 320);
            RPoint f1 = new RPoint(600, -100, 300);
            RPoint f2 = new RPoint(700, -115, 310);


            var pp = RobotCalculate.BaseRotation(s1, s2, f1, f2);

            Console.WriteLine($"Rotation  = {pp.ToString()}");
            _map3d.AddPoint(s1.ToDoubleMas());
            _map3d.AddPoint(s2.ToDoubleMas());
            _map3d.AddPoint(f1.ToDoubleMas(), 1);
            _map3d.AddPoint(f2.ToDoubleMas(), 1);



            //Thread thrd = new Thread(new ThreadStart(t_work)
            //Thread thrd = new Thread(new ThreadStart(LaserApprox));
            Thread thrd = new Thread(new ThreadStart(test_1));
            thrd.Start();




            void t_work()
            {
                while (true) {
                    Dispatcher.Invoke(InvokerFun);

                    Thread.Sleep(100);
                }
                void InvokerFun()
                {
                    tb_position.Text = s.Position.ToString();
                    tb_recive_point.Text = s.recive_p.ToString();
                    // Console.WriteLine(s.work);
                    tb_work.Text = s.rec.ToString();
                    tb_send_point.Text = send_point.ToString();

                    tb_strRecive.Text = s.StrRecive;


                    Laser.GetProfile(out double[] X, out double[] Z);
                    List<LPoint> data = Helper.GetLaserData(X, Z, true);

                    if (data.Count != 0) {
                        var tee = LFilters.GetLineCount(data, minLineLenght: 50, diff: 100);
                        _LV.SetData(data);
                        if (tee == 2) {
                            //  Console.WriteLine(tee);
                            // Console.Beep(1000, 100);
                        }
                    }



                    LPoint res = new LPoint(0, 0);
                    if (data.Count != 0 && test) {
                        _LV.SetData(data);
                        if (LFilters.GetLineCount(data, minLineLenght: 50, diff: 100) > 1) {
                            res = LVoronej.Type1_1point(data);
                            RPoint findPoint = Transform.Trans(s.Position, res);
                            tb_cur_point.Text = findPoint.ToString();
                            _LV.SetPoint(res);
                            _map2d.AddLaserPoint(findPoint);
                            _map3d.AddPoint(findPoint.ToDoubleMas(), 1);
                            //for (int i = 0; i < data.Count; i++) {
                            //    var t1 = Transform.Trans(s.Position, data[i]);
                            //    _map3d.AddPoint(t1.ToDoubleMas());
                            //}

                            if (l1) {
                                line1.Add(findPoint);
                            } else if (l2) {
                                line2.Add(findPoint);
                            }
                            //if (cal) {
                            //    send_point = RobotCalculate.CalculatePoint_2line(line1, line2);
                            //    _map3d.AddPoint(send_point.ToDoubleMas(),2);
                            //    cal = false;
                            //}

                        }




                        //if (Viewer2D) {
                        //    _LV.SetData(data);                          
                        //}
                        //if (Map2D) {
                        //    _map2d.AddLaserPoint(findPoint);
                        //}
                        //if (Map3D) {
                        //    _map3d.AddPoint(findPoint.ToDoubleMas());
                        //}
                    }
                    if (cal) {
                        send_point = RobotCalculate.CalculatePoint_2line(line1, line2);
                        _map3d.AddPoint(send_point.ToDoubleMas(), 2);
                        cal = false;
                    }




                    //if (data.Count != 0) {
                    //    Console.WriteLine(LFilters.GetLineCount(data, minLineLenght: 50, diff: 40));

                    //    if (s.work) {
                    //    start = true;
                    //        if (isFirst) {
                    //            line1.Add(findPoint);
                    //        } else {
                    //            line2.Add(findPoint);
                    //        }
                    //    }
                    //} else {
                    //    if (start) {
                    //        isFirst = false;
                    //        finish = true;
                    //    }
                    //    if (finish) {
                    //        // var t = RobotCalculate.CalculatePoint_2line(line1, line2);
                    //        // send_point = t;
                    //    }
                    //}



                    #region
                    //if (s.work) {
                    //    start = true;
                    //    if (data.Count != 0) {

                    //        RPoint findPoint = Transform.Trans(s.Position, res);
                    //        tb_cur_point.Text = findPoint.ToString();

                    //        if (Viewer2D) {
                    //            _LV.SetData(data);
                    //            _LV.SetPoint(res);
                    //        }
                    //        if (Map2D) {
                    //            _map2d.AddLaserPoint(findPoint);
                    //        }
                    //        if (Map3D) {
                    //            _map3d.AddPoint(findPoint.ToDoubleMas());
                    //        }

                    //        if (isFirst) {
                    //            line1.Add(findPoint);
                    //        } else {
                    //            line2.Add(findPoint);
                    //        }
                    //    }
                    //} else {
                    //    if (start) {
                    //        isFirst = false;
                    //        finish = true;
                    //    }
                    //    if (finish) {
                    //       // var t = RobotCalculate.CalculatePoint_2line(line1, line2);
                    //       // send_point = t;
                    //    }
                    //}
                    #endregion



                    //Dispatcher.Invoke(() => {

                    //});
                }
            }

            void LaserApprox()
            {
                var temp_point = Point_X(new LPoint(-25, 191.55), new LPoint(-10, 201.77), new LPoint(10, 167.04), new LPoint(25, 146.053));
                // Console.WriteLine($"X = {temp_point.X} | Z = {temp_point.Z}");

                while (true) {
                    Dispatcher.Invoke(InvokerFun);
                    Thread.Sleep(500);
                }
                void InvokerFun()
                {
                    Laser.GetProfile(out double[] X, out double[] Z);
                    List<LPoint> data = Helper.GetLaserData(X, Z, true);
                    data = LFilters.AveragingVerticalPro(data, _everyPoint: true);
                    tb_strRecive.Text = s.StrRecive;


                    if (data.Count != 0 && test) {
                        aprox_points = new List<LPoint>();
                        lines = new List<List<LPoint>>();
                        _LV.SetData(data);
                        int line_count = LFilters.GetLineCount(data, minLineLenght: data.Count / 6, diff: 70);
                        if (line_count > 0) {


                            //var aprx_mas = LFilters.GetLines(data, minLineLenght: 50, diff: 100);
                            //_LV.SetPoints(aprx_mas);
                            // Console.WriteLine(line_count);

                            var lp = LVoronej.Type1_1point(data);
                            var t23 = Transform.Trans(s.Position, lp);
                            if (seam_points2.Count > 0) {
                                double xDif = 5;
                                double yDif = 1;
                                double zDif = 1;

                                double x1 = seam_points2[seam_points2.Count - 1].X;
                                double y1 = seam_points2[seam_points2.Count - 1].Y;
                                double z1 = seam_points2[seam_points2.Count - 1].Z;
                                double x2 = t23.X;
                                double y2 = t23.Y;
                                double z2 = t23.Z;
                                if (Math.Abs(x1 - x2) < xDif) {
                                    if (Math.Abs(y1 - y2) < yDif) {
                                        if (Math.Abs(z1 - z2) < zDif) {
                                            seam_points2.Add(t23);
                                            _map3d.AddPoint(t23.ToDoubleMas(), 1);
                                        }
                                    }
                                }

                            } else {
                                seam_points2.Add(t23);
                                _map3d.AddPoint(t23.ToDoubleMas(), 1);
                            }


                            List<LPoint> data_aprx = new List<LPoint>();
                            var aprx_mas = LFilters.GetLinesByIndexes(data, minLineLenght: data.Count / 6, diff: 70);
                            for (int i = 0; i < aprx_mas.Count; i += 2) {
                                int start_indx = aprx_mas[i];
                                int finish_indx = aprx_mas[i + 1];
                                List<LPoint> l1 = new List<LPoint>();
                                for (int j = start_indx; j <= finish_indx; j++) {
                                    l1.Add(data[j]);
                                }
                                l1 = LFilters.LineAprox(l1);
                                lines.Add(l1);
                                foreach (var item in l1) {
                                    data_aprx.Add(item);
                                }
                                //if (i >= 2) {
                                //    aprox_points.Add(Point_X(data[aprx_mas[i - 2]], data[aprx_mas[i - 1]], data[aprx_mas[i]], data[aprx_mas[i + 1]]));
                                //}
                            }
                            aprx_v.SetData(data_aprx);
                            // aprx_v.SetPoints(aprox_points);



                            //for (int i = 0; i < data_aprx.Count; i++) {
                            //    var t1 = Transform.Trans(s.Position, data_aprx[i]);
                            //    _map3d.AddPoint(t1.ToDoubleMas());
                            //}



                            for (int i = 1; i < lines.Count; i++) {
                                var temp_res_point = Point_X(lines[i - 1][0], lines[i - 1][lines[i - 1].Count - 1], lines[i][0], lines[i][lines[i].Count - 1]);
                                //if (temp_res_point.Z != 0 && DistanceBetweenPoint(temp_res_point, lines[i - 1][lines[i - 1].Count - 1]) < 5 &&
                                //    DistanceBetweenPoint(temp_res_point, lines[i][0]) < 5) {
                                //    aprox_points.Add(temp_res_point);
                                //}
                                if (temp_res_point.Z != 0) {
                                    aprox_points.Add(temp_res_point);
                                }
                            }


                            aprx_v.SetPoints(aprox_points);

                            //for (int i = 0; i < aprox_points.Count; i++) {
                            //    var t1 = Transform.Trans(s.Position, aprox_points[i]);
                            //    _map3d.AddPoint(t1.ToDoubleMas(), 2);
                            //}

                            if (aprox_points.Count > 0) {
                                double minZ = aprox_points.Min(t => t.Z);
                                LPoint minPoint = aprox_points.Single(t => t.Z == minZ);


                                var t2 = Transform.Trans(s.Position, minPoint);

                                #region С уравнением прямых TODO
                                //if (seam_points.Count > 5) {
                                //    int start_index = 5;
                                //    int finish_index = 5;
                                //    bool add = true;
                                //    while (finish_index < seam_points.Count) {
                                //        if (RobotCalculate.OnLine(seam_points[])) {

                                //        }
                                //    }
                                //} else {
                                //    seam_points.Add(t2);
                                //    _map3d.AddPoint(t2.ToDoubleMas(), 2);
                                //}
                                #endregion

                                #region Каждая точка с предыдущей 
                                if (seam_points.Count > 0) {
                                    double xDif = 5;
                                    double yDif = 1;
                                    double zDif = 1;

                                    double x1 = seam_points[seam_points.Count - 1].X;
                                    double y1 = seam_points[seam_points.Count - 1].Y;
                                    double z1 = seam_points[seam_points.Count - 1].Z;
                                    double x2 = t2.X;
                                    double y2 = t2.Y;
                                    double z2 = t2.Z;
                                    if (Math.Abs(x1 - x2) < xDif) {
                                        if (Math.Abs(y1 - y2) < yDif) {
                                            if (Math.Abs(z1 - z2) < zDif) {
                                                seam_points.Add(t2);
                                                _map3d.AddPoint(t2.ToDoubleMas(), 2);
                                            }
                                        }
                                    }

                                } else {
                                    seam_points.Add(t2);
                                    _map3d.AddPoint(t2.ToDoubleMas(), 2);
                                }

                                #endregion

                                line1.Add(t2);
                            }

                            GC.Collect();

                        }
                    }
                    if (cal) {
                        int type_line1 = RobotCalculate.LineType(seam_points);
                        seam_points = RobotCalculate.LineApprox(seam_points, type_line1);
                        foreach (var item in seam_points) {
                            _map3d.AddPoint(item.ToDoubleMas(), 1);
                        }
                        cal = false;
                    }
                }

                void InvokerFunR()
                {
                    tb_strRecive.Text = s.StrRecive;
                }
            }

            void test_1()
            {

                RPoint r1 = new RPoint();
                RPoint r2 = new RPoint();
                RPoint r3 = new RPoint();
                RPoint r4 = new RPoint();
                RPoint res = new RPoint();
                bool Top = false;


                while (true) {
                    Dispatcher.Invoke(InvokerFun);
                    Thread.Sleep(500);
                }
                void InvokerFun()
                {
                    Laser.GetProfile(out double[] X, out double[] Z);
                    List<LPoint> data = Helper.GetLaserData(X, Z, true);
                    data = LFilters.AveragingVerticalPro(data, _everyPoint: true);

                    tb_strRecive.Text = s.StrRecive;
                    _LV.SetData(data);


                    Console.WriteLine(s.work);

                    if (data.Count != 0 && s.Position.X != double.MaxValue) {






                        LPoint tempPoint = new LPoint();
                        //if (Top) {
                        //    double maxZ = data.Max(t => t.Z);
                        //    tempPoint = data.First(t => t.Z == maxZ);
                        //    _LV.SetPoint(tempPoint);
                        //} else {
                        //    double mixZ = data.Min(t => t.Z);
                        //    tempPoint = data.First(t => t.Z == mixZ);
                        //    _LV.SetPoint(tempPoint);
                        //}
                        double mixZ = data.Min(t => t.Z);
                        if (mixZ != double.NaN && mixZ > 0) {
                            tempPoint = data.First(t => t.Z == mixZ);
                            _LV.SetPoint(tempPoint);
                        }
                       


                        if (s.rec == 9) {
                            var ttt = Transform.Trans(s.Position, tempPoint);
                            _map3d.AddPoint(ttt.ToDoubleMas(), 1);
                            show.Add(ttt);
                            s._MAP.Add(ttt);
                        }


                        //if (l1) {
                        //    r1 = Transform.Trans(s.Position, tempPoint);
                        //    _map3d.AddPoint(r1.ToDoubleMas(), 1);
                        //    l1 = false;
                        //} else if (l2) {
                        //    r2 = Transform.Trans(s.Position, tempPoint);
                        //    _map3d.AddPoint(r2.ToDoubleMas(), 1);
                        //    l2 = false;
                        //} else if (l3) {
                        //    r3 = Transform.Trans(s.Position, tempPoint);
                        //    _map3d.AddPoint(r3.ToDoubleMas(), 1);
                        //    l3 = false;
                        //} else if (l4) {
                        //    r4 = Transform.Trans(s.Position, tempPoint);
                        //    _map3d.AddPoint(r4.ToDoubleMas(), 1);
                        //    l4 = false;
                        //} else if (cal) {
                        //    res = RobotCalculate.CalcPoint4(r1, r2, r3, r4);
                        //    Console.WriteLine($"find point = {res.ToString()}");
                        //    _map3d.AddPoint(res.ToDoubleMas(), 2);
                        //    cal = false;
                        //}
                        //if (one) {
                        //    var tem = Transform.Trans(s.Position, tempPoint);
                        //    Console.WriteLine($"Trans point = {tem.ToString()}");
                        //    _map3d.AddPoint(tem.ToDoubleMas(), 2);
                        //    one = false;
                        //}
                    }
                }
            }
        }


        public LPoint Point_X(LPoint a1, LPoint a2, LPoint a3, LPoint a4, double TangDiff = 0.7) {
            LPoint res;
            double x1 = a1.X;
            double y1 = a1.Z;
            double x2 = a2.X;
            double y2 = a2.Z;
            double x3 = a3.X;
            double y3 = a3.Z;
            double x4 = a4.X;
            double y4 = a4.Z;

            double x = ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1)) / ((y1 - y2) * (x4 - x3) - (y3 - y4) * (x2 - x1));
            double y = ((y3 - y4) * x - (x3 * y4 - x4 * y3)) / (x4 - x3);

            res = new LPoint(-x, y);
            double k1 = (x2 - x1) / (y2 - y1);
            double k2 = (x4 - x3) / (y4 - y3);
            if (Math.Abs(k2 - k1) < TangDiff) {
                return new LPoint(0, 0);
            }
            res = new LPoint(-x, y);
            //Console.WriteLine($"k1 = {k1}  ||  k2 = {k2}");
            return res;
        }
        public double DistanceBetweenPoint(LPoint p1, LPoint p2) {
            double result = 0;
            double x1 = p1.X;
            double y1 = p1.Z;
            double x2 = p2.X;
            double y2 = p2.Z;


            result = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            return result;
        }

        [STAThread]
        public void LaserStart() {
            Laser.Init();

            #region vars
            int temp_x = 0;
            #endregion

            Thread laser_thrd = new Thread(new ThreadStart(LaserThread2));
            laser_thrd.Start();





            void LaserThread()
            {

                Dispatcher.Invoke(() => {
                    List<LPoint> dat = new List<LPoint>();
                    for (int i = 0; i < 20; i++) {
                        dat.Add(new LPoint(0, i));
                    }
                    for (int i = 0; i < 50; i++) {
                        dat.Add(new LPoint(i, 20));
                    }

                    _LV.SetData(dat);
                    for (int i = 0; i < 60; i++) {
                        Console.WriteLine($"i = {i}   Res = {LFilters.GetLineCount(dat, minLineLenght: i, maxDistance: 1)}");
                    }

                });







                while (true) {
                    Dispatcher.Invoke(InvokerFun);
                    temp_x++;
                    Thread.Sleep(500);
                }
                void InvokerFun()
                {




                    //LFilters.pointOnLine(new LPoint(2,-3), new LPoint(-3, 4), new LPoint(5, -1.5));
                    //LFilters.pointOnLine(new LPoint(0, 0), new LPoint(0, 3), new LPoint(0, 2));







                    Laser.GetProfile(out double[] X, out double[] Z);
                    List<LPoint> data = Helper.GetLaserData(X, Z, true);
                    if (data.Count != 0) {
                        LPoint res = LVoronej.Type1_1point(data);

                        double tempdd = LFilters.IsAngle(data);
                        double prov = data.Count / 1.2;
                        // Console.WriteLine($"{tempdd} - {prov}");
                        if (tempdd < prov) {
                            //  Console.WriteLine($"BEEP!  {tempdd} - {prov}");
                            Console.Beep(1000, 200);
                        }

                        Console.WriteLine(LFilters.GetLineCount(data, minLineLenght: 50, diff: 40));

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
            void LaserThread2()
            {



                Dispatcher.Invoke(() => {
                    List<LPoint> dat = new List<LPoint>();
                    for (int i = 0; i < 20; i++) {
                        dat.Add(new LPoint(0, i));
                    }
                    for (int i = 4; i < 50; i++) {
                        dat.Add(new LPoint(i, 20));
                    }

                    _LV.SetData(dat);
                    for (int i = 0; i < 60; i++) {
                        // Console.WriteLine($"i = {i}   Res = {LFilters.GetLineCount(dat, minLineLenght: i, maxDistance: 1)}");
                    }
                    Console.WriteLine(LFilters.GetLineCount(dat, minLineLenght: 10, diff: 1, maxDistance: 1));
                    var lines = LFilters.GetLines(dat, minLineLenght: 10, diff: 1, maxDistance: 1);
                    _LV.SetPoints(lines);

                    //FileWorker.LaserSaveOneProf(dat, "test");

                    //List<List<LPoint>> tttt = new List<List<LPoint>>();
                    //tttt.Add(dat);
                    //tttt.Add(dat);
                    //tttt.Add(dat);
                    //tttt.Add(dat);
                    //tttt.Add(dat);

                    //FileWorker.LaserSaveManyProfs(tttt, "TestMany");


                    var test = FileWorker.LaserLoadManyProfs("TestMany");
                    Console.WriteLine(test.Count);
                    //foreach (var item in test) {
                    //    Console.WriteLine(item.ToString());
                    //}
                });


                while (true) {
                    Dispatcher.Invoke(InvokerFun);
                    temp_x++;
                    Thread.Sleep(1000);
                }
                void InvokerFun()
                {

                    Laser.GetProfile(out double[] X, out double[] Z);
                    List<LPoint> data = Helper.GetLaserData(X, Z, true);
                    if (data.Count != 0) {
                        LPoint res = LVoronej.Type1_1point(data);

                        double tempdd = LFilters.IsAngle(data);
                        double prov = data.Count / 1.2;
                        // Console.WriteLine($"{tempdd} - {prov}");
                        if (tempdd < prov) {
                            //  Console.WriteLine($"BEEP!  {tempdd} - {prov}");
                            //Console.Beep(1000, 200);
                        }

                        Console.WriteLine(LFilters.GetLineCount(data, minLineLenght: 50, diff: 10, maxDistance: 1));
                        // var lines = LFilters.GetLines(data, minLineLenght: 50, diff: 10, maxDistance: 1);
                        //_LV.SetPoints(lines);

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

        private void bt_start_Click(object sender, RoutedEventArgs e) {
            Start();
            //  test = true;
            //for (int i = -500; i < 500; i++) {
            //    LPoint lp = new LPoint(-11.475, i);
            //    RPoint rp = new RPoint(699.44, -66.74, 247.03, -1.99, -1.12, -27.76);
            //    var tr = Transform.Trans(rp, lp);
            //    if (tr.Z < 190 && tr.Z > 170) {
            //        Console.WriteLine($"i = {i} ==== " + tr.ToString());
            //    }

            //}

            //LPoint lp = new LPoint(-11.475, 217.6);
            //RPoint rp = new RPoint(699.44, -66.74, 247.03, -1.99, -1.12, -27.76);
            //var tr = Transform.Trans(rp, lp);
            //Console.WriteLine(tr.ToString());
        }

        private void bt_stop_Click(object sender, RoutedEventArgs e) {
            test = !test;
            (sender as Button).Content = $"TEST = {test}";

            _R.StopListening();
        }
        private void bt_l1_Click(object sender, RoutedEventArgs e) {
            l1 = !l1;
            (sender as Button).Content = $"l1 = {l1}";
            s.rec = 9;
        }

        private void bt_l2_Click(object sender, RoutedEventArgs e) {
            l2 = !l2;
            (sender as Button).Content = $"l2 = {l2}";
            s.rec = 4;
        }

        private void bt_cal_Click(object sender, RoutedEventArgs e) {
            l2 = false;
            cal = true;
        }

        private void bt_l3_Click(object sender, RoutedEventArgs e) {
            l3 = !l3;
            (sender as Button).Content = $"l3 = {l3}";
            s.rec = 0;
        }
        private void bt_l4_Click(object sender, RoutedEventArgs e) {
            l4 = !l4;
            (sender as Button).Content = $"l4 = {l4}";

        }

        private void bt_oneP_Click(object sender, RoutedEventArgs e) {
            one = true;
        }
    }
}
