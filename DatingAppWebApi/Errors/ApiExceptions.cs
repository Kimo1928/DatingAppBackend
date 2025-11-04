namespace DatingAppWebApi.Errors
{
    public class ApiExceptions
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public string? Details { get; set; }
        public ApiExceptions(int statusCode,string Message,string?Details) { 
        this.StatusCode = statusCode;
        this.Message = Message;
        this.Details = Details;
        }
       
    }
}
