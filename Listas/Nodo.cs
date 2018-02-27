using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LFP_P1.IO;

namespace LFP_P1.Listas
{
	class Nodo
	{
		//Enlaza los nodos
		public Nodo siguiente;
		//Enlaza los nodos
		public Nodo anterior;
		//Guarda un objeto
		public Object objeto;
		/**
		 * Inicializar el Nodo para que guarde tipos Object
		 */
		public Nodo(Object objeto)
		{
			this.objeto = objeto;
		}
	}
}
