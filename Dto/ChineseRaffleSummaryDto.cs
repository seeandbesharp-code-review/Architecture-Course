namespace ChineseRaffleApi.Dto
{
    public class ChineseRaffleSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalTicketsSold { get; set; }
        public int TotalDonors { get; set; }
        public int TotalGifts { get; set; }
        public string TopSellingGiftName { get; set; }
    }
}
