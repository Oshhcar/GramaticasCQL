using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class Eliminar : Instruccion
    {
        public Eliminar(string id, int linea, int columna) : base(linea, columna)
        {
            Id = id;
        }

        public Eliminar(string id, Where where, int linea, int columna) : base(linea, columna)
        {
            Id = id;
            Where = where;
        }

        public string Id { get; set; }
        public Where Where { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            BD actual = e.Master.Actual;

            if (actual != null)
            {
                Simbolo sim = actual.GetTabla(Id);

                if (sim != null)
                {
                    Tabla tabla = (Tabla)sim.Valor;

                    if (Where == null)
                    {
                        tabla.Datos.Clear();
                    }
                    else
                    {
                        LinkedList<Entorno> delete = new LinkedList<Entorno>();

                        foreach (Entorno ent in tabla.Datos)
                        {
                            e.Master.EntornoActual = ent;

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

                            delete.AddLast(ent);
                        }

                        foreach (Entorno ent in delete)
                        {
                            tabla.Datos.Remove(ent);
                        }
                    }
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
