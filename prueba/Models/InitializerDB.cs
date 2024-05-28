namespace prueba.Models {
    public class InitializerDB {
        public static void Main(HomeBankingContext context) {


            if (!context.Clients.Any()) { // Si la tabla esta vacia entro
                var clients = new Client[]
                {
                    new Client{FirstName="Eduardo",LastName="Mendoza",Email="edu@gmail.com",Password="123"},
                    new Client{FirstName="Juan",LastName="Perez",Email="juan@gmail.com",Password="123"},
                    new Client{FirstName="Maria",LastName="Lopez",Email="maria@gmail.com",Password="123" },
                    new Client { FirstName = "Laureano", LastName = "Adreotti", Email = "laureano@mindhub.com", Password = "laureanoA2024?*" },
                    new Client { FirstName = "Manuel", LastName = "Figueira", Email = "manuel@mindhub.com", Password = "manuelF2024?*" }
                };

                context.Clients.AddRange(clients);
                // Guardar los cambios en la base de datos
                context.SaveChanges();
            };

            if (!context.Accounts.Any()) {
                var cliente = context.Clients.FirstOrDefault(cl => cl.Email == "laureano@mindhub.com");

                if (cliente != null) { //si encuentro el cliente
                    var accounts = new Account[] {

                        new Account{Number = "VIN001",
                            CreationDate=DateTime.Now,
                            ClientId = cliente.Id,
                            Balance=30000
                        },

                        new Account{Number = "VIN002",
                            CreationDate=DateTime.Now,
                            ClientId = cliente.Id,
                            Balance=10000
                        }

                    };

                    context.Accounts.AddRange(accounts);
                    context.SaveChanges();

                }
            }

        }

    }
}

