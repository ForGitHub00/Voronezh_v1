using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LaserDLL {
  public  class Laser2 {
        /* Global variables */
        public const int MAX_INTERFACE_COUNT = 5;
        public const int MAX_RESOULUTIONS = 6;

        static public uint m_uiRecivedProfileCount = 0;
        static public uint m_uiNeededProfileCount = 10;

        static public uint m_uiResolution = 0;
        static public uint m_hLLT = 0;
        static public CLLTI.TScannerType m_tscanCONTROLType;

        static public uint m_uiShutterTime = 100;
        static public uint m_uiIdleTime = 3900;

        [STAThread]
        static void Main(string[] args) {
            scanCONTROL_Sample();
        }

        public static void scanCONTROL_Sample() {
            uint[] auiInterfaces = new uint[MAX_INTERFACE_COUNT];
            uint[] auiResolutions = new uint[MAX_RESOULUTIONS];

            StringBuilder sbDevName = new StringBuilder(100);
            StringBuilder sbVenName = new StringBuilder(100);

            uint uiBufferCount = 20, uiMainReflection = 0, uiPacketSize = 1024;

            int iInterfaceCount = 0;

            int iRetValue;
            bool bOK = true;
            bool bConnected = false;
            ConsoleKeyInfo cki;

            m_hLLT = 0;
            m_uiResolution = 0;

            Console.WriteLine("----- Connect to scanCONTROL -----\n");

            //Create a Ethernet Device -> returns handle to LLT device
            m_hLLT = CLLTI.CreateLLTDevice(CLLTI.TInterfaceType.INTF_TYPE_ETHERNET);
            if (m_hLLT != 0)
                Console.WriteLine("CreateLLTDevice OK");
            else
                Console.WriteLine("Error during CreateLLTDevice\n");

            // Get the available interfaces from the scanCONTROL-device
            iInterfaceCount = 1;
            if (iInterfaceCount <= 0)
                Console.WriteLine("FAST: There is no scanCONTROL connected");
            else if (iInterfaceCount == 1)
                Console.WriteLine("FAST: There is 1 scanCONTROL connected ");
            else
                Console.WriteLine("FAST: There are " + iInterfaceCount + " scanCONTROL's connected");

            if (iInterfaceCount >= 1) {
                uint target4 = auiInterfaces[0] & 0x000000FF;
                uint target3 = (auiInterfaces[0] & 0x0000FF00) >> 8;
                uint target2 = (auiInterfaces[0] & 0x00FF0000) >> 16;
                uint target1 = (auiInterfaces[0] & 0xFF000000) >> 24;

                // Set the first IP address detected by GetDeviceInterfacesFast to handle
                Console.WriteLine("Select the device interface: " + target1 + "." + target2 + "." + target3 + "." + target4);
                if ((iRetValue = CLLTI.SetDeviceInterface(m_hLLT, auiInterfaces[0], 0)) < CLLTI.GENERAL_FUNCTION_OK) {
                    OnError("Error during SetDeviceInterface", iRetValue);
                    bOK = false;
                }

                if (bOK) {
                    // Connect to sensor with the device interface set before
                    Console.WriteLine("Connecting to scanCONTROL");
                    if ((iRetValue = CLLTI.Connect(m_hLLT)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during Connect", iRetValue);
                        bOK = false;
                    } else
                        bConnected = true;
                }

               

                if (bOK) {
                    // Get the scanCONTROL type and check if it is valid
                    Console.WriteLine("Get scanCONTROL type");
                    if ((iRetValue = CLLTI.GetLLTType(m_hLLT, ref m_tscanCONTROLType)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during GetLLTType", iRetValue);
                        bOK = false;
                    }

                    if (iRetValue == CLLTI.GENERAL_FUNCTION_DEVICE_NAME_NOT_SUPPORTED) {
                        Console.WriteLine(" - Can't decode scanCONTROL type. Please contact Micro-Epsilon for a newer version of the LLT.dll.");
                    }

                    if (m_tscanCONTROLType >= CLLTI.TScannerType.scanCONTROL28xx_25 && m_tscanCONTROLType <= CLLTI.TScannerType.scanCONTROL28xx_xxx) {
                        Console.WriteLine(" - The scanCONTROL is a scanCONTROL28xx");
                    } else if (m_tscanCONTROLType >= CLLTI.TScannerType.scanCONTROL27xx_25 && m_tscanCONTROLType <= CLLTI.TScannerType.scanCONTROL27xx_xxx) {
                        Console.WriteLine(" - The scanCONTROL is a scanCONTROL27xx");
                    } else if (m_tscanCONTROLType >= CLLTI.TScannerType.scanCONTROL26xx_25 && m_tscanCONTROLType <= CLLTI.TScannerType.scanCONTROL26xx_xxx) {
                        Console.WriteLine(" - The scanCONTROL is a scanCONTROL26xx");
                    } else if (m_tscanCONTROLType >= CLLTI.TScannerType.scanCONTROL29xx_25 && m_tscanCONTROLType <= CLLTI.TScannerType.scanCONTROL29xx_xxx) {
                        Console.WriteLine(" - The scanCONTROL is a scanCONTROL29xx");
                    } else {
                        Console.WriteLine(" - The scanCONTROL is a undefined type\nPlease contact Micro-Epsilon for a newer SDK");
                    }

                    // Get all possible resolutions for connected sensor and save them in array 
                    Console.WriteLine("Get all possible resolutions");
                    if ((iRetValue = CLLTI.GetResolutions(m_hLLT, auiResolutions, auiResolutions.GetLength(0))) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during GetResolutions", iRetValue);
                        bOK = false;
                    }

                    // Set the max. possible resolution
                    m_uiResolution = auiResolutions[0];
                }


                // Set scanner settings to valid parameters for this example

                if (bOK) {
                    Console.WriteLine("\n----- Set scanCONTROL Parameters -----\n");

                    Console.WriteLine("Set resolution to " + m_uiResolution);
                    if ((iRetValue = CLLTI.SetResolution(m_hLLT, m_uiResolution)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during SetResolution", iRetValue);
                        bOK = false;
                    }
                }



       

              

                if (bOK) {
                    Console.WriteLine("Set Profile config to PROFILE");
                    if ((iRetValue = CLLTI.SetProfileConfig(m_hLLT, CLLTI.TProfileConfig.PROFILE)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during SetProfileConfig", iRetValue);
                        bOK = false;
                    }
                }

                while (true) {

                }

                if (bOK) {
                    Console.WriteLine("Set trigger to internal");
                    if ((iRetValue = CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TRIGGER, 0x00000000)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during SetFeature(FEATURE_FUNCTION_TRIGGER)", iRetValue);
                        bOK = false;
                    }
                }

                if (bOK) {
                    Console.WriteLine("Set shutter time to " + m_uiShutterTime);
                    if ((iRetValue = CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, m_uiShutterTime)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during SetFeature(FEATURE_FUNCTION_SHUTTERTIME)", iRetValue);
                        bOK = false;
                    }
                }

                if (bOK) {
                    Console.WriteLine("Set idle time to " + m_uiIdleTime);
                    if ((iRetValue = CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, m_uiIdleTime)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during SetFeature(FEATURE_FUNCTION_IDLETIME)", iRetValue);
                        bOK = false;
                    }
                }


                // Main tasks in this example
                if (bOK) {
                    Console.WriteLine("\n----- Poll from scanCONTROL -----\n");

                    GetProfiles_Poll();
                }

                Console.WriteLine("\n----- Disconnect from scanCONTROL -----\n");

                if (bConnected) {
                    // Disconnect from the sensor
                    Console.WriteLine("Disconnect the scanCONTROL");
                    if ((iRetValue = CLLTI.Disconnect(m_hLLT)) < CLLTI.GENERAL_FUNCTION_OK) {
                        OnError("Error during Disconnect", iRetValue);
                    }
                }

                
            }

            //Wait for a keyboard hit
            while (true) {
                cki = Console.ReadKey();
                if (cki.KeyChar != 0) {
                    break;
                }
            }
        }

        /*
         * Evalute reveived profiles in polling mode
         */
        static void GetProfiles_Poll() {
            int iRetValue;
            uint uiLostProfiles = 0;
            double[] adValueX = new double[m_uiResolution];
            double[] adValueZ = new double[m_uiResolution];

            // Allocate profile buffer to the maximal profile size
            byte[] abyProfileBuffer = new byte[m_uiResolution * 64];
            byte[] abyTimestamp = new byte[16];

            // Allocate buffer for multiple profiles
            byte[] abyFullProfileBuffer = new byte[m_uiResolution * 64 * m_uiNeededProfileCount];

            Console.WriteLine("Demonstrate the profile transfer via poll function");

            // Start continous profile transmission
            Console.WriteLine("Enable the measurement");
            if ((iRetValue = CLLTI.TransferProfiles(m_hLLT, CLLTI.TTransferProfileType.NORMAL_TRANSFER, 1)) < CLLTI.GENERAL_FUNCTION_OK) {
                OnError("Error during TransferProfiles", iRetValue);
                return;
            }

            //Sleep for a while to warm up the transfer
            System.Threading.Thread.Sleep(100);

            /*
             * This shows how to get multiple Profile in polling mode with PROFILE config, which means all data is evaluated.
             * To see how to get a single profiles with the PURE_PROFILE config, see the Firewire Poll example!	
             */
            while (m_uiRecivedProfileCount < m_uiNeededProfileCount) {
                // Get the next transmitted partial profile
                if ((iRetValue = CLLTI.GetActualProfile(m_hLLT, abyProfileBuffer, abyProfileBuffer.GetLength(0), CLLTI.TProfileConfig.PROFILE, ref uiLostProfiles))
                                                     != abyProfileBuffer.GetLength(0)) {
                    OnError("Error during GetActualProfile", iRetValue);
                    return;
                }
                Console.WriteLine("Get profile in polling-mode and PROFILE configuration OK");

                // Copy received buffer to buffer for multiple profiles
                Array.Copy(abyProfileBuffer, 0, abyFullProfileBuffer, m_uiRecivedProfileCount * m_uiResolution * 64, abyProfileBuffer.GetLength(0));

                // Sleep according to scanner frequency (for high frequencies it is recommended to use the callback example)
                System.Threading.Thread.Sleep((int)(m_uiShutterTime + m_uiIdleTime) / 100);
                m_uiRecivedProfileCount++;
            }

            // Convert partial profile to x and z values
            Console.WriteLine("Converting of profile data from the first reflection");
            iRetValue = CLLTI.ConvertProfile2Values(m_hLLT, abyProfileBuffer, m_uiResolution, CLLTI.TProfileConfig.PROFILE, m_tscanCONTROLType, 0, 1, null, null, null, adValueX, adValueZ, null, null);
            if (((iRetValue & CLLTI.CONVERT_X) == 0) || ((iRetValue & CLLTI.CONVERT_Z) == 0)) {
                OnError("Error during Converting of profile data", iRetValue);
                return;
            }

            // Display x and z values
            DisplayProfile(adValueX, adValueZ, m_uiResolution);

            Console.WriteLine("Display the timestamp from the profile:");

            // Extract the 16-byte timestamp from the profile buffer into timestamp buffer and display it
            for (int i = 1; i < m_uiNeededProfileCount; i++) {
                for (int iPos = 0; iPos < 16; iPos++) {
                    abyTimestamp[iPos] = abyFullProfileBuffer[(i * m_uiResolution * 64 - 16) + iPos];
                }
                DisplayTimestamp(abyTimestamp);
            }

            // Stop continous profile transmission
            Console.WriteLine("Disable the measurement");
            if ((iRetValue = CLLTI.TransferProfiles(m_hLLT, CLLTI.TTransferProfileType.NORMAL_TRANSFER, 0)) < CLLTI.GENERAL_FUNCTION_OK) {
                OnError("Error during TransferProfiles", iRetValue);
                return;
            }
        }

        // Display the X/Z-Data of one profile
        static void DisplayProfile(double[] adValueX, double[] adValueZ, uint uiResolution) {
            int iNumberSize = 0;
            for (uint i = 0; i < uiResolution; i++) {

                //Prints the X- and Z-values
                iNumberSize = adValueX[i].ToString().Length;
                Console.Write("\r" + "Profiledata: X = " + adValueX[i].ToString());

                for (; iNumberSize < 8; iNumberSize++) {
                    Console.Write(" ");
                }

                iNumberSize = adValueZ[i].ToString().Length;
                Console.Write(" Z = " + adValueZ[i].ToString());

                for (; iNumberSize < 8; iNumberSize++) {
                    Console.Write(" ");
                }

                //Somtimes wait a short time (only for display)
                if (i % 8 == 0) {
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        // Display the timestamp
        static void DisplayTimestamp(byte[] abyTimestamp) {
            double dShutterOpen = 0, dShutterClose = 0;
            uint uiProfileCount = 0;

            //Decode the timestamp
            CLLTI.Timestamp2TimeAndCount(abyTimestamp, ref dShutterOpen, ref dShutterClose, ref uiProfileCount);
            Console.WriteLine("ShutterOpen: " + dShutterOpen + " ShutterClose: " + dShutterClose);
            Console.WriteLine("ProfileCount: " + uiProfileCount);
        }

        // Display the error text
        static void OnError(string strErrorTxt, int iErrorValue) {
            byte[] acErrorString = new byte[200];

            Console.WriteLine(strErrorTxt);
            if (CLLTI.TranslateErrorValue(m_hLLT, iErrorValue, acErrorString, acErrorString.GetLength(0))
                                            >= CLLTI.GENERAL_FUNCTION_OK)
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(acErrorString, 0, acErrorString.GetLength(0)));
        }
    }
}

