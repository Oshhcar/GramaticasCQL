using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using GramaticasCQL.Parsers.CQL.ast.instruccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CHISON.ast
{
    class Databases : Instruccion
    {
        public Databases(object valor, int linea, int columna) : base(linea, columna)
        {
            Valor = valor;
        }

        public object Valor { get; set; } //Debe ser una List (tipo bloque)

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, bool tc, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            if (Valor is Lista lista)
            {
                if (lista.Valores != null)
                {
                    foreach (Expresion expr in lista.Valores)
                    {
                        if (expr is BloqueChison bloque)
                        {
                            bloque.Obj = OBJ.DATABASE;
                            object obj = bloque.GetValor(e, log, errores);

                            if (obj != null)
                            {
                                if (obj is BD bd)
                                {
                                    if (e.MasterRollback.Get(bd.Id) == null)
                                    {
                                        e.MasterRollback.Data.AddLast(bd);

                                        Usuario admin = e.MasterRollback.GetUsuario("admin");

                                        if (admin != null)
                                        {
                                            if (!admin.GetPermiso(bd.Id))
                                            {
                                                admin.AddPermiso(bd.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
