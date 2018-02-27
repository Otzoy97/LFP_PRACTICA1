using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_P1.IO
{
	class Fila
	{
		public string tablaPDF;
		public string textoplano;
		/// <summary>
		/// Crea una nueva Fila :v
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="tablePDF"></param>
		public Fila(string plainText,string tablePDF)
		{
			this.tablaPDF = tablePDF;
			this.textoplano = plainText;
		}
	}
}
