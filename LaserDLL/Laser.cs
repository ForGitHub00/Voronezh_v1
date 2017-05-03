using CalculateDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaserDLL {
    public class Laser {
        #region Data

        public const int MAX_INTERFACE_COUNT = 5;
        public const int MAX_RESOULUTIONS = 6;

        static public uint m_uiResolution = 0;
        static public uint m_hLLT = 0;
        static public CLLTI.TScannerType m_tscanCONTROLType;
        static public uint toggle = 0;

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


            uint t = 0;

            //Console.WriteLine("FFFFFFFFFFFFFF");
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_MEASURINGFIELD, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_MEASURINGFIELD, t);
            //Console.WriteLine(t);


            Type tt = typeof(CLLTI);
            var mas = tt.GetFields();
            foreach (var item in mas) {
                if (item.Name.Contains("FEATURE") || item.Name.Contains("INQUIRY")) {
                    FieldInfo fi = typeof(CLLTI).GetField(item.Name);
                    object fieldValue = fi.GetValue(t);
                    CLLTI.GetFeature(m_hLLT, (uint)fieldValue, ref t);
                    CLLTI.SetFeature(m_hLLT, (uint)fieldValue, t);
                    //Console.WriteLine($"{item.Name}");
                }
                //Console.WriteLine($"{item.Name}");
            }

            //Console.WriteLine("FFFFFFFFFFFFFF");
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_LASERPOWER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_LASERPOWER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MEASURINGFIELD, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MEASURINGFIELD, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TRIGGER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TRIGGER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROCESSING_PROFILEDATA, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROCESSING_PROFILEDATA, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_THRESHOLD, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_THRESHOLD, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MAINTENANCEFUNCTIONS, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MAINTENANCEFUNCTIONS, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGFREQUENCY, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGFREQUENCY, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGOUTPUTMODES, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGOUTPUTMODES, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CMMTRIGGER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CMMTRIGGER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_REARRANGEMENT_PROFILE, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_REARRANGEMENT_PROFILE, t);
            // Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROFILE_FILTER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROFILE_FILTER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SATURATION, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SATURATION, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TEMPERATURE, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TEMPERATURE, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CAPTURE_QUALITY, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CAPTURE_QUALITY, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHARPNESS, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHARPNESS, t);
            //Console.WriteLine(t);



            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_LASERPOWER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_LASERPOWER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_MEASURINGFIELD, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_MEASURINGFIELD, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_TRIGGER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_TRIGGER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_SHUTTERTIME, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_SHUTTERTIME, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_IDLETIME, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_IDLETIME, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_PROCESSING_PROFILEDATA, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_PROCESSING_PROFILEDATA, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_THRESHOLD, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_THRESHOLD, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_MAINTENANCEFUNCTIONS, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_MAINTENANCEFUNCTIONS, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_ANALOGFREQUENCY, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_ANALOGFREQUENCY, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_ANALOGOUTPUTMODES, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_ANALOGOUTPUTMODES, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_CMMTRIGGER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_CMMTRIGGER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_REARRANGEMENT_PROFILE, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_REARRANGEMENT_PROFILE, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_PROFILE_FILTER, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_PROFILE_FILTER, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_RS422_INTERFACE_FUNCTION, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_RS422_INTERFACE_FUNCTION, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_SATURATION, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_SATURATION, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_TEMPERATURE, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_TEMPERATURE, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_CAPTURE_QUALITY, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_CAPTURE_QUALITY, t);
            //Console.WriteLine(t);
            //CLLTI.GetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_SHARPNESS, ref t);
            //CLLTI.SetFeature(m_hLLT, CLLTI.INQUIRY_FUNCTION_SHARPNESS, t);
            //Console.WriteLine(t);


            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SERIAL, 214020058);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_LASERPOWER, 2);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MEASURINGFIELD, 0);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TRIGGER, 16777316);      //0
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, 1);  //1
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, 3900);  //3999
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROCESSING_PROFILEDATA, 623);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_THRESHOLD, 3200);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MAINTENANCEFUNCTIONS, 2);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGFREQUENCY, 2);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGOUTPUTMODES, 2);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CMMTRIGGER, 0);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_REARRANGEMENT_PROFILE, 2149096449);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROFILE_FILTER, 0);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION, 4);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SATURATION, 4);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TEMPERATURE, 3034);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CAPTURE_QUALITY, 3034);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHARPNESS, 0);
            //// CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MEASURINGFIELD, 0);


            CLLTI.GetLLTType(m_hLLT, ref m_tscanCONTROLType);
            CLLTI.GetResolutions(m_hLLT, auiResolutions, auiResolutions.GetLength(0));

            m_uiResolution = auiResolutions[0];

            CLLTI.SetResolution(m_hLLT, m_uiResolution);
            CLLTI.SetProfileConfig(m_hLLT, CLLTI.TProfileConfig.PROFILE);

            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, uiShutterTime);
            //CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, uiIdleTime);



            //ushort start_z = 20000;
            //ushort size_z = 25000;
            //ushort start_x = 20000;
            //ushort size_x = 25000;
            //WriteCommand(0, 0); // Reset
            //WriteCommand(0, 0); // Initialization
            //WriteCommand(2, 8); // Navigate in register
            //WriteValue2Register(start_z);
            //WriteValue2Register(size_z);
            //WriteValue2Register(start_x);
            //WriteValue2Register(size_x);
            // WriteCommand(0, 0); // Stop writing process

        }
        public static void WriteCommand(uint command, uint data) {
            CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHARPNESS,
            (uint)(command << 9) + (toggle << 8) + data);
            if (toggle == 1) {
                toggle = 0;
            } 
            else {
                toggle = 1;
            }
        }
        // Write value to register position
        static void WriteValue2Register(ushort value) {
            WriteCommand(1, (uint)(value / 256));
            WriteCommand(1, (uint)(value % 256));
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

        public static List<LPoint> GetLines(List<LPoint> data, double maxDistance = 3, double diff = 1, int minLineLenght = 10) {
            List<LPoint> result = new List<LPoint>();
            int dataCount = data.Count;
            int tempCount = 0;
            int tempLenght = minLineLenght;
            bool line = false;
            int index = 0;


            int start = 0;
            int finish = minLineLenght;

            while (finish < dataCount) {
                bool dist = true;
                for (int i = start; i < finish - 1; i++) {
                    if (distanceBetweenTwoPoints(data[i], data[i + 1]) > maxDistance) {
                        dist = false;
                        break;
                    }
                }
                if (countPointsOnline(data, start, finish, diff, s: 1) && dist) {
                    line = true;
                    finish++;
                } else {
                    if (!line) {
                        start++;
                        finish = start + minLineLenght;
                    } else {
                        line = false;
                        result.Add(data[start]);
                        result.Add(data[finish - 1]);
                        start = finish;
                        finish = start + minLineLenght;
                    }

                }
            }
            if (line) {
                result.Add(data[start]);
                result.Add(data[dataCount - 1]);
            }
            return result;
        }
        public static List<int> GetLinesByIndexes(List<LPoint> data, double maxDistance = 3, double diff = 1, int minLineLenght = 10) {
            List<int> result = new List<int>();
            int dataCount = data.Count;
            int tempCount = 0;
            int tempLenght = minLineLenght;
            bool line = false;
            int index = 0;


            int start = 0;
            int finish = minLineLenght;

            while (finish < dataCount) {
                bool dist = true;
                for (int i = start; i < finish - 1; i++) {
                    if (distanceBetweenTwoPoints(data[i], data[i + 1]) > maxDistance) {
                        dist = false;
                        break;
                    }
                }
                if (countPointsOnline(data, start, finish, diff, s: 1) && dist) {
                    line = true;
                    finish++;
                } else {
                    if (!line) {
                        start++;
                        finish = start + minLineLenght;
                    } else {
                        line = false;
                        result.Add(start);
                        result.Add(finish - 1);
                        start = finish;
                        finish = start + minLineLenght;
                    }

                }
            }
            if (line) {
                result.Add(start);
                result.Add(dataCount - 1);
            }
            return result;
        }
        public static int GetLineCount(List<LPoint> data, double maxDistance = 3, double diff = 1, int minLineLenght = 10) {
            int result = 0;
            int dataCount = data.Count;
            int tempCount = 0;
            int tempLenght = minLineLenght;
            bool line = false;
            int index = 0;


            int start = 0;
            int finish = minLineLenght;

            while (finish < dataCount) {
                bool dist = true;
                for (int i = start; i < finish - 1; i++) {
                    if (distanceBetweenTwoPoints(data[i], data[i + 1]) > maxDistance) {
                        dist = false;
                        break;
                    }
                }
                if (countPointsOnline(data, start, finish, diff, s: 1) && dist) {
                    line = true;
                    finish++;
                } else {
                    if (!line) {
                        start++;
                        finish = start + minLineLenght;
                    } else {
                        line = false;
                        result++;
                        start = finish;
                        finish = start + minLineLenght;
                    }

                }
            }
            if (line) {
                result++;
            }



            //while (finish < dataCount) {
            //    tempCount = countPointsOnline(data, start, finish, diff);
            //    if (tempCount == finish - start - 1) {
            //        if (line) {
            //            start = finish - 1;
            //            finish = start + minLineLenght;
            //        } else {
            //            line = true;
            //           // result++;
            //           // Console.WriteLine($"Finish = {finish} | Start = {start}");
            //            finish++;
            //        }
            //    } else {
            //        if (line) {
            //            result++;
            //            Console.WriteLine($"Finish = {finish} | Start = {start}");
            //            line = false;
            //        } else {
            //            start = finish - 1;
            //            finish = start + minLineLenght;
            //        }
            //    }
            //}

            for (int i = 0; i < dataCount - 1; i++) {
                #region
                //if (i + tempLenght < dataCount) {
                //    tempCount = countPointsOnline(data, index, i + tempLenght, diff);
                //    if (tempCount == tempLenght + i - 1) {
                //        //index++;
                //        //tempLenght++;
                //        line = true;
                //    } else {
                //        if (line) {
                //            result++;
                //            i += tempLenght - 1;
                //            index = i;
                //            line = false;
                //        } else {
                //            index = i+ 1;
                //        }
                //    }
                //} else {
                //    if (line) {
                //        result++;
                //    }
                //}

                //tempLenght = minLineLenght;
                //while (index + tempLenght < dataCount && countPointsOnline(data, i, index + tempLenght, diff) == tempLenght - i - 1) {
                //    tempCount = countPointsOnline(data, i, index + tempLenght, diff);

                //    index++;
                //    tempLenght++;
                //    //Console.Write($"s = {index}  tempL = {tempLenght} ");
                //}

                //if (tempLenght != minLineLenght) {
                //    Console.WriteLine($"i = {i}  index = {index} tempL = {tempLenght}");
                //    result++;
                //}
                //index = i;


                //tempCount = countPointsOnline(data, i, i + tempLenght, diff);
                //Console.Write($"s = {i}  temp = {tempCount} ");
                //while (i + tempLenght < dataCount - 1 && (tempCount == tempLenght - i - 1)) {
                //    tempCount = countPointsOnline(data, i, i + tempLenght, diff);
                //    Console.Write($"  temp = {tempCount} ");
                //    line = true;
                //    tempLenght++;
                //}
                //if (line) {
                //    Console.WriteLine($" f = {tempLenght}");
                //    result++;
                //    i += tempLenght;
                //    tempLenght = minLineLenght;
                //}
                #endregion

            }
            return result;

        }
        public static double pointOnLine(LPoint left, LPoint right, LPoint cur) {

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
        private static int countPointsOnline(List<LPoint> data, int start, int finish, double diff = 1) {
            int result = 0;
            for (int i = start + 1; i < finish; i++) {
                if (pointOnLine(data[start], data[finish], data[i]) < diff) {
                    result++;
                }
            }
            // Console.WriteLine(result);
            return result;
        }
        private static bool countPointsOnline(List<LPoint> data, int start, int finish, double diff = 1, int s = 1) {
            int result = 0;
            for (int i = start + 1; i < finish; i++) {
                if (pointOnLine(data[start], data[finish], data[i]) < diff) {
                    result++;
                }
            }
            // Console.WriteLine(result);
            if (finish - start - 1 == result) {
                return true;
            } else {
                return false;
            }
        }
        private static double distanceBetweenTwoPoints(LPoint p1, LPoint p2) {
            double result = 0;
            double x1 = p1.X;
            double y1 = p1.Z;
            double x2 = p2.X;
            double y2 = p2.Z;

            result = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            result = Math.Sqrt(result);

            return result;
        }

        public static List<LPoint> LineAprox(List<LPoint> data) {
            List<LPoint> result = new List<LPoint>();
            double a;
            double b;
            (a, b) = GetApprox(data.Select(t => t.X).ToArray(), data.Select(t => t.Z).ToArray());
            for (int i = 0; i < data.Count; i++) {
                result.Add(new LPoint(data[i].X, a * data[i].X + b));
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
        public static List<LPoint> LineAproxPar(List<LPoint> data, out List<double> par) {
            List<LPoint> result = new List<LPoint>();
            par = new List<double>();
            double a;
            double b;
            (a, b) = GetApprox(data.Select(t => t.X).ToArray(), data.Select(t => t.Z).ToArray());
            par.Add(a);
            par.Add(b);
            for (int i = 0; i < data.Count; i++) {
                result.Add(new LPoint(data[i].X, a * data[i].X + b));
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
