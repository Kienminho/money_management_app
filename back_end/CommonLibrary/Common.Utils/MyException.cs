using Common.Enum;

namespace Common.Utils
{
    [Serializable]
    public class MyException : Exception
    {
        public SystemReturnCode RetCode { get; set; }

        public string Prefix { get; set; }

        public int MsgCode { get; set; }

        private string retMessage;
        public override string Message
        {
            get
            {
                return retMessage;
            }
        }

        public MyException(string message = "") : base(message)
        {
            RetCode = SystemReturnCode.Unexpected;
            Prefix = "";
            MsgCode = 0;
            retMessage = message;
        }

        public MyException(SystemReturnCode returnCode, string message = "") : base(message)
        {
            RetCode = returnCode;
            Prefix = "";
            MsgCode = 0;
            retMessage = message;
        }

        public MyException(SystemReturnCode returnCode, string MyModule, int msgCode, string message = "") : base(message)
        {
            RetCode = returnCode;
            Prefix = MyModule;
            MsgCode = msgCode;
            retMessage = message;
        }

        public MyException(Exception ex)
        {
            RetCode = SystemReturnCode.Error;
            Prefix = "";
            MsgCode = 0;
            retMessage = ex.GetExceptionMessage();
        }
    }
}