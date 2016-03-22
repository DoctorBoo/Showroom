using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>, IDisposable
        where TEntity : class, new()
        where TContext : DbContext
    {
        protected TContext Context;
        /// <summary>
        /// Used to determine if Dispose()
        /// has already been called.
        /// </summary>
        bool disposed = false;

        public Repository(TContext dbContext)
        {
            Context = dbContext as TContext;
        }

        public virtual TEntity Create()
        {
            return Context.Set<TEntity>().Create();
        }
        public virtual EntityKey GetEntityKey(TEntity entity)
        {
            var oc = ((IObjectContextAdapter)Context).ObjectContext;
            ObjectStateEntry ose;
            if (null != entity && oc.ObjectStateManager
                                    .TryGetObjectStateEntry(entity, out ose))
            {
                return ose.EntityKey;
            }
            return null;
        }
        public virtual IEnumerable<IRelatedEnd> GetEntityRelatedEnds(TEntity entity)
        {
            var oc = ((IObjectContextAdapter)Context).ObjectContext;
            RelationshipManager ose;
            if (null != entity && oc.ObjectStateManager
                                    .TryGetRelationshipManager(entity, out ose))
            {
                return ose.GetAllRelatedEnds();
            }
            return null;
        }
        public virtual object GetEntityValue(TEntity entity, string propertyName)
        {
            return Context.Entry<TEntity>(entity).CurrentValues.GetValue<object>(propertyName);
        }
        public virtual IEnumerable<string> GetEntityPropertyNames(TEntity entity)
        {
            return Context.Entry<TEntity>(entity).CurrentValues.PropertyNames;
        }
        //public virtual TAny Create<TAny>(TAny entity)
        //{
        //    return Context.Set <TAny>().Add(entity);
        //}
        public virtual TEntity Create(TEntity entity)
        {
            TEntity newEntity = new TEntity();
            try
            {
                newEntity = Context.Set<TEntity>().Add(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
            return newEntity;
        }
        public virtual TEntity SetState(TEntity entity, EntityState state)
        {
            try
            {
                Context.Entry(entity).State = state;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return entity;
        }
        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Detached;
                Context.Set<TEntity>().Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;

                //bool succeeded = 0 < Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Log<Repository<TEntity, TContext>>
                        .Write.Error(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        Log<Repository<TEntity, TContext>>
                            .Write.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                Log<Repository<TEntity, TContext>>.Write.Error(ex.Message, ex);
            }
            return entity;
        }

        public virtual TEntity ChangeState(TEntity entity, EntityState newState)
        {
            Context.Entry(entity).State = newState;
            return entity;
        }
        public virtual void Remove(decimal id)
        {
            var item = Context.Set<TEntity>().Find(id);
            Context.Set<TEntity>().Remove(item);
            bool succeeded = 0 < Context.SaveChanges();
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            //bool succeeded = 0 < Context.SaveChanges();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                var objects = Context.Set<TEntity>().Where(where);

                foreach (var item in objects)
                {
                    Context.Set<TEntity>().Remove(item);
                }
            }
            catch (Exception)
            {
            }
        }

        public virtual TEntity FindById(decimal id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where = null)
        {
            return FindAll(where).FirstOrDefault();
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> where = null)
        {
            return null != where ? Context.Set<TEntity>().Where(where) : Context.Set<TEntity>();
        }
        /// <summary>
        /// Always return a non-null list. During a CRUD-operation without savechanges
        /// entities are kept in local store.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IList<TEntity> GetLocal(Expression<Func<TEntity, bool>> predicate = null)
        {
            List<TEntity> localStore = new List<TEntity>();

            try
            {
                localStore = Context.Set<TEntity>().Local.Count() > 0 ?
                    Context.Set<TEntity>().Local.AsQueryable().Where(predicate).ToList() : new List<TEntity>();

            }
            catch (Exception ex)
            {
                Log<ResourceManager>.Write.Warn(ex.Message, ex);
            }
            return null != predicate ? localStore : new List<TEntity>();
        }

        /// <summary>
        /// GetAll including local storage.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IList<TEntity> GetAllIncludingLocals(Expression<Func<TEntity, bool>> predicate = null)
        {
            List<TEntity> allData = new List<TEntity>();

            try
            {
                allData = FindAll(predicate).ToList();
                var local = Context.Set<TEntity>().Local.Count() > 0 ?
                    Context.Set<TEntity>().Local.AsQueryable().ToList() : new List<TEntity>();
                local = predicate != null ? local.AsQueryable().Where(predicate).ToList() : local;
                allData = allData.Union(local).ToList();
            }
            catch (Exception ex)
            {
                Log<ResourceManager>.Write.Warn(ex.Message, ex);
            }
            return allData;
        }
        /// <summary>
        /// From database
        /// </summary>
        /// <returns></returns>
        public virtual IList<TEntity> GetAll()
        {
            try
            {
                return Context.Set<TEntity>().ToList();
            }
            catch (Exception ex)
            {
                Log<Repository<object, DbContext>>.Write.Fatal(ex);
                return null;
            }
        }
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        public DbContextConfiguration GetContextConfig()
        {
            return Context.Configuration;
        }
        public void Refresh<TEntity>(RefreshMode refreshmode)
        {
            var octx = ((IObjectContextAdapter)Context).ObjectContext;
            octx.Refresh(RefreshMode.ClientWins, FindAll().AsEnumerable());
        }
        public virtual bool SaveChanges()
        {
            bool succeeded = false;
            bool saveFailed = false;
            do
            {
                try
                {
                    succeeded = 0 < Context.SaveChanges() || saveFailed;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    SubSystem.HandleDbUpdateException(ex);
                    //https://msdn.microsoft.com/en-us/data/jj592904
                    saveFailed = true;

                    // Get the current entity values and the values in the database 
                    // as instances of the entity type 
                    var entry = ex.Entries.Single();
                    var databaseValues = entry.GetDatabaseValues();
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    SubSystem.HandleDbUpdateException(ex);
                    throw;
                }
                catch (DbEntityValidationException e)
                {
                    SubSystem.HandleEntityValidationErrors(e);
                    throw;
                }
            } while (false);

            return succeeded;
        }
        public void HaveUserResolveConcurrency(DbPropertyValues currentValues,
                                           DbPropertyValues databaseValues,
                                           DbPropertyValues resolvedValues)
        {
            // Show the current, database, and resolved values to the user and have 
            // them edit the resolved values to get the correct resolution. 
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            int result = 0;
            bool saveFailed = false;
            do
            {
                try
                {
                    result = await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    SubSystem.HandleDbUpdateException(ex);
                    //https://msdn.microsoft.com/en-us/data/jj592904
                    saveFailed = true;

                    // Get the current entity values and the values in the database 
                    // as instances of the entity type 
                    var entry = ex.Entries.Single();

                    // Update the original values with the database values and 
                    // the current values with whatever the user choose. 
                    entry.OriginalValues.SetValues(entry.CurrentValues);
                    entry.CurrentValues.SetValues(entry.CurrentValues);
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    SubSystem.HandleDbUpdateException(ex);
                    throw;
                }
                catch (DbEntityValidationException e)
                {
                    SubSystem.HandleEntityValidationErrors(e);
                    throw;
                }
            } while (saveFailed);
            return result;
        }
        /// <summary>
        /// Releases all resources used by the Entities
        /// </summary>
        public void Dispose()
        {
            // Call our helper method.
            // Specifying "true" signifies that
            // the object user triggered the cleanup.
            Dispose(true);
            // Now suppress finalization.
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (null != Context)
                {
                    Context.Dispose();
                }
                //cleanup unmanaged resources 
                disposed = true;
            }
        }

        ~Repository()
        {
            // Call our helper method.
            // Specifying "false" signifies that
            // the GC triggered the cleanup.
            Dispose(false);
        }
    }
}
