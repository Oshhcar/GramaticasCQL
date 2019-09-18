using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion
{
    class Open : Instruccion
    {
        public Open(string id, int linea, int columna) : base(linea, columna)
        {
            Id = id;
        }
        public string Id { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, bool tc, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            Simbolo sim = e.Get(Id);

            if (sim != null)
            {
                if (sim.Tipo.IsCursor())
                {
                    Cursor cursor = (Cursor)sim.Valor;
                    cursor.Data = (LinkedList<Entorno>)cursor.Select.Ejecutar(e, funcion, ciclo, sw, tc, log, errores);
                }
                else
                    errores.AddLast(new Error("Semántico", "La variable: " + Id + " no es un Cursor.", Linea, Columna));
            }
            else
                errores.AddLast(new Error("Semántico", "No se ha declarado un Cursor con el id: " + Id + ".", Linea, Columna));

            return null;
        }
    }
}
