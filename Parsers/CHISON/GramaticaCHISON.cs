using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramaticasCQL.Parsers.CHISON
{
    class GramaticaCHISON : Grammar
    {
        public GramaticaCHISON() : base(false)
        {
            CommentTerminal blockComment = new CommentTerminal("block-comment", "/*", "*/");
            CommentTerminal lineComment = new CommentTerminal("line-comment", "//",
                "\r", "\n", "\u2085", "\u2028", "\u2029");

            NonGrammarTerminals.Add(blockComment);
            NonGrammarTerminals.Add(lineComment);

            /* Reserved Words */
            KeyTerm
                null_ = ToTerm("null"),
                true_ = ToTerm("true"),
                false_ = ToTerm("false"),
                in_ = ToTerm("in"),
                out_ = ToTerm("out");

            MarkReservedWords("null", "true", "false", "in", "out");

            /* Symbols*/
            KeyTerm
                equal = ToTerm("="),
                menorque = ToTerm("<"),
                mayorque = ToTerm(">"),
                leftCor = ToTerm("["),
                rightCor = ToTerm("]"),
                dollar = ToTerm("$"),
                comma = ToTerm(",");

            var number = new NumberLiteral("number");
            var stringliteral = new StringLiteral("stringliteral", "\"", StringOptions.IsTemplate);
            var stringcodigo = new StringLiteral("stringcodigo", "$", StringOptions.IsTemplate);
            RegexBasedTerminal date = new RegexBasedTerminal("date", "'[0-2][0-9]{3}-([0][0-9]|[1][0-2])-([0][0-9]|[1][0-9]|[2][0-9]|[3][0-1])'");
            RegexBasedTerminal time = new RegexBasedTerminal("time", "'([0][0-9]|[1][0-9]|[2][0-4]):[0-5][0-9]:[0-5][0-9]'");
            //IdentifierTerminal fileName = new IdentifierTerminal("fileName", "!@#$%^*_'.?-", "!@#$%^*_'.?0123456789");


            NonTerminal
                INICIO = new NonTerminal("INICIO"),
                ARCHIVO = new NonTerminal("ARCHIVO"),
                INSTRUCCIONES = new NonTerminal("INSTRUCCIONES"),
                INSTRUCCION = new NonTerminal("INSTRUCCION"),
                BLOQUE = new NonTerminal("BLOQUE"),
                VALOR = new NonTerminal("VALOR"),
                LISTA = new NonTerminal("LISTA"),
                VALORES = new NonTerminal("VALORES");
            this.Root = INICIO;

            INICIO.Rule = ARCHIVO;

            ARCHIVO.Rule = dollar + BLOQUE + dollar;

            BLOQUE.Rule = menorque + INSTRUCCIONES + mayorque;

            INSTRUCCIONES.Rule = MakePlusRule(INSTRUCCIONES, comma, INSTRUCCION);

            INSTRUCCION.Rule = stringliteral + equal + VALOR;

            VALOR.Rule = number | stringliteral | true_ | false_ | date | time | in_ | out_ | null_ 
                        | LISTA | BLOQUE | stringcodigo;

            LISTA.Rule = leftCor + VALORES + rightCor;

            VALORES.Rule = MakeStarRule(VALORES, comma, VALOR);

        }
    }
}
