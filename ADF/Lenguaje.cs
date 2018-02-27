using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFP_P1.Listas;

namespace LFP_P1.ADF
{
	class Lenguaje
	{
		private int row = 1, column = 1;
		/// <summary>
		/// Esta es la joya de la corona. (la corona es el programa en si)
		/// Aqui se realiza el análisis léxico a través de AFD -Automata Finito Determinista-
		/// </summary>
		/// <param name="cadena"></param>
		/// <returns></returns>
		public ListaSimple AnalizarCadena(string cadena)//, ListaSimple listaSimple)
		{
			//Levara el control de los estados
			int estadoAceptacion = 0;
			//Guardara temporalmente los caracteres de la cadena ingresada
			char cadenaCaracter;
			//Se eliminan espacio al principio y al final
			//cadena = cadena.Trim();
			//Almacenará los caracteres que se van analizando
			String cadenaAceptada = "";
			//Declara el arrayliste que contendrá los tokens almacenados
			ListaSimple listaToken = new ListaSimple("Token");
			//A través del for se analizará caracter por caracter
			for (int i = 0; i < cadena.Length; i++)
			{
				cadenaCaracter = cadena[i];
				//Console.WriteLine(cadenaCaracter);
				switch (estadoAceptacion)
				{
					case 0:
						//MAYUSCULAS o minusculas o guión bajo o numeros
						if ((cadenaCaracter >= 65 && cadenaCaracter <= 90) ||
							(cadenaCaracter >= 97 && cadenaCaracter <= 122) ||
							cadenaCaracter == 95 || (cadenaCaracter >= 48 && cadenaCaracter <= 57))
						{
							//Se mantiene en el estado inicial;
							estadoAceptacion = 0;
							//Agrega el caracter a la cadena Aceptada
							cadenaAceptada += cadenaCaracter;
							//Aumenta la columna
							column++;
						}
						//Salto de línea
						else if (cadenaCaracter == 10 || cadenaCaracter == 11)
						{
							//Mantiene el estado de Aceptacion
							estadoAceptacion = 0;
							//Agrega lo que hay hasta ahora en la cadenaAceptada
							if (!string.IsNullOrEmpty(cadenaAceptada) || !cadenaAceptada.Equals(""))
							{
								Console.WriteLine(estadoAceptacion + " -> " + cadenaAceptada);
								listaToken.Enlistar(new Token(listaToken.GetLargo() + 1, cadenaAceptada, row, column - cadenaAceptada.Length, 0));
							}
							//Aumenta la línea
							row++;
							//Empieza nueva columna
							column = 0;
							//Limipia la cadena para aceptar otro lexema
							cadenaAceptada = "";
						}
						//Espacio y tab
						else if (cadenaCaracter == 32 || cadenaCaracter == 9)
						{
							//Mantiene el estado de Aceptacion
							estadoAceptacion = 0;
							//Agrega lo que hay hasta ahora en la cadenaAceptada
							if (!string.IsNullOrEmpty(cadenaAceptada) || !cadenaAceptada.Equals(""))
							{
								Console.WriteLine(estadoAceptacion + " -> " + cadenaAceptada);
								listaToken.Enlistar(new Token(listaToken.GetLargo() + 1, cadenaAceptada, row, column - cadenaAceptada.Length, 0));
							}
							//Aumenta la columna
							column++;
							//Limipia la cadena para aceptar otro lexema
							cadenaAceptada = "";
						}
						//Apostofre
						else if (cadenaCaracter == 39)
						{
							//Limpia la cadena para aceptar la ruta
							cadenaAceptada = "";
							//Aumenta la columna
							column++;
							//Cambia el estado de Aceptacion 
							estadoAceptacion = 1;
						}
						//Comillas
						else if (cadenaCaracter == 34)
						{
							//Limpia la cadena para aceptar el texto
							cadenaAceptada = "";
							//Aumenta la columna
							column++;
							//Cambia el estado de Aceptación
							estadoAceptacion = 2;
						}
						//Se produce un error léxico
						else
						{
							//Simplemente Agrega el caracter y el error se determinará en las siguientes fases
							cadenaAceptada += cadenaCaracter;
							column++;
						}
						break;
					case 1:
						//Apostrofe
						if (cadenaCaracter == 39)
						{
							Console.WriteLine(estadoAceptacion + " -> " + cadenaAceptada);
							//Debe agregar lo que hay hasta ahora en la cadenaAceptada
							listaToken.Enlistar(new Token(listaToken.GetLargo()+1, cadenaAceptada, row, column - cadenaAceptada.Length, "Ruta"));
							//Limpia la cadena para aceptar un nuevo lexema
							cadenaAceptada = "";
							//Aumenta la columna
							column++;
							//Regresa el estado de Aceptacion
							estadoAceptacion = 0;
						}
						else
						{
							//Guarda lo que esta en la cadena
							cadenaAceptada += cadenaCaracter;
							//Aumenta la columna
							column++;
							//Mantiene el estado de Aceptacion
							estadoAceptacion = 1;
						}
						break;
					case 2:
						//Comillas
						if (cadenaCaracter == 34)
						{
							Console.WriteLine(estadoAceptacion+ " -> " + cadenaAceptada);
							//Debe agregar lo que hay hasta ahora en la cadenaAceptada
							listaToken.Enlistar(new Token(listaToken.GetLargo()+1, cadenaAceptada, row, column-cadenaAceptada.Length, "Texto"));
							//Limpia la cadena para aceptar un nuevo lexema
							cadenaAceptada = "";
							//Aumenta la columna
							column++;
							//Regresa el estado de Aceptacion
							estadoAceptacion = 0;
						}
						else
						{
							//Guarda lo que esta en la cadena
							cadenaAceptada += cadenaCaracter;
							//Mantiene el estado de Aceptacion
							estadoAceptacion = 2;
							//Aumenta la columna
							column++;
						}
						break;
				}
			}
			//Guarda lo último que se encuentra en la cadena si fuera necesario
			if (!String.IsNullOrEmpty(cadenaAceptada))
			{
				//Console.WriteLine(cadenaAceptada);
				//Debe agregar lo que hay en la cadenaAceptada
				Console.WriteLine(estadoAceptacion + " -> " + cadenaAceptada);
				listaToken.Enlistar(new Token(listaToken.GetLargo()+1, cadenaAceptada, row, column, estadoAceptacion));
			}
			//Devuelve la ListaSimple de token
			return listaToken;
		}
	}
}
