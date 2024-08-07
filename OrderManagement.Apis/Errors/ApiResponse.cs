
namespace OrderManagement.Apis.Errors
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int code ,string?message=null)
        {
            Code = code;
            Message = message ?? GetDefaultMessageBasedOnCode(code);
        }
        private string? GetDefaultMessageBasedOnCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Requst , You Have Made ",
                401 => "You Are Not Authorized",
                404 => "Resource Not Found",
                500 => "Server Error",
                _ => null
            };
        }
    }
}
