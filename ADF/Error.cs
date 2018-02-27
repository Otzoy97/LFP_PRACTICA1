using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_P1.ADF
{
	class Error
	{
		public int indice;
		public String lexema;
		public int fila, columna;
		public String error;
		/// <summary>
		/// Crea un nuevo Error :v
		/// </summary>
		/// <param name="indice"></param>
		/// <param name="lexema"></param>
		/// <param name="fila"></param>
		/// <param name="columna"></param>
		/// <param name="error"></param>
		public Error(int indice, String lexema, int fila, int columna, String error)
		{
			this.indice = indice;
			this.lexema = lexema;
			this.fila = fila;
			this.columna = columna;
			this.error = error;
		}

	}
}
