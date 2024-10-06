namespace Common.Enum
{
    public static class EnumExtensions
    {
        public static T GetValue<T>(this System.Enum enumeration)
        {
            T result = default(T);

            try
            {
                result = (T)Convert.ChangeType(enumeration, typeof(T));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return result;
        }
    }

    public enum Platform
    {
        None = 0,
        iOS = 1,
        Android = 2
    }

    public enum LanguageType
    {
        vi = 1,
        en = 2
    }

    public enum SystemReturnCode
    {
        Success = 0,

        DataInputInvalid = 1002,
        DataInputIsRequire = 1003,
        DataNotFound = 1004,

        OldVersionAPI = 2001,
        DeviceUUIDNotFound = 2002,
        ApplicationNotSupport = 2003,

        SessionTokenExpired = 9000,
        RefreshTokenExpired = 9009,

        RequiredUpdateNewVersion = 9992,
        ServiceIsUpdating = 9993,
        AccessDenied = 9994,
        Unauthorized = 9995,
        BadRequest = 9996,
        RequestInvalid = 9997,
        Unexpected = 9998,
        Error = 9999,
    }

    public enum DateTimeType
    {
        Long24H,
        Long12H,
        OnlyDate,
        OnlyDateVI,
        OnlyLongTime24H,
        OnlyLongTime12H,
        OnlyShortTime24H,
        OnlyShortTime12H,
        OnlyDateInternational,
        DDMMM,
        TimeOrDate
    }

    public enum ActivityType
    {
        Read,
        Create,
        Update,
        Delete
    }
}