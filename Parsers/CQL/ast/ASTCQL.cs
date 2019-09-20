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

            object obj;

            foreach (NodoASTCQL stmt in Sentencias)
            {
                if (stmt is FuncionDef fun)
                {
                    obj = fun.Ejecutar(global, false, false, false, false, log, errores);

                    if (obj is Throw th)
                        errores.AddLast(new Error("Semántico", "Excepción no Controlada: " + th.Id + ".", th.Linea, th.Columna));
                }
            }

            foreach (NodoASTCQL stmt in Sentencias)
            {
                if (stmt is Instruccion instr)
                {
                    if (!(stmt is FuncionDef))
                    {
                        obj = instr.Ejecutar(global, false, false, false, false, log, errores);
                        
                        if(obj is Throw th)
                            errores.AddLast(new Error("Semántico", "Excepción no Controlada: " + th.Id + ".", th.Linea, th.Columna));
                    }
                }
                else  if(stmt is Expresion expr)
                {
                    if (expr is FuncionCall fun)
                        fun.IsExpresion = false;

                    obj = expr.GetValor(global, log, errores);

                    if(obj is Throw th)
                        errores.AddLast(new Error("Semántico", "Excepción no Controlada: " + th.Id + ".", th.Linea, th.Columna));
                }

            }
        }

    }
}
