using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Time
    {
        public Time(string time)
        {
            time = time.Replace("\'", "");
            string[] d = time.Split(':');

            Hours = 0;
            Minutes = 0;
            Seconds = 0;

            try
            {
                Hours = Convert.ToInt32(d[0]);
                Minutes = Convert.ToInt32(d[1]);
                Seconds = Convert.ToInt32(d[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Time: " + ex.Message);
            }

        }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public override string ToString()
        {
            return Hours + ":" + Minutes + ":" + Seconds;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
