using System.ComponentModel;

namespace Auth.Models.ViewModels
{
    public class Response<T>  where T : class 
    {
        
        public T Result { get; set; }
        public string  Message { get; set; }
        public bool Success { get; set; }

        public Response(bool success, string message, T result = null)
        {
            Success = success;
            Result = result;
            Message = message;
        }
        
    }
}