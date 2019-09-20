using GramaticasCQL.Parsers.CQL.ast;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using GramaticasCQL.Parsers.CQL.ast.expresion.operacion;
using GramaticasCQL.Parsers.CQL.ast.instruccion;
using GramaticasCQL.Parsers.CQL.ast.instruccion.ciclos;
using GramaticasCQL.Parsers.CQL.ast.instruccion.condicionales;
using GramaticasCQL.Parsers.CQL.ast.instruccion.ddl;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = GramaticasCQL.Parsers.CQL.ast.entorno.Type;

namespace GramaticasCQL.Parsers.CQL
{
    class AnalizadorCQL
    {
        public ParseTree Raiz { get; set; }

        public bool AnalizarEntrada(String entrada)
        {
            GramaticaCQL gramatica = new GramaticaCQL();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            this.Raiz = arbol;

            if (arbol.Root != null)
                return true;

            return false;
        }

        public object GenerarArbol(ParseTreeNode raiz)
        {
            string r = raiz.ToString();
            ParseTreeNode[] hijos = null;

            int linea = 0;
            int columna = 0;

            if (raiz.ChildNodes.Count > 0)
            {
                hijos = raiz.ChildNodes.ToArray();
            }

            switch (r)
            {
                case "INICIO":
                    return GenerarArbol(hijos[0]);
                case "INSTRUCCIONES":
                    LinkedList<NodoASTCQL> sentencias = new LinkedList<NodoASTCQL>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        sentencias.AddLast((NodoASTCQL)GenerarArbol(hijo));
                    }
                    return new ASTCQL(sentencias);
                case "INSTRUCCION":
                    return GenerarArbol(hijos[0]);
                case "TYPE":
                    switch (hijos[0].Term.Name)
                    {
                        case "int":
                            return new Tipo(Type.INT);
                        case "double":
                            return new Tipo(Type.DOUBLE);
                        case "string":
                            return new Tipo(Type.STRING);
                        case "boolean":
                            return new Tipo(Type.BOOLEAN);
                        case "date":
                            return new Tipo(Type.DATE);
                        case "time":
                            return new Tipo(Type.TIME);
                        case "identifier":
                            return new Tipo(hijos[0].Token.Text.ToLower());
                        case "counter":
                            return new Tipo(Type.COUNTER);
                        case "map":
                            return new Tipo(Type.MAP);
                        case "list":
                            return new Tipo(Type.LIST);
                        case "set":
                            return new Tipo(Type.SET);
                        default:
                            return null;
                    }
                case "TYPE_PRIMITIVE":
                    switch (hijos[0].Term.Name)
                    {
                        case "int":
                            return new Tipo(Type.INT);
                        case "double":
                            return new Tipo(Type.DOUBLE);
                        case "string":
                            return new Tipo(Type.STRING);
                        case "boolean":
                            return new Tipo(Type.BOOLEAN);
                        case "date":
                            return new Tipo(Type.DATE);
                        case "time":
                            return new Tipo(Type.TIME);
                        default:
                            return null;
                    }
                case "TYPE_COLLECTION":
                    switch (hijos[0].Term.Name)
                    {
                        case "int":
                            return new Tipo(Type.INT);
                        case "double":
                            return new Tipo(Type.DOUBLE);
                        case "string":
                            return new Tipo(Type.STRING);
                        case "boolean":
                            return new Tipo(Type.BOOLEAN);
                        case "date":
                            return new Tipo(Type.DATE);
                        case "time":
                            return new Tipo(Type.TIME);
                        case "identifier":
                            return new Tipo(hijos[0].Token.Text.ToLower());
                        case "counter":
                            return new Tipo(Type.COUNTER);
                        case "map":
                            return new Tipo((Tipo)GenerarArbol(hijos[2]), (Tipo)GenerarArbol(hijos[4]));
                        case "list":
                            return new Tipo(Type.LIST, (Tipo)GenerarArbol(hijos[2]));
                        case "set":
                            return new Tipo(Type.SET, (Tipo)GenerarArbol(hijos[2]));
                        default:
                            return null;
                    }
                case "TYPEDEF":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 6)
                        return new TypeCrear(hijos[2].Token.Text, false, (LinkedList<Simbolo>)GenerarArbol(hijos[4]), linea, columna);
                    else
                        return new TypeCrear(hijos[3].Token.Text, true, (LinkedList<Simbolo>)GenerarArbol(hijos[5]), linea, columna);
                case "ATTRIBUTE_LIST":
                    LinkedList<Simbolo> atribute = new LinkedList<Simbolo>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        atribute.AddLast((Simbolo)GenerarArbol(hijo));
                    }
                    return atribute;
                case "ATTRIBUTE":
                    return new Simbolo((Tipo)GenerarArbol(hijos[1]), Rol.ATRIBUTO, hijos[0].Token.Text.ToLower());
                case "USE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Use(hijos[1].Token.Text, linea, columna);
                case "DATABASEDEF":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new BDCrear(hijos[2].Token.Text, false, linea, columna);
                    else
                        return new BDCrear(hijos[3].Token.Text, true, linea, columna);
                case "DROP":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new BDBorrar(hijos[2].Token.Text, false, linea, columna);
                    else
                        return new BDBorrar(hijos[3].Token.Text, true, linea, columna);
                case "TABLEDEF":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    TablaCrear tabla;
                    if (hijos.Count() == 6)
                    {
                        tabla = (TablaCrear)GenerarArbol(hijos[4]);
                        tabla.Id = hijos[2].Token.Text;
                    }
                    else
                    {
                        tabla = (TablaCrear)GenerarArbol(hijos[5]);
                        tabla.IfNotExist = true;
                        tabla.Id = hijos[3].Token.Text;
                    }
                    tabla.Linea = linea;
                    tabla.Columna = columna;
                    return tabla;
                case "COLUMN_LIST":
                    TablaCrear t = new TablaCrear();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        object obj = GenerarArbol(hijo);
                        if (obj is Simbolo)
                            t.Simbolos.AddLast((Simbolo)obj);
                        else
                            t.Primary = (LinkedList<string>)obj;
                    }
                    return t;
                case "COLUMN":
                    if (hijos.Count() == 2)
                        return new Simbolo((Tipo)GenerarArbol(hijos[1]), Rol.COLUMNA, hijos[0].Token.Text.ToLower());
                    else if (hijos.Count() == 4)
                        return new Simbolo((Tipo)GenerarArbol(hijos[1]), Rol.PRIMARY, hijos[0].Token.Text.ToLower());
                    else
                        return GenerarArbol(hijos[3]);
                case "ID_LIST":
                    LinkedList<string> idList = new LinkedList<string>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        idList.AddLast(hijo.Token.Text.ToLower());
                    }
                    return idList;
                case "TABLEALTER":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos[3].Term.Name.Equals("add"))
                        return new TablaModificar(hijos[2].Token.Text, (LinkedList<Simbolo>)GenerarArbol(hijos[4]), linea, columna);
                    else
                        return new TablaModificar(hijos[2].Token.Text, (LinkedList<string>)GenerarArbol(hijos[4]), linea, columna);
                case "TABLEDROP":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new TablaBorrar(hijos[2].Token.Text, false, linea, columna);
                    return new TablaBorrar(hijos[3].Token.Text, true, linea, columna);
                case "TABLETRUNCATE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new TablaTruncar(hijos[2].Token.Text, linea, columna);
                case "COMMIT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Commit(linea, columna);
                case "ROLLBACK":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Rollback(linea, columna);
                case "USERDEF":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new UsuarioCrear(hijos[2].Token.Text, new Cadena(hijos[5].Token.Text), linea, columna);
                case "GRANT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new UsuarioGrant(hijos[1].Token.Text, hijos[3].Token.Text, linea, columna);
                case "REVOKE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new UsuarioRevoke(hijos[1].Token.Text, hijos[3].Token.Text, linea, columna);
                case "WHERE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 2)
                        return new Where((Expresion)GenerarArbol(hijos[1]), linea, columna);
                    else if (hijos.Count() == 4)
                        return new Where((Expresion)GenerarArbol(hijos[1]), (LinkedList<Expresion>)GenerarArbol(hijos[3]), linea, columna);
                    return new Where((Expresion)GenerarArbol(hijos[1]), (LinkedList<Expresion>)GenerarArbol(hijos[4]), linea, columna);
                case "INSERT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 7)
                        return new Insertar(hijos[2].Token.Text, (LinkedList<Expresion>)GenerarArbol(hijos[5]), linea, columna);
                    return new Insertar(hijos[2].Token.Text, (LinkedList<string>)GenerarArbol(hijos[4]), (LinkedList<Expresion>)GenerarArbol(hijos[8]), linea, columna);
                case "UPDATE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if(hijos.Count() == 4)
                        return new Actualizar(hijos[1].Token.Text, (LinkedList<Asignacion>)GenerarArbol(hijos[3]), linea, columna);
                    return new Actualizar(hijos[1].Token.Text, (LinkedList<Asignacion>)GenerarArbol(hijos[3]),(Where)GenerarArbol(hijos[4]), linea, columna);
                case "DELETE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new Eliminar(hijos[2].Token.Text, linea, columna);
                    return new Eliminar(hijos[2].Token.Text, (Where)GenerarArbol(hijos[3]), linea, columna);
                case "SELECT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 4)
                        return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, linea, columna);
                    else if (hijos.Count() == 5)
                        return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (Where)GenerarArbol(hijos[4]), linea, columna);
                    else if (hijos.Count() == 6)
                        return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (Expresion)GenerarArbol(hijos[5]), linea, columna);
                    else if (hijos.Count() == 7)
                    {
                        if(hijos[5].Term.Name.Equals("limit"))
                            return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (Where)GenerarArbol(hijos[4]), (Expresion)GenerarArbol(hijos[6]), linea, columna);
                        else
                            return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (LinkedList<Identificador>)GenerarArbol(hijos[6]), linea, columna);
                    }
                    else if (hijos.Count() == 8)
                        return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (Where)GenerarArbol(hijos[4]), (LinkedList<Identificador>)GenerarArbol(hijos[7]), linea, columna);
                    else if (hijos.Count() == 9)
                        return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (LinkedList<Identificador>)GenerarArbol(hijos[6]), (Expresion)GenerarArbol(hijos[8]), linea, columna);
                    return new Seleccionar((LinkedList<Expresion>)GenerarArbol(hijos[1]), hijos[3].Token.Text, (Where)GenerarArbol(hijos[4]), (LinkedList<Identificador>)GenerarArbol(hijos[7]), (Expresion)GenerarArbol(hijos[9]), linea, columna);
                case "SELECT_EXP":
                    if (hijos[0].Term.Name.Equals("por"))
                        return null;
                    return GenerarArbol(hijos[0]);
                case "ORDER_LIST":
                    LinkedList<Identificador> order = new LinkedList<Identificador>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        order.AddLast((Identificador)GenerarArbol(hijo));
                    }
                    return order;
                case "ORDER":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 2)
                        if(hijos[1].Term.Name.Equals("desc"))
                            return new Identificador(hijos[0].Token.Text, false, false, linea, columna);
                    return new Identificador(hijos[0].Token.Text, linea, columna);
                case "BATCH":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Batch((LinkedList<Instruccion>)GenerarArbol(hijos[2]), linea, columna);
                case "DML_LIST":
                    LinkedList<Instruccion> dmlList = new LinkedList<Instruccion>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        dmlList.AddLast((Instruccion)GenerarArbol(hijo));
                    }
                    return dmlList;
                case "DML":
                    return GenerarArbol(hijos[0]);
                case "BLOQUE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new Bloque((LinkedList<NodoASTCQL>)GenerarArbol(hijos[1]), linea, columna);
                    else
                        return new Bloque(null, linea, columna);
                case "SENTENCIAS":
                    LinkedList<NodoASTCQL> bloques = new LinkedList<NodoASTCQL>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        bloques.AddLast((NodoASTCQL)GenerarArbol(hijo));
                    }
                    return bloques;
                case "SENTENCIA":
                    return GenerarArbol(hijos[0]);
                case "TARGET_LIST":
                    LinkedList<Expresion> target = new LinkedList<Expresion>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        target.AddLast((Expresion)GenerarArbol(hijo));
                    }
                    return target;
                case "TARGET":
                    if (hijos[0].Term.Name.Equals("identifier"))
                    {
                        return new Identificador(hijos[0].Token.Text, hijos[0].Token.Location.Line + 1, hijos[0].Token.Location.Column + 1);
                    }
                    else if (hijos[0].Term.Name.Equals("identifier2"))
                    {
                        return new Identificador(hijos[0].Token.Text, true, hijos[0].Token.Location.Line + 1, hijos[0].Token.Location.Column + 1);
                    }
                    else
                    {
                        return GenerarArbol(hijos[0]);
                    }
                case "EXPRESSION_STMT":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Unario((Expresion)GenerarArbol(hijos[0]), GetOperador(hijos[1]), linea, columna);
                    }
                case "DECLARATION_STMT":
                    linea = hijos[0].ChildNodes.ToArray()[0].Token.Location.Line + 1;
                    columna = hijos[0].ChildNodes.ToArray()[0].Token.Location.Column + 1;

                    if (hijos.Count() == 2)
                    {
                        return new Declaracion((Tipo)GenerarArbol(hijos[0]), (LinkedList<Expresion>)GenerarArbol(hijos[1]), null, linea, columna);
                    }
                    else
                    {
                        return new Declaracion((Tipo)GenerarArbol(hijos[0]), (LinkedList<Expresion>)GenerarArbol(hijos[1]), (Expresion)GenerarArbol(hijos[3]), linea, columna);
                    }
                case "ASSIGNMENT_STMT":
                    linea = hijos[1].Token.Location.Line + 1;
                    columna = hijos[1].Token.Location.Column + 1;
                    if (hijos[2].Term.Name.Equals("CALL"))
                    {
                        LinkedList<Expresion> targetList = new LinkedList<Expresion>();
                        targetList.AddLast((Expresion)GenerarArbol(hijos[0]));
                        return new AsignacionCall(targetList, (Call)GenerarArbol(hijos[2]), linea, columna);
                    }
                    return new Asignacion((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), linea, columna);
                case "ASSIGNMENT_CALL":
                    linea = hijos[1].Token.Location.Line + 1;
                    columna = hijos[1].Token.Location.Column + 1;
                    return new AsignacionCall((LinkedList<Expresion>)GenerarArbol(hijos[0]), (Call)GenerarArbol(hijos[2]), linea, columna);
                case "ASSIGNMENT_LIST":
                    LinkedList<Asignacion> asignaLista = new LinkedList<Asignacion>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        asignaLista.AddLast((Asignacion)GenerarArbol(hijo));
                    }
                    return asignaLista;
                case "AUGMENTED_ASSIGNMENT_STMT":
                    linea = hijos[1].ChildNodes.ToArray()[0].Token.Location.Line + 1;
                    columna = hijos[1].ChildNodes.ToArray()[0].Token.Location.Column + 1;
                    return new AsignacionOperacion((Expresion)GenerarArbol(hijos[0]), (Operador)GenerarArbol(hijos[1]), (Expresion)GenerarArbol(hijos[2]), linea, columna);
                case "AUG_OPERATOR":
                    return GetAugOperador(hijos[0]);
                case "IF_STMT":
                    if (hijos.Count() == 1)
                        return new If((LinkedList<SubIf>)GenerarArbol(hijos[0]), 0, 0);
                    else
                    {
                        LinkedList<SubIf> subIfs = (LinkedList<SubIf>)GenerarArbol(hijos[0]);
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        subIfs.AddLast(new SubIf((Bloque)GenerarArbol(hijos[2]), linea, columna));
                        return new If(subIfs, 0, 0);
                    }
                case "IF_LIST":
                    if (hijos.Count() == 5)
                    {
                        LinkedList<SubIf> subIfs = new LinkedList<SubIf>();
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        subIfs.AddLast(new SubIf((Expresion)GenerarArbol(hijos[2]), (Bloque)GenerarArbol(hijos[4]), linea, columna));
                        return subIfs;
                    }
                    else
                    {
                        LinkedList<SubIf> subIfs = (LinkedList<SubIf>)GenerarArbol(hijos[0]);
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        subIfs.AddLast(new SubIf((Expresion)GenerarArbol(hijos[4]), (Bloque)GenerarArbol(hijos[6]), linea, columna));
                        return subIfs;

                    }
                case "SWITCH_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 7)
                        return new Switch((Expresion)GenerarArbol(hijos[2]), (LinkedList<Case>)GenerarArbol(hijos[5]), linea, columna);
                    else
                    {
                        LinkedList<Case> cases = (LinkedList<Case>)GenerarArbol(hijos[5]);
                        cases.AddLast(new Case((Bloque)GenerarArbol(hijos[8]), linea, columna));
                        return new Switch((Expresion)GenerarArbol(hijos[2]), cases, linea, columna);
                    }
                case "CASES":
                    if (hijos.Count() == 4)
                    {
                        LinkedList<Case> cases = new LinkedList<Case>();
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        cases.AddLast(new Case((Expresion)GenerarArbol(hijos[1]), (Bloque)GenerarArbol(hijos[3]), linea, columna));
                        return cases;
                    }
                    else
                    {
                        LinkedList<Case> cases = (LinkedList<Case>)GenerarArbol(hijos[0]);
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        cases.AddLast(new Case((Expresion)GenerarArbol(hijos[2]), (Bloque)GenerarArbol(hijos[4]), linea, columna));
                        return cases;

                    }
                case "WHILE_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new While((Expresion)GenerarArbol(hijos[2]), (Bloque)GenerarArbol(hijos[4]), linea, columna);
                case "DOWHILE_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new DoWhile((Expresion)GenerarArbol(hijos[4]), (Bloque)GenerarArbol(hijos[1]), linea, columna);
                case "FOR_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new For((Instruccion)GenerarArbol(hijos[2]), (Expresion)GenerarArbol(hijos[3]), (NodoASTCQL)GenerarArbol(hijos[4]), (Bloque)GenerarArbol(hijos[6]), linea, columna);
                case "FOR_INIT":
                    return GenerarArbol(hijos[0]);
                case "FOR_UPDATE":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Unario((Expresion)GenerarArbol(hijos[0]), GetOperador(hijos[1]), linea, columna);
                    }
                case "FUNDEF":
                    linea = hijos[1].Token.Location.Line + 1;
                    columna = hijos[1].Token.Location.Column + 1;
                    if (hijos.Count() == 5)
                        return new FuncionDef((Tipo)GenerarArbol(hijos[0]), hijos[1].Token.Text, (Bloque)GenerarArbol(hijos[4]), linea, columna);
                    else
                        return new FuncionDef((Tipo)GenerarArbol(hijos[0]), hijos[1].Token.Text, (LinkedList<Identificador>)GenerarArbol(hijos[3]), (Bloque)GenerarArbol(hijos[5]), linea, columna);
                case "PARAMETER_LIST":
                    if (hijos.Count() == 2)
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;

                        LinkedList<Identificador> parametro = new LinkedList<Identificador>();
                        Identificador id = new Identificador(hijos[1].Token.Text, linea, columna);
                        id.Tipo = (Tipo)GenerarArbol(hijos[0]);
                        parametro.AddLast(id);
                        return parametro;
                    }
                    else
                    {
                        linea = hijos[3].Token.Location.Line + 1;
                        columna = hijos[3].Token.Location.Column + 1;

                        LinkedList<Identificador> parametro = (LinkedList<Identificador>)GenerarArbol(hijos[0]);
                        Identificador id = new Identificador(hijos[3].Token.Text, linea, columna);
                        id.Tipo = (Tipo)GenerarArbol(hijos[2]);
                        parametro.AddLast(id);
                        return parametro;
                    }
                case "PROCDEF":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 10)
                        return new ProcedimientoDef(hijos[1].Token.Text, (LinkedList<Identificador>)GenerarArbol(hijos[3]), (LinkedList<Identificador>)GenerarArbol(hijos[7]), (Bloque)GenerarArbol(hijos[9]), linea, columna);
                    else if (hijos.Count() == 8)
                        return new ProcedimientoDef(hijos[1].Token.Text, null, null, (Bloque)GenerarArbol(hijos[7]), linea, columna);
                    else
                    {
                        if(hijos[4].Term.Name.Equals(","))
                            return new ProcedimientoDef(hijos[1].Token.Text, null, (LinkedList<Identificador>)GenerarArbol(hijos[6]), (Bloque)GenerarArbol(hijos[8]), linea, columna);
                        else
                            return new ProcedimientoDef(hijos[1].Token.Text, (LinkedList<Identificador>)GenerarArbol(hijos[3]), null, (Bloque)GenerarArbol(hijos[8]), linea, columna);
                    }
                case "BREAK_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Break(linea, columna);
                case "CONTINUE_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Continue(linea, columna);
                case "RETURN_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 1)
                        return new Return(linea, columna);
                    else
                        return new Return((LinkedList<Expresion>)GenerarArbol(hijos[1]), linea, columna);
                case "CURSOR_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new CursorDef(hijos[1].Token.Text, (Seleccionar)GenerarArbol(hijos[3]), linea, columna);
                case "FOREACH_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 7)
                        return new ForEach(null, hijos[5].Token.Text, (Bloque)GenerarArbol(hijos[6]), linea, columna);
                    return new ForEach((LinkedList<Identificador>)GenerarArbol(hijos[3]), hijos[6].Token.Text, (Bloque)GenerarArbol(hijos[7]), linea, columna);
                case "OPEN_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Open(hijos[1].Token.Text, linea, columna);
                case "CLOSE_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Close(hijos[1].Token.Text, linea, columna);
                case "LOG_STMT":
                    return new Log((Expresion)GenerarArbol(hijos[2]), hijos[0].Token.Location.Line + 1, hijos[0].Token.Location.Column + 1);
                case "THROW_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new Throw(hijos[2].Token.Text, linea, columna);
                case "TRYCATCH_STMT":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 6)
                        return new TryCatch((Bloque)GenerarArbol(hijos[1]), (Bloque)GenerarArbol(hijos[5]), linea, columna);
                    return new TryCatch((Bloque)GenerarArbol(hijos[1]), (LinkedList<Identificador>)GenerarArbol(hijos[4]), (Bloque)GenerarArbol(hijos[6]), linea, columna);
                case "EXPRESSION_LIST":
                    LinkedList<Expresion> exprList = new LinkedList<Expresion>();
                    foreach (ParseTreeNode hijo in hijos)
                    {
                        exprList.AddLast((Expresion)GenerarArbol(hijo));
                    }
                    return exprList;
                case "EXPRESSION":
                    return GenerarArbol(hijos[0]);
                case "INSTANCE":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 2)
                        return new Instancia(hijos[1].Token.Text, linea, columna);
                    else if (hijos.Count() == 5)
                        return new Instancia(hijos[1].Token.Text, (Tipo)GenerarArbol(hijos[3]), linea, columna);
                    else
                        return new Instancia(hijos[1].Token.Text, (Tipo)GenerarArbol(hijos[3]), (Tipo)GenerarArbol(hijos[5]), linea, columna);
                case "CONDITIONAL_EXPRESSION":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Ternario((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), (Expresion)GenerarArbol(hijos[4]), linea, columna);
                    }
                case "OR_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Logica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), Operador.OR, linea, columna);
                    }
                case "AND_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Logica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), Operador.AND, linea, columna);
                    }
                case "XOR_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Logica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), Operador.XOR, linea, columna);
                    }
                case "COMPARISON_EQ":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Relacional((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), GetOperador(hijos[1]), linea, columna);
                    }
                case "COMPARISON":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].ChildNodes.ToArray()[0].Token.Location.Line + 1;
                        columna = hijos[1].ChildNodes.ToArray()[0].Token.Location.Column + 1;
                        return new Relacional((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), (Operador)GenerarArbol(hijos[1]), linea, columna);
                    }
                case "COMP_OPERATOR":
                    return GetOperador(hijos[0]);
                case "A_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Aritmetica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), GetOperador(hijos[1]), linea, columna);
                    }
                case "M_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Aritmetica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), GetOperador(hijos[1]), linea, columna);
                    }
                case "U_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Unario((Expresion)GenerarArbol(hijos[1]), GetOperador(hijos[0]), linea, columna);
                    }
                case "NOT_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Logica((Expresion)GenerarArbol(hijos[1]), linea, columna);
                    }
                case "POWER":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Aritmetica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), GetOperador(hijos[1]), linea, columna);
                    }
                case "SHIFT_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Unario((Expresion)GenerarArbol(hijos[0]), GetOperador(hijos[1]), linea, columna);
                    }
                case "PRIMARY":
                    return GenerarArbol(hijos[0]);
                case "ATOM":
                    if (hijos[0].Term.Name.Equals("identifier"))
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Identificador(hijos[0].Token.Text, linea, columna);

                    }
                    else if (hijos[0].Term.Name.Equals("identifier2"))
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Identificador(hijos[0].Token.Text, true, linea, columna);
                    }
                    return GenerarArbol(hijos[0]);
                case "LITERAL":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos[0].Term.Name.Equals("number"))
                    {
                        try
                        {
                            int valor2 = Convert.ToInt32(hijos[0].Token.Text);
                            return new Literal(new Tipo(Type.INT), valor2, linea, columna);
                        }
                        catch (Exception)
                        {
                            double valor = Convert.ToDouble(hijos[0].Token.Text);
                            return new Literal(new Tipo(Type.DOUBLE), valor, linea, columna);
                        }
                    }
                    else if (hijos[0].Term.Name.Equals("stringliteral"))
                    {
                        return new Literal(new Tipo(Type.STRING), new Cadena(hijos[0].Token.Text), linea, columna);
                    }
                    else if (hijos[0].Term.Name.Equals("true"))
                    {
                        return new Literal(new Tipo(Type.BOOLEAN), true, linea, columna);
                    }
                    else if (hijos[0].Term.Name.Equals("false"))
                    {
                        return new Literal(new Tipo(Type.BOOLEAN), false, linea, columna);
                    }
                    else if (hijos[0].Term.Name.Equals("date"))
                    {
                        return new Literal(new Tipo(Type.DATE), new Date(hijos[0].Token.Text), linea, columna);
                    }
                    else if (hijos[0].Term.Name.Equals("time"))
                    {
                        return new Literal(new Tipo(Type.TIME), new Time(hijos[0].Token.Text), linea, columna);
                    }
                    else if (hijos[0].Term.Name.Equals("null"))
                    {
                        return new Literal(new Tipo(Type.NULL), new Null(), linea, columna);
                    }
                    return null;
                case "ATTRIBUTEREF":
                    linea = hijos[1].Token.Location.Line + 1;
                    columna = hijos[1].Token.Location.Column + 1;
                    if (hijos[2].Term.Name.Equals("identifier"))
                        return new AtributoRef((Expresion)GenerarArbol(hijos[0]), new Identificador(hijos[2].Token.Text, linea, columna), linea, columna);
                    else
                        return new AtributoRef((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), linea, columna);
                case "AGGREGATION":
                    linea = hijos[0].ChildNodes.ToArray()[0].Token.Location.Line + 1;
                    columna = hijos[0].ChildNodes.ToArray()[0].Token.Location.Column + 1;
                    return new Agregacion((Aggregation)GenerarArbol(hijos[0]), (Seleccionar)GenerarArbol(hijos[3]), linea, columna);
                case "AGGREGATION_FUN":
                    switch (hijos[0].Term.Name)
                    {
                        case "count":
                            return Aggregation.COUNT;
                        case "min":
                            return Aggregation.MIN;
                        case "max":
                            return Aggregation.MAX;
                        case "sum":
                            return Aggregation.SUM;
                        default:
                            return Aggregation.AVG;
                    }
                case "ENCLOSURE":
                    return GenerarArbol(hijos[0]);
                case "PARENTH_FORM":
                    if (hijos.Count() == 3)
                        return GenerarArbol(hijos[1]);
                    else
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Casteo((Tipo)GenerarArbol(hijos[1]), (Expresion)GenerarArbol(hijos[3]), linea, columna);
                    }
                case "MAP_DISPLAY":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                        return new MapDisplay((LinkedList<CollectionValue>)GenerarArbol(hijos[1]), linea, columna);
                case "MAP_LIST":
                    if (hijos.Count() == 3)
                    {
                        LinkedList<CollectionValue> collection = new LinkedList<CollectionValue>();
                        collection.AddLast(new CollectionValue(GenerarArbol(hijos[0]), GenerarArbol(hijos[2])));
                        return collection;
                    }
                    else
                    {
                        LinkedList<CollectionValue> collection = (LinkedList<CollectionValue>)GenerarArbol(hijos[0]);
                        collection.AddLast(new CollectionValue(GenerarArbol(hijos[2]), GenerarArbol(hijos[4])));
                        return collection;
                    }
                case "LIST_DISPLAY":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    return new ListDisplay((LinkedList<Expresion>)GenerarArbol(hijos[1]), linea, columna);
                case "SET_DISPLAY":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new SetDisplay((LinkedList<Expresion>)GenerarArbol(hijos[1]), linea, columna);
                    else
                        return new ObjetoDisplay(hijos[4].Token.Text,(LinkedList<Expresion>)GenerarArbol(hijos[1]), linea, columna);
                case "FUNCALL":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new FuncionCall(hijos[0].Token.Text, linea, columna);
                    else
                        return new FuncionCall(hijos[0].Token.Text, (LinkedList<Expresion>)GenerarArbol(hijos[2]), linea, columna);
                case "CALL":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 4)
                        return new Call(hijos[1].Token.Text, linea, columna);
                    return new Call(hijos[1].Token.Text, (LinkedList<Expresion>)GenerarArbol(hijos[3]), linea, columna);
                case "ACCESS":
                    linea = hijos[1].Token.Location.Line + 1;
                    columna = hijos[1].Token.Location.Column + 1;
                    return new Acceso((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), linea, columna);
            }

            return null;
        }

        private Operador GetOperador(ParseTreeNode raiz)
        {
            switch (raiz.Token.Text)
            {
                case "+":
                    return Operador.SUMA;
                case "-":
                    return Operador.RESTA;
                case "*":
                    return Operador.MULTIPLICACION;
                case "**":
                    return Operador.POTENCIA;
                case "%":
                    return Operador.MODULO;
                case "/":
                    return Operador.DIVISION;
                case "++":
                    return Operador.AUMENTO;
                case "--":
                    return Operador.DECREMENTO;
                case "<":
                    return Operador.MENORQUE;
                case ">":
                    return Operador.MAYORQUE;
                case "==":
                    return Operador.IGUAL;
                case ">=":
                    return Operador.MAYORIGUAL;
                case "<=":
                    return Operador.MENORIGUAL;
                case "!=":
                    return Operador.DIFERENTE;
            }
            return Operador.INDEFINIDO;
        }

        public Operador GetAugOperador(ParseTreeNode raiz)
        {
            switch (raiz.Token.Text)
            {
                case "+=":
                    return Operador.SUMA;
                case "-=":
                    return Operador.RESTA;
                case "*=":
                    return Operador.MULTIPLICACION;
                case "/=":
                    return Operador.DIVISION;
            }
            return Operador.INDEFINIDO;
        }
    }
}
