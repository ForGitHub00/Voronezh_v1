using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDLL {
    public struct LPoint {
        public double X;
        public double Z;
        public LPoint(double x = 0, double z = 0) {
            X = x;
            Z = z;
        }
        public override string ToString() {
            return $"{X}|{Z}";
        }
        public static bool operator == (LPoint c1, LPoint c2) {
            return c1.Equals(c2);
        }

        public static bool operator != (LPoint c1, LPoint c2) {
            return !c1.Equals(c2);
        }

    }
    public struct RPointCoordinates {
        public double X;
        public double Y;
        public double Z;
        public RPointCoordinates(double x = 0, double y = 0, double z = 0) {
            X = x;
            Y = y;
            Z = z;
        }
    }
    public struct RPointAngles {
        public double A;
        public double B;
        public double C;
    }
    public struct RPoint {
        public RPoint(string str) {
            var res = str.Split('|');
            X = Convert.ToDouble(res[0]);
            Y = Convert.ToDouble(res[1]);
            Z = Convert.ToDouble(res[2]);
            A = Convert.ToDouble(res[3]);
            B = Convert.ToDouble(res[4]);
            C = Convert.ToDouble(res[5]);
        }
        public double X;
        public double Y;
        public double Z;
        public double A;
        public double B;
        public double C;
        public RPoint(double x = 0, double y = 0, double z = 0, double a = 0, double b = 0, double c = 0) {
            X = x;
            Y = y;
            Z = z;
            A = a;
            B = b;
            C = c;
        }
        public override string ToString() {
            return $"{X}|{Y}|{Z}|{A}|{B}|{C}";
        }
        public double[] ToDoubleMas() {
            return new double[] { X, Y, Z };
        }
    }
}
