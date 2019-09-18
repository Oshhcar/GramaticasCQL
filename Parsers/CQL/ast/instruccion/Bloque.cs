using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion
{
    class Bloque : Instruccion
    {
        public Bloque(LinkedList<NodoASTCQL> bloques, int linea, int columna) : base(linea, columna)
        {
            Bloques = bloques;
        }

        public LinkedList<NodoASTCQL> Bloques { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            if (Bloques != null)
            {
                foreach (NodoASTCQL bloque in Bloques)
                {
                    if (bloque is Instruccion inst)
                    {
                        object obj = inst.Ejecutar(e, funcion, ciclo, sw, log, errores);

                        if (obj is Break)
                        {
                            if (ciclo || sw)
                                return obj;
                            else
                                errores.AddLast(new Error("Semántico", "Sentencia break no se encuentra dentro de un switch o ciclo.", Linea, Columna));

                        }
                        else if (obj is Continue)
                        {
                            if (ciclo)
                                return obj;
                            else
                                errores.AddLast(new Error("Semántico", "Sentencia continue no se encuentra dentro de un ciclo.", Linea, Columna));

                        }
                        else if (obj is Return)
                        {
                            if (funcion)
                                return obj;
                            else
                                errores.AddLast(new Error("Semántico", "Sentencia return no se encuentra dentro de una función o procedimiento.", Linea, Columna));

                        }
                    }
                    else if (bloque is Expresion expr)
                    {
                        if (expr is FuncionCall fun)
                            fun.IsExpresion = false;

                        expr.GetValor(e, log, errores);
                    }
                }
            }
            return null;
        }
    }
}
