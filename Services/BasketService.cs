using AutoMapper;
using ChineseRaffleApi.Dto;
using ChineseRaffleApi.Models;
using ChineseRaffleApi.Repository.DI;
using ChineseRaffleApi.Services.DI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChineseRaffleApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly KafkaProducerService _kafkaProducerService;

        public BasketService(IBasketRepo basketRepo, IMapper mapper, ITicketService ticketService, KafkaProducerService kafkaProducerService)
        {
            _basketRepo = basketRepo;
            _ticketService = ticketService;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<GetBasketDto?> GetBasketByIdAsync(int id)
        {
            var basket = await _basketRepo.GetBasketByIdAsync(id);
            return _mapper.Map<GetBasketDto>(basket);
        }

        public async Task<IEnumerable<GetBasketDto>> GetBasketsByUserIdAsync(int userId)
        {
            var basket = await _basketRepo.GetBasketsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<GetBasketDto>>(basket);
        }

        public async Task<IEnumerable<GetBasketDto>> GetBasketsByGiftIdAsync(int giftId)
        {
            var basket = await _basketRepo.GetBasketsByGiftIdAsync(giftId);
            return _mapper.Map<IEnumerable<GetBasketDto>>(basket);
        }

        public async Task<int?> AddBasketAsync(AddBasketDto basketDto)
        {
            var basket = _mapper.Map<Basket>(basketDto);

            var existingBasket = await _basketRepo.GetByUserAndGiftAsync(basket.UserId, basket.GiftId);

            if (existingBasket != null)
            {
                existingBasket.Quantity += basket.Quantity;
                await _basketRepo.UpdateBasketAsync(existingBasket.Id, existingBasket);

                // שליחת הודעה ל-Kafka גם בעת עדכון פריט קיים בסל
                await SendKafkaNotificationAsync(existingBasket.Id, basket.UserId, basket.Quantity);

                return existingBasket.Id;
            }

            var basketId = await _basketRepo.AddBasketAsync(basket);

            if (basketId.HasValue)
            {
                // שליחת הודעה ל-Kafka עבור פריט חדש בסל
                await SendKafkaNotificationAsync(basketId.Value, basket.UserId, basket.Quantity);
            }

            return basketId;
        }

        public async Task UpdateBasketAsync(int id, UpdateBasketDto basket)
        {
            if (basket.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var existingBasket = await _basketRepo.GetBasketByIdAsync(id);

            if (existingBasket == null)
                throw new KeyNotFoundException($"Basket with id {id} not found");

            existingBasket.Quantity = basket.Quantity;
            await _basketRepo.UpdateBasketAsync(id, existingBasket);
        }

        public async Task DeleteBasketAsync(int id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }

        public async Task<bool> BasketExistsAsync(int id)
        {
            return await _basketRepo.BasketExistsAsync(id);
        }

        public async Task BuyTicketsFromBasket(int userId)
        {
            var baskets = await _basketRepo.GetBasketsByUserIdAsync(userId);
            int totalTicketsPurchased = 0;

            foreach (var basket in baskets)
            {
                var gift = basket.Gift;
                if (gift == null)
                    throw new Exception($"Gift with id {basket.GiftId} not found");

                var ticket = new AddTicketDto
                {
                    UserId = userId,
                    GiftId = gift.Id
                };

                for (int i = 0; i < basket.Quantity; i++)
                {
                    await _ticketService.AddTicketAsync(ticket);
                    totalTicketsPurchased++;
                }

                await _basketRepo.DeleteBasketAsync(basket.Id);
            }

            // הפקת אירוע Kafka מרכזי ברגע שבוצעה רכישת כרטיסים סופית מהסל
            if (totalTicketsPurchased > 0)
            {
                var message = new OrderTransactionMessage
                {
                    OrderId = userId, // מזהה המשתמש שביצע את הרכישה
                    CustomerName = $"User_{userId}_Checkout",
                    TotalAmount = totalTicketsPurchased, // כמות הכרטיסים הכוללת שנרכשה בעסקה זו
                    CreatedAt = DateTime.UtcNow
                };

                await _kafkaProducerService.PublishTransactionAsync(message);
            }
        }

        // מתודת עזר פרטית למניעת כפל קוד בעת שליחת הודעות זמניות מהסל
        private async Task SendKafkaNotificationAsync(int orderId, int userId, int quantity)
        {
            var message = new OrderTransactionMessage
            {
                OrderId = orderId,
                CustomerName = $"User:{userId}",
                TotalAmount = quantity,
                CreatedAt = DateTime.UtcNow
            };

            await _kafkaProducerService.PublishTransactionAsync(message);
        }
    }
}