using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Utilities
{
    /// <summary>
    /// Regex Patterns
    /// </summary>
    public static class RegexPatterns
    {
        /// <summary>
        /// Alphabetic regex.
        /// </summary>
        public const string Alpha = @"^[a-zA-Z]*$";


        /// <summary>
        /// Uppercase Alphabetic regex.
        /// </summary>
        public const string AlphaUpperCase = @"^[A-Z]*$";


        /// <summary>
        /// Lowercase Alphabetic regex.
        /// </summary>
        public const string AlphaLowerCase = @"^[a-z]*$";


        /// <summary>
        /// Alphanumeric regex.
        /// </summary>
        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";


        /// <summary>
        /// Alphanumeric and space regex.
        /// </summary>
        public const string AlphaNumericSpace = @"^[a-zA-Z0-9 ]*$";


        /// <summary>
        /// Alphanumeric and space and dash regex.
        /// </summary>
        public const string AlphaNumericSpaceDash = @"^[a-zA-Z0-9 \-]*$";


        /// <summary>
        /// Alphanumeric plus space, dash and underscore regex.
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscore = @"^[a-zA-Z0-9 \-_]*$";


        /// <summary>
        /// Alphaumieric plus space, dash, period and underscore regex.
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscorePeriod = @"^[a-zA-Z0-9\. \-_]*$";


        /// <summary>
        /// Numeric regex.
        /// </summary>
        public const string Numeric = @"^\-?[0-9]*\.?[0-9]*$";


        /// <summary>
        /// Numeric regex.
        /// </summary>
        public const string Integer = @"^\-?[0-9]*$";


        /// <summary>
        /// Ssn regex.
        /// </summary>
        public const string SocialSecurity = @"\d{3}[-]?\d{2}[-]?\d{4}";


        /// <summary>
        /// E-mail regex.
        /// </summary>
        public const string Email = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";


        /// <summary>
        /// Url regex.
        /// </summary>
        public const string Url = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";

        /// <summary>
        /// Phone
        /// </summary>
        public const string Phone = @"^((\+?[0-9]{2,4}\-[0-9]{3,4}\-)|([0-9]{3,4}\-))?([0-9]{7,8})(\-[0-9]+)?$";

        /// <summary>
        /// Mobile Pattern
        /// </summary>
        public const string Mobile = @"^0?(13[0-9]|15[012356789]|18[0123456789]|14[57])[0-9]{8}$";

        /// <summary>
        /// Zip Pattern
        /// </summary>
        public const string Zip = @"^[1-9][0-9]{5}$";

    }
}
