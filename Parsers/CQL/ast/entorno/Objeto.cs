using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Objeto
    {
        public Objeto(string id, Entorno entorno)
        {
            Id = id;
            Entorno = entorno;
        }

        public string Id { get; set; }
        public Entorno Entorno { get; set; } /*Reconsiderar guardar solo una lista aquí*/

        public Simbolo GetAtributo(string id)
        {
            foreach (Simbolo sim in Entorno.Simbolos)
            {
                if (sim.Id.Equals(id.ToLower()))// && sim.Rol == Rol.ATRIBUTO)
                    return sim;
            }
            return null;
        }
    }
}
