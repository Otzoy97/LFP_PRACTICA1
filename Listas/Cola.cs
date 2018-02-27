using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_P1.Listas
{
	class Cola
	{
		//Apuntador de la cabeza :v
		public Nodo primero;
		//Apuntador a la cola :v
		public Nodo ultimo;
		//Levará el conteo de Objetos
		private int largo;
		//Identifica a la cola
		private string id;
		/**
		 * Consulta cuantos (nodos) elementos tiene la cola
		 */
		public int GetSize()
		{
			return largo;
		}
		/**
		 * Consulta el identificador de la cola
		 */
		public string GetId()
		{
			return id;
		}
		/**
		 * Constructor por defecto
		 */
		public Cola(String nombre)
		{
			this.primero = null;
			this.largo = 0;
			this.id = nombre;
			ultimo = primero;
		}
		/**
		 * Consulta si la cola esta vacía
		 */
		public bool EsVacia()
		{
			return primero == null;
		}
		/**
		 * Agrega nodos al final de la cola
		 */
		public void Encolar(Object objeto)
		{
			Nodo nuevo = new Nodo(objeto);
			if (EsVacia())
			{
				primero = nuevo;
				ultimo = primero;
			}
			else
			{
				ultimo.siguiente = nuevo;
				ultimo = nuevo;
			}
			largo++;
		}
		/**
		 * Retira la cabeza de la cola :v
		 */
		public Object Desencolar()
		{
			Object objeto = primero.objeto;
			primero = primero.siguiente;
			largo--;
			return objeto;
		}
		/**
		 * Devuelve el objeto alojado en la cabeza
		 */
		public Object ShutearPrimero()
		{
			return primero.objeto;
		}
		/**
		 * Devuelve el objeto alojado al final 
		 */
		public Object ShutearUltimo()
		{
			return ultimo.objeto;
		}

		public String[] VolverArray()
		{
			String[] retorno = new String[GetSize()];
			for (int i = 0; i<retorno.Length;i++)
			{
				retorno[i] = (string) Desencolar();
				//Console.WriteLine("En Cola -> "+retorno[i]);
			}
			return retorno;
		}
	}
}
