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
    class Users : Instruccion
    {
        public Users(Expresion valor, int linea, int columna) : base(linea, columna)
        {
            Valor = valor;
        }

        public Expresion Valor { get; set; } //Un list con objeto 

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
                            bloque.Obj = OBJ.USER;
                            object obj = bloque.GetValor(e, log, errores);

                            if (obj != null)
                            {
                                if (obj is Usuario usuario)
                                {
                                    Usuario old = e.MasterRollback.GetUsuario(usuario.Id);
                                    if (old == null)
                                        e.MasterRollback.Usuarios.AddLast(usuario);
                                    else
                                    {
                                        foreach (string permiso in usuario.Permisos)
                                        {
                                            if (!old.GetPermiso(permiso))
                                                old.AddPermiso(permiso);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
                return null; //debe ser una lista
            return null;
        }
    }
}
