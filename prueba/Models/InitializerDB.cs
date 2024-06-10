using HomeBanking.Models;

namespace prueba.Models {
    public class InitializerDB {

        
        public static void Main(HomeBankingContext context) {


            if (!context.Clients.Any()) { // Si la tabla esta vacia entro
                var clients = new Client[]
                {
                    new Client{FirstName="Victor",LastName="Coronado",Email="vcoronado@gmail.com",Password="123"},
                    new Client{FirstName="Facundo",LastName="Delgado",Email="facudelga3@gmail.com",Password="123"},
                    new Client{FirstName="Maria",LastName="Lopez",Email="maria@gmail.com",Password="123" },
                    new Client { FirstName = "Laureano", LastName = "Adreotti", Email = "laureano@mindhub.com", Password = "laureanoA2024?*" },
                    new Client { FirstName = "Manuel", LastName = "Figueira", Email = "manuel@mindhub.com", Password = "manuelF2024?*" }
                };

                context.Clients.AddRange(clients);
                // Guardar los cambios en la base de datos
                context.SaveChanges();
            };

            if (!context.Accounts.Any()) {
                creoCuentas(context);
            }

            if (!context.Transactions.Any()) {

                creoTransacciones(context);
            }

            if (!context.Loans.Any()) {
                creoPrestamos(context);

                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null) {
                    
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null) {
                        var clientLoan1 = new ClientLoan {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null) {
                        var clientLoan2 = new ClientLoan {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null) {
                        var clientLoan3 = new ClientLoan {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                }

                var client2 = context.Clients.FirstOrDefault(c => c.Email == "laureano@mindhub.com");
                if (client2 != null) {
                    var accounts = new Account[] {

                        new Account{Number = "LAN001",
                            CreationDate=DateTime.Now,
                            ClientId = client2.Id,
                            Balance=300000
                        },

                        new Account{Number = "LAN002",
                            CreationDate=DateTime.Now.AddMonths(3),
                            ClientId = client2.Id,
                            Balance=1000
                        },

                        new Account{Number = "LAN003",
                            CreationDate=DateTime.Now.AddMonths(12),
                            ClientId = client2.Id,
                            Balance=50000
                        }

                    };

                    context.Accounts.AddRange(accounts);
                    //context.SaveChanges();

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null) {
                        var clientLoan4 = new ClientLoan {
                            Amount = 1000000,
                            ClientId = client2.Id,
                            LoanId = loan3.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan4);
                    }

                    
                }
                context.SaveChanges();

            }

            if (!context.Cards.Any()) {
                var client = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");

                if (client != null) {
                    var cards = new Card[] {
                         new Card {
                            ClientId= client.Id,
                            CardHolder = client.FirstName + " " + client.LastName,
                            Type = CardType.DEBIT,
                            Color = CardColor.GOLD,
                            Number = "3325-6745-7876-4445",
                            Cvv = 990,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(4),
                        },
                        new Card {
                            ClientId= client.Id,
                            CardHolder = client.FirstName + " " + client.LastName,
                            Type = CardType.CREDIT,
                            Color = CardColor.TITANIUM,
                            Number = "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(5),
                        },
                    };

                    context.Cards.AddRange(cards);
                }

                var client2 = context.Clients.FirstOrDefault(c => c.Email == "manuel@mindhub.com");

                if (client2 != null) {
                    var cards = new Card[] {
                        new Card {
                            ClientId = client2.Id,
                            CardHolder = client2.FirstName + " " + client2.LastName,
                            Type = CardType.DEBIT,
                            Color = CardColor.TITANIUM,
                            Number = "3555-1235-7906-2245",
                            Cvv = 272,
                            FromDate = DateTime.Now.AddDays(1),
                            ThruDate = DateTime.Now.AddYears(5),
                        },

                        new Card {
                            ClientId = client2.Id,
                            CardHolder = client2.FirstName + " " + client2.LastName,
                            Type = CardType.CREDIT,
                            Color = CardColor.SILVER,
                            Number = "9090-2315-7456-3945",
                            Cvv = 909,
                            FromDate = DateTime.Now.AddMonths(1),
                            ThruDate = DateTime.Now.AddYears(3),
                        },

                        new Card {
                            ClientId = client2.Id,
                            CardHolder = client2.FirstName + " " + client2.LastName,
                            Type = CardType.CREDIT,
                            Color = CardColor.GOLD,
                            Number = "4390-8595-8686-2245",
                            Cvv = 848,
                            FromDate = DateTime.Now.AddMonths(1),
                            ThruDate = DateTime.Now.AddYears(3),
                        }
                    };

                    context.Cards.AddRange(cards);
                }

                context.SaveChanges();
            }

        }

        public static void creoCuentas(HomeBankingContext context) {
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
                        },

                        new Account{Number = "VIN003",
                            CreationDate=DateTime.Now,
                            ClientId = 2,
                            Balance=100000000
                        }

                    };

                context.Accounts.AddRange(accounts);
                context.SaveChanges();

            }
        }

        public static void creoTransacciones(HomeBankingContext context) {
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

        public static void creoPrestamos(HomeBankingContext context) {
            var loans = new Loan[] {
                    new Loan { Name = "Hipotecario", MaxAmount = 40000000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 22000000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 3000000, Payments = "6,12,24,36" },
                    new Loan { Name = "Prendarios", MaxAmount = 4000000, Payments = "6,12,24,36" },
                    new Loan { Name = "Estudios", MaxAmount = 10000000, Payments = "6,12,24,36,48,60" }
                };
            //agrego los tipos de prestamo
            context.Loans.AddRange(loans);
            context.SaveChanges();
        }


    }
}

