using System;
using System.Collections;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Expression;


namespace Business
{
	/// <summary>
	/// Summary description for BaseDataAccess.
	/// </summary>
	public class BaseDataAccess 
	{
		protected ISession m_session;
		
		public BaseDataAccess()
		{
			m_session = NHibernateHttpModule.CurrentSession;
		}

		/// <summary>
		/// Save a collection.
		/// </summary>
		/// <param name="items"></param>
		public virtual void Save(IList items)
		{
			ITransaction tx = null;		
			try
			{
				if (items!=null)
				{
					tx = m_session.BeginTransaction();
				
					foreach(object item in items)
					{
						m_session.Save(item);
					}
					tx.Commit();
                }
			}
			catch (Exception ex)
			{
				tx.Rollback();				
				throw ex;
			}
		}

		public virtual void SaveOrUpdate(IList items, bool withTX)
		{
			ITransaction tx = null;
		
			try
			{
				if (items!=null)
				{
					if (withTX) tx = m_session.BeginTransaction();
				
					foreach(object item in items)
					{
						m_session.SaveOrUpdate(item) ;
					}

					if (withTX) tx.Commit();
				}
			}
			catch (Exception ex)
			{
				if (withTX) tx.Rollback();
				
				throw ex;
			}
		}

		public virtual void SaveOrUpdate(IList items)
		{
			ITransaction tx = null;
		
			try
			{
				if (items!=null)
				{
					tx = m_session.BeginTransaction();
				
					foreach(object item in items)
					{
						m_session.SaveOrUpdate(item) ;
					}

					tx.Commit();
				}
			}
			catch (Exception ex)
			{
				tx.Rollback();
				
				throw ex;
			}
		}

		public virtual void Save(ISession sesion, IList items)
		{
			try
			{
				if (items!=null)
				{
					foreach(object item in items)
					{
						sesion.SaveOrUpdate(item);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// Saves an item and then saves the child items inside of a transaction.
		/// </summary>
		/// <param name="parentItem"></param>
		/// <param name="childItems"></param>
		public virtual void Save(object parentItem, IList childItems)
		{
			ITransaction tx = null;
		
			try
			{
				if (childItems!=null)
				{
					tx = m_session.BeginTransaction();
				
					m_session.SaveOrUpdate(parentItem);

					foreach(object item in childItems)
					{
						m_session.SaveOrUpdate(item);
					}

					tx.Commit();
				}
			}
			catch (Exception ex)
			{
				tx.Rollback();				
				throw ex;
			}
		}

		public virtual void Save(object parentItem, IList childItems, bool withTx)
		{
			ITransaction tx = null;
		
			try
			{
				if (childItems!=null)
				{
					if (withTx) tx = m_session.BeginTransaction();
				
					m_session.SaveOrUpdate(parentItem);

					foreach(object item in childItems)
					{
						m_session.SaveOrUpdate(item);
					}

					if (withTx) tx.Commit();
				}
			}
			catch (Exception ex)
			{
				if (tx != null) tx.Rollback();
				
				throw ex;
			}
		}

		/// <summary>
		/// Saves the item.
		/// </summary>
		/// <param name="item"></param>
		public virtual void Save(object item)
		{
			Save(item, true);
		}

		public virtual void Save()
		{
			Save(this, true);
		}

		/// <summary>
		/// Guarda el item con o sin transaction.
		/// </summary>
		/// <param name="item"></param>
		public virtual void Save(object item, bool withTx)
		{
			ITransaction tx=null;

			try
			{
				if (withTx) tx = m_session.BeginTransaction();
				
				m_session.SaveOrUpdate(item);
				
				if (withTx) tx.Commit();
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}
		}

		/// <summary>
		/// Modifica el item.
		/// </summary>
		/// <param name="item"></param>
		public virtual void Update()
		{
			this.Update(this);
		}

		public virtual void Update(object item)
		{
			this.Update(this,true);
		}

		public virtual void Update(object item, bool withTx)
		{
			ITransaction tx=null;

			try
			{
				tx = m_session.BeginTransaction();
				
				m_session.Update(item);
				
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw ex;
			}
		}

		public virtual void Update(IList items)
		{
			ITransaction tx=null;

			try
			{
				tx = m_session.BeginTransaction();
				
				foreach (object item in items)
				{
					m_session.Update(item);
				}
				
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw ex;
			}
		}
		/// <summary>
		/// Returns a list of items matching the type supplied.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IList Get(Type type)
		{
			return GetByType(type);
		}

		public object Get(Type type, object id)
		{
			object returnValue=null;

			try
			{
				returnValue=m_session.Load(type, id);
				
				return returnValue;
			}
			catch (Exception ex)
			{
				//TODO: disciminar en caso q la excepcion sea del tipo id inexistente.
				throw ex;
			}
		}

		public object Get(object id)
		{
			return this.Get(this.GetType(),id);
		}

		public IList GetListByPropertyValue(Type type, string propertyName, object propertyValue)
		{
			try
			{
				ICriteria crit=m_session.CreateCriteria(type);
				crit.Add(Expression.Eq(propertyName, propertyValue));			
				return crit.List();
			}
			catch (Exception ex)
			{				
				throw ex;
			}
		}

		public IList GetListByPropertyValue(Type type, string propertyName, object propertyValue,
			string propertyName2, object propertyValue2)
		{
			try
			{
				ICriteria crit=m_session.CreateCriteria(type);

				crit.Add(Expression.Eq(propertyName, propertyValue));
				crit.Add(Expression.Eq(propertyName2, propertyValue2));
			
				return crit.List();
			}
			catch (Exception ex)
			{				
				throw ex;
			}
		}

		public IList GetListByPropertyValue(Type type, string[] propertyName, object[] propertyValue)
		{
			try
			{
				ICriteria crit=m_session.CreateCriteria(type);
				for(int i= 0;i<=propertyName.GetUpperBound(0);i++)
				{
					crit.Add(Expression.Eq(propertyName[i], propertyValue[i]));
				}
				return crit.List();
			}
			catch (Exception ex)
			{				
				throw ex;
			}
		}

		public object Get(Type type, string propertyName, object propertyValue)
		{
			try
			{
				ICriteria crit=m_session.CreateCriteria(type);

				crit.Add(Expression.Eq(propertyName, propertyValue));
			
				IList list = crit.List();

				if (list==null || list.Count<1)
				{
					return null;
				}
				else
				{
					return list[0];
				}
			}
			catch (Exception ex)
			{				
				throw ex;
			}
		}


		public object Get(Type type, string propertyName, object propertyValue,string propertyName2, object propertyValue2)
		{
			try
			{
				ICriteria crit=m_session.CreateCriteria(type);

				crit.Add(Expression.Eq(propertyName, propertyValue));
				crit.Add(Expression.Eq(propertyName2, propertyValue2));
			
				IList list = crit.List();

				if (list==null || list.Count<1)
				{
					return null;
				}
				else
				{
					return list[0];
				}
			}
			catch (Exception ex)
			{				
				throw ex;
			}
		}

		private IList GetByType(Type type)
		{
			IList items = null;
			ITransaction tx = null;
			
			try
			{
				tx = m_session.BeginTransaction();
				
				items = m_session.CreateCriteria(type).List();
                
				tx.Commit();

				return items;
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}

		public void Delete(object item)
		{
			ITransaction tx = null;
			
			try
			{
				tx = m_session.BeginTransaction();
				
				m_session.Delete(item);
				
				tx.Commit();
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}

		public void Delete()
		{
			this.Delete(this);
		}

		public void Delete(IList items)
		{
			ITransaction tx = null;
			
			try
			{
				tx = m_session.BeginTransaction();

				foreach (object item in items)
				{
					m_session.Delete(item);
				}
				
				tx.Commit();
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}

		public void Delete(object parent, IList childs)
		{
			ITransaction tx = null;
			
			try
			{
				tx = m_session.BeginTransaction();

				foreach (object item in childs)
				{
					m_session.Delete(item);
				}

				m_session.Delete(parent);
				
				tx.Commit();
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}

		public void Delete(ISession sesion, IList items)
		{
			try
			{
				foreach (object item in items)
				{
					sesion.Delete(item);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}			
		}

		public void Delete(IList items, bool withTx)
		{
			ITransaction tx = null;
			
			try
			{
				if (withTx) tx = m_session.BeginTransaction();

				foreach (object item in items)
				{
					m_session.Delete(item);
				}
				
				if (withTx) tx.Commit();
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}


		public void DeleteAll(ISession sesion, Type tipo)
		{
			try
			{
				IList lista = sesion.CreateCriteria(tipo).List() ;

				foreach (object item in lista)
				{
					sesion.Delete(item);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}			
		}

		public void DeleteAll(Type tipo)
		{
			ITransaction tx = null;

			try
			{
				tx = m_session.BeginTransaction();

				IList lista = m_session.CreateCriteria(tipo).List() ;

				foreach (object item in lista)
				{
					m_session.Delete(item);
				}

				tx.Commit ();
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}


		/// <summary>
		/// limpia el cache de datos, si hay algo que no se haya persistido se borra
		/// </summary>
		/// <example >usado en el caso del boton cancelar donde los obj de memoria se han cambiado
		///  y son distintos de los de la BD</example>
		public void Clear()
		{
			try
			{
				m_session.Clear();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public void Flush ()
		{
			try
			{
				NHibernateHttpModule.CurrentSession.Flush() ;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Refresh (object item)
		{
			try
			{
				NHibernateHttpModule.CurrentSession.Refresh(item);
			}
			catch
			{}
		}

		public void Refresh (object item, NHibernate.LockMode lockMode)
		{
			try
			{
				NHibernateHttpModule.CurrentSession.Refresh(item, lockMode);
			}
			catch
			{}
		}

		public IList getListByQuery (string HQL)
		{
			IList items = null;
			ITransaction tx = null;
			IQuery query = null;
			
			try
			{
				tx = m_session.BeginTransaction();
				
				query = m_session.CreateQuery(HQL);
				items = query.List();
                
				tx.Commit();

				return items;
			}
			catch (Exception ex)
			{
				if (tx!=null) tx.Rollback();

				throw ex;
			}			
		}

		
	}
}