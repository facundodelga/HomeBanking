using prueba.Models;
using System.Text.Json.Serialization;

namespace prueba.DTOS {
    public class ClientDTO {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }

        public ClientDTO(Client client) {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;

            foreach (var account in client.Accounts) {
                this.Accounts.Add(new AccountDTO(account));
            }
        }
    }
}
