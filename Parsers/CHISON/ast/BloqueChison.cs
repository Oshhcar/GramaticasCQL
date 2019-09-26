using GramaticasCQL.Parsers.CQL;
using GramaticasCQL.Parsers.CQL.ast;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using GramaticasCQL.Parsers.CQL.ast.instruccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CHISON.ast
{
    class BloqueChison : Expresion
    {
        public BloqueChison(LinkedList<Instruccion> instrucciones, int linea, int columna) : base(linea, columna)
        {
            Instrucciones = instrucciones;
            Obj = OBJ.CUALQUIERA;
        }

        public LinkedList<Instruccion> Instrucciones { get; set; }
        public OBJ Obj { get; set; }
        public Entorno Ent { get; set; }
        public override object GetValor(Entorno e, LinkedList<Salida> log, LinkedList<Error> errores)
        {
            Tipo = new Tipo(Type.OBJECT);
            bool cqlType;

            switch (Obj)
            {
                case OBJ.PRINCIPAL:
                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Databases || inst is Users)
                        {
                            inst.Ejecutar(e, false, false, false, false, log, errores);
                        }
                        else
                        {
                            /*eRROR*/
                        }

                    }
                    break;
                case OBJ.USER:
                    Usuario usuario = new Usuario(null, null);

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    usuario.Id = valAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("password"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    usuario.Password = valAtributo.ToString();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("permissions"))
                            {
                                if (atributo.Valor is Lista lista)
                                {
                                    if (lista.Valores != null)
                                    {
                                        foreach (Expresion parm in lista.Valores)
                                        {
                                            if (parm is BloqueChison bloque)
                                            {
                                                bloque.Obj = OBJ.PERMISSIONS;

                                                object valBloque = bloque.GetValor(e, log, errores);

                                                if (valBloque != null)
                                                {
                                                    if (e.MasterRollback.Get(valBloque.ToString()) != null)
                                                    {
                                                        if (!usuario.GetPermiso(valBloque.ToString()))
                                                            usuario.AddPermiso(valBloque.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (usuario.Id != null && usuario.Password != null)
                        return usuario;
                    break;
                case OBJ.PERMISSIONS:
                    string name = null;

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    name = valAtributo.ToString().ToLower();
                                }
                            }
                        }
                    }

                    return name;
                case OBJ.DATABASE:
                    BD bd = new BD(null);
                    e.MasterRollback.Actual = bd;

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    bd.Id = valAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("data"))
                            {
                                if (atributo.Valor is Lista lista)
                                {
                                    if (lista.Valores != null)
                                    {
                                        foreach (Expresion expr in lista.Valores)
                                        {
                                            object obj;

                                            if (expr is BloqueChison bloque)
                                            {
                                                bloque.Obj = OBJ.USERTYPE;
                                                obj = bloque.GetValor(e, log, errores);

                                                if (obj != null)
                                                {
                                                    if (obj is Simbolo ut)
                                                    {
                                                        if (bd.GetUserType(ut.Id) == null)
                                                        {
                                                            bd.Add(ut);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    bloque.Obj = OBJ.PROCEDURE;
                                                    obj = bloque.GetValor(e, log, errores);

                                                    if (obj != null)
                                                    {
                                                        if (obj is Simbolo pr)
                                                        {
                                                            if (bd.GetProcedimiento(pr.Id) == null)
                                                            {
                                                                bd.Add(pr);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bloque.Obj = OBJ.TABLA;
                                                        obj = bloque.GetValor(e, log, errores);

                                                        if (obj != null)
                                                        {
                                                            if (obj is Simbolo tb)
                                                            {
                                                                if (bd.GetTabla(tb.Id) == null)
                                                                {
                                                                    bd.Add(tb);
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (bd.Id != null)
                        return bd;
                    break;
                case OBJ.USERTYPE:
                    LinkedList<Simbolo> sims = new LinkedList<Simbolo>();
                    Simbolo usertype = new Simbolo(Rol.USERTYPE, null, new Entorno(null, sims));
                    cqlType = false;
                    bool attrs = false;

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valorAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("cql-type"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    if (valorAtributo.ToString().ToLower().Equals("object"))
                                        cqlType = true;
                                    else
                                        return null;
                                }
                            }
                            else if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    usertype.Id = valorAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("attrs"))
                            {
                                if (atributo.Valor is Lista lista)
                                {
                                    attrs = true;
                                    if (lista.Valores != null)
                                    {
                                        foreach (Expresion expr in lista.Valores)
                                        {
                                            if (expr is BloqueChison bloque)
                                            {
                                                bloque.Obj = OBJ.ATRIBUTO;
                                                object obj = bloque.GetValor(e, log, errores);

                                                if (obj != null)
                                                {
                                                    if (obj is Simbolo at)
                                                    {
                                                        bool exists = false;

                                                        foreach (Simbolo s in sims)
                                                        {
                                                            if (s.Id.Equals(at.Id))
                                                            {
                                                                exists = true;
                                                                break;
                                                            }
                                                        }

                                                        if (!exists)
                                                            sims.AddLast(at);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (usertype.Id != null && attrs && cqlType)
                        return usertype;
                    break;
                case OBJ.ATRIBUTO:
                    Simbolo sim = new Simbolo();
                    sim.Rol = Rol.ATRIBUTO;
                    sim.Id = null;
                    sim.Tipo = null;
                    sim.Valor = null;

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valorAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    sim.Id = valorAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("type"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    AnalizadorCHISON chison = new AnalizadorCHISON();

                                    if (chison.AnalizarEntrada(valorAtributo.ToString()))
                                    {
                                        object obj = chison.GenerarArbol(chison.Raiz.Root);

                                        if (obj != null)
                                        {
                                            if (obj is Tipo t)
                                            {
                                                sim.Tipo = t;
                                                sim.Valor = sim.Predefinido();
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    if (sim.Id != null && sim.Tipo != null)
                        return sim;
                    break;
                case OBJ.PROCEDURE:
                    Bloque bl = new Bloque(null, "", Linea, Columna);
                    Procedimiento procedimiento = new Procedimiento(new LinkedList<Identificador>(), new LinkedList<Identificador>(), bl);
                    Simbolo proc = new Simbolo(Rol.PROCEDIMIENTO, null, procedimiento);
                    cqlType = false;
                    string firma = "";

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valorAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("cql-type"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    if (valorAtributo.ToString().ToLower().Equals("procedure"))
                                        cqlType = true;
                                    else
                                        return null;
                                }
                            }
                            else if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    proc.Id = valorAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("parameters"))
                            {
                                if (atributo.Valor is Lista lista)
                                {
                                    if (lista.Valores != null)
                                    {
                                        foreach (Expresion expr in lista.Valores)
                                        {
                                            if (expr is BloqueChison bloque)
                                            {
                                                bloque.Obj = OBJ.PARAMETRO;
                                                object obj = bloque.GetValor(e, log, errores);

                                                if (obj != null)
                                                {
                                                    if (obj is Identificador iden)
                                                    {
                                                        bool exists = false;

                                                        if (iden.In)
                                                        {
                                                            foreach (Identificador i in procedimiento.Parametro)
                                                            {
                                                                if (i.Id.Equals(iden.Id))
                                                                {
                                                                    exists = true;
                                                                    break;
                                                                }
                                                            }

                                                            if (!exists)
                                                            {
                                                                firma += "-" + iden.Tipo.Type.ToString();
                                                                procedimiento.Parametro.AddLast(iden);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (Identificador i in procedimiento.Retorno)
                                                            {
                                                                if (i.Id.Equals(iden.Id))
                                                                {
                                                                    exists = true;
                                                                    break;
                                                                }
                                                            }

                                                            if (!exists)
                                                                procedimiento.Retorno.AddLast(iden);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (atributo.Id.ToString().Equals("instr"))
                            {
                                if (atributo.Valor.Tipo.IsVoid())
                                {
                                    string cadena = valorAtributo.ToString().Replace("$", "");

                                    AnalizadorCQL cql = new AnalizadorCQL();

                                    if (cql.AnalizarEntrada(cadena))
                                    {
                                        object obj = cql.GenerarArbol(cql.Raiz.Root);

                                        if (obj != null)
                                        {
                                            if (obj is ASTCQL ast)
                                            {
                                                bl.Bloques = ast.Sentencias;
                                                bl.Cadena = "{\n" + ast.Cadena + "}\n";
                                            }
                                        }
                                        else
                                            Console.WriteLine("error en archivo entrada");
                                    }
                                    else
                                    {
                                        Console.WriteLine("error en archivo entrada");
                                    }

                                }
                            }
                        }
                    }

                    if (proc.Id != null && cqlType)
                    {
                        proc.Id += firma.ToString().ToLower();
                        return proc;
                    }
                    break;
                case OBJ.PARAMETRO:
                    Identificador parametro = new Identificador(null, Linea, Columna);
                    bool as_ = false;

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valorAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    parametro.Id = valorAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("type"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    AnalizadorCHISON chison = new AnalizadorCHISON();

                                    if (chison.AnalizarEntrada(valorAtributo.ToString()))
                                    {
                                        object obj = chison.GenerarArbol(chison.Raiz.Root);

                                        if (obj != null)
                                        {
                                            if (obj is Tipo t)
                                            {
                                                parametro.Tipo = t;
                                            }
                                        }
                                    }

                                }
                            }
                            else if (atributo.Id.ToString().Equals("as"))
                            {
                                if (atributo.Valor.Tipo.IsIn() || atributo.Valor.Tipo.IsOut())
                                {
                                    as_ = true;

                                    if (atributo.Valor.Tipo.IsOut())
                                        parametro.In = false;
                                    else
                                        parametro.In = true;
                                }
                            }
                        }
                    }

                    if (parametro.Id != null && parametro.Tipo != null && as_)
                        return parametro;
                    break;
                case OBJ.TABLA:
                    Tabla tabla = new Tabla();
                    Simbolo tab = new Simbolo(Rol.TABLA, null, tabla);
                    cqlType = false;

                    foreach (Instruccion instr in Instrucciones)
                    {
                        if (instr is Atributo atributo)
                        {
                            object valorAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("cql-type"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    if (valorAtributo.ToString().ToLower().Equals("table"))
                                        cqlType = true;
                                    else
                                        return null;
                                }
                            }
                            else if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                    tab.Id = valorAtributo.ToString().ToLower();
                            }
                            else if (atributo.Id.ToString().Equals("columns"))
                            {
                                if (atributo.Valor is Lista lista)
                                {
                                    if (lista.Valores != null)
                                    {
                                        foreach (Expresion expr in lista.Valores)
                                        {
                                            if (expr is BloqueChison bloque)
                                            {
                                                bloque.Obj = OBJ.COLUMNA;
                                                object obj = bloque.GetValor(e, log, errores);

                                                if (obj != null)
                                                {
                                                    if (obj is Simbolo cl)
                                                    {
                                                        if (tabla.Cabecera.GetCualquiera(cl.Id) == null)
                                                        {
                                                            bool counter = false;
                                                            bool another = false;

                                                            if (cl.Tipo.IsCounter())
                                                                counter = true;
                                                            else
                                                                another = true;

                                                            if (cl.Rol == Rol.PRIMARY)
                                                            {
                                                                foreach (Simbolo cab in tabla.Cabecera.Simbolos)
                                                                {
                                                                    if (cab.Rol == Rol.PRIMARY)
                                                                    {
                                                                        if (cab.Tipo.IsCounter())
                                                                            counter = true;
                                                                        else
                                                                            another = true;
                                                                    }
                                                                }
                                                            }

                                                            if (counter)
                                                                if (another)
                                                                    continue; //no se puede 

                                                            tabla.Cabecera.Add(cl);
                                                        }
                                                        /*Ya existe*/
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (atributo.Id.ToString().Equals("data"))
                            {
                                if (atributo.Valor is Lista lista)
                                {
                                    if (lista.Valores != null)
                                    {
                                        foreach (Expresion expr in lista.Valores)
                                        {
                                            if (expr is BloqueChison bloque)
                                            {
                                                bloque.Obj = OBJ.DATO;
                                                bloque.Ent = tabla.GetNuevaFila();
                                                object obj = bloque.GetValor(e, log, errores);

                                                if (obj != null)
                                                {
                                                    if (obj is Entorno fila)
                                                    {
                                                        LinkedList<Simbolo> primaria = new LinkedList<Simbolo>();
                                                        bool err = false;

                                                        foreach (Simbolo row in fila.Simbolos)
                                                        {
                                                            if (row.Tipo.IsCounter())
                                                                row.Valor = tabla.Contador;

                                                            if (row.Rol == Rol.PRIMARY)
                                                            {
                                                                if (row.Valor is Null)
                                                                {
                                                                    err = true;
                                                                    break;
                                                                }
                                                                primaria.AddLast(row);
                                                            }
                                                        }

                                                        if (!err)
                                                        {
                                                            if (tabla.Insertar(fila, primaria))
                                                            {
                                                                tabla.Contador++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (tab.Id != null && cqlType && tabla.Cabecera.Simbolos.Count() > 0)
                        return tab;
                    break;
                case OBJ.COLUMNA:
                    Simbolo columna = new Simbolo();
                    columna.Rol = Rol.COLUMNA;
                    columna.Tipo = null;
                    columna.Id = null;
                    columna.Valor = null;
                    bool pk = false;

                    foreach (Instruccion inst in Instrucciones)
                    {
                        if (inst is Atributo atributo)
                        {
                            object valorAtributo = atributo.Valor.GetValor(e, log, errores);

                            if (atributo.Id.ToString().Equals("name"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    columna.Id = valorAtributo.ToString().ToLower();
                                }
                            }
                            else if (atributo.Id.ToString().Equals("type"))
                            {
                                if (atributo.Valor.Tipo.IsString())
                                {
                                    AnalizadorCHISON chison = new AnalizadorCHISON();

                                    if (chison.AnalizarEntrada(valorAtributo.ToString()))
                                    {
                                        object obj = chison.GenerarArbol(chison.Raiz.Root);

                                        if (obj != null)
                                        {
                                            if (obj is Tipo t)
                                            {
                                                columna.Tipo = t;
                                                columna.Valor = columna.Predefinido();
                                            }
                                        }
                                    }

                                }
                            }
                            else if (atributo.Id.ToString().Equals("pk"))
                            {
                                if (atributo.Valor.Tipo.IsBoolean())
                                {
                                    pk = true;

                                    if ((bool)valorAtributo)
                                        columna.Rol = Rol.PRIMARY;
                                }
                            }
                        }
                    }


                    if (columna.Id != null && columna.Tipo != null && pk)
                        return columna;
                    break;
                case OBJ.DATO:
                    if (Ent != null)
                    {
                        foreach (Instruccion inst in Instrucciones)
                        {
                            if (inst is Atributo atributo)
                            {
                                object valAtributo = atributo.Valor.GetValor(e, log, errores);

                                Simbolo col = Ent.GetCualquiera(atributo.Id.ToString());

                                if (col != null)
                                {
                                    switch (col.Tipo.Type)
                                    {
                                        case Type.INT:
                                        case Type.DOUBLE:
                                        case Type.STRING:
                                        case Type.BOOLEAN:
                                        case Type.DATE:
                                        case Type.TIME:
                                        case Type.COUNTER:
                                            if (col.Tipo.Equals(atributo.Valor.Tipo))
                                            {
                                                col.Valor = valAtributo;
                                            }
                                            else
                                            {
                                                Casteo cast = new Casteo(col.Tipo, new Literal(atributo.Valor.Tipo, valAtributo, 0, 0), 0, 0)
                                                {
                                                    Mostrar = false
                                                };

                                                object valCast = cast.GetValor(e, log, errores);
                                                if (valCast != null)
                                                {
                                                    if (!(valCast is Throw))
                                                    {
                                                        col.Valor = valCast;
                                                    }
                                                }
                                            }
                                            break;
                                        case Type.OBJECT:
                                            if (e.MasterRollback.Actual != null)
                                            {
                                                Simbolo objeto = e.MasterRollback.Actual.GetUserType(col.Tipo.Objeto);
                                                if (objeto != null)
                                                {
                                                    LinkedList<Simbolo> atributos = new LinkedList<Simbolo>();

                                                    foreach (Simbolo s in ((Entorno)objeto.Valor).Simbolos)
                                                    {
                                                        atributos.AddLast(new Simbolo(s.Tipo, Rol.ATRIBUTO, s.Id));
                                                    }

                                                    if (atributo.Valor is BloqueChison bloque)
                                                    {
                                                        bloque.Obj = OBJ.DATO;
                                                        bloque.Ent = new Entorno(null, atributos);

                                                        object obj = bloque.GetValor(e, log, errores);

                                                        if (obj != null)
                                                        {
                                                            if(obj is Entorno ent)
                                                            {
                                                                col.Valor = new Objeto(col.Tipo.Objeto, ent);
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            break;
                                    }
                                }   
                            }
                        }
                        return Ent;
                    }
                    break;
            }

            return null;
        }
    }

    public enum OBJ
    {
        PRINCIPAL,
        DATABASE,
        USER,
        NAME,
        PASSWORD,
        PERMISSIONS,
        USERTYPE,
        ATRIBUTO,
        PROCEDURE,
        PARAMETRO,
        TABLA,
        COLUMNA,
        DATO,
        CUALQUIERA
    }
}
