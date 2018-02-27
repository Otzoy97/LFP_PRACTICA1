using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using LFP_P1.Listas;

namespace LFP_P1.IO
{
	class Ruta
	{
		/// <summary>
		/// Ruta de Raíz
		/// </summary>
		private readonly string raiz = "C:\\Users\\otzoy\\Desktop\\";
		/// <summary>
		/// Analiza la una URL, de la forma raiz/carpeta/carpeta/.../archivo
		/// y lo edita de tal forma que quede un AbsolutePath de la forma C:\\carpeta...\\archivo
		/// </summary>
		/// <param name="URL">URL de la forma raiz/carpeta/.../archivo</param>
		/// <returns>URL de la forma C:\\carpeta...\\archivo</returns>
		public String GetAbsolutePath(String URL)
		{
			//Quita las primeras 5 letras >>> raiz/
			string prePath = URL.Substring(5);
			//Aqui se guarda el Path correcto
			string Path = raiz;
			//Es más o menos un analizador
			for (int i =0; i<prePath.Length;i++ )
			{
				if (prePath[i] == 47)
				{
					Path += "\\";
				}
				else
				{
					Path += prePath[i];
				}
			}
			return Path;
		}
		/// <summary>
		/// Lee un archivo guardado en la URL de la forma raiz/carpeta.../archivo
		/// y regresa un cadena de Texto para almacenarse en un TextBox
		/// </summary>
		/// <param name="NakedURL">URL de la forma raiz/carpeta../archivo</param>
		/// <returns>cadena de Texto para almacenar en un TextoBox</returns>
		public String GetTextFromFile(String URL)
		{
			//Prepara el archivo para su lectura
			StreamReader reader = null;
			//Se almacenar temporalmente una linea
			String texto = "";
			//Aquí se almacenara TODAS las lineas leídas :v
			String retorno = "";
			//El sistema intentará escribir un string
			try
			{
				//Se instancia el archivo para su lectura
				reader = new StreamReader(URL);
				//Mientras el texto no sea nulo
				while ( (texto = reader.ReadLine()) != null)
				{
					//Agrega texto a la cadena a retornar
					retorno += texto+Environment.NewLine;
				}
			}
			catch (Exception e)
			{
				//Ocurre un error al leer el archivo, queda registro en el txtBlanco
				retorno += "Ocurrio un error al leer el archivo."+e.Message;
			}
			finally
			{
				//Intentará cerrar la lectura del archivo
				try
				{
					//Verifica que se haya instanciado el StreamReader
					if (reader != null)
					{
						//Cierra la lectura del archivo
						reader.Close();
					}
				}
				catch (Exception e)
				{
					//Ocurre un error al cerrar el archivo
					retorno += "Ocurrio un error al cerrar el archivo.";
				}
			}
			//Retorna la cadena de texto, con o sin errores en ella
			return retorno;
		}
		/// <summary>
		/// Crea un String con el codigo HTML
		/// </summary>
		/// <param name="token"></param>
		/// <param name="err"></param>
		/// <returns></returns>
		public String ReporteTokens(ListaSimple token, ListaSimple err)
		{
			//Aquí se alojará el string final a retornar
			string htmlString = "";
			//Ayudará a crear el string a retornar
			StringBuilder htmlBuilder = new StringBuilder();
			//Crea un html
			htmlBuilder.Append("<!DOCTYPE html>");
			htmlBuilder.Append("<html>");
			//Crea un css
			htmlBuilder.Append("<style type=\"text / css\">");
			htmlBuilder.Append(".token  {border-collapse:collapse;border-spacing:0;border-color:#aaa;margin:0px auto;}");
			htmlBuilder.Append(".token td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;border-color:#aaa;color:#333;background-color:#fff;border-top-width:1px;border-bottom-width:1px;}");
			htmlBuilder.Append(".token th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;border-color:#aaa;color:#fff;background-color:#f38630;border-top-width:1px;border-bottom-width:1px;}");
			htmlBuilder.Append(".err  {border-collapse:collapse;border-spacing:0;border-color:#999;margin:0px auto;}");
			htmlBuilder.Append(".err td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;border-color:#999;color:#444;background-color:#F7FDFA;border-top-width:1px;border-bottom-width:1px;}");
			htmlBuilder.Append(".err th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;border-color:#999;color:#fff;background-color:#26ADE4;border-top-width:1px;border-bottom-width:1px;}");
			htmlBuilder.Append(".tg-f4we{font-weight:bold;font-size:15px;font-family:Arial, Helvetica, sans-serif !important;;text-align:center;vertical-align:top}");
			htmlBuilder.Append(".tg-cjcd{font-style:italic;font-size:12px;font-family:Arial, Helvetica, sans-serif !important;;vertical-align:top}");
			htmlBuilder.Append(".tg-do2s{font-size:12px;font-family:Arial, Helvetica, sans-serif !important;;vertical-align:top}");
			htmlBuilder.Append("</style>");
			//Permite la utilización de acentos utf-8
			htmlBuilder.Append("<head>");
			htmlBuilder.Append("<meta charset =\"UTF-8\">");
			htmlBuilder.Append("<title>");
			htmlBuilder.Append("Reporte-");
			htmlBuilder.Append(Guid.NewGuid().ToString());
			htmlBuilder.Append("</title>");
			htmlBuilder.Append("</head>");
			htmlBuilder.Append("<body>");
			///
			///Inicia tabla de TOKEN
			///
			htmlBuilder.Append("<table class= \"token\">");
			htmlBuilder.Append("<tr>" +
								"<th class=\"tg-f4we\" colspan=\"5\">TOKEN</th> " +
							   "</tr>");
			htmlBuilder.Append("<tr>" +
								"<td class=\"tg-cjcd\">#<br></td>" +
								"<td class=\"tg-cjcd\">LEXEMA<br></td>" +
								"<td class=\"tg-cjcd\">FILA</td>" +
								"<td class=\"tg-cjcd\">COLUMNA</td>" +
								"<td class=\"tg-cjcd\">TOKEN</td>" +
							   "</tr>");
			///Realiza una copia de la lista de TOKENS
			Nodo aux = token.inicio;
			///Agrega los elementos de la lista a la tabla recién creada
			while (aux!=null)
			{
				LFP_P1.ADF.Token cast = (LFP_P1.ADF.Token)aux.objeto;
				htmlBuilder.Append("<tr>");
				htmlBuilder.Append("<td class=\"tg-do2s\">"+cast.indice+"</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">"+cast.lexema+"</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">"+cast.fila+"</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">"+cast.columna+"</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">"+cast.token+"</td>");
				htmlBuilder.Append("</tr>");
				aux = aux.siguiente;
			}
			htmlBuilder.Append("<tr>" +
								"<th class=\"tg-cjcd\" colspan=\"5\"> ---------- Fin de línea. " + token.GetLargo() + " tokens ---------- </th> " +
							   "</tr>");
			htmlBuilder.Append("</table><br><br>");
			///
			///Inicia tabla de ERRORES
			///
			htmlBuilder.Append("<table class= \"err\">");
			htmlBuilder.Append("<tr>" +
								"<th class=\"tg-f4we\" colspan=\"5\">ERROR</th> " +
							   "</tr>");
			htmlBuilder.Append("<tr>" +
								"<td class=\"tg-cjcd\">#<br></td>" +
								"<td class=\"tg-cjcd\">LEXEMA<br></td>" +
								"<td class=\"tg-cjcd\">FILA</td>" +
								"<td class=\"tg-cjcd\">COLUMNA</td>" +
								"<td class=\"tg-cjcd\">ERROR</td>" +
							   "</tr>");
			///Realiza una copia de la lista de TOKENS
			aux = err.inicio;
			///Agrega los elementos de la lista a la tabla recién creada
			while (aux != null)
			{
				LFP_P1.ADF.Error cast = (LFP_P1.ADF.Error)aux.objeto;
				htmlBuilder.Append("<tr>");
				htmlBuilder.Append("<td class=\"tg-do2s\">" + cast.indice + "</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">" + cast.lexema + "</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">" + cast.fila + "</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">" + cast.columna + "</td>");
				htmlBuilder.Append("<td class=\"tg-do2s\">" + cast.error + "</td>");
				htmlBuilder.Append("</tr>");
				aux = aux.siguiente;
			}
			htmlBuilder.Append("<tr>" +
					"<th class=\"tg-cjcd\" colspan=\"5\"> ---------- Fin de línea. "+ err.GetLargo()+" errores ---------- </th> " +
				   "</tr>");
			htmlBuilder.Append("<tr>" +
					"<th class=\"tg-f4we\" colspan=\"5\"> -1 ► Durante tiempo de ejecución </th> " +
				   "</tr>");
			htmlBuilder.Append("</table><br><br>");
			htmlBuilder.Append("</body>");
			htmlBuilder.Append("</html>");
			//Crea un string de todo lo escrito anteriormente
			htmlString = htmlBuilder.ToString();
			return htmlString;
		}
		/// <summary>
		/// Generar y Abre un archivo
		/// </summary>
		/// <param name="Texto"></param>
		public void GenerarYAbrirFile(String Texto, String nombre ,String extension,bool abrir)
		{
			StreamWriter outputFile = null;
			try
			{
				using (outputFile = new StreamWriter(raiz+"\\"+nombre+extension))
				{
					outputFile.Write(Texto.ToString());
				}
			}
			catch (Exception ex)
			{
			}
			finally
			{
				try
				{
					if (outputFile != null)
					{
						outputFile.Close();
						if (abrir)
						{
							System.Diagnostics.Process.Start(raiz + "\\" + nombre + extension);
						}
					}
				}
				catch (Exception e)
				{
				}
			}

		}
	}
}


