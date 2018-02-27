using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace LFP_P1.IO
{
	class Tabla
	{
		public string nombre;
		public string encabezado;
		public int indice;

		public Tabla(string name, string header, int index)
		{
			this.nombre = name;
			this.encabezado = header;
			this.indice = index;
		}
	}
}
