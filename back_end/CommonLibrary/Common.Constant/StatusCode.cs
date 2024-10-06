namespace Common.Constant
{
    public enum StatusCode
    {
        UnExpectedError = -99,
        AccessDenied = 403,
        Success = 200,
        DataInputInvalid = 400,
        Error = 500,

        AccessTokenInvalid = 401, //Require Activate again
        AccessTokenExpiry = 401, //Require renew token again
        RefreshTokenExpiry = 401, //Require Activate again

        DataNotFound = 404,
        DataInputIsRequire = 400,
        DataIsLock = 423,

        Unauthorized = 401 //the requested resource requires authentication.
    }

    public static class StatusMessage
    {
        public const string Success = "Successful.";
        public const string UnExpectedError = "Unexpected error.";
        public const string AccessDenied = "Access is denied.";
        public const string DataInputInvalid = "Input data is invalid.";
        public const string Error = "Error";
        public const string Failure = "Failure";
        public const string AccessTokenInvalid = "Access token invalid.";
        public const string DataNotFound = "Data cannot found.";
        public const string DataInputIsRequire = "{0} is require.";
        public const string DataIsLock = "Lock! Please contact Adminstrator.";
        public const string Unauthorized = "Your app requires authentication.";
        public const string Unavailable = "Unavailable";
    }
}