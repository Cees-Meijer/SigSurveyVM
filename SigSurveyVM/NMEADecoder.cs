using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigSurveyVM_Review
{
    class NMEADecoder
    {
        static CultureInfo provider = CultureInfo.InvariantCulture;
        static NumberFormatInfo NumberFormat = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ""
        };
        public struct Nav {
            public double TimeTag;
            public double Time;
            public double Lat;
            public double Lon;
            public double Height;
        };
        public struct Hdg { public double TimeTag;public double Heading; };
        


        private static List<Nav> navigationdata = new List<Nav>();
        public static List<Nav> NavigationData { get => navigationdata; set => navigationdata = value; }

        private static List<Hdg> headingdata = new List<Hdg>();
        public static List<Hdg> HeadingData { get => headingdata; set => headingdata = value; }

        public NMEADecoder()
        {
            
        }
        public static int Decode(ref string[]NMEA)
        {
            foreach (string NMEASentence in NMEA) { Decode(NMEASentence); }
            return NMEA.Length;
        }

        public static void Decode(string NMEASentence)
        {
            string[] Fields = NMEASentence.Split(new char[] { ',','#' });
            int NrOfFields = Fields.Count();
            double timeTag=0;
          //Example TimeTag: 2017-08-25T12:22:42.52+02:00
            string format1 = "HHmmss.ff";
            try
            {
                timeTag = DateTime.Parse(Fields[NrOfFields-1],  provider).ToOADate();
               // Console.WriteLine("{0} converts to {1}.", Fields[NrOfFields-1], timeTag.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", Fields[NrOfFields-1]);
            }
            
            switch (Fields[0])
            {
                case "$GPGGA":
                    try
                    {
                        Nav N = new Nav
                        {
                            TimeTag = timeTag,
                            Time = DateTime.ParseExact(Fields[1],format1, provider,DateTimeStyles.NoCurrentDateDefault).ToOADate(),
                            Lat = Convert.ToDouble(Fields[2], NumberFormat),
                            Lon = Convert.ToDouble(Fields[4], NumberFormat),
                            Height = Convert.ToDouble(Fields[9], NumberFormat)
                        };
                        navigationdata.Add(N);
                    }
                    catch (Exception e) { Console.WriteLine("A number is not in the correct format."); }
                    break;
                case "$GPHDT":
                    try
                    {
                        Hdg H = new Hdg
                        {
                            TimeTag = timeTag,
                            Heading = Convert.ToDouble(Fields[2], NumberFormat),
                        };
                        headingdata.Add(H);
                    }
                    catch (Exception e) { Console.WriteLine("A number is not in the correct format."); }
                    break;
            }
        }
    }
}
