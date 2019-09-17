using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class Actualizar : Instruccion
    {
        public Actualizar(string id, LinkedList<Asignacion> asignaciones, int linea, int columna) : base(linea, columna)
        {
            Id = id;
            Asignaciones = asignaciones;
        }

        public Actualizar(string id, LinkedList<Asignacion> asignaciones, Where where, int linea, int columna) : base(linea, columna)
        {
            Id = id;
            Asignaciones = asignaciones;
            Where = where;
        }

        public string Id { get; set; }
        public LinkedList<Asignacion> Asignaciones { get; set; }
        public Where Where { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            BD actual = e.Master.Actual;

            if (actual != null)
            {
                Simbolo sim = actual.GetTabla(Id);

                if (sim != null)
                {
                    Tabla tabla = (Tabla)sim.Valor;

                    foreach (Entorno ent in tabla.Datos)
                    {
                        e.Master.EntornoActual = ent;

                        if (Where != null)
                        {
                            object valWhere = Where.GetValor(e, log, errores);
                            if (valWhere != null)
                            {
                                if (Where.Tipo.IsBoolean())
                                {
                                    if (!(bool)valWhere)
                                        continue;
                                }
                                else
                                {
                                    errores.AddLast(new Error("Semántico", "Cláusula Where debe ser booleana.", Linea, Columna));
                                    return null;
                                }
                            }
                            else
                                return null;
                        }

                        foreach (Asignacion asigna in Asignaciones)
                        {
                            asigna.Ejecutar(e, funcion, ciclo, sw, log, errores);
                        }
                    }
                    e.Master.EntornoActual = null;
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
