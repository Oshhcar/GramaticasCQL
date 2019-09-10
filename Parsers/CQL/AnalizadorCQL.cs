﻿using GramaticasCQL.Parsers.CQL.ast;
using GramaticasCQL.Parsers.CQL.ast.entorno;
using GramaticasCQL.Parsers.CQL.ast.expresion;
using GramaticasCQL.Parsers.CQL.ast.expresion.operacion;
using GramaticasCQL.Parsers.CQL.ast.instruccion;
using GramaticasCQL.Parsers.CQL.ast.instruccion.ciclos;
using GramaticasCQL.Parsers.CQL.ast.instruccion.condicionales;
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
                            return new Tipo(hijos[0].Token.Text);
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
                            return new Tipo(hijos[0].Token.Text);
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
                    if (hijos[0].Term.Name.Equals("identifier") || hijos[0].Term.Name.Equals("identifier2"))
                    {
                        return new Identificador(hijos[0].Token.Text, hijos[0].Token.Location.Line + 1, hijos[0].Token.Location.Column + 1);
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
                    return new Asignacion((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), linea, columna);

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

                case "LOG_STMT":
                    return new Log((Expresion)GenerarArbol(hijos[2]), hijos[0].Token.Location.Line + 1, hijos[0].Token.Location.Column + 1);

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
                case "XOR_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Logica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), Operador.XOR, linea, columna);
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
                case "NOT_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Logica((Expresion)GenerarArbol(hijos[1]), linea, columna);
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
                case "SHIFT_EXPR":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Unario((Expresion)GenerarArbol(hijos[0]), GetOperador(hijos[1]), linea, columna);
                    }
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
                case "POWER":
                    if (hijos.Count() == 1)
                        return GenerarArbol(hijos[0]);
                    else
                    {
                        linea = hijos[1].Token.Location.Line + 1;
                        columna = hijos[1].Token.Location.Column + 1;
                        return new Aritmetica((Expresion)GenerarArbol(hijos[0]), (Expresion)GenerarArbol(hijos[2]), GetOperador(hijos[1]), linea, columna);
                    }
                case "PRIMARY":
                    return GenerarArbol(hijos[0]);
                case "ATOM":
                    if (hijos[0].Term.Name.Equals("identifier2") || hijos[0].Term.Name.Equals("identifier"))
                    {
                        linea = hijos[0].Token.Location.Line + 1;
                        columna = hijos[0].Token.Location.Column + 1;
                        return new Identificador(hijos[0].Token.Text, linea, columna);

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
                    return new SetDisplay((LinkedList<Expresion>)GenerarArbol(hijos[1]), linea, columna);
                case "FUNCALL":
                    linea = hijos[0].Token.Location.Line + 1;
                    columna = hijos[0].Token.Location.Column + 1;
                    if (hijos.Count() == 3)
                        return new FuncionCall(hijos[0].Token.Text, linea, columna);
                    else
                        return new FuncionCall(hijos[0].Token.Text, (LinkedList<Expresion>)GenerarArbol(hijos[2]), linea, columna);

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
