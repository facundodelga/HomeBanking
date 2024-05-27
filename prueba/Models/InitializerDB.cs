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
        }
    }
}
