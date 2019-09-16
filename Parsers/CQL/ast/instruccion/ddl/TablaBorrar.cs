﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class TablaBorrar : Instruccion
    {
        public TablaBorrar(string id, bool ifNotExist, int linea, int columna) : base(linea, columna)
        {
            Id = id;
            IfNotExist = ifNotExist;
        }

        public string Id { get; set; }
        public bool IfNotExist { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            BD actual = e.Master.Actual;

            if (actual != null)
            {
                if (!actual.DropTabla(Id))
                {
                    if (!IfNotExist)
                        errores.AddLast(new Error("Semántico", "No existe una Tabla con el id: " + Id + " en la base de datos.", Linea, Columna));
                }
            }
            else
                errores.AddLast(new Error("Semántico", "No se ha seleccionado una base de datos, no se pudo borrar la Tabla.", Linea, Columna));

            return null;
        }
    }
}
