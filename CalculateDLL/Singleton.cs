using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDLL {
    public class Singleton {
        private static readonly Singleton instance = new Singleton();

        public string Name { get; set; }

        private Singleton() {
            Name = Guid.NewGuid().ToString();
            _MAP = new List<RPoint>();
            Position = new RPoint(x: Double.MinValue);
            recive_p = new RPoint(x: Double.MinValue);
        }
        public List<RPoint> _MAP;
        public RPoint Position;
        public RPoint recive_p;
        public int work;
        public int rec;

        private string _strRecive;

        public string StrRecive {
            get { return _strRecive; }
            set { _strRecive = value; }
        }


        public static Singleton GetInstance() {
            return instance;
        }
    }
}
