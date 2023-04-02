using System;

namespace OobDev.Calculators
{
    public static class Calculator
    {
        public static int CalculateDays(DateTime fromDate, DateTime toDate, InterestAccrualType accrualType = InterestAccrualType.APR360)
        {
            switch (accrualType)
            {
                case InterestAccrualType.APR360:
                default:
                    {
                        if (fromDate.Day == 31)
                            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day - 1);

                        if (toDate.Day == 31 && (fromDate.Day == 31 || fromDate.Day == 30))
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day - 1);

                        return (toDate.Year - fromDate.Year) * 360 + (toDate.Month - fromDate.Month) * 30 + (toDate.Day - fromDate.Day);
                    }
            }
        }

    }
    public enum InterestAccrualType
    {
        APR360
    }
}
