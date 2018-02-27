using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace LFP_P1.IO
{
	class Texto
	{
		public Paragraph texto;
		public int indice;

		public Texto(string plainText, char type , char sub , int index )
		{
			Font arial = (sub == (char)83) ? new Font(FontFactory.GetFont("Arial", 12, (type == (char)66) ? Font.BOLD : (type == (char)73) ? Font.ITALIC : Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK))
								   : /*else*/ new Font(FontFactory.GetFont("Arial", 12, (type == (char)66) ? Font.BOLD : (type == (char)73) ? Font.ITALIC : Font.NORMAL, BaseColor.BLACK));
			texto = new Paragraph(plainText,arial);
			this.indice = index;
		}
	}
}
