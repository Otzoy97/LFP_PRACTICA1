using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_P1.ADF
{
	class Token
	{
		public int indice;
		public String lexema;
		public int fila, columna;
		public String token;

		private readonly String[] PrimerEstado = {"iniciarA","ingresarAPdf","crear",
		"escribirTexto","imagen","crearTabla","crearFila","ejecutar",
		"mostrar","reporteTokens" , "acercaDe"};
		
		public Token(int index, String lexema, int row, int column, int estadoAceptacion)
		{
			this.indice = index;
			this.lexema = lexema;
			this.fila = row;
			this.columna = column;
			this.token = PalabraReservada(estadoAceptacion);
		}

		public Token(int index, String lexema, int row, int column, String token)
		{
			this.indice = index;
			this.lexema = lexema;
			this.fila = row;
			this.columna = column;
			this.token = token;
		}

		/// <summary>
		/// Este metodo utiliza el hecho de que las sentencias utilizadas solo pueden ser de hasta 4 palabras
		/// De tal forma que 
		/// la primera palabra únicamente puede ser PalabraReservada
		/// la segunda palabra puede ser PalabraReservada
		///								 Texto
		///								 Ruta
		///								 Nombre
		/// la tercera palabra puede ser PalabraReservada
		///								 Texto
		///								 Ruta
		///								 Nombre
		/// la cuarta palabra únicamente puede ser PalabraReservada
		/// </summary>
		/// <param name="estadoAceptacion"></param>
		/// <returns>Palabra Reservada</returns>
		public String PalabraReservada(int estadoAceptacion)
		{
			switch (indice)
			{	
				case 1:
					foreach (var firstState in PrimerEstado)
					{
						//Busca un match con las palabras Reservadas
						if (firstState.Equals(lexema))
						{
							return lexema;
						}
					}
					return "Error";
				case 2:
					if (estadoAceptacion == 0)
					{
						if (lexema.Equals("ManualUsuario"))
						{
							return lexema;
						}
						else if (lexema.Equals("ManualTecnico"))
						{
							return lexema;
						}
						else
						{
							//Si ninguna coincide entonces es un nombre
							return "Nombre";
						}
					}
					else if (estadoAceptacion == 1)
					{
						return "Ruta";
					}
					else if (estadoAceptacion == 2)
					{
						return "Texto";
					}
					break;
				case 3:
					if (estadoAceptacion == 0)
					{
						if (lexema.Equals("ARIAL") || lexema.Equals("ITALIC") || lexema.Equals("BOLD"))
						{
							return "TipoLetra";
						}
						else if (lexema.Equals("ALIGN_CENTER") || lexema.Equals("ALIGN_LEFT") || lexema.Equals("ALIGN_RIGHT"))
						{
							return "Alineacion";
						}
						else
						{
							return "Nombre";
						}
					}
					break;
				case 4:
					if (estadoAceptacion == 0)
					{
						//Solo puede ser una palabra reservada
						if (lexema.Equals("Abrir"))
						{
							return lexema;
						}
						else if (lexema.Equals("Subrayado"))
						{
							return lexema;
						}
						else
						{
							return "Error";
						}
					}
					break;
				default:
					return "Error";
			}
			return "Error";
		}
	}
}
