using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class BD
    {
        public BD (string id)
        {
            Id = id;
            Simbolos = new LinkedList<Simbolo>();
        }

        public string Id { get; set; }
        public LinkedList<Simbolo> Simbolos { get; set; }

        public void Add(Simbolo sim)
        {
            Simbolos.AddLast(sim);
        }

        public Simbolo GetUserType(string id)
        {
            foreach (Simbolo sim in Simbolos)
            {
                if (sim.Id.Equals(id.ToLower()) && sim.Rol == Rol.USERTYPE)
                    return sim;
            }
            return null;
        }

        public void Recorrer()
        {
            foreach (Simbolo sim in Simbolos)
            {
                Console.WriteLine(sim.Id + " " + sim.Rol.ToString() + " " + sim.Valor.ToString());
               /* if (sim.Valor is Entorno ent)
                    ent.Recorrer();*/

            }
        }
    }
}
