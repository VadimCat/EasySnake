namespace FyberPlugin
{
    public class VirtualCurrencyErrorResponse
    {

        public ErrorType Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }


        /**
        * Contains different error types for a Virtual Currency response.
        */
        public enum ErrorType
        {
    
            /**
            * Returned response is not formatted in the expected way.
            */
            ERROR_INVALID_RESPONSE = 0,
    
            /**
            * Response doesn't contain a valid signature.
            */
            ERROR_INVALID_RESPONSE_SIGNATURE,
    
            /**
            * The server returned an error. Use {@link VirtualCurrencyErrorResponse#getErrorCode()}
            * and {@link VirtualCurrencyErrorResponse#getErrorMessage()} to extract more details
            * about this error.
            */
            SERVER_RETURNED_ERROR,
    
            /**
            * An error whose cause couldn't be determined.
            */
            ERROR_OTHER
        }
    }




}