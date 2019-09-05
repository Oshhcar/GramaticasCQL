using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class Identificador : Expresion
    {
        public Identificador(string id, int linea, int columna) : base(linea, columna)
        {
            Id = id;
        }

        public string Id { get; set; }

        public override object GetValor(Entorno e, LinkedList<Error> errores)
        {
            Simbolo sim = e.Get(Id);

            if (sim != null)
            {
                Tipo = sim.Tipo;
                return sim.Valor;
            }

            errores.AddLast(new Error("Semántico", "No hay una variabla declarada con el id: "+ Id + ".", Linea, Columna));
            return null;
        }

        public override Simbolo GetSimbolo(Entorno e)
        {
            return e.Get(Id);
        }

        public override string GetId()
        {
            return Id;
        }
    }
}
