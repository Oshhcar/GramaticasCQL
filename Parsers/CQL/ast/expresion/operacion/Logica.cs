using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion.operacion
{
    class Logica : Operacion
    {
        public Logica(Expresion op1, Expresion op2, Operador op, int linea, int columna) : base(op1, op2, op, linea, columna) { }

        public Logica(Expresion op1, int linea, int columna) : base(op1, null, Operador.NOT, linea, columna) { }

        public override object GetValor(Entorno e, LinkedList<Error> errores)
        {
            object valOp1 = Op1.GetValor(e, errores);

            if (valOp1 != null)
            {
                if (Op != Operador.NOT)
                {
                    object valOp2 = Op2.GetValor(e, errores);

                    if (valOp2 != null)
                    {
                        Tipo = new Tipo(Type.BOOLEAN);

                        if (Op1.Tipo.IsBoolean() && Op2.Tipo.IsBoolean())
                        {
                            switch (Op)
                            {
                                case Operador.OR:
                                    return (Boolean)valOp1 || (Boolean)valOp2;
                                case Operador.AND:
                                    return (Boolean)valOp1 && (Boolean)valOp2;
                                case Operador.XOR:
                                    return (Boolean)valOp1 ^ (Boolean)valOp2;
                            }
                        }
                        errores.AddLast(new Error("Semántico", "No se pudo realizar la operación lógica.", Linea, Columna));
                    }
                }
                else
                {
                    Tipo = new Tipo(Type.BOOLEAN);

                    if (Op1.Tipo.IsBoolean())
                    {
                        return !(Boolean)valOp1;
                    }
                    errores.AddLast(new Error("Semántico", "No se pudo realizar la operación lógica.", Linea, Columna));
                }
            }
            return null;
        }
    }
}
