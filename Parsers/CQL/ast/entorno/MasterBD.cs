using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class MasterBD
    {
        public MasterBD()
        {
            Data = new LinkedList<BD>();
        }
        
        public LinkedList<BD> Data { get; set; }
        public BD Actual { get; set; }

        public void Add(string id)
        {
            Data.AddLast(new BD(id.ToLower()));
        }

        public BD Get(string id)
        {
            foreach (BD bd in Data)
            {
                if (bd.Id.Equals(id.ToLower()))
                    return bd;
            }
            return null;
        }

        public bool Drop(string id)
        {
            foreach (BD bd in Data)
            {
                if (bd.Id.Equals(id.ToLower()))
                {
                    bd.Simbolos.Clear();
                    return true;
                }
            }
            return false;
        }

        public void Recorrer()
        {
            Console.WriteLine("***************");
            foreach (BD bd in Data)
            {
                Console.WriteLine("DataBase: " + bd.Id);
                bd.Recorrer();
            }
            if (Actual != null)
                Console.WriteLine("\tActual: " + Actual.Id + "\n\n");
        }
    }
}
