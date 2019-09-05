using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using GramaticasCQL.Parsers.CQL.ast.instruccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast
{
    class ASTCQL
    {
        public ASTCQL(LinkedList<NodoASTCQL> sentencias)
        {
            Sentencias = sentencias;
        }

        public LinkedList<NodoASTCQL> Sentencias;

        public void Ejecutar(LinkedList<string> log, LinkedList<Error> errores)
        {
            Entorno global = new Entorno(null);

            foreach (NodoASTCQL stmt in Sentencias)
            {
                if (stmt is Instruccion instr)
                {
                    instr.Ejecutar(global, false, false, false, log, errores);
                }
                else  if(stmt is Expresion expr)
                {
                    expr.GetValor(global, errores);
                }

            }
        }

    }
}
