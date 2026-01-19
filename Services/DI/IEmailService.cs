namespace ChineseRaffleApi.Services.DI
{
    public interface IEmailService
    {
        Task SendWinnerEmailAsync(string toEmail, string userName, string giftTitle);
    }
}
