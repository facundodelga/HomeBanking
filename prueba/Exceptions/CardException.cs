namespace HomeBanking.Exceptions
{
    [Serializable]
    internal class CardException : Exception
    {
        public CardException()
        {
        }

        public CardException(string message) : base(message)
        {
        }

        public CardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}