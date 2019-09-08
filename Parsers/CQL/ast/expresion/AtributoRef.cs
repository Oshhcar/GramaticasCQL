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
                            {
                                errores.AddLast(new Error("Semántico", "La función nativa getSeconds solo se puede aplicar a valores de tipo Time.", Linea, Columna));
                            }
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
