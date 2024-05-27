namespace prueba.Models {
    public class InitializerDB {
        public static void Main(HomeBankingContext context) {
            if (!context.Clients.Any()) {
                var clients = new Client[]
                {
                    new Client{FirstName="Eduardo",LastName="Mendoza",Email="edu@gmail.com",Password="123"},
                    new Client{FirstName="Juan",LastName="Perez",Email="juan@gmail.com",Password="123"},
                    new Client{FirstName="Maria",LastName="Lopez",Email="maria@gmail.com",Password="123" }
                };

                context.Clients.AddRange(clients);
                // Guardar los cambios en la base de datos
                context.SaveChanges();
            }
            else {
                context.Clients.Add(new Client { FirstName = "Laureano", LastName = "Adreotti", Email = "laureano@mindhub.com", Password = "laureanoA2024?*" });
                context.Clients.Add(new Client { FirstName = "Manuel", LastName = "Figueira", Email = "manuel@mindhub.com", Password = "manuelF2024?*" });

                context.SaveChanges() ;
            }
        }
    }
}
