using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CQL.ast.instruccion.ddl
{
    class Seleccionar : Instruccion
    {
        public Seleccionar(LinkedList<Expresion> columnas, string id, int linea, int columna) : base(linea, columna)
        {
            Columnas = columnas;
            Id = id;
        }

        public Seleccionar(LinkedList<Expresion> columnas, string id, Where where, int linea, int columna) : base(linea, columna)
        {
            Columnas = columnas;
            Id = id;
            Where = where;
        }

        public Seleccionar(LinkedList<Expresion> columnas, string id, LinkedList<Identificador> order, int linea, int columna) : base(linea, columna)
        {
            Columnas = columnas;
            Id = id;
            Order = order;
        }

        public LinkedList<Expresion> Columnas;
        public string Id { get; set; }
        public Where Where { get; set; }
        public LinkedList<Identificador> Order { get; set; }

        public override object Ejecutar(Entorno e, bool funcion, bool ciclo, bool sw, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            BD actual = e.Master.Actual;
            if (actual != null)
            {
                Simbolo sim = actual.GetTabla(Id);

                if (sim != null)
                {
                    Tabla tabla = (Tabla)sim.Valor;

                    LinkedList<Entorno> datos = new LinkedList<Entorno>();

                    if (Order != null)
                    {
                        if (tabla.Datos.Count() > 1)
                        {
                            foreach (Entorno ent in tabla.Datos)
                            {
                                Entorno entActual = new Entorno(null, new LinkedList<Simbolo>());

                                foreach (Simbolo simActual in ent.Simbolos)
                                {
                                    entActual.Add(new Simbolo(simActual.Tipo, simActual.Rol, simActual.Id, simActual.Valor));
                                }

                                datos.AddLast(entActual);
                            }

                            e.Master.EntornoActual = tabla.Datos.ElementAt(0);

                            foreach (Identificador ident in Order)
                            {
                                LinkedList<Entorno> tmp = new LinkedList<Entorno>();
                                IEnumerable<Entorno> ordered;

                                object identValor = ident.GetValor(e, log, errores);

                                if (identValor != null)
                                {
                                    if (ident.Tipo.IsString() || ident.Tipo.IsDate() || ident.Tipo.IsTime())
                                        ordered = datos.OrderBy(p => p.GetCualquiera(ident.GetId()).Valor.ToString()).AsEnumerable();
                                    else if (ident.Tipo.IsInt())
                                        ordered = datos.OrderBy(p => (int)p.GetCualquiera(ident.GetId()).Valor).AsEnumerable();
                                    else if (ident.Tipo.IsDouble())
                                        ordered = datos.OrderBy(p => (double)p.GetCualquiera(ident.GetId()).Valor).AsEnumerable();
                                    else
                                    {
                                        errores.AddLast(new Error("Semántico", "Solo se puede usar la cláusula Order By sobre datos primitivos.", Linea, Columna));
                                        return null;
                                    }

                                    if (ident.IsASC)
                                    {
                                        foreach (Entorno eTmp in ordered)
                                        {
                                            tmp.AddLast(eTmp);
                                        }
                                    }
                                    else
                                    {
                                        for (int i = ordered.Count() - 1; i >= 0; i--)
                                        {
                                            tmp.AddLast(ordered.ElementAt(i));
                                        }
                                    }
                                    datos = tmp;
                                }
                                else
                                    continue;//return null;
                                break;
                            }
                        }
                        else
                        {
                            datos = tabla.Datos;
                        }
                    }
                    else
                    {
                        datos = tabla.Datos;
                    }

                    LinkedList<Entorno> data = new LinkedList<Entorno>();

                    foreach (Entorno ent in datos)
                    {
                        e.Master.EntornoActual = ent;
                        Entorno entActual = new Entorno(null, new LinkedList<Simbolo>());

                        if (Where != null)
                        {
                            object valWhere = Where.GetValor(e, log, errores);
                            if (valWhere != null)
                            {
                                if (Where.Tipo.IsBoolean())
                                {
                                    if (!(bool)valWhere)
                                        continue;
                                }
                                else
                                {
                                    errores.AddLast(new Error("Semántico", "Cláusula Where debe ser booleana.", Linea, Columna));
                                    return null;
                                }
                            }
                            else
                                return null;
                        }

                        if (Columnas != null)
                        {
                            int numCol = 1;

                            foreach (Expresion colExp in Columnas)
                            {
                                Simbolo simActual = new Simbolo();

                                object valColExp = colExp.GetValor(e, log, errores);

                                if (valColExp != null)
                                {
                                    simActual.Tipo = colExp.Tipo;
                                    simActual.Rol = Rol.COLUMNA;

                                    if (colExp is Identificador iden)
                                    {
                                        simActual.Id = iden.GetId();
                                    }
                                    else
                                    {
                                        simActual.Id = "Columna " + numCol++;
                                    }
                                    simActual.Valor = valColExp;
                                }
                                else
                                    return null;

                                entActual.Add(simActual);
                            }
                        }
                        else
                        {
                            foreach (Simbolo col in ent.Simbolos)
                            {
                                entActual.Add(new Simbolo(col.Tipo, col.Rol, col.Id, col.Valor));
                            }
                        }

                        data.AddLast(entActual);
                    }

                    e.Master.EntornoActual = null;

                    string salida;

                    if (data.Count() >= 1)
                    {
                        salida = "<table> \n";

                        salida += "<tr> \n";

                        foreach (Simbolo col in data.ElementAt(0).Simbolos)
                        {
                            salida += "\t<th>" + col.Id + "</th>\n";
                        }

                        salida += "</tr>\n";

                        foreach (Entorno ent in data)
                        {
                            salida += "<tr>\n";

                            foreach (Simbolo col in ent.Simbolos)
                            {
                                salida += "\t<td>" + col.Valor.ToString() + "</td>\n";
                            }

                            salida += "</tr>\n";
                        }

                        salida += "</table>\n\n\n";

                    }
                    else
                    {
                        salida = "No hay datos en la consulta.\n\n";
                    }

                    log.AddLast(new Salida(2, salida));
                    return null;
                }
                else
                    errores.AddLast(new Error("Semántico", "No existe una Tabla con el id: " + Id + " en la base de datos.", Linea, Columna));
            }
            else
                errores.AddLast(new Error("Semántico", "No se ha seleccionado una base de datos, no se pudo Actualizar.", Linea, Columna));

            return null;
        }
    }
}
