using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class Where : Expresion
    {
        public Where(Expresion expr, int linea, int columna) : base(linea, columna)
        {
            Expr = expr;
        }

        public Where(Expresion expr, LinkedList<Expresion> inExpr, int linea, int columna) : base(linea, columna)
        {
            Expr = expr;
            InExpr = inExpr;
        }

        public Expresion Expr { get; set; }
        public LinkedList<Expresion> InExpr { get; set; }

        public override object GetValor(Entorno e, LinkedList<string> log, LinkedList<Error> errores)
        {
            if (InExpr == null)
            {
                object valExpr = Expr.GetValor(e, log, errores);

                if (valExpr != null)
                {
                    Tipo = Expr.Tipo;
                    return valExpr;
                }
            }
            return null;
        }
    }
}
