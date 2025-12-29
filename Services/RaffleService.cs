using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using System.IO.Compression;

namespace ChineseRaffleApi.Services
{
    public class RaffleService : IRaffleService
    {
        private readonly IGiftRepo _giftRepo;
        private readonly IUserRepo _userRepo;


        public RaffleService(IGiftRepo giftRepo, IUserRepo userRepo)
        {
            _giftRepo = giftRepo;
            _userRepo = userRepo;
        }
        public async Task<byte[]> DrawRaffleFileAsync()
        {
            var giftsWithTickets = await _giftRepo.GetGiftsWithTicketsAsync();
            var winners = new List<string>();
            var random = new Random();
            int totalRevenue = 0;

            foreach (var gift in giftsWithTickets)
            {
                var tickets = gift.TicketList.ToList();
                if (tickets != null && tickets.Count() > 0)
                {
                    totalRevenue += gift.TicketPrice * gift.TicketList.Count();
                    int winningIndex = random.Next(tickets.Count());
                    var winningTicket = tickets[winningIndex];
                    var winner = await _userRepo.GetUserByIdAsync(winningTicket.UserId);

                    winners.Add($"Gift: {gift.Title}, WinnerId: {winningTicket.UserId}, WinnerName: {winner.UserName}");

                    await _giftRepo.UpdateGiftAsync(gift.Id, new Gift()
                    {
                        Title = gift.Title,
                        CategoryId = gift.CategoryId,
                        DonorId = gift.DonorId,
                        TicketPrice = gift.TicketPrice,
                        Image = gift.Image,
                        WinnerId = winningTicket.UserId
                    });
                }
            }

            using var ms = new MemoryStream();
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                var winnersEntry = archive.CreateEntry("winners.txt");
                using (var entryStream = winnersEntry.Open())
                using (var sw = new StreamWriter(entryStream))
                {
                    foreach (var line in winners)
                        sw.WriteLine(line);
                }

                var revenueEntry = archive.CreateEntry("revenue.txt");
                using (var entryStream = revenueEntry.Open())
                using (var sw = new StreamWriter(entryStream))
                {
                    sw.WriteLine($"Total Revenue: {totalRevenue}");
                }
            }

            return ms.ToArray();



        }
    }
    }
