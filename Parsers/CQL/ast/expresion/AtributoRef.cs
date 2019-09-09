using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL.ast.expresion
{
    class AtributoRef : Expresion
    {
        public AtributoRef(Expresion target, Expresion atributo, int linea, int columna) : base(linea, columna)
        {
            Target = target;
            Atributo = atributo;
        }

        public Expresion Target;
        public Expresion Atributo;

        public override object GetValor(Entorno e, LinkedList<string> log, LinkedList<Error> errores)
        {
            object valorTarget = Target.GetValor(e, log, errores);

            if (valorTarget != null)
            {
                if (Atributo is FuncionCall funcion)
                {
                    switch (funcion.Id.ToLower())
                    {
                        case "length":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa length no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsString())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return valorTarget.ToString().Length;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));

                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa Length solo se puede aplicar a valores de tipo String.", Linea, Columna));
                            }
                            break;
                        case "touppercase":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa toUpperCase no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsString())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.STRING);
                                    return valorTarget.ToString().ToUpper();
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa toUpperCase solo se puede aplicar a valores de tipo String.", Linea, Columna));
                            }
                            break;
                        case "tolowercase":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa toLowerCase no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsString())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.STRING);
                                    return valorTarget.ToString().ToLower();
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa toLowerCase solo se puede aplicar a valores de tipo String.", Linea, Columna));
                            }
                            break;
                        case "startswith":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 1)
                                {
                                    if (funcion.Parametro.Count() > 1)
                                        errores.AddLast(new Error("Semántico", "La función nativa startsWith recibe un solo parámetro de tipo String.", Linea, Columna));

                                    Expresion parametro = funcion.Parametro.ElementAt(0);
                                    object valParametro = parametro.GetValor(e, log, errores);

                                    if (valParametro != null)
                                    {
                                        if (parametro.Tipo.IsString())
                                        {
                                            if (Target.Tipo.IsString())
                                            {
                                                if (valorTarget is Null)
                                                {
                                                    Tipo = new Tipo(Type.BOOLEAN);
                                                    return valorTarget.ToString().StartsWith(valParametro.ToString());
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                                            }
                                            else
                                            {
                                                errores.AddLast(new Error("Semántico", "La función nativa startsWith solo se puede aplicar a valores de tipo String.", Linea, Columna));
                                            }
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "El parámetro en la función nativa startsWith no es de tipo String.", Linea, Columna));

                                    }
                                }
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función nativa startsWith necesita un parámetro de tipo String.", Linea, Columna));
                            break;
                        case "endswith":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 1)
                                {
                                    if (funcion.Parametro.Count() > 1)
                                        errores.AddLast(new Error("Semántico", "La función nativa endsWith recibe un solo parámetro de tipo String.", Linea, Columna));

                                    Expresion parametro = funcion.Parametro.ElementAt(0);
                                    object valParametro = parametro.GetValor(e, log, errores);

                                    if (valParametro != null)
                                    {
                                        if (parametro.Tipo.IsString())
                                        {
                                            if (Target.Tipo.IsString())
                                            {
                                                if (valorTarget is Null)
                                                {
                                                    Tipo = new Tipo(Type.BOOLEAN);
                                                    return valorTarget.ToString().EndsWith(valParametro.ToString());
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                                            }
                                            else
                                            {
                                                errores.AddLast(new Error("Semántico", "La función nativa endsWith solo se puede aplicar a valores de tipo String.", Linea, Columna));
                                            }
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "El parámetro en la función nativa endsWith no es de tipo String.", Linea, Columna));

                                    }
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "La función nativa endsWith necesita un parámetro de tipo String.", Linea, Columna));

                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función nativa endsWith necesita un parámetro de tipo String.", Linea, Columna));
                            break;
                        case "substring":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 2)
                                {
                                    if (funcion.Parametro.Count() > 2)
                                        errores.AddLast(new Error("Semántico", "La función nativa subString recibe dos parámetros de tipo Int.", Linea, Columna));

                                    Expresion parametro1 = funcion.Parametro.ElementAt(0);
                                    object valParametro1 = parametro1.GetValor(e, log, errores);

                                    Expresion parametro2 = funcion.Parametro.ElementAt(1);
                                    object valParametro2 = parametro2.GetValor(e, log, errores);

                                    if (valParametro1 != null && valParametro2 != null)
                                    {
                                        if (parametro1.Tipo.IsInt() && parametro2.Tipo.IsInt())
                                        {
                                            if (Target.Tipo.IsString())
                                            {
                                                if (!(valorTarget is Null))
                                                {
                                                    try
                                                    {
                                                        string ret = valorTarget.ToString().Substring((int)valParametro1, (int)valParametro2);
                                                        Tipo = new Tipo(Type.STRING);
                                                        return ret;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("Exception subString: " + ex.Message.ToString());
                                                        errores.AddLast(new Error("Semántico", "Índices fuera de rango en funcion subString.", Linea, Columna));

                                                    }
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El String no ha sido inicializado.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "La función nativa subString solo se puede aplicar a valores de tipo String.", Linea, Columna));
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "Los parametros en la función nativa subString no son de tipo Int.", Linea, Columna));
                                    }
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "La función nativa subString necesita dos parámetro de tipo Int.", Linea, Columna));

                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función nativa subString necesita dos parámetro de tipo Int.", Linea, Columna));
                            break;
                        case "getyear":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa getYear no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsDate())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Date)valorTarget).Year;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El Date no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa getYear solo se puede aplicar a valores de tipo Date.", Linea, Columna));
                            }
                            break;
                        case "getmonth":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa getMonth no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsDate())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Date)valorTarget).Month;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El Date no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa getMonth solo se puede aplicar a valores de tipo Date.", Linea, Columna));
                            }
                            break;
                        case "getday":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa getDay no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsDate())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Date)valorTarget).Day;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El Date no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa getDay solo se puede aplicar a valores de tipo Date.", Linea, Columna));
                            }
                            break;
                        case "gethour":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa getHour no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsTime())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Time)valorTarget).Hours;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El Time no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa getHour solo se puede aplicar a valores de tipo Time.", Linea, Columna));
                            }
                            break;
                        case "getminuts":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa getMinuts no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsTime())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Time)valorTarget).Minutes;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El Time no ha sido inicializado.", Linea, Columna));
                            }
                            else
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa getMinuts solo se puede aplicar a valores de tipo Time.", Linea, Columna));
                            }
                            break;
                        case "getseconds":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función nativa getSeconds no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsTime())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Time)valorTarget).Seconds;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El Time no ha sido inicializado.", Linea, Columna));
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función nativa getSeconds solo se puede aplicar a valores de tipo Time.", Linea, Columna));
                            break;
                        case "insert":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() == 1)
                                {
                                    Expresion parametro1 = funcion.Parametro.ElementAt(0);
                                    object valParametro1 = parametro1.GetValor(e, log, errores);

                                    if (valParametro1 != null)
                                    {
                                        if (Target.Tipo.IsMap())
                                        {
                                            errores.AddLast(new Error("Semántico", "La funcion Insert necesita dos parámetros para Map.", Linea, Columna));
                                            return null;
                                        }
                                        else if (Target.Tipo.IsList())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection list = (Collection)valorTarget;
                                                if (list.TipoValor.Equals(parametro1.Tipo))
                                                {
                                                    list.Insert(list.Posicion++, valParametro1);
                                                    return null;
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo del parametro no coinciden con el valor del List.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El List no ha sido inicializado.", Linea, Columna));
                                        }
                                        else if (Target.Tipo.IsSet())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection set = (Collection)valorTarget;
                                                if (set.TipoValor.Equals(parametro1.Tipo))
                                                {
                                                    set.Insert(set.Posicion++, valParametro1);
                                                    set.Ordenar();
                                                    return null;
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo del parametro no coinciden con la valor del Set.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Set no ha sido inicializado.", Linea, Columna));
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "La función Insert solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                                    }
                                }
                                else
                                {
                                    if (funcion.Parametro.Count() >= 2)
                                    {
                                        if(funcion.Parametro.Count() > 2)
                                            errores.AddLast(new Error("Semántico", "La funcion Insert no necesita más de dos parámetros.", Linea, Columna));

                                        Expresion parametro1 = funcion.Parametro.ElementAt(0);
                                        Expresion parametro2 = funcion.Parametro.ElementAt(1);

                                        object valParametro1 = parametro1.GetValor(e, log, errores);
                                        object valParametro2 = parametro2.GetValor(e, log, errores);

                                        if (valParametro1 != null && valParametro2 != null)
                                        {
                                            if (Target.Tipo.IsMap())
                                            {
                                                if (!(valorTarget is Null))
                                                {
                                                    Collection map = (Collection)valorTarget;

                                                    if (map.TipoClave.Equals(parametro1.Tipo) && map.TipoValor.Equals(parametro2.Tipo))
                                                    {
                                                        if (map.Get(valParametro1) == null)
                                                        {
                                                            map.Insert(valParametro1, valParametro2);
                                                            return null;
                                                        }
                                                        else
                                                            errores.AddLast(new Error("Semántico", "Ya existe un valor con la clave: " + valParametro1.ToString() + " en Map.", Linea, Columna));
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "Los tipos de los parametros no coinciden con la clave:valor del Map.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El Map no ha sido inicializado.", Linea, Columna));
                                            }
                                            else if (Target.Tipo.IsList())
                                            {
                                                errores.AddLast(new Error("Semántico", "La funcion Insert no necesita más de un parámetro para List.", Linea, Columna));

                                                if (!(valorTarget is Null))
                                                {
                                                    Collection list = (Collection)valorTarget;
                                                    if (list.TipoValor.Equals(parametro1.Tipo))
                                                    {
                                                        list.Insert(list.Posicion++, valParametro1);
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "El tipo de parametro no coinciden con la valor del List.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El List no ha sido inicializado.", Linea, Columna));
                                            }
                                            else if (Target.Tipo.IsSet())
                                            {
                                                errores.AddLast(new Error("Semántico", "La funcion Insert no necesita más de un parámetro para Set.", Linea, Columna));

                                                if (!(valorTarget is Null))
                                                {
                                                    Collection set = (Collection)valorTarget;
                                                    if (set.TipoValor.Equals(parametro1.Tipo))
                                                    {
                                                        set.Insert(set.Posicion++, valParametro1);
                                                        set.Ordenar();
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "El tipo del parametro no coinciden con la valor del Set.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El Set no ha sido inicializado.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "La función Insert solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                                        }
                                    }
                                }
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función Insert necesita parámetros.", Linea, Columna));
                            break;
                        case "get":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 1)
                                {
                                    if (funcion.Parametro.Count() > 1)
                                        errores.AddLast(new Error("Semántico", "La función Get no necesita más de un parámetros.", Linea, Columna));

                                    Expresion parametro1 = funcion.Parametro.ElementAt(0);

                                    object valParametro1 = parametro1.GetValor(e, log, errores);

                                    if (valParametro1 != null)
                                    {
                                        if (Target.Tipo.IsMap())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection map = (Collection)valorTarget;

                                                if (map.TipoClave.Equals(parametro1.Tipo))
                                                {
                                                    object valValor = map.Get(valParametro1);
                                                    if (valValor != null)
                                                    {
                                                        Tipo = map.TipoValor;
                                                        return valValor;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor con la clave: " + valParametro1.ToString() + " en Map.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la clave no coincide con el declarado en el Map: " + map.TipoClave.Type.ToString() + ".", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Map no ha sido inicializado.", Linea, Columna));

                                        }
                                        else if (Target.Tipo.IsList())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection list = (Collection)valorTarget;

                                                if (parametro1.Tipo.IsInt())
                                                {
                                                    object valValor = list.Get(valParametro1);
                                                    if (valValor != null)
                                                    {
                                                        Tipo = list.TipoValor;
                                                        return valValor;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valParametro1.ToString() + " en List.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la posición del List debe ser Int.", Linea, Columna));

                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El List no ha sido inicializado.", Linea, Columna));
                                        }
                                        else if (Target.Tipo.IsSet())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection set = (Collection)valorTarget;

                                                if (parametro1.Tipo.IsInt())
                                                {
                                                    //set.Recorrer();

                                                    object valValor = set.Get(valParametro1);
                                                    if (valValor != null)
                                                    {
                                                        Tipo = set.TipoValor;
                                                        return valValor;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valParametro1.ToString() + " en Set.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la posición del Set debe ser Int.", Linea, Columna));

                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Set no ha sido inicializado.", Linea, Columna));
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "La función Get solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                                    }
                                }
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función Get necesita un parámetro.", Linea, Columna));
                            break;
                        case "set":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 2)
                                {
                                    if (funcion.Parametro.Count() > 2)
                                        errores.AddLast(new Error("Semántico", "La función Set no necesita más de dos parámetros.", Linea, Columna));

                                    Expresion parametro1 = funcion.Parametro.ElementAt(0);
                                    Expresion parametro2 = funcion.Parametro.ElementAt(1);

                                    object valParametro1 = parametro1.GetValor(e, log, errores);
                                    object valParametro2 = parametro2.GetValor(e, log, errores);

                                    if (valParametro1 != null && valParametro2 != null)
                                    {
                                        if (Target.Tipo.IsMap())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection map = (Collection)valorTarget;

                                                if (map.TipoClave.Equals(parametro1.Tipo) && map.TipoValor.Equals(parametro2.Tipo))
                                                {
                                                    if (map.Set(valParametro1, valParametro2))
                                                    {
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor con la clave: " + valParametro1.ToString() + " en Map.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "Los tipos de los parametros no coinciden con la clave:valor del Map.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Map no ha sido inicializado.", Linea, Columna));
                                        }
                                        else if (Target.Tipo.IsList())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection list = (Collection)valorTarget;

                                                if (parametro1.Tipo.IsInt() && list.TipoValor.Equals(parametro2.Tipo))
                                                {
                                                    if (list.Set(valParametro1, valParametro2))
                                                    {
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valParametro1.ToString() + " del List.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "Los tipos de los parametros no coinciden con la clave:valor del List.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El List no ha sido inicializado.", Linea, Columna));
                                        }
                                        else if (Target.Tipo.IsSet())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection set = (Collection)valorTarget;

                                                if (parametro1.Tipo.IsInt() && set.TipoValor.Equals(parametro2.Tipo))
                                                {
                                                    if (set.Set(valParametro1, valParametro2))
                                                    {
                                                        set.Ordenar();
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valParametro1.ToString() + " del Set.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "Los tipos de los parametros no coinciden con la clave:valor del Set.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Set no ha sido inicializado.", Linea, Columna));
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "La funcion insert solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                                    }
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "La funcion Set necesita dos parámetros.", Linea, Columna));
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La funcion Set necesita parámetros.", Linea, Columna));
                            break;
                        case "remove":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 1)
                                {
                                    if (funcion.Parametro.Count() > 1)
                                        errores.AddLast(new Error("Semántico", "La función Remove no necesita más de un parámetros.", Linea, Columna));

                                    Expresion parametro1 = funcion.Parametro.ElementAt(0);

                                    object valParametro1 = parametro1.GetValor(e, log, errores);

                                    if (valParametro1 != null)
                                    {
                                        if (Target.Tipo.IsMap())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection map = (Collection)valorTarget;

                                                if (map.TipoClave.Equals(parametro1.Tipo))
                                                {
                                                    if (map.Remove(valParametro1))
                                                    {
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor con la clave: " + valParametro1.ToString() + " en Map.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la clave no coincide con el declarado en el Map: " + map.TipoClave.Type.ToString() + ".", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Map no ha sido inicializado.", Linea, Columna));

                                        }
                                        else if (Target.Tipo.IsList())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection list = (Collection)valorTarget;

                                                if (parametro1.Tipo.IsInt())
                                                {
                                                    if (list.RemoveList(valParametro1))
                                                    {
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valParametro1.ToString() + " del List.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la posición debe ser Int.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El List no ha sido inicializado.", Linea, Columna));
                                        }
                                        else if (Target.Tipo.IsSet())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection list = (Collection)valorTarget;

                                                if (parametro1.Tipo.IsInt())
                                                {
                                                    if (list.Remove(valParametro1))
                                                    {
                                                        list.Ordenar();
                                                        return null;
                                                    }
                                                    else
                                                        errores.AddLast(new Error("Semántico", "No existe un valor en la posición: " + valParametro1.ToString() + " del Set.", Linea, Columna));
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la posición debe ser Int.", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Set no ha sido inicializado.", Linea, Columna));
                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "La función Remove solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                                    }
                                }
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función Remove necesita un parámetro.", Linea, Columna));
                            break;
                        case "size":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función Size no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsMap() || Target.Tipo.IsList() || Target.Tipo.IsSet())
                            {
                                if (!(valorTarget is Null))
                                {
                                    Tipo = new Tipo(Type.INT);
                                    return ((Collection)valorTarget).Valores.Count();
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El " + Target.Tipo.Type.ToString() + " no ha sido inicializado.", Linea, Columna));
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función Size solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                            break;
                        case "clear":
                            if (funcion.Parametro != null)
                                errores.AddLast(new Error("Semántico", "La función Clear no necesita parámetros.", Linea, Columna));

                            if (Target.Tipo.IsMap() || Target.Tipo.IsList() || Target.Tipo.IsSet())
                            {
                                if (!(valorTarget is Null))
                                {
                                    ((Collection)valorTarget).Valores.Clear();

                                    if (Target.Tipo.IsList() || Target.Tipo.IsSet())
                                        ((Collection)valorTarget).Posicion = 0;

                                    return null;
                                }
                                else
                                    errores.AddLast(new Error("Semántico", "El  " + Target.Tipo.Type.ToString() + " no ha sido inicializado.", Linea, Columna));
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función Clear solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                            break;
                        case "contains":
                            if (funcion.Parametro != null)
                            {
                                if (funcion.Parametro.Count() >= 1)
                                {
                                    if (funcion.Parametro.Count() > 1)
                                        errores.AddLast(new Error("Semántico", "La función Contains no necesita más de un parámetros.", Linea, Columna));

                                    Expresion parametro1 = funcion.Parametro.ElementAt(0);

                                    object valParametro1 = parametro1.GetValor(e, log, errores);

                                    if (valParametro1 != null)
                                    {
                                        if (Target.Tipo.IsMap())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection map = (Collection)valorTarget;

                                                if (map.TipoClave.Equals(parametro1.Tipo))
                                                {
                                                    object valValor = map.Get(valParametro1);
                                                    Tipo = new Tipo(Type.BOOLEAN);
                                                    if (valValor != null)
                                                        return true;
                                                    else
                                                        return false;
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo de la clave no coincide con el declarado en el Map: " + map.TipoClave.Type.ToString() + ".", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Map no ha sido inicializado.", Linea, Columna));

                                        }
                                        else if (Target.Tipo.IsList())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection list = (Collection)valorTarget;

                                                if (list.TipoValor.Equals(parametro1.Tipo))
                                                {
                                                    Tipo = new Tipo(Type.BOOLEAN);
                                                    if (list.Contains(valParametro1))
                                                        return true;
                                                    else
                                                        return false;
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo del valor no coincide con el declarado en el List: " + list.TipoValor.Type.ToString() + ".", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El List no ha sido inicializado.", Linea, Columna));

                                        }
                                        else if (Target.Tipo.IsSet())
                                        {
                                            if (!(valorTarget is Null))
                                            {
                                                Collection set = (Collection)valorTarget;

                                                if (set.TipoValor.Equals(parametro1.Tipo))
                                                {
                                                    Tipo = new Tipo(Type.BOOLEAN);
                                                    if (set.Contains(valParametro1))
                                                        return true;
                                                    else
                                                        return false;
                                                }
                                                else
                                                    errores.AddLast(new Error("Semántico", "El tipo del valor no coincide con el declarado en el Set: " + set.TipoValor.Type.ToString() + ".", Linea, Columna));
                                            }
                                            else
                                                errores.AddLast(new Error("Semántico", "El Set no ha sido inicializado.", Linea, Columna));

                                        }
                                        else
                                            errores.AddLast(new Error("Semántico", "La función Contains solo se puede aplicar a valores de tipo Map, List o Set.", Linea, Columna));
                                    }
                                }
                            }
                            else
                                errores.AddLast(new Error("Semántico", "La función Contains necesita un parámetro.", Linea, Columna));
                            break;
                        default:
                            errores.AddLast(new Error("Semántico", "No se ha encontrado la función: " + funcion.Id + ".", Linea, Columna));
                            break;
                    }
                }
                else
                {
                    //atributo es Id
                }
            }
            return null;
        }
    }
}
