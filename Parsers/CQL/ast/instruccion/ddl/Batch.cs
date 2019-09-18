using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class Batch : Instruccion
    {
        public Batch(LinkedList<Instruccion> inst, int linea, int columna) : base(linea, columna)
        {
            Inst = inst;
        }

        public LinkedList<Instruccion> Inst { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            foreach (Instruccion inst in Inst)
            {
                if (inst is Actualizar act)
                {
                    act.Ejecutar(e, funcion, ciclo, sw, log, errores);
                    if (!act.Correcto)
                    {
                        errores.AddLast(new Error("Semántico", "Error en Batch.", Linea, Columna));
                        return null;
                    }
                }
                else if (inst is Insertar inser)
                {
                    inser.Ejecutar(e, funcion, ciclo, sw, log, errores);
                    if (!inser.Correcto)
                    {
                        errores.AddLast(new Error("Semántico", "Error en Batch.", Linea, Columna));
                        return null;
                    }
                }
                else if (inst is Eliminar eli)
                {
                    eli.Ejecutar(e, funcion, ciclo, sw, log, errores);
                    if (!eli.Correcto)
                    {
                        errores.AddLast(new Error("Semántico", "Error en Batch.", Linea, Columna));
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
