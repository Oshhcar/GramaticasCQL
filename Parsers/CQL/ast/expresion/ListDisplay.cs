using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class ListDisplay : Expresion
    {
        public ListDisplay(LinkedList<Expresion> collection, int linea, int columna) : base(linea, columna)
        {
            Collection = collection;
        }

        public LinkedList<Expresion> Collection { get; set; }

        public override object GetValor(Entorno e, LinkedList<string> log, LinkedList<Error> errores)
        {
            Expresion valor = Collection.ElementAt(0);
            object valValor = valor.GetValor(e, log, errores);

            if (valValor != null)
            {
                Collection list = new Collection(new Tipo(Type.INT), valor.Tipo);
                list.Insert(list.Posicion++, valValor);

                for (int i = 1; i < Collection.Count(); i++)
                {
                    valor = Collection.ElementAt(i);
                    valValor = valor.GetValor(e, log, errores);

                    if (valValor != null)
                    {
                        if (list.TipoValor.Equals(valor.Tipo))
                            list.Insert(list.Posicion++, valValor);
                        else
                            errores.AddLast(new Error("Semántico", "El tipo no coinciden con el valor del List.", Linea, Columna));
                        continue;
                    }
                    //return null;
                }

                Tipo = new Tipo(Type.LIST);
                return list;
            }
            return null;
        }
    }
}
