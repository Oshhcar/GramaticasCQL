using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion
{
    class Asignacion : Instruccion
    {
        public Asignacion(Expresion target, Expresion expr, int linea, int columna) : base(linea, columna)
        {
            Target = target;
            Expr = expr;
        }

        public Expresion Target { get; set; }
        public Expresion Expr { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            Object valExpr = Expr.GetValor(e, errores);

            if (valExpr != null)
            {
                Simbolo sim = Target.GetSimbolo(e);

                if (sim != null)
                {
                    if (sim.Tipo.Equals(Expr.Tipo))
                    {
                        sim.Valor = valExpr;
                        return null;
                    }
                    errores.AddLast(new Error("Semántico", "El valor no corresponde al tipo de la variable.", Linea, Columna));
                    return null;
                }
                errores.AddLast(new Error("Semántico", "No se ha declarado una variable con el id: " + Target.GetId() + ".", Linea, Columna));
            }
            return null;
        }
    }
}
