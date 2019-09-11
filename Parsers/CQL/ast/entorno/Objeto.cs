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
        public Entorno Entorno { get; set; }
    }
}
