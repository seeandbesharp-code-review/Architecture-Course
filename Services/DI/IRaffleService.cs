namespace ChineseRaffleApi.Services.DI
{
    public interface IRaffleService
    {
        Task<byte[]> DrawRaffleFileAsync();
    }
}