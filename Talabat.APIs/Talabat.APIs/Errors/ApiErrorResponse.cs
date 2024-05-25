namespace Talabat.APIs.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int StatusCode,string? message=null)
        {
            this.StatusCode = StatusCode;
            this.Message = message ?? GetDefaulMessageForStatusCode(StatusCode);
                                // ?? => معناها لو الرسالة فاضية نفذ الامر اللي بعديه
        }

        public string? GetDefaulMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "a BadRequest, You Have Made",
                401 => "Authorized, You Have Not",
                404 => "Resources Not Found",
                500 => "There is Server Error",
                _   => null
            };
        }
    }
}
