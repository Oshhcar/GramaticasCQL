﻿using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ciclos
{
    class DoWhile : Instruccion
    {
        public DoWhile(Expresion expr, Bloque bloque, int linea, int columna) : base(linea, columna)
        {
            Expr = expr;
            Bloque = bloque;
        }

        public Expresion Expr { get; set; }
        public Bloque Bloque { get; set; }
        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            bool condicion;

            do
            {
                object obj = Bloque.Ejecutar(e, funcion, true, sw, log, errores);

                if (obj is Break)
                    break;
                else if (obj is Return)
                    return obj;

                object valExpr = Expr.GetValor(e, log, errores);

                if (valExpr != null)
                {
                    if (Expr.Tipo.IsBoolean())
                    {
                        condicion = (bool)valExpr;
                        continue;
                    }
                    errores.AddLast(new Error("Semántico", "Se esperaba un booleano en condicion do while.", Linea, Columna));
                }
                break;
            } while (condicion);

            return null;
        }
    }
}
