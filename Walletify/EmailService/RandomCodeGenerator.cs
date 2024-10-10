namespace Walletify.EmailService
{
    public static class RandomCodeGenerator
    {
        private static readonly Random _random = new();

        public static string GenerateRandomCode()
        {
            int num = _random.Next(100000, 999999);
            return num.ToString();
        }
    }

}
