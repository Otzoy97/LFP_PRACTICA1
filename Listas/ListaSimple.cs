using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFP_P1.IO;

namespace LFP_P1.Listas
{
	class ListaSimple
	{
		public Nodo inicio;
		//Levará el conteo de Objetos
		private int largo;
		//Identifica la lista
		private string id;
		/**
		 * Consulta cuando (nodos) elementos tiene la lista
		 */
		public int GetLargo()
		{ 
			return largo;
		}
		/**
		 * Consulta el identificador de la lista
		 */
		public string GetId()
		{
			return id;
		}
		/**
		 *Constructor por defecto 
		 */
		public ListaSimple(String nombre)
		{
			this.inicio = null;
			this.largo = 0;
			this.id = nombre;
		}
		/**
		 * Consulta si la lista esta vacía
		 */
		public bool EsVacia()
		{
			return inicio == null;
		}
		/**
		 * Agrega un nuevo nodo al final de la lista
		 */
		public void Enlistar(Object objeto)
		{
			Nodo nuevo = new Nodo(objeto);
			if (EsVacia())
			{
				inicio = nuevo;
			}
			else
			{
				//Recorre la lista y agrega el nodo
				Nodo aux = inicio;
				while (aux.siguiente != null)
				{
					aux = aux.siguiente;
				}
				aux.siguiente = nuevo;
			}
			largo++;
		}
		/**
		 * Busca si existe un Objeto en la lista
		 */
		public bool Buscar(Object referencia)
		{
			//Crea una copia de la lista
			Nodo aux = inicio;
			//Recorre la lista hasta llegar al final o encontrar
			//el objeto deseado
			while (aux != null)
			{
				if (referencia == aux.objeto)
				{
					//Encontrado
					return true;
				}
				else
				{
					aux = aux.siguiente;
				}
			}
			return false;
		}

		public Fila BuscarFila(String nombreTabla)
		{
			//Crea una copia de la lista
			Nodo aux = inicio;
			//Recorre la lista hasta llegar al final o encontrar
			//el objeto deseado
			while (aux != null)
			{
				//Obtiene el objeto y lo castea a una Fila
				Fila temp = (Fila)aux.objeto;
				if (temp.tablaPDF.Equals(nombreTabla))
				{
					//Devuelve el id
					return temp;
				}
				else
				{
					//Pasa al siguiente nodo
					aux = aux.siguiente;
				}
			}
			//No ninguna tabla
			return null;
		}
		
		public PDF BuscarPDF(String NombrePDF)
		{
			//Crea una copia de la lista
			Nodo aux = inicio;
			//Recorre la lista hasta llegar al final o encontrar
			//el objeto deseado
			while (aux != null)
			{
				//Obtiene el objeto y lo castea a un PDF
				PDF temp = (PDF) aux.objeto;
				if (temp.nombre.Equals(NombrePDF))
				{
					//Devuelve el id
					return temp;
				}
				else
				{
					//Pasa al siguiente nodo
					aux = aux.siguiente;
				}
			}
			//No existe el PDF
			return null;
		}

		/**
		 * Elimina un nodo que se encuentre en la lista
		 */
		public void Remover(Object referencia)
		{
			if (Buscar(referencia))
			{
				//Verifica si el nodo a eliminar es el primero
				if (inicio.objeto == referencia)
				{
					//Corrige el apuntador hacia el siguiente
					inicio = inicio.siguiente;
				}
				else
				{
					//Crea una copia de la lista
					Nodo aux = inicio;
					//Recorrre la lista y se detiene 
					//justo antes del Nodo que contiene el 
					//Objeto referencia
					while (aux.siguiente.objeto != referencia)
					{
						aux = aux.siguiente;
					}
					//Guarda el nodo siguiente del nodo a eliminar
					Nodo siguiente = aux.siguiente.siguiente;
					//Enlaza el nodo anterior al de eliminar
					//con el siguiente despues de el
					aux.siguiente = siguiente;
				}
				//Disminuye el contador
				largo--;
			}
		}
		/**
		 * Desmonta la lista desde el principio
		 */
		public Object Listar()
		{
			if (!EsVacia())
			{
				Object obj = inicio.objeto;
				inicio = inicio.siguiente;
				largo--;
				return obj;
			}
			return null;
		}
	}
}
