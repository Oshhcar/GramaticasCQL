using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion.operacion
{
    class Relacional : Operacion
    {
        public Relacional(Expresion op1, Expresion op2, Operador op, int linea, int columna) : base(op1, op2, op, linea, columna) { }

        public override object GetValor(Entorno e, LinkedList<Error> errores)
        {
            object valOp1 = Op1.GetValor(e, errores);
            object valOp2 = Op2.GetValor(e, errores);

            if (valOp1 != null && valOp2 != null)
            {
                Tipo = new Tipo(Type.BOOLEAN);

                if (Op1.Tipo.IsNumeric() && Op2.Tipo.IsNumeric())
                {
                    double op1 = Convert.ToDouble(valOp1.ToString());
                    double op2 = Convert.ToDouble(valOp2.ToString());

                    switch (Op)
                    {
                        case Operador.MAYORQUE:
                            return op1 > op2;
                        case Operador.MENORQUE:
                            return op1 < op2;
                        case Operador.IGUAL:
                            return op1 == op2;
                        case Operador.MENORIGUAL:
                            return op1 <= op2;
                        case Operador.MAYORIGUAL:
                            return op1 >= op2;
                        case Operador.DIFERENTE:
                            return op1 != op2;
                    }
                }
                else if (Op1.Tipo.IsDate() && Op2.Tipo.IsDate())
                {
                    Date op1 = (Date)valOp1;
                    Date op2 = (Date)valOp2;

                    switch (Op)
                    {
                        case Operador.MAYORQUE:
                            if (op1.Year != op2.Year)
                            {
                                return op1.Year > op2.Year;
                            }
                            else if (op1.Month != op2.Month)
                            {
                                return op1.Month > op2.Month;
                            }
                            else if (op1.Day != op2.Day)
                            {
                                return op1.Day > op2.Day;
                            }
                            return false;
                        case Operador.MENORQUE:
                            if (op1.Year != op2.Year)
                            {
                                return op1.Year < op2.Year;
                            }
                            else if (op1.Month != op2.Month)
                            {
                                return op1.Month < op2.Month;
                            }
                            else if (op1.Day != op2.Day)
                            {
                                return op1.Day < op2.Day;
                            }
                            return false;
                        case Operador.IGUAL:
                            if (op1.Year != op2.Year)
                            {
                                return false;
                            }
                            else if (op1.Month != op2.Month)
                            {
                                return false;
                            }
                            else if (op1.Day != op2.Day)
                            {
                                return false;
                            }
                            return true;
                        case Operador.MENORIGUAL:
                            if (op1.Year != op2.Year)
                            {
                                return op1.Year < op2.Year;
                            }
                            else if (op1.Month != op2.Month)
                            {
                                return op1.Month < op2.Month;
                            }
                            else if (op1.Day != op2.Day)
                            {
                                return op1.Day < op2.Day;
                            }
                            return true;
                        case Operador.MAYORIGUAL:
                            if (op1.Year != op2.Year)
                            {
                                return op1.Year > op2.Year;
                            }
                            else if (op1.Month != op2.Month)
                            {
                                return op1.Month > op2.Month;
                            }
                            else if (op1.Day != op2.Day)
                            {
                                return op1.Day > op2.Day;
                            }
                            return true;
                        case Operador.DIFERENTE:
                            if (op1.Year != op2.Year)
                            {
                                return true;
                            }
                            else if (op1.Month != op2.Month)
                            {
                                return true;
                            }
                            else if (op1.Day != op2.Day)
                            {
                                return true;
                            }
                            return false;
                    }

                }
                else if (Op1.Tipo.IsTime() && Op2.Tipo.IsTime())
                {
                    Time op1 = (Time)valOp1;
                    Time op2 = (Time)valOp2;

                    switch (Op)
                    {
                        case Operador.MAYORQUE:
                            if (op1.Hours != op2.Hours)
                            {
                                return op1.Hours > op2.Hours;
                            }
                            else if (op1.Minutes != op2.Minutes)
                            {
                                return op1.Minutes > op2.Minutes;
                            }
                            else if (op1.Seconds != op2.Seconds)
                            {
                                return op1.Seconds > op2.Seconds;
                            }
                            return false;
                        case Operador.MENORQUE:
                            if (op1.Hours != op2.Hours)
                            {
                                return op1.Hours < op2.Hours;
                            }
                            else if (op1.Minutes != op2.Minutes)
                            {
                                return op1.Minutes < op2.Minutes;
                            }
                            else if (op1.Seconds != op2.Seconds)
                            {
                                return op1.Seconds < op2.Seconds;
                            }
                            return false;
                        case Operador.IGUAL:
                            if (op1.Hours != op2.Hours)
                            {
                                return false;
                            }
                            else if (op1.Minutes != op2.Minutes)
                            {
                                return false;
                            }
                            else if (op1.Seconds != op2.Seconds)
                            {
                                return false;
                            }
                            return true;
                        case Operador.MENORIGUAL:
                            if (op1.Hours != op2.Hours)
                            {
                                return op1.Hours < op2.Hours;
                            }
                            else if (op1.Minutes != op2.Minutes)
                            {
                                return op1.Minutes < op2.Minutes;
                            }
                            else if (op1.Seconds != op2.Seconds)
                            {
                                return op1.Seconds < op2.Seconds;
                            }
                            return true;
                        case Operador.MAYORIGUAL:
                            if (op1.Hours != op2.Hours)
                            {
                                return op1.Hours > op2.Hours;
                            }
                            else if (op1.Minutes != op2.Minutes)
                            {
                                return op1.Minutes > op2.Minutes;
                            }
                            else if (op1.Seconds != op2.Seconds)
                            {
                                return op1.Seconds > op2.Seconds;
                            }
                            return true;
                        case Operador.DIFERENTE:
                            if (op1.Hours != op2.Hours)
                            {
                                return true;
                            }
                            else if (op1.Minutes != op2.Minutes)
                            {
                                return true;
                            }
                            else if (op1.Seconds != op2.Seconds)
                            {
                                return true;
                            }
                            return false;
                    }
                }
                else if ((Op1.Tipo.IsBoolean() && Op2.Tipo.IsBoolean()) || (Op1.Tipo.IsString() && Op2.Tipo.IsString()))
                {
                    if (Op == Operador.IGUAL)
                        return valOp1.ToString().Equals(valOp2.ToString());
                    else if (Op == Operador.DIFERENTE)
                        return !valOp1.ToString().Equals(valOp2.ToString());

                    errores.AddLast(new Error("Semántico", "Con " + Op1.Tipo.Type + " solo se puede operar el igual y diferente.", Linea, Columna));
                    return null;
                }

                errores.AddLast(new Error("Semántico", "No se pudo realizar la operación relacional.", Linea, Columna));
            }

            return null;
        }
    }
}
