using System;
using System.Numerics;

namespace YazilimNedirBSCToken.Utils
{
    public static class BalanceExtensions
    {
        public static double ToFormatted(this BigInteger balance, int decimals)
        {
            return (ulong)balance / (Math.Pow(10, decimals));
        }
    }
}
