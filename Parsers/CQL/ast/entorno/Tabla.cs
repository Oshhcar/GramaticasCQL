using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Tabla
    {
        public Tabla()
        {
            Cabecera = new Entorno(null, new LinkedList<Simbolo>());
            Datos = new LinkedList<Entorno>();
            Contador = 0;
        }

        public Entorno Cabecera { get; set; }
        public LinkedList<Entorno> Datos {get; set;}
        public int Contador { get; set; }

        public Simbolo GetColumna(string id)
        {
            foreach (Simbolo sim in Cabecera.Simbolos)
            {
                if (sim.Id.Equals(id.ToLower()))//&& sim.Rol == Rol.COLUMNA || sim.Rol == Rol.PRIMARY)
                    return sim;
            }
            return null;
        }

        public void Add(Simbolo columna)
        {
            Cabecera.Add(columna);
            /*Agregar en todos los datos*/
        }

        public int Drop(string columna)
        {
            foreach (Simbolo sim in Cabecera.Simbolos)
            {
                if (sim.Id.Equals(columna))
                {
                    if (sim.Rol != Rol.PRIMARY)
                    {
                        Cabecera.Simbolos.Remove(sim);
                        /*Remover en los datos*/

                        return 1;
                    }
                    return 2; //Es llave primaria
                }
            }
            return 3; // No se encontro
        }

        public void Recorrer()
        {
            foreach (Simbolo col in Cabecera.Simbolos)
            {
                Console.WriteLine(col.Id + " " + col.Tipo.Type.ToString() + " " + col.Rol.ToString() + " " + col.Valor.ToString());
            }
        }
    }
}
