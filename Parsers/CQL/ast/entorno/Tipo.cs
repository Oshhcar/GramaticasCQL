using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.entorno
{
    public class Tipo
    {

        public Tipo(Type type)
        {
            Type = type;
        }

        public Tipo(string objeto)
        {
            Type = Type.OBJECT;
            Objeto = objeto;
        }

        public Type Type { get; set; }
        public string Objeto { get; set; }

        public bool IsInt() { return Type == Type.INT; }
        public bool IsDouble() { return Type == Type.DOUBLE; }
        public bool IsString() { return Type == Type.STRING; }
        public bool IsBoolean() { return Type == Type.BOOLEAN; }
        public bool IsDate() { return Type == Type.DATE; }
        public bool IsTime() { return Type == Type.TIME; }
        public bool IsObject() { return Type == Type.OBJECT; }
        public bool IsCounter() { return Type == Type.COUNTER; }
        public bool IsMap () { return Type == Type.MAP; }
        public bool IsList() { return Type == Type.LIST; }
        public bool IsSet() { return Type == Type.SET; }
        public bool IsNull() { return Type == Type.NULL; }
        public bool IsNumeric() { return Type == Type.INT || Type == Type.DOUBLE; }

        public override bool Equals(object obj)
        {
            if (obj is Tipo t)
            {
                if (IsInt() || IsDouble() || IsBoolean())
                {
                    return Type == t.Type;
                }
                else
                {
                    if (IsObject() && t.IsObject())
                    {
                        return Objeto.Equals(t.Objeto);
                    }

                    return t.IsNull() ? true : Type == t.Type;
                }
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool isPrimitivo()
        {

            return false;
        }
    }

    public enum Type
    {
        INT,
        DOUBLE,
        STRING,
        BOOLEAN,
        DATE,
        TIME,
        OBJECT,
        COUNTER,
        MAP,
        LIST,
        SET,
        NULL
    }

    public enum Rol
    {
        VARIABLE,
        FUNCION,
        COLLECTION
    }
}
