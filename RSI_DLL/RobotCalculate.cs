using CalculateDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSI_DLL {
    public static class RobotCalculate {
        public static RPoint CalculatePoint_2line(List<RPoint> line1, List<RPoint> line2) {
            RPoint result = new RPoint(0, 0, 0);

            int type_line1 = LineType(line1);
            int type_line2 = LineType(line2);

            line1 = LineApprox(line1, type_line1);
            line2 = LineApprox(line2, type_line2);

            var A = line1[0];
            var B = line1[line1.Count - 1];
            var C = line2[0];
            var D = line2[line2.Count - 1];

            double xo = A.X;
            double yo = A.Y;
            double zo = A.Z;
            double p = B.X - A.X;
            double q = B.Y - A.Y;
            double r = B.Z - A.Z;

            double x1 = C.X;
            double y1 = C.Y;
            double z1 = C.Z;
            double p1 = D.X - C.X;
            double q1 = D.Y - C.Y;
            double r1 = D.Z - C.Z;

            double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p);
            double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q);
            double z = (zo * q * r1 - z1 * q1 * r - yo * r * r1 + y1 * r * r1) /
                (q * r1 - q1 * r);

            if (true) {

            }
            while (double.IsNaN(x)) {
                xo = A.X + 0.01;
                yo = A.Y;
                zo = A.Z;
                p = B.X - xo;
                q = B.Y - A.Y;
                r = B.Z - A.Z;

                x1 = C.X + 0.01;
                y1 = C.Y;
                z1 = C.Z;
                p1 = D.X - x1;
                q1 = D.Y - C.Y;
                r1 = D.Z - C.Z;

                x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                   (q * p1 - q1 * p);
                y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                   (p * q1 - p1 * q);
                z = (zo * q * r1 - z1 * q1 * r - yo * r * r1 + y1 * r * r1) /
                   (q * r1 - q1 * r);

                Console.WriteLine($"{x}");
            }

            result = new RPoint(x, y, z);



            #region
            //if (type_line1 == 1) {
            //    if (type_line2 == 2) {

            //    } else if (type_line2 == 2) {
            //    }
            //}
            //else if(type_line1 == 2) {
            //    if (type_line2 == 1) {

            //    } else if (type_line2 == 3) {

            //    }
            //} else if (type_line1 == 3) {
            //    if (type_line2 == 1) {

            //    } else if (type_line2 == 2) {

            //    }
            //}
            #endregion





            return result;
        }
        public static RPoint CalculatePoint_3line(List<RPoint> line1, List<RPoint> line2, List<RPoint> line3) {
            RPoint result = new RPoint(0, 0, 0);
            List<RPoint> tempResult = new List<RPoint>();

            RPoint tempPoint = CalculatePoint_2line(line1, line2);
            tempResult.Add(tempPoint);

            tempPoint = CalculatePoint_2line(line1, line3);
            tempResult.Add(tempPoint);

            tempPoint = CalculatePoint_2line(line3, line2);
            tempResult.Add(tempPoint);

            double x = tempResult.Sum(it => it.X) / tempResult.Count;
            double y = tempResult.Sum(it => it.Y) / tempResult.Count;
            double z = tempResult.Sum(it => it.Z) / tempResult.Count;

            result = new RPoint(x, y, z);
            return result;
        }
        public static int LineType(List<RPoint> line) {
            double x_diff = Math.Abs(line[0].X - line[line.Count - 1].X);
            double y_diff = Math.Abs(line[0].Y - line[line.Count - 1].Y);
            double z_diff = Math.Abs(line[0].Z - line[line.Count - 1].Z);
            if (x_diff > y_diff) {
                if (x_diff > z_diff) {
                    return 1; // 1 - x, 2 - y, 3 - z;
                } else {
                    return 3;
                }
            } else {
                if (y_diff > z_diff) {
                    return 2;
                } else {
                    return 3;
                }
            }
        }
        public static List<RPoint> LineApprox(List<RPoint> line, int type) {
            List<RPoint> result = new List<RPoint>();

            #region временное усреднение соседних точек

            //if (type == 1) {
            //    for (int i = 0; i < line.Count - 1; i++) {
            //        result.Add(new RPoint(line[i].X, (line[i].Y + line[i + 1].Y) / 2, (line[i].Z + line[i + 1].Z) / 2));
            //    }
            //} else if (type == 2) {
            //    for (int i = 0; i < line.Count - 1; i++) {
            //        result.Add(new RPoint((line[i].X + line[i + 1].X) / 2, line[i].Y, (line[i].Z + line[i + 1].Z) / 2));
            //    }
            //} else if (type == 3) {
            //    for (int i = 0; i < line.Count - 1; i++) {
            //        result.Add(new RPoint((line[i].X + line[i + 1].X) / 2, (line[i].Y + line[i + 1].Y) / 2, line[i].Z));
            //    }
            //}
            //result.Add(new RPoint(line[line.Count - 1].X, line[line.Count - 1].Y, line[line.Count - 1].Z));
            #endregion

            #region

            //int count = line.Count;
            //if (type == 1) {
            //    double y_sred = 0;
            //    double z_sred = 0;
            //    for (int i = 0; i < count; i++) {
            //        y_sred += line[i].Y;
            //        z_sred += line[i].Z;
            //    }
            //    y_sred /= count;
            //    z_sred /= count;
            //    for (int i = 0; i < count; i++) {
            //        result.Add(new RPoint(line[i].X, y_sred, z_sred));
            //    }
            //} else if (type == 2) {
            //    double x_sred = 0;
            //    double z_sred = 0;
            //    for (int i = 0; i < count; i++) {
            //        x_sred += line[i].X;
            //        z_sred += line[i].Z;
            //    }
            //    x_sred /= count;
            //    z_sred /= count;
            //    for (int i = 0; i < count; i++) {
            //        result.Add(new RPoint(x_sred, line[i].Y, z_sred));
            //    }
            //} else if (type == 3) {
            //    double y_sred = 0;
            //    double x_sred = 0;
            //    for (int i = 0; i < count; i++) {
            //        y_sred += line[i].Y;
            //        x_sred += line[i].X;
            //    }
            //    y_sred /= count;
            //    x_sred /= count;
            //    for (int i = 0; i < count; i++) {
            //        result.Add(new RPoint(x_sred, y_sred, line[i].X));
            //    }
            //}

            #endregion

            int count = line.Count;
            double a, b, c, d;

            if (type == 1) {
                (a, b) = GetApprox(line.Select(t => t.X).ToArray(), line.Select(t => t.Y).ToArray());
                (c, d) = GetApprox(line.Select(t => t.X).ToArray(), line.Select(t => t.Z).ToArray());
                for (int i = 0; i < count; i++) {
                    result.Add(new RPoint(line[i].X, a * line[i].X + b, c * line[i].X + d));
                }
            } else if (type == 2) {
                (a, b) = GetApprox(line.Select(t => t.Y).ToArray(), line.Select(t => t.X).ToArray());
                (c, d) = GetApprox(line.Select(t => t.Y).ToArray(), line.Select(t => t.Z).ToArray());
                for (int i = 0; i < count; i++) {
                    result.Add(new RPoint(a * line[i].Y + b, line[i].Y, c * line[i].Y + d));
                }
            } else if (type == 3) {
                (a, b) = GetApprox(line.Select(t => t.Z).ToArray(), line.Select(t => t.X).ToArray());
                (c, d) = GetApprox(line.Select(t => t.Z).ToArray(), line.Select(t => t.Y).ToArray());
                for (int i = 0; i < count; i++) {
                    result.Add(new RPoint(a * line[i].Z + b, c * line[i].Z + d, line[i].Z));
                }
            }

            return result;


            (double a1, double b1) GetApprox(double[] x, double[] y)
            {
                double sumx = 0;
                double sumy = 0;
                double sumx2 = 0;
                double sumxy = 0;
                int n = x.Length;
                for (int i = 0; i < n; i++) {
                    sumx += x[i];
                    sumy += y[i];
                    sumx2 += x[i] * x[i];
                    sumxy += x[i] * y[i];
                }
                double a1 = (n * sumxy - (sumx * sumy)) / (n * sumx2 - sumx * sumx);
                double b1 = (sumy - a1 * sumx) / n;
                return (a1, b1);
            }
        }

        public static RPoint CalcPoint4(RPoint po1, RPoint po2, RPoint po3, RPoint po4) {
            double xo = po1.X;
            double yo = po1.Y;
            double zo = po1.Z;
            double p = po2.X - po1.X;
            double q = po2.Y - po1.Y;
            double r = po1.Z - po1.Z;

            double x1 = po3.X;
            double y1 = po3.Y;
            double z1 = po3.Z;
            double p1 = po4.X - po3.X;
            double q1 = po4.Y - po3.Y;
            double r1 = po4.Z - po3.Z;

            double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p);
            double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q);
            double z = (zo * q * r1 - z1 * q1 * r - yo * r * r1 + y1 * r * r1) /
                (q * r1 - q1 * r);

            while (double.IsNaN(x)) {
                 xo = po1.X + 0.001;
                 yo = po1.Y + 0.001;
                 zo = po1.Z + 0.001;
                 p = po2.X - po1.X;
                 q = po2.Y - po1.Y;
                 r = po1.Z - po1.Z;

                 x1 = po3.X;
                 y1 = po3.Y;
                 z1 = po3.Z;
                 p1 = po4.X - po3.X;
                 q1 = po4.Y - po3.Y;
                 r1 = po4.Z - po3.Z;

                x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                   (q * p1 - q1 * p);
                y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                   (p * q1 - p1 * q);
                z = (zo * q * r1 - z1 * q1 * r - yo * r * r1 + y1 * r * r1) /
                   (q * r1 - q1 * r);
            }

            return new RPoint(x, y, z);
        }
        public static bool OnLine(RPoint left, RPoint right, RPoint cur, double dif = 10, double dif2 = 1) {
            double x = cur.X;
            double y = cur.Y;
            double z = cur.Z;
            double x1 = left.X;
            double y1 = left.Y;
            double z1 = left.Z;
            double x2 = right.X;
            double y2 = right.Y;
            double z2 = right.Z;

            double vx = (x - x1);
            double nx = (x2 - x1);

            double vy = (y - y1);
            double ny = (y2 - y1);

            double vz = (z - z1);
            double nz = (z2 - z1);

            bool bx = false;
            bool by = false;
            bool bz = false;

            if (Math.Abs(x - x1) < dif2 && Math.Abs(x - x2) < dif2) {
                bx = true;
            }

            if (Math.Abs(y - y1) < dif2 && Math.Abs(y - y2) < dif2) {
                by = true;
            }

            if (Math.Abs(z - z1) < dif2 && Math.Abs(z - z2) < dif2) {
                bz = true;
            }

            double tx = vx / nx;
            double ty = vy / ny;
            double tz = vz / nz;

            if (Math.Abs(tx - ty) > dif && !bx && !by) {
                return false;
            } else if (Math.Abs(tx - tz) > dif && !bx && !bz) {
                return false;
            } else if (Math.Abs(ty - tz) > dif && !by && !bz) {
                return false;
            }
            return true;
        }

        public static RPoint BaseRotation(RPoint source1, RPoint source2, RPoint find1, RPoint find2) {
            double x, y, z, a, b, c;

            x = find1.X - source1.X;
            y = find1.Y - source1.Y;
            z = find1.Z - source1.Z;
            a = Math.Atan2(y, z) * 180 / Math.PI;
            b = Math.Atan2(y, x) * 180 / Math.PI;
            c = Math.Atan2(z, x) * 180 / Math.PI;


            double sa = Math.Atan2(source2.Y - source1.Y, source2.X - source1.X) * 180 / Math.PI;
            double fa = Math.Atan2(find2.Y - find1.Y, find2.X - find1.X) * 180 / Math.PI;
            a = fa - sa ;

            double sb = Math.Atan2(source2.Z - source1.Z, source2.X - source1.X) * 180 / Math.PI;
            double fb = Math.Atan2(find2.Z - find1.Z, find2.X - find1.X) * 180 / Math.PI;
            b = fb - sb;

            double sc = Math.Atan2(source2.Z - source1.Z, source2.Y - source1.Y) * 180 / Math.PI;
            double fc = Math.Atan2(find2.Z - find1.Z, find2.Y - find1.Y) * 180 / Math.PI;
            c = fc - sc;


            return new RPoint(x, y, z, a, b, c);
        }
    }

}
