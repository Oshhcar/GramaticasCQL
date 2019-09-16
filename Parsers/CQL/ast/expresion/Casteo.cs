using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class Casteo : Expresion
    {
        public Casteo(Tipo tipo, Expresion expr, int linea, int columna) : base(linea, columna)
        {
            Tipo = tipo;
            Expr = expr;
            Mostrar = true;
        }

        public Expresion Expr { get; set; }
        public bool Mostrar { get; set; }

        public override object GetValor(Entorno e, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            object valExpr = Expr.GetValor(e, log, errores);

            if (valExpr != null)
            {
                if (Tipo.IsString())
                {
                    if (Expr.Tipo.IsString() || Expr.Tipo.IsInt() || Expr.Tipo.IsDouble() || Expr.Tipo.IsTime() || Expr.Tipo.IsDate())
                    {
                        if (valExpr is Null)
                        {
                            if(Mostrar)
                                errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                            return valExpr;
                        }
                        return valExpr.ToString();
                    }
                }
                else if (Tipo.IsInt())
                {
                    if (Expr.Tipo.IsInt())
                    {
                        return valExpr;
                    }
                    else if (Expr.Tipo.IsDouble())
                    {
                        try
                        {
                            return Convert.ToInt32(Math.Truncate(Convert.ToDouble(valExpr)));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception Casteo: " + ex.Message.ToString());
                        }
                    }
                    else if (Expr.Tipo.IsString())
                    {
                        if (!(valExpr is Null))
                        {
                            if (int.TryParse(valExpr.ToString(), out int i))
                            {
                                return i;
                            }
                        }
                        else
                        {
                            if (Mostrar)
                                errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                        }

                    }
                }
                else if (Tipo.IsDouble())
                {
                    if (Expr.Tipo.IsDouble())
                    {
                        return valExpr;
                    }
                    else if (Expr.Tipo.IsInt())
                    {
                        return Convert.ToDouble(valExpr);
                    }
                    else if (Expr.Tipo.IsString())
                    {
                        if (!(valExpr is Null))
                        {
                            if (double.TryParse(valExpr.ToString(), out double d))
                            {
                                return d;
                            }
                        }
                        else
                        {
                            if (Mostrar)
                                errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                        }

                    }
                }
                else if (Tipo.IsDate())
                {
                    if (Expr.Tipo.IsDate())
                    {
                        return valExpr;
                    }
                    else if (Expr.Tipo.IsString())
                    {
                        if (!(valExpr is Null))
                        {
                            Date date = new Date(valExpr.ToString());

                            if (date.Correcto)
                                return date;
                        }
                        else
                        {
                            if(Mostrar)
                                errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                            return valExpr;
                        }
                    }
                }
                else if (Tipo.IsTime())
                {
                    if (Expr.Tipo.IsTime())
                    {
                        return valExpr;
                    }
                    else if (Expr.Tipo.IsString())
                    {
                        if (!(valExpr is Null))
                        {
                            Time time = new Time(valExpr.ToString());

                            if (time.Correcto)
                                return time;
                        }
                        else
                        {
                            if(Mostrar)
                                errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                            return valExpr;

                        }
                    }
                }

                if(Mostrar)
                    errores.AddLast(new Error("Semántico", "No se pudo castear la expresión a " + Tipo.Type.ToString() +".", Linea, Columna));
            }

            Tipo = null;
            return null;
        }
    }
}
