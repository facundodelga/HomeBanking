using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using prueba.Models;
using System.Linq.Expressions;

namespace prueba.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {   
        protected HomeBankingContext RepositoryContext { get; set; }

        protected RepositoryBase(HomeBankingContext repository)
        {
            this.RepositoryContext = repository;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTrackingWithIdentityResolution();
        }

        //este método proporciona una forma genérica de obtener todos los elementos de una entidad específica de la base de datos,
        //con la opción de incluir datos relacionados u otra lógica personalizada en la consulta antes de devolverla
        public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null) {
            IQueryable<T> queryable = this.RepositoryContext.Set<T>();


            if (includes != null) {
                // Si se proporcionó, se aplica la función includes a queryable
                // Esto permite incluir datos relacionados u otra lógica personalizada en la consulta
                queryable = includes(queryable);
            }

            return queryable.AsNoTrackingWithIdentityResolution();
        }

        // Obtiene un IQueryable<T> de la entidad T a partir del contexto de la base de datos
        // Aplica la condición proporcionada en expression al IQueryable<T>, filtrando los elementos
        // que no cumplen con la condición
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) {
            
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTrackingWithIdentityResolution();
            //AsNoTrackingWithIdentityResolution evita que Entity Framework realice un seguimiento de los cambios
            // en los objetos devueltos, lo que puede mejorar el rendimiento en ciertas situaciones
        }

        public void Create(T entity) {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Delete(T entity) {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void Update(T entity) {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void SaveChanges()
        {
            this.RepositoryContext.SaveChanges();
        }

    }
}
