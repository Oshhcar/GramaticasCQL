using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion.operacion
{
    class Aritmetica : Operacion
    {

        public Aritmetica(Expresion op1, Expresion op2, Operador op, int linea, int columna) : base(op1, op2, op, linea, columna) { }

        public override object GetValor(Entorno e, LinkedList<Error> errores)
        {
            object valOp1 = Op1.GetValor(e, errores);
            object valOp2 = Op2.GetValor(e, errores);

            if (valOp1 != null && valOp2 != null)
            {
                TipoDominante(errores);

                if (Tipo != null)
                {
                    switch (Tipo.Type)
                    {
                        case Type.STRING:
                            return valOp1.ToString() + valOp2.ToString();
                        case Type.DOUBLE:
                            switch (Op)
                            {
                                case Operador.SUMA:
                                    return Convert.ToDouble(valOp1.ToString()) + Convert.ToDouble(valOp2.ToString());
                                case Operador.RESTA:
                                    return Convert.ToDouble(valOp1.ToString()) - Convert.ToDouble(valOp2.ToString());
                                case Operador.MULTIPLICACION:
                                    return Convert.ToDouble(valOp1.ToString()) * Convert.ToDouble(valOp2.ToString());
                                case Operador.POTENCIA:
                                    return Math.Pow(Convert.ToDouble(valOp1.ToString()), Convert.ToDouble(valOp2.ToString()));
                                case Operador.MODULO:
                                    return Convert.ToDouble(valOp1.ToString()) % Convert.ToDouble(valOp2.ToString());
                                case Operador.DIVISION:
                                    if (Convert.ToDouble(valOp2.ToString()) != 0)
                                    {
                                        return Convert.ToDouble(valOp1.ToString()) / Convert.ToDouble(valOp2.ToString());
                                    }
                                    errores.AddLast(new Error("Semántico", "División entre 0.", Linea, Columna));
                                    return null;

                            }
                            break;
                        case Type.INT:
                            switch (Op)
                            {
                                case Operador.SUMA:
                                    return Convert.ToInt32(valOp1.ToString()) + Convert.ToInt32(valOp2.ToString());
                                case Operador.RESTA:
                                    return Convert.ToInt32(valOp1.ToString()) - Convert.ToInt32(valOp2.ToString());
                                case Operador.MULTIPLICACION:
                                    return Convert.ToInt32(valOp1.ToString()) * Convert.ToInt32(valOp2.ToString());
                                case Operador.POTENCIA:
                                    return Math.Pow(Convert.ToInt32(valOp1.ToString()), Convert.ToInt32(valOp2.ToString()));
                                case Operador.MODULO:
                                    return Convert.ToInt32(valOp1.ToString()) % Convert.ToInt32(valOp2.ToString());
                                case Operador.DIVISION:
                                    if (Convert.ToInt32(valOp2.ToString()) != 0)
                                    {
                                        return Convert.ToInt32(valOp1.ToString()) / Convert.ToInt32(valOp2.ToString());
                                    }
                                    errores.AddLast(new Error("Semántico", "División entre 0.", Linea, Columna));
                                    return null;

                            }
                            break;
                    }
                }

            }

            return null;
        }

        public void TipoDominante(LinkedList<Error> errores)
        {
            if (Op1.Tipo != null && Op2.Tipo != null)
            {
                if (Op1.Tipo.IsString() || Op2.Tipo.IsString())
                {
                    if (Op == Operador.SUMA)
                    {
                        Tipo = new Tipo(Type.STRING);
                        return;
                    }
                    else
                    {
                        errores.AddLast(new Error("Semántico", "Las cadenas solo admiten el operador: " + "+.", Linea, Columna));
                        return;
                    }
                }
                else if (!Op1.Tipo.IsBoolean() && !Op2.Tipo.IsBoolean())
                {
                    if (Op1.Tipo.IsDouble() || Op2.Tipo.IsDouble())
                    {
                        Tipo = new Tipo(Type.DOUBLE);
                        return;
                    }
                    else if (Op1.Tipo.IsInt() || Op2.Tipo.IsInt())
                    {
                        if (Op == Operador.POTENCIA)
                        {
                            Tipo = new Tipo(Type.DOUBLE);
                            return;
                        }
                        Tipo = new Tipo(Type.INT);
                        return;
                    }
                }
                errores.AddLast(new Error("Semántico", "Error de tipos en operación aritmética.", Linea, Columna));
            }
        }

    }
}
