using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Cadena
    {
        public Cadena(string valor)
        {
            Valor = valor.Substring(1, valor.Length-2);
        }

        public string Valor { get; set; }

        public override string ToString()
        {
            return Valor;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
