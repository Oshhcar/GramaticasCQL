using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Date
    {
        public Date(string date)
        {
            date = date.Replace("\'", "");
            string[] d = date.Split('-');

            Year = 0;
            Month = 0;
            Day = 0;

            try
            {
                Year = Convert.ToInt32(d[0]);
                Month = Convert.ToInt32(d[1]);
                Day = Convert.ToInt32(d[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Date: " + ex.Message);
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public override string ToString()
        {
            return Year + "-" + Month + "-" + Day;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
