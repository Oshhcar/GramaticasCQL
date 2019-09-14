using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class Actualizar : Instruccion
    {
        public Actualizar(string id, LinkedList<Asignacion> asignaciones, int linea, int columna) : base(linea, columna)
        {
            Id = id;
            Asignaciones = asignaciones;
        }

        public string Id { get; set; }
        public LinkedList<Asignacion> Asignaciones { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            BD actual = e.Master.Actual;

            if (actual != null)
            {
                Simbolo sim = actual.GetTabla(Id);

                if (sim != null)
                {

                }
                else
                    errores.AddLast(new Error("Semántico", "No existe una Tabla con el id: " + Id + " en la base de datos.", Linea, Columna));
            }
            else
                errores.AddLast(new Error("Semántico", "No se ha seleccionado una base de datos, no se pudo Actualizar.", Linea, Columna));

            return null;
        }
    }
}
