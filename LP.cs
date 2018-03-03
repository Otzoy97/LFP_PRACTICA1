using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualBasic;
using LFP_P1.IO;
using LFP_P1.Listas;
using LFP_P1.ADF;
using System.IO;

namespace WindowsFormsApp1
{
	public partial class Main : Form
	{
		private String iniciales = "SFOG<<";
		ListaSimple pdf, fila, token, err;
		Cola texto, imagen, tabla;
		/// <summary>
		/// Mantiene el control del PDF enfocado
		/// </summary>
		private String focusedPDF;
		/// <summary>
		/// Mantiene el control de cuantas veces se a editado el focusedPDF
		/// </summary>
		private int indiceEdicion = 1;
		/// <summary>
		/// Incializa todo el formulario
		/// </summary>
		public Main()
		{
			InitializeComponent();
			TxtArea.Text = "  PRIMERA PRACTICA DE LABORATORIO" + Environment.NewLine + "© LP-CLOUD [Lenguajes Formales y de Programación] 1S 2018" + Environment.NewLine;
			pdf = new ListaSimple("PDF");
			fila = new ListaSimple("Fila");
			token = new ListaSimple("Token");
			texto = new Cola("Texto");
			imagen = new Cola("Imagen");
			tabla = new Cola("Tabla");
			err = new ListaSimple("Error");
			///Agrega SFOG<< y coloca el puntero al final
			TxtArea.Text += Environment.NewLine + iniciales;
			TxtArea.Select(TxtArea.Text.Length, 0);
		}
		/// <summary>
		/// Controla la acción de las teclas Enter, Backspace y Tab
		/// Enter -> Añade un SFOG>>> y elimina el último > , esto ayuda a colocar el cursor al final de la cadena y que se muestre de la forma SFOG>>
		/// Backspace -> Verifica que la última línea se mayor a 6 caracteres (de la cadena SFOG>>) quita 1 caracter y coloca al final de la cadena el cursor
		/// Tab -> Evita la acción de Tabular
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxtArea_KeyPress(object sender, KeyPressEventArgs e)
		{
			String[] lineaCodigo = TxtArea.Lines;
			int rowsCodigo = lineaCodigo.Length;
			int lenCodigo = lineaCodigo[rowsCodigo - 1].Length;
			//Enter
			if (e.KeyChar == Convert.ToChar(Keys.Enter))
			{
				e.Handled = true;
				if (lenCodigo > 6)
				{
					//Obtiene la cadena a Escanear
					String cadenaEscanear = lineaCodigo[rowsCodigo - 1].Substring(iniciales.Length);
					//Manda al analizador toda esta onda
					ListaSimple analizador = new Lenguaje().AnalizarCadena(cadenaEscanear);
					//Guarda la cadena 
					Cola AEjecutar = GuardarTokens(analizador);
					//Determina si la cadena se ejecuta o no
					if (Escanear(analizador))
					{
						//Ejecuta la cadena
						EjecutarToken(AEjecutar);
					}
				}
				NuevaLinea();
			}
			//Backspace
			else if (e.KeyChar == Convert.ToChar(Keys.Back))
			{
				e.Handled = true;
				//Coloca el cursor al final del txtbox
				if (lenCodigo > iniciales.Length)
				{
					//Elimina un caracter del final de la última linea
					TxtArea.Text = TxtArea.Text.Substring(0,TxtArea.Text.Length - 1);
				}
			}
			//Tabular
			else if (e.KeyChar == Convert.ToChar(Keys.Tab))
			{
				e.Handled = true;
			}
			//Coloca el cursor al final del txtbox
			TxtArea.Focus();
			TxtArea.Select(TxtArea.Text.Length, 0);
			TxtArea.ScrollToCaret();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Obtiene el nombre del archivo
			string input = Interaction.InputBox("Guardar...", "Escriba el nombre del archivo", "");
			if (!string.IsNullOrEmpty(input) && !input.Equals(""))
			{
				new Ruta().GenerarYAbrirFile(TxtEditor.Text, input, ".if", false);
				TxtArea.Text += Environment.NewLine + "Archivo guardado en raiz/"+ input+".if";
				NuevaLinea();
			}
		}
		/// <summary>
		/// Evita el uso de Keys.Delete y da un uso especifico a Keys.Up y Keys.Down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxtArea_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Delete)
			{
				e.Handled = true;
			}
		}
		/// <summary>
		/// Sale de la aplicación
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Dispose();
			Close();
		}
		/// <summary>
		/// Muestra un dialogo para buscar y abrir un archivo y mostrar el contenido en el cuadro blanco.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AbrirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Crea una instancia de un cuadro de dialogo para abri archivos
			OpenFileDialog dialog1 = new OpenFileDialog
			{
				//Establece el directorio inicial
				InitialDirectory = "C:\\Users\\otzoy\\Desktop",
				//Se crea y agrega un filtro para los archivos
				Filter = "Archivos de Prueba (*.if) |*.if",
				FilterIndex = 0,
				//Deja el directorio en el directorio en el que se cerro la 
				//última vez que se abrio el cuadro de dialogo durante 
				//el tiempo de ejecuacion
				RestoreDirectory = true,
				//Evita que se seleccione mas de un archivo
				Multiselect = false
			};
			//Hace visible el dialogo
			//Si se presiona OK se ejecuta lo del if
			if (dialog1.ShowDialog() == DialogResult.OK)
			{
				//Llena el cuadro de texto con lo que se encuentra en el archivo *.if
				TxtEditor.Text = new Ruta().GetTextFromFile(dialog1.FileName);
			}
		}
		/// <summary>
		/// Determina si se ejecuta o no una sentencia Analizada.
		/// Si en la lista se encuentra un Error entonces no
		/// </summary>
		/// <param name="lista"></param>
		/// <returns></returns>
		private bool Escanear(ListaSimple lista)
		{
			//Realiza una copia de la lista
			Nodo aux = lista.inicio;
			//Recorre la lista siempre que existan nodos válidos
			while (aux!=null)
			{
				//Castea el objeto a Token
				Token temp = (Token) aux.objeto;
				//Verifica que no existan Tokens con error
				if (temp.token.Equals("Error"))
				{
					//Informa al usuario de cual es el error
					//No se reconoce <lexema>. [Lín ##, Col ##], 
					TxtArea.Text += "Error en <"+temp.lexema+">.  [Lín " + temp.fila + ", Col "+ temp.columna +"]" ;
					//Si existe un token error devuelve false
					return false;
				}
				else
				{
					//Si no pasa al siguiente nodo
					aux = aux.siguiente;
				}
			}
			//El loop terminó y no existe error la sentencia es válida léxicamente
			return true;
		}
		/// <summary>
		/// Guarda la sentencia Analizada en la lista de Tokens General
		/// </summary>
		/// <param name="lista"></param>
		/// <returns></returns>
		private Cola GuardarTokens(ListaSimple lista)
		{
			//Declara la Cola de salida
			Cola instrucciones = new Cola("Instrucciones");
			//El loop verificará que no este vacia la lista
			//Sacará de la lista los tokens y los guardará en la lista general
			while (!lista.EsVacia())
			{
				//Saca el primer dato de la lista y lo caste a Token
				Token temp = (Token) lista.Listar();
				//Guarda la instruccion a realizar
				instrucciones.Encolar(temp.lexema);
				//Guarda el token temp en la lista general, con diferente id
				//Verifica si el token es un error y lo guarda en otra lista
				if (temp.token.Equals("Error"))
				{
					err.Enlistar(new Error(err.GetLargo()+1,temp.lexema,temp.fila,temp.columna, "Error léxico"));
				} else
				{
					token.Enlistar(new Token(token.GetLargo() + 1, temp.lexema, temp.fila, temp.columna, temp.token));
				}
			}
			return instrucciones;
		}
		/// <summary>
		/// Consulta los Tokens de la cola recibida y determina la lista en donde guardar los argumentos
		/// </summary>
		/// <param name="instrucciones"></param>
		private void EjecutarToken(Cola instrucciones)
		{
			//Convierte en un array de String la Cola recibida para facilitar su acceso
			string[] instruccion = instrucciones.VolverArray();// = (string)instrucciones.Desencolar();
			//A través de la instrucción del primer espacio determina que hacer con lo contenido en el resto del Array
			switch (instruccion[0])
			{
				//1
				case "iniciarA":
					if (string.IsNullOrEmpty(focusedPDF) || focusedPDF.Equals(""))//Verifica que no haya ya un PDF iniciado
					{
						//Intentará instanciar un PDF
						try
						{
							//Verifica que no existan nombres duplicados
							if (!pdf.Buscar(pdf.BuscarPDF(instruccion[1])))
							{
								//Crea la instancia de PDF
								pdf.Enlistar(new PDF(pdf.GetLargo() + 1, instruccion[1]));
								//Informa al usuario de la creacion de la instancia PDF
								TxtArea.Text += Environment.NewLine + "Instancia " + instruccion[1] + " creada";
							}
							else
							{
								//Si existe duplicado se informa al usuario
								TxtArea.Text += Environment.NewLine + "El PDF " + instruccion[1] + " ya existe. Consulte el Manual de Usuario <mostrar> <ManualUsuario>";
								//Agrega error a la lista de errores
								err.Enlistar(new Error(err.GetLargo() + 1, instruccion[1], -1, instruccion[0].Length + 2, "Nombre <PDF> Duplicado"));
							}
						}
						catch (IndexOutOfRangeException)
						{
							//Informa al error de un error (generico) sintáctico
							TxtArea.Text += Environment.NewLine + "Se esperaba Nombre";
							//No hay argumentos después de iniciar
							err.Enlistar(new Error(err.GetLargo() + 1, null, -1, instruccion[0].Length + 2, "Se esperaba Nombre"));
						}
					}
					else
					{
						//Si ya un PDF ingresado se informa al usuario
						TxtArea.Text += Environment.NewLine + "Instrucción fuera de entorno. Consulte el Manual de Usuario <mostrar> <ManualUsuario>";
						//Agrega el error a la lista
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[0], -1, 1, "Error de Entorno. Hay un PDF enfocado"));
					}
					break;
				//2
				case "ingresarAPdf":
					//Busca el PDF en la lista de PDF, si existe focusedPDF = instruccion[1]. Sino informa al usuario
					if (pdf.Buscar(pdf.BuscarPDF(instruccion[1])))
					{
						//Pone en global el nombre del pdf
						focusedPDF = instruccion[1];
						//Informa al usuario
						TxtArea.Text += Environment.NewLine + "Instancia " + instruccion[1] + " enfocada";
					}
					else
					{
						//Informa al usuario de que no se pudo encontrar el PDF en cuestión
						TxtArea.Text += Environment.NewLine + "No se pudo encontrar el PDF " + instruccion[1] + ". Intente iniciarA " + instruccion[1] + " y pruebe de nuevo";
						//Agrega al error a la lista
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[1], -1, instruccion[0].Length + 2, "PDF no se encontró"));
					}
					break;
				//3
				case "crear":
					//Determina el largo del String[], si == 3 entonces solo crea, si == 4 abre de una vez el PDF
					if (pdf.Buscar(pdf.BuscarPDF(focusedPDF)))
					{
						//Obtiene la instancia de PDF
						LFP_P1.IO.PDF aux = pdf.BuscarPDF(focusedPDF);
						//Genera el PDF
						TxtArea.Text += Environment.NewLine + aux.GenerarPDF(new Ruta().GetAbsolutePath(instruccion[2]), this.indiceEdicion, this.texto, this.imagen, this.tabla, this.fila);
						//Si Abrir esta presente lo abre
						if (instruccion.Length == 4 && instruccion[3].Equals("Abrir"))
						{
							TxtArea.Text += Environment.NewLine + aux.AbrirPDF(new Ruta().GetAbsolutePath(instruccion[2]) + (char) 92 + aux.nombre + ".pdf");
						}
						//Elimina el PDF de la lista
						pdf.Remover(aux);
						//Desenfoca el PDF
						focusedPDF = null;
						//Reinicia el indice
						this.indiceEdicion = 1;
					}
					else
					{
						//Sino informa al usuario
						TxtArea.Text += Environment.NewLine + "PDF sin enfocar " + instruccion[1] + ". Intente ingresarAPdf " + instruccion[1] + " y pruebe de nuevo";
						//Agrega el error a la lista
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[1], -1, instruccion[0].Length + 2, "Error de entorno. PDF sin enfocar"));
					}
					break;
				//4
				case "escribirTexto":
					//Determina si el entorno es correcto, buscando el PDF enfocado actualmente
					if (string.IsNullOrEmpty(focusedPDF) || focusedPDF.Equals(""))
					{
						//Informa al usuario del error y el sistema agrega a la lista el error
						TxtArea.Text +=Environment.NewLine + "Instrucción fuera de entorno. No ha ingresado a PDF, consulte el Manual de Usuario";
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[0], -1, 1, "Error de Entorno. No hay PDF enfocado"));
					}
					else
					{
						//Determina si se subraya o no
						if (instruccion.Length < 4 )
						{
							//Añade un texto subrayado
							texto.Encolar(new Texto(instruccion[1], instruccion[2][0], (char)48, this.indiceEdicion++));
						}
						else
						{
							//Añade un texto sin subrayar
							texto.Encolar(new Texto(instruccion[1], instruccion[2][0], instruccion[3][0], this.indiceEdicion++));
						}
						//Informa al usuario
						TxtArea.Text += Environment.NewLine + "Se escribió texto";
					}
					break;
				//5
				case "imagen":
					if (string.IsNullOrEmpty(focusedPDF) || focusedPDF.Equals(""))
					{
						TxtArea.Text += Environment.NewLine + "Instrucción fuera de entorno. No ha ingresado a PDF, consulte el Manual de Usuario";
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[0], -1, 1, "Error de Entorno. No hay PDF enfocado"));
					}
					else
					{
						//Determina la existencia del archivo de imagen
						if (File.Exists(new Ruta().GetAbsolutePath(instruccion[1])))
						{
							this.imagen.Encolar(new Imagen(new Ruta().GetAbsolutePath(instruccion[1]), instruccion[2], indiceEdicion++));
							TxtArea.Text += Environment.NewLine + "Imagen agregada correctamente";
						}
						else
						{
							TxtArea.Text += Environment.NewLine + "No se pudo encontrar la imagen en la ruta especificada.";
							err.Enlistar(new Error(err.GetLargo() + 1, instruccion[1], -1,instruccion[0].Length+2, "Archivo no Encontrado. Ruta inválida"));
						}
					}
					break;
				//6
				case "crearTabla":
					if (string.IsNullOrEmpty(focusedPDF) || focusedPDF.Equals(""))
					{
						//Informa al usuario que no hay ningun PDF enfocado y lista el error
						TxtArea.Text += Environment.NewLine + "Instrucción fuera de entorno. No ha ingresado a PDF, consulte el Manual de Usuario";
						err.Enlistar(new Error(err.GetLargo()+1,instruccion[0],-1,0,"Error de Entorno. No hay PDF enfocado."));
					}
					else
					{
						//Añade la tabla a la cola e informa al usuario
						this.tabla.Encolar(new Tabla(instruccion[1], instruccion[2], indiceEdicion++));
						TxtArea.Text += Environment.NewLine + "Tabla agregada correctamente";
					}
					break;
				//7
				case "crearFila":
					if (string.IsNullOrEmpty(focusedPDF) || focusedPDF.Equals(""))
					{
						//Informa al usuario que no hay ningun PDF enfocado y enlista el error
						TxtArea.Text += Environment.NewLine + "Instrucción fuera de entorno. No ha ingresado a PDF. Consulte el Manual de Usuario <mostrar> <ManualUsuario>";
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[0], -1, 0, "Error de Entorno. No hay PDF enfocado."));
					}
					else
					{
						//Agrega una nueva fila a la lista e informa al usuario
						this.fila.Enlistar(new Fila(instruccion[1], instruccion[2]));
						TxtArea.Text += Environment.NewLine + "Fila agregada a " + instruccion[2];
					}
					break;
				//8
				case "ejecutar":
					if (string.IsNullOrEmpty(focusedPDF) || focusedPDF.Equals(""))
					{
						//Verifica si hay más de una instrucción ejecutar, es decir si se encuentra un Ruta
						if (instruccion.Length > 1)
						{
							//Verifica que el archivo exista y que sea .If
							if (File.Exists(new Ruta().GetAbsolutePath(instruccion[1])) && (instruccion[1].Substring(instruccion[1].Length - 3, 3).Equals(".If") || instruccion[1].Substring(instruccion[1].Length - 3, 3).Equals(".if")))
							{
								//Limpia el editor y agrega el texto obtenido del archivo
								this.TxtEditor.Text = new Ruta().GetTextFromFile(new Ruta().GetAbsolutePath(instruccion[1]) );
							}
							else
							{
								//No se encuentra el archivo y se informa al usuario
								TxtArea.Text += Environment.NewLine + "El archivo no se encuentra en la ruta especificada o la extensión es incorrecta";
								err.Enlistar(new Error(err.GetLargo() + 1, instruccion[1], -1, instruccion[0].Length + 2, "Archivo no Encontrado. Ruta inválida"));
							}
						}
						//lineaCodigo guarda todo lo que contiene el txt en un array , separados por filas
						String[] lineaCodigo = TxtEditor.Lines;
						//Obtiene el número de filas que tiene el array lineaCodigo
						int rowsCodigo = lineaCodigo.Length;
						//Instancia un Lenguaje de esta forma se mantiene las filas y columnas
						Lenguaje leng = new Lenguaje();
						for (int i = 0; i < rowsCodigo; i++)
						{
							//Obtiene la cadena a Escanear
							String cadenaEscanear = lineaCodigo[i];
							//Verifica que la cadena no esté vacía y evita crear un loop
							if ( !string.IsNullOrEmpty(cadenaEscanear) && !cadenaEscanear.Equals("") && !cadenaEscanear.Contains("ejecutar") )
							{
								//Manda al analizador toda esta onda
								ListaSimple analizador = leng.AnalizarCadena(cadenaEscanear+(char)10);
								//Guarda la cadena 
								Cola AEjecutar = GuardarTokens(analizador);
								//Determina si la cadena se ejecuta o no
								if (Escanear(analizador))
								{
									//Ejecuta la cadena
									EjecutarToken(AEjecutar);
								}
							}
						}
					}
					else
					{
						//Informa a usuario que no hay ningun PDF enfocado y lo agrega a la lista
						TxtArea.Text += Environment.NewLine + "Instrucción fuera de entorno. Consulte el Manual de Usuario <mostrar> <ManualUsuario>";
						err.Enlistar(new Error(err.GetLargo() + 1, instruccion[0], -1, 0, "Error de entorno. Hay un PDF enfocado"));
					}
					break;
				//9
				case "mostrar":
					if (instruccion[1].Equals("ManualUsuario"))
					{
						new PDF().AbrirPDF(new Ruta().GetAbsolutePath("raiz/ManualUsuario.pdf"));
					}
					if (instruccion[1].Equals("ManualTecnico"))
					{
						new PDF().AbrirPDF(new Ruta().GetAbsolutePath("raiz/ManualTecnico.pdf"));
					}
					break;
				//10
				case "reporteTokens":
					//Crea y despliega el pvto >:V HTML xdDXdX
					new Ruta().GenerarYAbrirFile(new Ruta().ReporteTokens(token,err),"ReporteTokens",".html",true);
					break;
				//11
				case "acercaDe":
					MessageBox.Show("201602782 - Sergio Fernando Otzoy Gonzalez","Lenguajes Formales y de Programación");
					break;
				default:
					TxtArea.Text += Environment.NewLine + "«"+ instruccion[0]+"» no se reconoce como instruccion. Consulte los Manuales -> <mostrar> <ManualUsuario> ó <ManualTecnico>";
					break;
			}
			TxtArea.Text += Environment.NewLine;
		}
		/// <summary>
		/// Agrega el inicio de linea SFOG<<
		/// </summary>
		public void NuevaLinea()
		{
			//Agrega las líneas inicials
			TxtArea.Text += Environment.NewLine+ "SFOG<<<";
			//No estoy seguro que hace esta parte, elimina el último caracter presente
			TxtArea.Text = TxtArea.Text.Substring(0, TxtArea.Text.Length - 1);
		}
	}
}
