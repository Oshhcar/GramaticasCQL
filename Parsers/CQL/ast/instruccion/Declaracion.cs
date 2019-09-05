﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion
{
    class Declaracion : Instruccion
    {
        public Declaracion(Tipo tipo, LinkedList<Expresion> target, Expresion expr,  int linea, int columna) : base(linea, columna)
        {
            Tipo = tipo;
            Target = target;
            Expr = expr;
        }

        public Tipo Tipo { get; set; }
        public LinkedList<Expresion> Target { get; set; }
        public Expresion Expr { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            object valorExpr = Expr?.GetValor(e, errores);

            if (Expr != null)
            {
                if (valorExpr == null)
                    return null;

                if (!Tipo.Equals(Expr.Tipo))
                {
                    errores.AddLast(new Error("Semántico", "El valor no corresponde al tipo declarado.", Linea, Columna));
                    return null;
                }
            }

            foreach (Expresion target in Target)
            {
                if (target is Identificador id)
                {
                    Simbolo sim = e.GetLocal(id.Id);

                    if (sim != null)
                    {
                        errores.AddLast(new Error("Semántico", "Ya se ha declarado una variable con el id: " + id.Id +".", Linea, Columna));
                        continue;
                    }

                    sim = new Simbolo(Tipo, id.Id, valorExpr);
                    e.Add(sim);
                }
                else
                {
                    errores.AddLast(new Error("Semántico", "Solo se pueden declarar variables.", Linea, Columna));
                }
            }


            return null;
        }
    }
}
