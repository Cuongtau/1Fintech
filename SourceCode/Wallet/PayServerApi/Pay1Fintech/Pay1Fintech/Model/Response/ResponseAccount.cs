namespace Pay1Fintech.Model.Response
{
    public class RegisterResultModel: CommonResponse
    {
        public string Description { get; set; }
    }
    public class AuthenticateResultModel: CommonResponse
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
        public string ReturnUrl { get; set; }
    }
}
