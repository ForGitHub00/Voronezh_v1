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
            return $"X = {X} | Y = {Y} | Z = {Z} | A = {A} | B = {B} | C = {C} | ";
        }
        public double[] ToDoubleMas() {
            return new double[] { X, Y, Z };
        }
    }
}
