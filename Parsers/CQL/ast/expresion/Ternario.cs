using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class Ternario : Expresion
    {
        public Ternario(Expresion expr, Expresion v, Expresion f, int linea, int columna) : base(linea, columna)
        {
            Expr = expr;
            V = v;
            F = f;
        }

        public Expresion Expr { get; set; }
        public Expresion V { get; set; }
        public Expresion F { get; set; }

        public override object GetValor(Entorno e, LinkedList<Error> errores)
        {
            object valExpr = Expr.GetValor(e, errores);

            if (valExpr != null)
            {
                if (Expr.Tipo.IsBoolean())
                {
                    if ((Boolean)valExpr)
                    {
                        object valV = V.GetValor(e, errores);

                        if (valV != null)
                        {
                            Tipo = V.Tipo;
                            return valV;
                        }
                    }
                    else
                    {
                        object valF = F.GetValor(e, errores);

                        if (valF != null)
                        {
                            Tipo = F.Tipo;
                            return valF;
                        }
                    }
                }
                else
                { 
                    errores.AddLast(new Error("Semántico", "Se esperaba un booleano en expresion ternario.", Linea, Columna));
                }
            }
            return null;
        }
    }
}
