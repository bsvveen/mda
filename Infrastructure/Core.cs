namespace MDA.Infrastructure
{
    public class ActionResponse
    {
        public ActionResponse(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        public ActionResponse(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; set; } = false;
        public string Error { get; set; } = string.Empty;
        
    }  
}
