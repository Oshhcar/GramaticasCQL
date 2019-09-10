using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class SetDisplay : Expresion
    {
        public SetDisplay(LinkedList<Expresion> collection, int linea, int columna) : base(linea, columna)
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
                Collection set = new Collection(new Tipo(Type.SET, valor.Tipo));
                set.Insert(set.Posicion++, valValor);

                for (int i = 1; i < Collection.Count(); i++)
                {
                    valor = Collection.ElementAt(i);
                    valValor = valor.GetValor(e, log, errores);

                    if (valValor != null)
                    {
                        if (set.Tipo.Valor.Equals(valor.Tipo))
                            set.Insert(set.Posicion++, valValor);
                        else
                            errores.AddLast(new Error("Semántico", "El tipo no coinciden con el valor del List.", Linea, Columna));
                        continue;
                    }
                    //return null;
                }

                set.Ordenar();

                Tipo = new Tipo(Type.SET);
                return set;
            }
            return null;
        }
    }
}
