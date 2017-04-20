using CalculateDLL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserDLL {
    public class Laser {
        #region Data

        public const int MAX_INTERFACE_COUNT = 5;
        public const int MAX_RESOULUTIONS = 6;

        static public uint m_uiResolution = 0;
        static public uint m_hLLT = 0;
        static public CLLTI.TScannerType m_tscanCONTROLType;

        [STAThread]
        public static void main() {
            Init();
        }

        [STAThread]
        public static void Init() {
            uint[] auiFirewireInterfaces = new uint[MAX_INTERFACE_COUNT];
            uint[] auiResolutions = new uint[MAX_RESOULUTIONS];
            uint uiShutterTime = 11;
            uint uiIdleTime = 900;

            m_hLLT = 0;
            m_uiResolution = 0;


            m_hLLT = CLLTI.CreateLLTDevice(CLLTI.TInterfaceType.INTF_TYPE_ETHERNET);
            CLLTI.SetDeviceInterface(m_hLLT, 3232240897, 0);
            CLLTI.Connect(m_hLLT);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SERIAL, 214020058);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_LASERPOWER, 2);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MEASURINGFIELD, 0);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TRIGGER, 0);      //0
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, 1);  //100
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, 3999);  //900
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROCESSING_PROFILEDATA, 2639);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_THRESHOLD, 3200);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MAINTENANCEFUNCTIONS, 2);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGFREQUENCY, 2);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGOUTPUTMODES, 2);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CMMTRIGGER, 0);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_REARRANGEMENT_PROFILE, 2149096449);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROFILE_FILTER, 0);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION, 4);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SATURATION, 4);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TEMPERATURE, 3034);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CAPTURE_QUALITY, 3034);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHARPNESS, 0);


            CLLTI.GetLLTType(m_hLLT, ref m_tscanCONTROLType);
            CLLTI.GetResolutions(m_hLLT, auiResolutions, auiResolutions.GetLength(0));

            m_uiResolution = auiResolutions[0];

            CLLTI.SetResolution(m_hLLT, m_uiResolution);
            CLLTI.SetProfileConfig(m_hLLT, CLLTI.TProfileConfig.PROFILE);

            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, uiShutterTime);
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, uiIdleTime);

        }

        public static void GetProfile(out double[] adValueX, out double[] adValueZ) {
            int iRetValue;
            uint uiLostProfiles = 0;
            adValueX = new double[m_uiResolution];
            adValueZ = new double[m_uiResolution];

            //Resize the profile buffer to the maximal profile size
            byte[] abyProfileBuffer = new byte[m_uiResolution * 4 + 16];
            byte[] abyTimestamp = new byte[16];

            CLLTI.TransferProfiles(m_hLLT, CLLTI.TTransferProfileType.NORMAL_TRANSFER, 1);

            //Sleep for a while to warm up the transfer
            System.Threading.Thread.Sleep(12);

            //Gets 1 profile in "polling-mode" and PURE_PROFILE configuration
            CLLTI.GetActualProfile(m_hLLT, abyProfileBuffer, abyProfileBuffer.GetLength(0), CLLTI.TProfileConfig.PURE_PROFILE, ref uiLostProfiles);

            iRetValue = CLLTI.ConvertProfile2Values(m_hLLT, abyProfileBuffer, m_uiResolution, CLLTI.TProfileConfig.PURE_PROFILE, m_tscanCONTROLType,
              0, 1, null, null, null, adValueX, adValueZ, null, null);

            //if (((iRetValue & CLLTI.CONVERT_X) == 0) || ((iRetValue & CLLTI.CONVERT_Z) == 0)) {
            //    Console.WriteLine("Error during Converting of profile data", iRetValue);
            //    return;
            //}
        }
        #endregion       
    }
    #region Calculate
    public static class LFilters {
        public static List<LPoint> SortByX(List<LPoint> data, bool _rightToLeft = false) {
            List<LPoint> res = new List<LPoint>();
            if (_rightToLeft) {
                res = data.OrderBy(item => -item.X).ToList();
            } else {
                res = data.OrderBy(item => item.X).ToList();
            }
            return res;
        }
        public static List<LPoint> SortByZ(List<LPoint> data, bool _topToBot = false) {
            List<LPoint> res = new List<LPoint>();
            if (_topToBot) {
                res = data.OrderBy(item => item.Z).ToList();
            } else {
                res = data.OrderBy(item => -item.Z).ToList();
            }
            return res;
        }
        public static List<LPoint> AveragingVertical(List<LPoint> data, double _verticalStep = 1, double _horizontalStep = 100, int _pointsInStep = 2) {
            List<LPoint> res = data;
            int current = 0;
            int dataCount = data.Count;
            while (current + _pointsInStep < dataCount) {
                if (Math.Abs(data[current].Z - data[current + _pointsInStep].Z) <= _verticalStep && Math.Abs(data[current].X - data[current + _pointsInStep].X) <= _horizontalStep) {
                    double tempZ = (data[current].Z + data[current + _pointsInStep].Z) / 2;
                    for (int i = current + 1; i < current + _pointsInStep; i++) {
                        data[i] = new LPoint() {
                            Z = tempZ,
                            X = data[i].X
                        };
                    }
                }
                current += _pointsInStep - 1;
                //current++;
            }
            return res;
        }
        public static List<LPoint> AveragingVerticalPro(List<LPoint> data, double _verticalStep = 1, double _horizontalStep = 100,
            int _pointsInStep = 2, bool _everyPoint = false) {
            List<LPoint> res = data;
            int current = 0;
            int dataCount = data.Count;
            while (current + _pointsInStep < dataCount) {
                if (Math.Abs(data[current].Z - data[current + _pointsInStep].Z) <= _verticalStep && Math.Abs(data[current].X - data[current + _pointsInStep].X) <= _horizontalStep) {
                    for (int i = current + 1; i < current + _pointsInStep; i++) {
                        data[i] = new LPoint() {
                            Z = _calcTempZ(data[current], data[current + _pointsInStep], data[i].X),
                            X = data[i].X
                        };
                    }
                }
                if (_everyPoint) {
                    current++;
                } else {
                    current += _pointsInStep - 1;
                }
            }
            return res;

            double _calcTempZ(LPoint p1, LPoint p2, double difX)
            {
                double x1 = Math.Min(p1.X, p2.X);
                double x2 = Math.Max(p1.X, p2.X);
                double x = difX;


                double z1 = p1.X == x1 ? p1.Z : p2.Z;
                double z2 = p1.X == x2 ? p1.Z : p2.Z;

                double zRes = (((x - x1) * (z2 - z1)) / (x2 - x1)) + z1;


                return zRes;
            }

        }
        public static int IsAngle(List<LPoint> data, double dif = 0.15) {
            int result = 0;
            var point = LVoronej.Type1_1point(data);
            for (int i = 1; i < data.Count - 1; i++) {
                // if (Math.Abs(OnLine(data[0], data[data.Count - 1], data[i])) < dif) {
                if (Math.Abs(pointOnLine(point, data[data.Count - 1], data[i])) < dif) {
                    result++;
                }
            }
            return result;
        }

        public static int GetLineCount(List<LPoint> data, double maxDistance = 3, double diff = 1, int minLineLenght = 10) {
            int result = 0;
            int dataCount = data.Count;
            int tempCount = 0;
            int tempLenght = minLineLenght;

            for (int i = 0; i < dataCount; i++) {
                bool line = false;
                while (i + tempLenght < dataCount - 1 && (countPointsOnline(data, i, i + tempLenght, diff) == tempLenght - i - 1)) {
                    line = true;
                    tempLenght++;
                }
                if (line) {
                    result++;
                    i += tempLenght;
                    tempLenght = minLineLenght;
                }
            }
            return result;
        }
        public static double pointOnLine(LPoint left, LPoint right, LPoint cur) {
            if (true) {
                double x = cur.X;
                double y = cur.Z;
                double x1 = left.X;
                double y1 = left.Z;
                double x2 = right.X;
                double y2 = right.Z;


                

                double tx = (x - x1) * (y2 - y1);
                double ty = (y - y1) * (x2 - x1);

               // Console.WriteLine(tx - ty);
                return Math.Abs(tx - ty);
            }
        }
        private static int countPointsOnline(List<LPoint> data, int start, int finish, double diff = 1) {
            int result = 0;
            for (int i = start + 1; i < finish; i++) {
                if (pointOnLine(data[start], data[finish], data[i]) < diff) {
                    result++;
                }
            }
            Console.WriteLine(result);
            return result;
        }

    }
    public static class LVoronej {
        public static LPoint Type1_1point(List<LPoint> data, double lifting = 0) {
            LPoint result = new LPoint() { X = 0, Z = 0 };
            if (data.Count != 0) {
                result = data.OrderBy(item => item.Z).ToList()[0];
            }
            return new LPoint(result.X, result.Z + lifting);
        }

        public static (LPoint left, LPoint right) Type3_2point(List<LPoint> data) {
            LPoint left = new LPoint(0, 0), right = new LPoint(0, 0);
            Dictionary<int, double> diffs = new Dictionary<int, double>();
            for (int i = 0; i < data.Count - 1; i++) {
                diffs.Add(i, calculate_ratio(data[i], data[i + 1]));
            }

            //int count = (int)(data.Count * percent);
            var tempResult = diffs.OrderByDescending(pair => pair.Value).Take(2).ToDictionary(pair => pair.Key, pair => pair.Value);

            List<LPoint> result = new List<LPoint>();
            foreach (var item in tempResult) {
                if (data[item.Key + 1].Z > data[item.Key].Z) {
                    result.Add(data[item.Key + 1]);
                } else {
                    result.Add(data[item.Key]);
                }
            }

            if (tempResult.Count > 1) {
                if (result[0].X < result[1].X) {
                    left = result[0];
                    right = result[1];
                } else {
                    left = result[1];
                    right = result[0];
                }
            }



            return (left, right);

            double calculate_ratio(LPoint p1, LPoint p2)
            {
                double xDiff = Math.Abs(p1.X - p2.X);
                double zDiff = Math.Abs(p1.Z - p2.Z);
                return zDiff / xDiff;
            }


        }

        public static LPoint Type3_1point(List<LPoint> data, double decrease = 0) {
            LPoint left = new LPoint(0, 0), right = new LPoint(0, 0);
            (left, right) = Type3_2point(data);

            return new LPoint((left.X + right.X) / 2, (left.Z + right.Z) / 2 - decrease);
        }

    }
    #endregion

    #region
    // Угловой шов
    public static class AngularSeam {
        public static List<LPoint> FindAngularSeamByRegions(List<LPoint> data, int countOfRegions = 10) {
            LPoint result = new LPoint() { X = 0, Z = 0 };
            List<LPoint> tempResult = new List<LPoint>();

            int pointsInRegion = data.Count / countOfRegions;
            for (int t = 0; t < countOfRegions; t++) {
                double min = Double.MaxValue;
                LPoint tempPoint = new LPoint();
                for (int i = t * pointsInRegion; i < (t + 1) * pointsInRegion; i++) {
                    if (data[i].Z < min) {
                        min = data[i].Z;
                        tempPoint = data[i];
                    }
                }
                tempResult.Add(tempPoint);
            }
            result = tempResult[5];

            return tempResult;
        }

        public static LPoint FindAngularSeamSimple(List<LPoint> data) {
            LPoint result = new LPoint() { X = 0, Z = 0 };
            if (data.Count != 0) {
                result = data.OrderBy(item => item.Z).ToList()[0];
            }
            return result;
        }
        public static List<LPoint> FindMasPoint_ZX_Diff(List<LPoint> data, int count) {
            //поиск по каждой соседней точке, по отношению dz к dx
            Dictionary<int, double> diffs = new Dictionary<int, double>();
            for (int i = 0; i < data.Count - 1; i++) {
                diffs.Add(i, calculate_ratio(data[i], data[i + 1]));
            }

            //int count = (int)(data.Count * percent);
            var tempResult = diffs.OrderByDescending(pair => pair.Value).Take(count).ToDictionary(pair => pair.Key, pair => pair.Value);

            List<LPoint> result = new List<LPoint>();
            foreach (var item in tempResult) {
                if (data[item.Key + 1].Z > data[item.Key].Z) {
                    result.Add(data[item.Key + 1]);
                } else {
                    result.Add(data[item.Key]);
                }
            }
            return result;

            double calculate_ratio(LPoint p1, LPoint p2)
            {
                double xDiff = Math.Abs(p1.X - p2.X);
                double zDiff = Math.Abs(p1.Z - p2.Z);
                return zDiff / xDiff;
            }
        }
        public static (LPoint left, LPoint right) FindTwoPoints(List<LPoint> data) {
            LPoint Lres = new LPoint() { X = 0, Z = 0 };
            LPoint Rres = new LPoint() { X = 0, Z = 0 };
            //TODO
            return (Lres, Rres);
        }
        public static List<LPoint> FindMasPoint_Z_Diff(List<LPoint> data, int count) {
            Dictionary<int, double> diffs = new Dictionary<int, double>();
            for (int i = 0; i < data.Count - 1; i++) {
                diffs.Add(i, Math.Abs(data[i].Z - data[i + 1].Z));
            }

            //int count = (int)(data.Count * percent);
            var tempResult = diffs.OrderByDescending(pair => pair.Value).Take(count).ToDictionary(pair => pair.Key, pair => pair.Value);

            List<LPoint> result = new List<LPoint>();
            foreach (var item in tempResult) {
                if (data[item.Key + 1].Z > data[item.Key].Z) {
                    result.Add(data[item.Key + 1]);
                } else {
                    result.Add(data[item.Key]);
                }
            }
            return result;

            double calculate_ratio(LPoint p1, LPoint p2)
            {
                double xDiff = Math.Abs(p1.X - p2.X);
                double zDiff = Math.Abs(p1.Z - p2.Z);
                return zDiff / xDiff;
            }
        }
    }
    // Стыковое соединение
    public static class SpliceSeam {
        public static LPoint FindOnePoint(List<LPoint> data) {
            LPoint result = new LPoint() { X = 0, Z = 0 };
            //TODO
            return result;
        }
        public static (LPoint left, LPoint right) FindTwoPoints(List<LPoint> data) {
            LPoint Lres = new LPoint() { X = 0, Z = 0 };
            LPoint Rres = new LPoint() { X = 0, Z = 0 };
            //TODO
            return (Lres, Rres);
        }
    }
    #endregion

}
