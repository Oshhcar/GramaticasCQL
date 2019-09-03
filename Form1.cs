using GramaticasCQL.Parsers.CHISON;
using GramaticasCQL.Parsers.CQL;
using GramaticasCQL.Parsers.LUP;
using Irony;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramaticasCQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (!txtSource.Text.Equals(String.Empty))
            {
                AnalizadorCQL analizador = new AnalizadorCQL();

                if (analizador.AnalizarEntrada(txtSource.Text))
                {
                    MessageBox.Show("Documento ok.", "Mensaje");
                    ReporteErrores(analizador.Raiz);
                }
                else
                {
                    MessageBox.Show("El archivo contiene errores.", "Error");
                    tabBottom.SelectedTab = tabPage3;
                    ReporteErrores(analizador.Raiz);
                }
            }
        }

        private void ReporteErrores(ParseTree raiz)
        {
            gridErrors.Rows.Clear();
            for (int i = 0; i < raiz.ParserMessages.Count(); i++)
            {
                LogMessage m = raiz.ParserMessages.ElementAt(i);
                if (m.Message.ToString().Contains("character"))
                    gridErrors.Rows.Add("Léxico", m.Message.Replace("Invalid character", "Carácter inválido"), (m.Location.Line + 1));
                else
                    gridErrors.Rows.Add("Sintáctico", m.Message.Replace("Syntax error, expected:", "Error de sintáxis, se esperaba:"), (m.Location.Line + 1));

            }
        }

        private void BtnParse_Click(object sender, EventArgs e)
        {
            if (!txtSource.Text.Equals(String.Empty))
            {
                AnalizadorCQL analizador = new AnalizadorCQL();

                if (analizador.AnalizarEntrada(txtSource.Text))
                {
                    MessageBox.Show("Documento ok.", "Mensaje");
                    ReporteErrores(analizador.Raiz);
                    GraficarArbol(analizador.Raiz.Root);
                }
                else
                {
                    MessageBox.Show("El archivo contiene errores.", "Error");
                    tabBottom.SelectedTab = tabPage3;
                    ReporteErrores(analizador.Raiz);
                }
            }
        }

        public void GraficarArbol(ParseTreeNode raiz)
        {
            string archivo = "arbol.dot";

            StreamWriter writer = null;

            try
            {
                //FileStream stream = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Write);
                //StreamWriter writer = new StreamWriter(stream);
                writer = new StreamWriter(archivo);

                string grafica = "digraph arbol{\n";
                grafica += "rankdir=UD;\n";
                grafica += "node [shape = box, style=filled, color=blanchedalmond];\n";
                grafica += "edge[color=chocolate3];\n";
                grafica += GraficarNodo(raiz) + "\n";
                grafica += "}\n";

                writer.Write(grafica);
                writer.Close();

                var command = string.Format("dot -Tpng arbol.dot  -o arbol.png");
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();

                //var appname = "Microsoft.Photos.exe";
                // Process.Start(appname, "arbol.jpg");
                if (proc.ExitCode == 1)
                {
                    MessageBox.Show("Error al graficar", "Graphviz");
                }
                else
                {
                    Process.Start("arbol.png");
                }

            }
            catch (Exception x)
            {
                MessageBox.Show("Error en graficar! \n" + x);
            }
        }

        public string GraficarNodo(ParseTreeNode raiz)
        {
            string nodoString = "";
            //MessageBox.Show("label = "+raiz.ToString());
            string label = raiz.ToString().Replace("\"", "\\\""); //caracteres especiales

            nodoString = "nodo" + raiz.GetHashCode() + "[label=\"" + label + " \", fillcolor=\"blanchedalmond\", style =\"filled\", shape=\"box\"]; \n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    nodoString += GraficarNodo(hijos[i]);
                    nodoString += "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \n";
                }
            }

            return nodoString;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!fastColoredTextBox2.Text.Equals(String.Empty))
            {
                AnalizadorLUP analizador = new AnalizadorLUP();

                if (analizador.AnalizarEntrada(fastColoredTextBox2.Text))
                {
                    MessageBox.Show("Documento ok.", "Mensaje");
                    ReporteErrores(analizador.Raiz);
                }
                else
                {
                    MessageBox.Show("El archivo contiene errores.", "Error");
                    tabBottom.SelectedTab = tabPage3;
                    ReporteErrores(analizador.Raiz);
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!fastColoredTextBox2.Text.Equals(String.Empty))
            {
                AnalizadorLUP analizador = new AnalizadorLUP();

                if (analizador.AnalizarEntrada(fastColoredTextBox2.Text))
                {
                    MessageBox.Show("Documento ok.", "Mensaje");
                    ReporteErrores(analizador.Raiz);
                    GraficarArbol(analizador.Raiz.Root);
                }
                else
                {
                    MessageBox.Show("El archivo contiene errores.", "Error");
                    tabBottom.SelectedTab = tabPage3;
                    ReporteErrores(analizador.Raiz);
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (!fastColoredTextBox3.Text.Equals(String.Empty))
            {
                AnalizadorCHISON analizador = new AnalizadorCHISON();

                if (analizador.AnalizarEntrada(fastColoredTextBox3.Text))
                {
                    MessageBox.Show("Documento ok.", "Mensaje");
                    ReporteErrores(analizador.Raiz);
                }
                else
                {
                    MessageBox.Show("El archivo contiene errores.", "Error");
                    tabBottom.SelectedTab = tabPage3;
                    ReporteErrores(analizador.Raiz);
                }
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (!fastColoredTextBox3.Text.Equals(String.Empty))
            {
                AnalizadorCHISON analizador = new AnalizadorCHISON();

                if (analizador.AnalizarEntrada(fastColoredTextBox3.Text))
                {
                    MessageBox.Show("Documento ok.", "Mensaje");
                    ReporteErrores(analizador.Raiz);
                    GraficarArbol(analizador.Raiz.Root);
                }
                else
                {
                    MessageBox.Show("El archivo contiene errores.", "Error");
                    tabBottom.SelectedTab = tabPage3;
                    ReporteErrores(analizador.Raiz);
                }
            }
        }
    }
}
