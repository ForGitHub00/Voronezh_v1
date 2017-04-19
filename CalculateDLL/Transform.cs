﻿using CalculateDLL.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDLL {
    public class Transform {

        public static RPoint Trans(RPoint robP, LPoint lasP) {
            double X = Convert.ToDouble(Resources.CalibX.Replace('.', ','));
            double Y = Convert.ToDouble(Resources.CalibY.Replace('.', ','));
            double Z = Convert.ToDouble(Resources.CalibZ.Replace('.', ','));

            RPoint res = Transform.Trans(robP.X, robP.Y, robP.Z, robP.A, robP.B, robP.C, X, -Y + lasP.X, -(Z - lasP.Z)); //todo CalibY + X ???
            //RPoint res = Transform.Trans(robP.X, robP.Y, robP.Z, 0, 0, 0, CalibX, -CalibY + lasP.X, -(CalibZ - lasP.Z)); //todo CalibY + X ???
            //Map.Add(res);
            return res;
        }


        public static RPoint Trans(double x, double y, double z, double a, double b, double c, double lx, double ly, double lz) {
            double[][] mat = matrix(x, y, z, a, b, c);
            double[] point = { lx, ly, lz };
            double[] point_transformed = mul(mat, point);

            return new RPoint() {
                X = point_transformed[0],
                Y = point_transformed[1],
                Z = point_transformed[2]
            };
        }        
        public static double[][] matrix(double x, double y, double z, double aDeg, double bDeg, double cDeg) {
            //ABC: Euler angles, A: round z-axis     B: round y-axis        C: round y-axis
            double a = -aDeg * Math.PI / 180;
            double b = -bDeg * Math.PI / 180;
            double c = -cDeg * Math.PI / 180;
            double ca = Math.Cos(a);
            double sa = Math.Sin(a);
            double cb = Math.Cos(b);
            double sb = Math.Sin(b);
            double cc = Math.Cos(c);
            double sc = Math.Sin(c);
            double[][] tt = new double[3][];
            tt[0] = new double[] { ca * cb, sa * cc + ca * sb * sc, sa * sc - ca * sb * cc, x };
            tt[1] = new double[] { -sa * cb, ca * cc - sa * sb * sc, ca * sc + sa * sb * cc, y };
            tt[2] = new double[] { sb, -cb * sc, cb * cc, z };
            return tt;
        }
        public static double[] mul(double[][] a, double[] b) { // multiple a 3*4 (or 3*3) matrix with a vector
            double[] re = new double[3];
            int len = a[0].Length;
            if (len == 4) {
                for (int i = 0; i < 3; i++)
                    re[i] = a[i][0] * b[0] + a[i][1] * b[1] + a[i][2] * b[2] + a[i][3];
            } else if (len == 3) {
                for (int i = 0; i < 3; i++)
                    re[i] = a[i][0] * b[0] + a[i][1] * b[1] + a[i][2] * b[2];
            }
            return re;

        }
    }
}
