namespace Common
{
    public static class NumberExtension
    {
        public static double SquareMoneyToNearest1000(double amount)
        {
            // Round the amount to the nearest 1000
            return (double)Math.Round(amount / 1000.0) * 1000;
        }

    }
}
