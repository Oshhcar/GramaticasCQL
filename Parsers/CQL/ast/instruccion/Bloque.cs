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

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            if (Bloques != null)
            {
                foreach (NodoASTCQL bloque in Bloques)
                {
                    /*Capturar los retornos*/
                    if (bloque is Instruccion inst)
                    {
                        inst.Ejecutar(e, funcion, ciclo, sw, log, errores);
                    }
                    else if(bloque is Expresion expr)
                    {
                        expr.GetValor(e, errores);
                    }
                }
            }
            return null;
        }
    }
}
