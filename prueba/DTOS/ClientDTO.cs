using HomeBanking.DTOS;
using prueba.Models;
using System.Text.Json.Serialization;

namespace prueba.DTOS {
    public class ClientDTO {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountClientDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Loans { get; set; }
        public ICollection<CardDTO> Cards { get; set; }

        public ClientDTO(Client client) {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;

            //Console.WriteLine(client.Accounts.ToArray());

            Accounts = client.Accounts.Select(ac => new AccountClientDTO(ac)).ToList();
            Loans = client.ClientLoans.Select(cl => new ClientLoanDTO(cl)).ToList();
            Cards = client.Cards.Select(card => new CardDTO(card)).ToList();

        }

        public ClientDTO(SignUpDTO dto) { 
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Email = dto.Email;
        }
    }
}
