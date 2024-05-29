using HomeBanking.Models;

namespace prueba.Models {
    public class InitializerDB {
        public static void Main(HomeBankingContext context) {


            if (!context.Clients.Any()) { // Si la tabla esta vacia entro
                var clients = new Client[]
                {
                    new Client{FirstName="Victor",LastName="Coronado",Email="vcoronado@gmail.com",Password="123"},
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
                var cliente = context.Clients.FirstOrDefault(cl => cl.Email == "vcoronado@gmail.com");

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

            if (!context.Transactions.Any()) {
                var cuenta = context.Accounts.FirstOrDefault(ac => ac.Number == "VIN001");

                if (cuenta != null) {
                    var transacciones = new Transaction[] {
                        new Transaction{
                            Amount = 1000,
                            Type = TransactionType.CREDIT,
                            AccountId = cuenta.Id,
                            Date = DateTime.Now.AddHours(-10),
                            Description = "Transferencia de Laureano"
                        },

                        new Transaction{
                            Amount = -1700,
                            Type = TransactionType.DEBIT,
                            AccountId = cuenta.Id,
                            Date= DateTime.Now.AddHours(-4),
                            Description = "Pago servicio Youtube"
                        },

                        new Transaction {
                            Amount = -10000,
                            Type = TransactionType.DEBIT,
                            AccountId = cuenta.Id,
                            Date = DateTime.Now.AddHours(-2),
                            Description = "Pago artTicket:Asspera"
                        },

                        new Transaction {
                            Amount = 150000,
                            Type = TransactionType.CREDIT,
                            AccountId = cuenta.Id,
                            Date = DateTime.Now.AddDays(1).AddHours(4),
                            Description = "Devolucion de pago MeLi"
                        }
                    };

                    context.Transactions.AddRange(transacciones);
                    context.SaveChanges();
                }
            }

        }

    }
}

