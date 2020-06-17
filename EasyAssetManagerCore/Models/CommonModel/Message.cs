namespace EasyAssetManagerCore.Model.CommonModel
{

    public class Message
    {
        public MessageTypes MessageType { get; set; }
        public string MessageString { get; set; }

        public Message()
        {
            MessageType = MessageTypes.None;
        }
    }

    public static class MessageHelper
    {
        public static bool IsSuccess(this MessageTypes messteType)
        {
            return messteType == MessageTypes.Success ? true : false;
        }

        public static bool HasError(this MessageTypes messteType)
        {
            return messteType == MessageTypes.Error ? true : false;
        }

        public static bool IsError(this MessageTypes messteType)
        {
            return messteType == MessageTypes.Error ? true : false;
        }


        public static void Success(this Message message, string stringMessage = null)
        {
            if (message == null)
                message = new Message();

            message.MessageType = MessageTypes.Success;
            message.MessageString = stringMessage.IsNotNullOrEmpty() ? stringMessage : ApplicationConstant.GlobalSuccessMessage;
        }

        public static void Error(this Message message, string stringMessage = null)
        {
            if (message == null)
                message = new Message();
            message.MessageType = MessageTypes.Error;
            message.MessageString = stringMessage.IsNotNullOrEmpty() ? stringMessage : ApplicationConstant.GlobalErrorMessage;
        }

        public static void Info(this Message message, string stringMessage = null)
        {
            if (message == null)
                message = new Message();
            message.MessageType = MessageTypes.Info;
            message.MessageString = stringMessage.IsNotNullOrEmpty() ? stringMessage : ApplicationConstant.GlobalInfoMessage;
        }


    }


}



