namespace FyberPlugin
{

    public sealed class RequestError
    {

        public string Description { get; set; }

        private RequestError(string description) { Description = description; }

        /**
         * Only devices running Android API level 10 and above are supported.
         */
        public static RequestError DEVICE_NOT_SUPPORTED { get { return new RequestError("Only devices running Android API level 10 and above are supported"); } }
        /**
         * Internet connection error.
         */
        public static RequestError CONNECTION_ERROR { get { return new RequestError("Internet connection error"); } }
        /**
         * An unknown error occurred.
         */
        public static RequestError UNKNOWN_ERROR { get { return new RequestError("An unknown error occurred"); } }
        /**
         * You need to start the SDK.
         */
        public static RequestError SDK_NOT_STARTED { get { return new RequestError("You need to start the SDK"); } }
        /**
         * Context reference cannot be null.
         */
        public static RequestError NULL_CONTEXT_REFERENCE { get { return new RequestError("The context reference cannot be null"); } }
        /**
         * The security token was not provided when starting the SDK.
         */
        public static RequestError SECURITY_TOKEN_NOT_PROVIDED { get { return new RequestError("The security token was not provided when starting the SDK."); } }
        /**
         * An error happened while trying to retrieve ads
         */
        public static RequestError ERROR_REQUESTING_ADS { get { return new RequestError("An error happened while trying to retrieve ads"); } }
        /**
         * The SDK is unable to request right now because it is either already performing a request or showing an ad.
         */
        public static RequestError UNABLE_TO_REQUEST_ADS { get { return new RequestError("The SDK is unable to request right now because it is either already performing a request or showing an ad"); } }

        internal static RequestError FromNative(int ordinal)
        {
            switch(ordinal)
            {
                case 0:
                    return RequestError.DEVICE_NOT_SUPPORTED;
                case 1:
                    return RequestError.CONNECTION_ERROR;
                case 2:
                case 4:
                default:
                    return RequestError.UNKNOWN_ERROR;
                case 3:
                    return RequestError.SDK_NOT_STARTED;
                case 5:
                    return RequestError.NULL_CONTEXT_REFERENCE;
                case 6:
                    return RequestError.SECURITY_TOKEN_NOT_PROVIDED;
                case 7:
                    return RequestError.ERROR_REQUESTING_ADS;
                case 8:
                    return RequestError.UNABLE_TO_REQUEST_ADS;
            }
        }
    }
    

}