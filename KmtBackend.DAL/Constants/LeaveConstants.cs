namespace KmtBackend.DAL.Constants
{
    /// <summary>
    /// Static class that defines all leave-related constants according to Egyptian labor law
    /// </summary>
    public static class LeaveConstants
    {
        #region Leave Types

        public const string RegularAnnualLeave = "Regular Annual Leave";

        public const string CasualLeave = "Casual Leave";

        public const string SickLeave = "Sick Leave";

        public const string MaternityLeave = "Maternity Leave";

        public const string MarriageLeave = "Marriage Leave";

        public const string BereavementLeave = "Bereavement Leave";

        public const string PilgrimageLeave = "Pilgrimage Leave";

        #endregion


        #region Egyptian Labor Law Entitlement Rules

        /// <summary>
        /// Years threshold for senior status
        /// </summary>
        public const int SeniorityYearsThreshold = 10;

        /// <summary>
        /// Regular annual leave days for junior employees (less than 10 years)
        /// </summary>
        public const int JuniorRegularLeaveDays = 15;

        /// <summary>
        /// Regular annual leave days for senior employees (10+ years)
        /// </summary>
        public const int SeniorRegularLeaveDays = 24;

        /// <summary>
        /// Casual leave days for junior employees
        /// </summary>
        public const int JuniorCasualLeaveDays = 6;

        /// <summary>
        /// Casual leave days for senior employees
        /// </summary>
        public const int SeniorCasualLeaveDays = 6;

        /// <summary>
        /// Maximum sick leave days per year
        /// </summary>
        public const int SickLeaveDays = 180;

        /// <summary>
        /// Maternity leave days
        /// </summary>
        public const int MaternityLeaveDays = 90;

        /// <summary>
        /// Bereavement leave days
        /// </summary>
        public const int BereavementLeaveDays = 3;

        /// <summary>
        /// Marriage leave days
        /// </summary>
        public const int MarriageLeaveDays = 3;

        /// <summary>
        /// Pilgrimage leave days
        /// </summary>
        public const int PilgrimageLeaveDays = 30;

        #endregion

        /// <summary>
        /// Gets all leave types with their descriptions and properties
        /// </summary>
        public static Dictionary<string, (string NameAr, string Description, string DescriptionAr, bool IsSeniorityBased, bool AllowCarryOver)> GetAllLeaveTypes()
        {
            return new Dictionary<string, (string, string, string, bool, bool)>
            {
                [RegularAnnualLeave] = (
                    "إجازة اعتيادية",
                    "Regular annual leave based on seniority",
                    "الإجازة السنوية الاعتيادية حسب الأقدمية",
                    true,
                    true
                ),
                [CasualLeave] = (
                    "إجازة عارضة",
                    "Emergency leave for unexpected circumstances",
                    "إجازة طارئة للظروف غير المتوقعة",
                    false,
                    false
                ),
                [SickLeave] = (
                    "إجازة مرضية",
                    "Leave for illness and medical reasons",
                    "إجازة للمرض والأسباب الطبية",
                    false,
                    false
                ),
                [MaternityLeave] = (
                    "إجازة أمومة",
                    "Leave for childbirth and recovery",
                    "إجازة للولادة والتعافي",
                    false,
                    false
                ),
                [MarriageLeave] = (
                    "إجازة زواج",
                    "Leave for employee's marriage",
                    "إجازة لزواج الموظف",
                    false,
                    false
                ),
                [BereavementLeave] = (
                    "إجازة وفاة",
                    "Compassionate leave for death of immediate family",
                    "إجازة لوفاة أحد أفراد العائلة المباشرة",
                    false,
                    false
                ),
                [PilgrimageLeave] = (
                    "إجازة حج",
                    "Leave for religious pilgrimage",
                    "إجازة لأداء فريضة الحج",
                    false,
                    false
                )
            };
        }

        /// <summary>
        /// Gets the default entitlement days for each leave type
        /// </summary>
        public static Dictionary<string, int> GetDefaultEntitlementDays()
        {
            return new Dictionary<string, int>
            {
                // These are just initial values, actual calculation will consider seniority
                [RegularAnnualLeave] = JuniorRegularLeaveDays, // Will be overridden based on seniority
                [CasualLeave] = JuniorCasualLeaveDays,         // Will be overridden based on seniority
                [SickLeave] = SickLeaveDays,
                [MaternityLeave] = MaternityLeaveDays,
                [MarriageLeave] = MarriageLeaveDays,
                [BereavementLeave] = BereavementLeaveDays,
                [PilgrimageLeave] = PilgrimageLeaveDays
            };
        }
    }
}
