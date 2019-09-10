using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class Acceso : Expresion
    {
        public Acceso(Expresion target, Expresion expr, int linea, int columna) : base(linea, columna)
        {
            Target = target;
            Expr = expr;
            GetCollection = false;
        }

        public Expresion Target { get; set; }
        public Expresion Expr { get; set; }
        public bool GetCollection { get; set; }

        public override object GetValor(Entorno e, LinkedList<string> log, LinkedList<Error> errores)
        {
            object valExpr = Expr.GetValor(e, log, errores);

            if (valExpr != null)
            {
                if (Expr.Tipo.IsInt())
                {
                    object valTarget = Target.GetValor(e, log, errores);

                    if (valTarget != null)
                    {
                        if (Target.Tipo.IsMap() || Target.Tipo.IsList() || Target.Tipo.IsSet())
                        {
                            if (!(valTarget is Null))
                            {
                                Collection collection = (Collection)valTarget;
                                object valor = GetCollection? collection.GetCollection(valExpr.ToString()) : collection.Get(valExpr.ToString());
                                if (valor != null)
                                {
                                     Tipo = collection.Tipo.Valor;
                                    return valor;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valExpr.ToString() + " del " + Target.Tipo.Type.ToString() + ".", Linea, Columna));
                            }
                            else
                                errores.AddLast(new Error("Semántico", "El " + Target.Tipo.Type.ToString() + " no ha sido inicializado.", Linea, Columna));

                        }
                        else
                            errores.AddLast(new Error("Semántico", "La variable debe ser de tipo Map, List o Set.", Linea, Columna));
                    }
                }
                else
                    errores.AddLast(new Error("Semántico", "La posición debe ser Int.", Linea, Columna));
            }

            return null;
        }

        public override string GetId()
        {
            return Target.GetId();
        }
    }
}
