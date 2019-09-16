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

        public void Ejecutar(LinkedList<Salida> log, LinkedList<Error> errores)
        {
            MasterBD master = new MasterBD(); //aqui iran todas las tablas
            master.AddUsuario("admin", "admin");
            
            Entorno global = new Entorno(null);
            global.Global = global;
            global.Master = master;

            foreach (NodoASTCQL stmt in Sentencias)
            {
                if (stmt is FuncionDef fun)
                    fun.Ejecutar(global, false, false, false, log, errores);
            }

            foreach (NodoASTCQL stmt in Sentencias)
            {
                if (stmt is Instruccion instr)
                {
                    if(!(stmt is FuncionDef))
                        instr.Ejecutar(global, false, false, false, log, errores);
                }
                else  if(stmt is Expresion expr)
                {
                    expr.GetValor(global, log, errores);
                }

            }
        }

    }
}
