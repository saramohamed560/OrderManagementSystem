namespace OrderManagement.Apis.Errors
{
    public class ApiExceptionRespomse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionRespomse(int code ,string?message=null,string ?details=null):base(code,message)
        {
            Details = details;
        }



    }
}
