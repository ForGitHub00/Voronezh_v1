using CalculateDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronezh_v1 {
    public static class FileWorker {
        public static string Path = @"Data\";
        public static string Path2 = @"Points\";

        public static void LaserSaveOneProf(List<LPoint> data, string path) {
            System.IO.Directory.CreateDirectory(Path + path);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path + path + @"/Data.txt", true)) {
                foreach (var item in data) {
                    file.WriteLine(item.ToString());
                }
            }
        }
        public static void LaserSaveManyProfs(List<List<LPoint>> data, string path) {
            System.IO.Directory.CreateDirectory(Path + path);
            for (int i = 0; i < data.Count; i++) {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path + path + $"//Data{i}.txt", true)) {
                    foreach (var item in data[i]) {
                        file.WriteLine(item.ToString());
                    }
                }
            }
        }

        public static List<LPoint> LaserLoadOneProf(string path, bool zeroZ = true) {
            double[] x;
            double[] z;
            if (!path.Contains(".txt")) {
                path += "\\Data.txt";
            }
            GetLaserDataFromTXT(Path + path, out x, out z);
            return GetLaserData(x, z, zeroZ);
        }
        public static List<List<LPoint>> LaserLoadManyProfs(string path, bool zeroZ = true) {
            List<List<LPoint>> result = new List<List<LPoint>>();
            var files = System.IO.Directory.GetFiles(Path + path);
            for (int i = 0; i < files.Length; i++) {
                result.Add(LaserLoadOneProf(files[i].Remove(0, 5), zeroZ));
            }
            return result;
        }

        public static void PointsSave(List<RPoint> data, string path) {
            System.IO.Directory.CreateDirectory(Path2 + path);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path + path + @"/Data.txt", true)) {
                foreach (var item in data) {
                    file.WriteLine(item.ToString());
                }
            }
        }

        public static void PointsSave(RPoint point, string path) {
            System.IO.Directory.CreateDirectory(Path2 + path);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path + path + @"/Data.txt", true)) {
                file.WriteLine(point.ToString());
            }
        }
        public static List<RPoint> PointsLoad(string path) {
            path = Path2 + path + @"\Data.txt";
            List<RPoint> result = new List<RPoint>();

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    result.Add(new RPoint(line));
                }
            }
            return result;
        }

        private static void GetLaserDataFromTXT(string path, out double[] X, out double[] Z) {
            List<string> result = new List<string>();
            try {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        result.Add(line);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            X = new double[result.Count];
            Z = new double[result.Count];

            for (int i = 0; i < result.Count; i++) {
                string[] temp = result[i].Split('|');
                X[i] = Convert.ToDouble(temp[0]);
                Z[i] = Convert.ToDouble(temp[1]);
            }


        }
        private static List<LPoint> GetLaserData(double[] X, double[] Z, bool _deleteZeroZ = false) {
            List<LPoint> result = new List<LPoint>();
            for (int i = 0; i < X.Length; i++) {
                if (Z[i] == 0 ^ !_deleteZeroZ) {
                    continue;
                }
                result.Add(new LPoint() {
                    X = X[i],
                    Z = Z[i]
                });
            }
            return result;
        }
    }
}
