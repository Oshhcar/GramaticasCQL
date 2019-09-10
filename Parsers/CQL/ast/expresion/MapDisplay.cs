using GramaticasCQL.Parsers.CQL.ast.entorno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class MapDisplay : Expresion
    {
        public MapDisplay(LinkedList<CollectionValue> collection, int linea, int columna) : base(linea, columna)
        {
            Collection = collection;
        }

        public LinkedList<CollectionValue> Collection { get; set; }

        public override object GetValor(Entorno e, LinkedList<string> log, LinkedList<Error> errores)
        {
            Expresion clave = (Expresion)Collection.ElementAt(0).Clave;
            Expresion valor = (Expresion)Collection.ElementAt(0).Valor;

            object valClave = clave.GetValor(e, log, errores);
            object valValor = valor.GetValor(e, log, errores);

            if (valClave != null && valValor != null)
            {
                Collection map = new Collection(new Tipo(clave.Tipo, valor.Tipo));
                map.Insert(valClave, valValor);

                for (int i = 1; i < Collection.Count(); i++)
                {
                    CollectionValue value = Collection.ElementAt(i);
                    clave = (Expresion)value.Clave;
                    valor = (Expresion)value.Valor;
                    valClave = clave.GetValor(e, log, errores);
                    valValor = valor.GetValor(e, log, errores);

                    if (valClave != null && valValor != null)
                    {
                        if (map.Tipo.Clave.Equals(clave.Tipo) && map.Tipo.Valor.Equals(valor.Tipo))
                        {
                            if (map.Get(valClave) == null)
                                map.Insert(valClave, valValor);
                            else
                                errores.AddLast(new Error("Semántico", "Ya existe un valor con la clave: " + valClave.ToString() + " en Map.", Linea, Columna));
                        }
                        else
                            errores.AddLast(new Error("Semántico", "Los tipos no coinciden con la clave:valor del Map.", Linea, Columna));
                        continue;
                    }
                    //return null;
                }

                Tipo = new Tipo(Type.MAP);
                return map;
            }
            return null;
        }
    }
}
