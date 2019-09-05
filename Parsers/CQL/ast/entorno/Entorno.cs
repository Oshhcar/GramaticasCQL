using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Entorno
    {
        public Entorno(Entorno padre)
        {
            Simbolos = new LinkedList<Simbolo>();
            Padre = padre;

            if (padre != null)
                Global = padre.Global;
        }

        public LinkedList<Simbolo> Simbolos { get; set; }
        public Entorno Padre { get; set; }
        public Entorno Global { get; set; }

        public void Add(Simbolo sim)
        {
            Simbolos.AddLast(sim);

        }

        public Simbolo Get(string id)
        {
            foreach (Simbolo sim in Simbolos)
            {
                if (sim.Id.Equals(id))
                {
                    return sim;
                }
            }

            return Padre?.Get(id);
        }

        public Simbolo GetLocal(string id)
        {
            foreach (Simbolo sim in Simbolos)
            {
                if (sim.Id.Equals(id))
                {
                    return sim;
                }
            }
            return null;
        }

        public void Recorrer()
        {
            Console.WriteLine("**Entorno**");
            foreach (Simbolo s in Simbolos)
            {
                Console.WriteLine(s.Id + ", " + s.Tipo.Type + ", " + s.Valor.ToString());

            }

            Padre?.Recorrer();
        }
    }
}
