using System;

namespace VacationMasters.UnitTests.Infrastructure
{
    public static class CreateRandom
    {
        private static readonly Random Generator = new Random();

        public static TEnum Enum<TEnum>()
        {
            int value = Int();
            return (TEnum)(object)value;
        }

        public static int Int()
        {
            return Generator.Next();
        }

        public static long Long()
        {
            return Int();
        }

        public static string String()
        {
            return Int().ToString();
        }
    }
}
