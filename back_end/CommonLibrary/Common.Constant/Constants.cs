namespace Common.Constant;

public static class Constants
{
    public static string Schema = "dbo";
    public static int TimeExtendForgotPassword = 120;
    public static int DefaultTimeExpired = 300;
}

public class CommonMessage
{
    public static string Message_DataNotFound = "{0} is not found";
    public static string Message_Required = "{0} is required";
    public static string Message_NotExistsData = "Not exist data";
    public static string Message_InvalidRequest = "{0} is invalid";
    public static string Message_Exists = "{0} has exists";
    public static string Message_NotFound = "Data not found";
    public static string Message_Update = "{0} was updated.";
}

public class CacheKey
{
    public const string SysActivity = "SysActivity";
    public const string SysRole = "SysRole";
    public const string SysUserRole = "SysUserRole";
    public const string BlackListSms = "BlackListSms";
    public const string ListUser = "TmsUsers";
}

public class CacheTime
{
    public static TimeSpan CommmonUncache = TimeSpan.FromMinutes(10);
    public static TimeSpan BlackList = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
    public static TimeSpan QrCode = TimeSpan.FromMinutes(5);
    public static TimeSpan OTP = TimeSpan.FromMinutes(5);
}

public class MyFormatConst
{
    public const string CM_Format_Money_Style = "CM_Format_Money_Style";
    public const string CM_Format_Money_Unit_VND_ShortText = "CM_Format_Money_Unit_VND_ShortText";
    public const string CM_Format_LongDate24_None_Second = "CM_Format_LongDate24_None_Second";
    public const string CM_Format_Money_Unit_VND = "CM_Format_Money_Unit_VND";
    public const string CM_Format_Money_Unit_USD = "CM_Format_Money_Unit_USD";
    public const string CM_Format_ShortDate = "CM_Format_ShortDate";
    public const string CM_Format_Money_Style_With_Precision_Two = "CM_Format_Money_Style_With_Precision_Two";
}

public class MyColor
{
    public const string APP_BLUE = "#1976D2";
    public const string APP_GREEN = "#388E3C";
    public const string APP_ORANGE = "#F57C00";
    public const string APP_LIGHT_GRAY = "#CDCDCD";
    public const string APP_BLACK = "#000000";
    public const string APP_RED = "#F44336";
    public const string COLOR_GRAY = "#BDBDBD";
    public const string COLOR_BLACK = "#000000";
    public const string COLOR_WHITE = "#FFFFFF";
    public const string COLOR_GREEN = "#43A047";
    public const string COLOR_RED = "#F44336";
    public const string COLOR_BLUE = "#1976D2";
    public const string COLOR_YELLOW = "#FBC02D";
    public const string COLOR_ORANGE = "#F57C00";
    public const string COLOR_LIGHTGRAY = "#D8D8D8";
}

public class MyModule
{
    public const string SYSTEM = "SYS";
}