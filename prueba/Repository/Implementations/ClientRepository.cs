using Microsoft.EntityFrameworkCore;
using prueba.Models;

namespace prueba.Repository.Implementations {
    public class ClientRepository : RepositoryBase<Client>, IClientRepository {

        public ClientRepository(HomeBankingContext repository) : base(repository) {

        }

        public Client FindById(long id) {
            // Utiliza el método FindByCondition de la clase base RepositoryBase<T>
            // para obtener un IQueryable<Client> que cumpla la condición especificada
            return FindByCondition(client => client.Id == id)
                // Incluye los datos relacionados de las cuentas (Accounts) del cliente
                .Include(client => client.Accounts)
                //aca agrego la lista de clientloans y luego la referencia al prestamo
                .Include(cloans=> cloans.ClientLoans).ThenInclude(loan => loan.Loan) 
                .FirstOrDefault();// Toma el primer elemento del IQueryable o devuelve null si no hay elementos
        }

        public IEnumerable<Client> GetAllClients() {
            return FindAll().Include(c => c.Accounts)
                .Include(cloans => cloans.ClientLoans).ThenInclude(loan => loan.Loan)
                .ToList();
        }

        public void Save(Client client) {
            Create(client);
            SaveChanges();
        }
    }
}
