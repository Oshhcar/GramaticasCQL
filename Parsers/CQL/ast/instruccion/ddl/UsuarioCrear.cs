using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class UsuarioCrear : Instruccion
    {
        public UsuarioCrear(string id, Cadena password, int linea, int columna) : base(linea, columna)
        {
            Id = id;
            Password = password;
        }

        public string Id { get; set; }
        public Cadena Password { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<string> log, LinkedList<Error> errores)
        {
            if (e.Master.GetUsuario(Id) == null)
            {
                e.Master.AddUsuario(Id, Password.Valor);
            }
            else
                errores.AddLast(new Error("Semántico", "Ya existe una Usuario con el id: " + Id + ".", Linea, Columna));

            return null;
        }
    }
}
