namespace HomeBanking.Exceptions {
    public class ClientExistException : Exception {
        public ClientExistException(string message) : base(message) {
        }
    }
}
