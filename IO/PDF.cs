using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using LFP_P1.Listas;

namespace LFP_P1.IO
{
	class PDF
	{
		//Nombre del PDF
		public String nombre;
		//Orden de PDF
		public int idPDF;
		//Indicador de que tiene imagen
		public bool imagen = false;
		//Indicador de que tiene tabla
		public bool tabla = false;
		/// <summary>
		/// Bob el Constructor :v
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		public PDF(int id, String name)
		{
			this.idPDF = id;
			this.nombre = name;
		}
		/// <summary>
		/// Toma todas las colas y listas de la edición, y conforme el indiceEdicion, va editando el PDF
		/// de lo que contengan las colas y listas
		/// Algoritmo:
		///		Cada una de los objeto alojados en las colas y listas, tiene un índice
		///		ese indice va incrementando segun se vaya editado el PDF en general, 
		///		A la larga puede que los indices queden así:
		///		-> Texto {1,4,8,9}
		///		-> Imagen {2,3,5}
		///		-> Tabla {6,7}
		///		El ciclo for que contiene el metodo va recorriendo el indice de cada objeto alojado 
		///		en las listas/colas y las va agregando al PDF, en el orden en el que fue editado originalmente el PDF
		///		a través de comandos
		///		Conforme se vayan agregando las colas/listas, estas se van vaciando
		/// </summary>
		/// <param name="url"></param>
		/// <param name="indiceEdicion"></param>
		/// <param name="Textos"></param>
		/// <param name="Imagenes"></param>
		/// <param name="Tablas"></param>
		/// <param name="Filas"></param>
		/// <returns>Respuesta del sistema</returns>
		public String GenerarPDF(String url, int indiceEdicion ,Cola Textos, Cola Imagenes, Cola Tablas, ListaSimple Filas )
		{
			//Se guardará la respuesta del sistema
			String respuesta = "";
			//Aquí se guardarán las intancias que ayudaran en la creación del PDF
			Document doc = null;
			FileStream fs = null;
			PdfWriter writer = null;
			//Edita el PDF en un entorno a prueba de idiotas :v
			try
			{
				//Crea el documento pdf (lo instancia)
				doc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
				//Prepara el documento para su escritura
				fs = new FileStream(url+"\\"+nombre+".pdf", FileMode.Create, FileAccess.Write, FileShare.None);
				//Determina la localización del PDF dentro de
				writer = PdfWriter.GetInstance(doc, fs);
				if (!Textos.EsVacia() || !Imagenes.EsVacia() || !Tablas.EsVacia())
				{
					//Abre el documento para editarlo
					doc.Open();
				}
				for (int i = 1; i <= indiceEdicion; i++)
				{
					//Verifica si la cola está vacía
					if (!Textos.EsVacia())
						{
							//Castea a Texto
							Texto text = (Texto)Textos.ShutearPrimero();
							//Verifica que el indice guardado coincida con la <i>
							if (text.indice == i)
							{
								//Agrega texto al PDF
								doc.Add(text.texto);
								//Elimna información de la lista
								Textos.Desencolar();
							}
						}
					//Verifica si la cola está vacía
					if (!Imagenes.EsVacia())
						{
							//Esta instancia ayudara a agregar imagenes si las hubiera
							iTextSharp.text.Image jpg;
							//Caste a Imagen
							Imagen image = (Imagen)Imagenes.ShutearPrimero();
							//Verifica que el indice guardado coincida con la <i>
							if (image.indice == i)
							{
								//Aloja la referencia guardada
								jpg = image.imagen;
								//Encaja la imagen en el documento
								jpg.ScaleToFit(400f, 300f);
								//Deja un espacio antes y despues de la imagen
								jpg.SpacingBefore = 12f;
								jpg.SpacingAfter = 12f;
								//Establece al alineacion
								jpg.Alignment = image.alineacion;
								//Agrega la imagen al PDF
								doc.Add(jpg);
								//Elimina infomación de la cola
								Imagenes.Desencolar();
							}
						}
					//Verifica si la cola está vacía
					if (!Tablas.EsVacia())
						{
							//Esta instancia ayudara a agrega tablas si las hubiera
							iTextSharp.text.pdf.PdfPTable pTable;
							//Catea a Tabla
							Tabla table = (Tabla)Tablas.ShutearPrimero();
							//Verifica que el indice guardado coincida con la <i>
							if (table.indice == i)
							{
								//Aloja la referencia guardada
								pTable = new PdfPTable(1)
								{
									//Alinea la tabla
									HorizontalAlignment = Element.ALIGN_CENTER,
									//Espacio antes y despues
									SpacingBefore = 12f,
									SpacingAfter = 12f
								};
								//Añade el encabezado
								PdfPCell encabezado = new PdfPCell(new Phrase(table.encabezado))
								{
									Colspan = 1,//Indica cuantas columna se combinan
									HorizontalAlignment = 1//Indica la alineación de la tabla
								};
								//Añade el encabezado a la tabla
								pTable.AddCell(encabezado);
								//Busca en la lista de Filas todas las filas relacionadas
								for (int j = 0; j < Filas.GetLargo(); j++)
								{
									//Busca una Fila (desde adelante hasta atrás) que tenga el nombre de la Tabla
									Fila auxRow = Filas.BuscarFila(table.nombre);
									//Verifica que auxRow no sea nulo
									if (Filas.Buscar(auxRow))
									{
										//Agrega la Fila almacenada en Filas que se relacionen con la tabla en cuestión :v
										pTable.AddCell(auxRow.textoplano);
										//Elimina la Fila de la Lista
										Filas.Remover(auxRow);
									}
								}
								//Añade la tabla al documento
								doc.Add(pTable);
								//Elimina información de la Cola
								Tablas.Desencolar();
							}
						}
				}
			}
			catch (Exception ex)//Ocurre un error
			{
				respuesta += "La escritura de archivo PDF se detuvo. Posible pérdida de datos " + ex.Message;
			}
			finally
			{
				try
				{
					//Si la instancia doc no es nula intentará cerrarla
					if (doc != null)
					{
						doc.Close();
						respuesta += "Archivo PDF creado con éxito. Absolute Path -> " + url + "\\" + nombre + ".pdf";
					}
				}
				catch (Exception ex2)
				{
					//Ocurre un error al cerrar
					respuesta += Environment.NewLine + "No se pudo cerrar la escritura de PDF. "+ ex2.Message;
				}	
			}
			return respuesta;
		}
		/// <summary>
		/// Despliega en PANTALL el PDF alojado en la URL
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public string AbrirPDF(String url)
		{
			string respuesta = "";
			try
			{
				System.Diagnostics.Process.Start(url);
				respuesta = "Archivo PDF desplegado exitosamente";
			}
			catch
			{
				respuesta = "No se puede encontrar la ruta especificada";
			}
			return respuesta;
		}
		
	}
}

