using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    class Collection
    {
        public Collection(Tipo clave, Tipo valor)
        {
            TipoClave = clave;
            TipoValor = valor;
            Valores = new LinkedList<CollectionValue>();
            Posicion = 0;
        }

        public Tipo TipoClave { get; set; }
        public Tipo TipoValor { get; set; }
        public LinkedList<CollectionValue> Valores { get; set; }
        public int Posicion { get; set; }

        public void Insert(object clave, object valor)
        {
            Valores.AddLast(new CollectionValue(clave.ToString(), valor ?? Predefinido()));

        }

        public object Get(object clave)
        {
            foreach (CollectionValue val in Valores)
            {
                if (val.Clave.Equals(clave.ToString()))
                    return val.Valor;
            }
            return null;
        }

        public CollectionValue GetCollection(object clave)
        {
            foreach (CollectionValue val in Valores)
            {
                if (val.Clave.Equals(clave.ToString()))
                    return val;
            }
            return null;
        }

        public bool Set(object clave, object valor)
        {
            foreach (CollectionValue val in Valores)
            {
                if (val.Clave.Equals(clave.ToString()))
                {
                    val.Valor = valor ?? Predefinido();
                    return true;
                }
            }
            return false;
        }

        public bool Remove(object clave)
        {
            foreach (CollectionValue val in Valores)
            {
                if (val.Clave.Equals(clave.ToString()))
                {
                    Valores.Remove(val);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveList(object clave)
        {
            foreach (CollectionValue val in Valores)
            {
                if (val.Clave.Equals(clave.ToString()))
                {
                    val.Valor = Predefinido();
                    return true;
                }
            }
            return false;
        }

        //Para list
        public bool Contains(object valor)
        {
            foreach (CollectionValue val in Valores)
            {
                /*Provar si se puede hacer una relacional*/
                if (val.Valor.Equals(valor))
                {
                    return true;
                }
            }
            return false;
        }

        //Para set
        public void Ordenar()
        {
            LinkedList<CollectionValue> tmp = new LinkedList<CollectionValue>();
            IEnumerable<CollectionValue> ordered;

            if (TipoValor.IsString() || TipoValor.IsDate() || TipoValor.IsTime())
                ordered = Valores.OrderBy(p => p.Valor.ToString()).AsEnumerable();
            else if (TipoValor.IsInt())
                ordered = Valores.OrderBy(p => (int)p.Valor).AsEnumerable();
            else if (TipoValor.IsDouble())
                ordered = Valores.OrderBy(p => (double)p.Valor).AsEnumerable();
            else
                ordered = null;

            if (ordered != null)
            {
                int contador = 0;

                foreach (CollectionValue value in ordered)
                {
                    value.Clave = (contador++).ToString();
                    tmp.AddLast(value);
                }
                Valores = tmp;
            }
        }

        public object Predefinido()
        {
            if (TipoValor.IsInt())
                return 0;
            else if (TipoValor.IsDouble())
                return 0.0;
            else if (TipoValor.IsBoolean())
                return false;
            else
                return new Null();
        }

        public void Recorrer()
        {
            Console.WriteLine("***Map***");
            foreach (CollectionValue val in Valores)
            {
                Console.WriteLine("\t" + val.Clave.ToString() + ":" + val.Valor.ToString());
            }
            Console.WriteLine("*********");
        }
    }
}
