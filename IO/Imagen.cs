using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace LFP_P1.IO
{
	class Imagen
	{
		public iTextSharp.text.Image imagen;
		public int alineacion;
		public int indice;

		public Imagen(string urlImagen, String align, int index)
		{
			this.imagen = iTextSharp.text.Image.GetInstance(urlImagen);
			alineacion = (align.Equals("ALIGN_LEFT") ? Element.ALIGN_LEFT : align.Equals("ALIGN_RIGHT") ? Element.ALIGN_RIGHT : Element.ALIGN_CENTER);
			this.indice = index;
		}
	}
}
