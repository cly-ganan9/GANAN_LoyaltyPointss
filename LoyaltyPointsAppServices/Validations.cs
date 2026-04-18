using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LoyaltyPointsAppServices
{
    // Validations: checks if input FORMAT is correct (empty, date format, age, etc.)
    public static class Validations
    {
        public static bool IsEmpty(string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        // Birthdate must be mm/dd/yyyy format
        public static bool IsValidDateFormat(string input)
        {
            return DateTime.TryParseExact(input, "MM/dd/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out _);
        }

        // User must be at least 18 years old
        public static bool IsAtLeast18(string birthdate)
        {
            if (!IsValidDateFormat(birthdate)) return false;

            DateTime dob = DateTime.ParseExact(birthdate, "MM/dd/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);

            int age = DateTime.Today.Year - dob.Year;
            if (DateTime.Today < dob.AddYears(age)) age--;

            return age >= 18;
        }

        // y or n only
        public static bool IsYesOrNo(string input)
        {
            return input == "y" || input == "n";
        }

        // Must be a positive whole number
        public static bool IsValidInt(string input, out int result)
        {
            return int.TryParse(input, out result) && result > 0;
        }

        // Flight class options: 1-4
        public static bool IsValidFlightClassOption(int option)
        {
            return option >= 1 && option <= 4;
        }

        // Reward options: 0-5
        public static bool IsValidRewardOption(int option)
        {
            return option >= 0 && option <= 5;
        }
    }
}
