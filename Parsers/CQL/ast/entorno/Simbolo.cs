using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Simbolo
    {

        public Simbolo(Tipo tipo, string id)
        {
            Tipo = tipo;
            Id = id;
            Valor = Predefinido();
        }

        public Simbolo(Tipo tipo, string id, object valor)
        {
            Tipo = tipo;
            Id = id;
            Valor = valor ?? Predefinido();
        }

        public Tipo Tipo { get; set; }
        public string Id { get; set; }
        public object Valor { get; set; }

        public object Predefinido()
        {
            if (Tipo.IsInt())
                return 0;
            else if (Tipo.IsDouble())
                return 0.0;
            else if (Tipo.IsBoolean())
                return false;
            else
                return new Null();
        }

    }
}
