using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using Microsoft.IdentityModel.Tokens;
using prueba.Repository;

namespace HomeBanking.Services.Implementations
{
    public class ClientLoanService : IClientLoanService {
        private ILoanRepository _loanRepository;
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;

        public ClientLoanService(ILoanRepository loanRepository, IAccountRepository accountRepository, IClientRepository clientRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository) {
            
            _loanRepository = loanRepository;
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        public ServiceResponse<ClientLoan> MakeLoan(LoanApplicationDTO loanDTO, string email) {

            //Verificar que el prestamo exista
            var loan = _loanRepository.FindById(loanDTO.LoanId);
            if (loan == null) {
                return new ServiceResponse<ClientLoan>(null, 403, "Tipo de prestamo no existente");
            }

            //Que el monto NO sea 0 y que no sobrepase el maximo autorizado
            if (loanDTO.Amount <= 0 || loanDTO.Amount > loan.MaxAmount)
                return new ServiceResponse<ClientLoan>(null, 403, "monto 0 o sobrepasa el maximo autorizado");

            //Que los payments no lleguen vacios.
            if (loanDTO.Payments.IsNullOrEmpty())
                return new ServiceResponse<ClientLoan>(null, 403, "Campo payment vacio");

            //si la cantidad de cuotas no esta disponible para el prestamo solicitado
            HashSet<string> payments = new HashSet<string>(loan.Payments.Split(','));
            if (!payments.Contains(loanDTO.Payments))
                return new ServiceResponse<ClientLoan>(null, 403, "Cantidad de cuotas no disponible");

            //Que exista la cuenta de destino
            var account = _accountRepository.FindByNumber(loanDTO.ToAccountNumber);
            if (account == null) {
                return new ServiceResponse<ClientLoan>(null, 403, "No existe la cuenta destino");
            }

            //Que la cuenta de destino pertenezca al Cliente autentificado
            var client = _clientRepository.FindById(account.ClientId);
            if (client == null || client.Email != email) {
                return new ServiceResponse<ClientLoan>(null, 403, "Cuenta no perteneciente al usuario autentificado");
            }

            var creditTransaction = new Transaction {
                AccountId = account.Id,
                Type = TransactionType.CREDIT,
                Amount = loanDTO.Amount,
                Date = DateTime.Now,
                Description = loan.Name + " - Loan Approved"
            };

            _transactionRepository.Save(creditTransaction);

            var clientLoan = new ClientLoan() {
                Amount = loanDTO.Amount * 1.20, //Cuando guardes clientLoan el monto debes multiplicarlo por el 20%
                LoanId = loan.Id,
                ClientId = client.Id,
                Payments = loanDTO.Payments,
            };

            _clientLoanRepository.save(clientLoan);

            return new ServiceResponse<ClientLoan>(clientLoan, 201, "Prestamo creado con exito");
        }

        public void save(ClientLoan clientLoan) {
            _clientLoanRepository.save(clientLoan);
        }
    }
}
