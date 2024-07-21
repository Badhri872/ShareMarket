namespace Services.AngelWebSocket
{
    public class OutputResponse
    {
        public OutputResponse() 
        {
            Status = true;
            HttpCode = "200";
        }
        public AngelToken TokenResponse { get; set; }
        public bool Status { get; set; }
        public string HttpError { get; set; }
        public string HttpCode { get; set; }
    }
}
