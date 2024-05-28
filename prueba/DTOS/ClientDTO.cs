using Microsoft.IdentityModel.Tokens;
using prueba.Models;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace prueba.DTOS {
    public class ClientDTO {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }

        public ClientDTO(Client client) {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;

            Console.WriteLine(client.Accounts.ToArray());

            Accounts = client.Accounts.Select(ac => new AccountDTO(ac)).ToList();

        }
    }
}
