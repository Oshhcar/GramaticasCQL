using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ciclos
{
    class For : Instruccion
    {
        public For(Instruccion init, Expresion expr, NodoASTCQL update, Bloque bloque, int linea, int columna) : base(linea, columna)
        {
            Init = init;
            Expr = expr;
            Update = update;
            Bloque = bloque;
        }

        public Instruccion Init { get; set; }
        public Expresion Expr { get; set; }
        public NodoASTCQL Update { get; set; }
        public Bloque Bloque { get; set; }
        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, bool tc, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            Entorno local = new Entorno(e);

            Init.Ejecutar(local, funcion, ciclo, sw, tc, log, errores);

            object valExpr = Expr.GetValor(local, log, errores);

            if (valExpr != null)
            {
                if (Expr.Tipo.IsBoolean())
                {
                    bool condicion = (bool)valExpr;

                    while (condicion)
                    {
                        object obj = Bloque.Ejecutar(local, funcion, true, sw, tc, log, errores);

                        if (obj is Break)
                            break;
                        else if (obj is Return)
                            return obj;

                        if (Update is Instruccion instr)
                        {
                            instr.Ejecutar(local, funcion, ciclo, sw, tc, log, errores);
                        }
                        else if (Update is Expresion expr)
                        {
                            expr.GetValor(local, log, errores);
                        }

                        valExpr = Expr.GetValor(local, log, errores);

                        if (valExpr != null)
                        {
                            condicion = (bool)valExpr;
                            continue;
                        }
                        break;
                    }
                    return null;
                }
                errores.AddLast(new Error("Semántico", "Se esperaba un booleano en condicion for.", Linea, Columna));
            }

            return null;
        }
    }
}
