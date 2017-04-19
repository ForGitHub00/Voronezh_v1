using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RSI_DLL {
    public static class ParserXML {
        public static void SetValue(ref string strXML, string[] par, double val) {
            try {
                XDocument xdoc = XDocument.Parse(strXML);
                if (par.Length == 3) {
                    foreach (XElement phoneElement in xdoc.Element(par[0]).Elements(par[1])) {
                        XAttribute nameAttribute = phoneElement.Attribute(par[2]);
                        if (nameAttribute != null) {
                            nameAttribute.Value = val.ToString().Replace(',', '.');
                        }
                    }
                } else if (par.Length == 2) {
                    foreach (XElement phoneElement in xdoc.Element(par[0]).Elements(par[1])) {
                        phoneElement.Value = val.ToString().Replace(',', '.');
                    }
                }
                strXML = xdoc.ToString();
            } catch (Exception ex) {
                Console.WriteLine("Parser SetValue ERROR: " + ex.Message);
            }
        }
        public static void SetValue(ref string strXML, string par, double val) {
            try {
                string[] temp = par.Split('\\').Where(X => X != "").ToArray();
                SetValue(ref strXML, temp, val);
            } catch (Exception ex) {
                Console.WriteLine("Parser SetValue2 ERROR: " + ex.Message);
            }
        }
        public static double GetValues(string strXML, string[] par) {
            XDocument xdoc = XDocument.Parse(strXML);
            if (par.Length == 3) {
                foreach (XElement phoneElement in xdoc.Element(par[0]).Elements(par[1])) {
                    XAttribute nameAttribute = phoneElement.Attribute(par[2]);
                    if (nameAttribute != null) {
                        return Convert.ToDouble(nameAttribute.Value.Replace('.', ','));
                    }
                }
            } else if (par.Length == 2) {
                foreach (XElement phoneElement in xdoc.Element(par[0]).Elements(par[1])) {
                    return Convert.ToDouble(phoneElement.Value.Replace('.', ','));
                }
            }
            return 0;
        }
        public static double GetValues(string strXML, string par) {
            try {
                string[] temp = par.Split('\\').Where(X => X != "").ToArray();
                return GetValues(strXML, temp);
            } catch (Exception ex) {
                Console.WriteLine("Parser SetValue2 ERROR: " + ex.Message);
            }
            return 0;
        }
    }
}
