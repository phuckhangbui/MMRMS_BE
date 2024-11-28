namespace Common
{
    public static class NumberExtension
    {
        public static double SquareMoneyToNearest1000(double? amount)
        {
            // Round the amount to the nearest 1000
            return (double)Math.Round((decimal)(amount / 1000)) * 1000;
        }

        public static string FormatToVND(double? amount)
        {
            return $"{(amount):N0} ₫";
        }
    }
}
