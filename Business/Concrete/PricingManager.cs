using Business.Abstract;
using Core.Entities.Requests;
using Newtonsoft.Json.Linq;
using Entities.Concrete;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Business.Concrete
{
    public class PricingManager:IPricingService
    {
        private readonly HttpClient _httpClient;
        private readonly IRentalService _rentalService;
        private readonly ICarService _carService;

        public PricingManager(HttpClient httpClient, ICarService carService,IRentalService rentalService)
        {
            _httpClient = httpClient;
            _carService = carService;
            _rentalService = rentalService;
        }

        public async Task<List<PricingResponse>> UpdateAllPricesAsync()
        {
            var cars = _carService.GetAll().Data;
            var updates = new List<PricingResponse>();

            foreach (var car in cars)
            {
                if (car.Status == CarStatus.Rented) continue;

                var rentalCount = _rentalService.GetRentalsByCarId(car.Id)?.Data?.Count ?? 0;

                var request = new PricingRequest
                {
                    CarId = car.Id,
                    CurrentPrice = car.DailyPrice,
                    RentalCount = rentalCount
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://127.0.0.1:8000/update-price", content);
                response.EnsureSuccessStatusCode();

                var respJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PricingResponse>(respJson);

                // DB güncelleme
                car.DailyPrice = result.NewPrice;
                _carService.Update(car);

                updates.Add(result);
            }

            return updates;
        }



        public async Task<string> GetRecommendedActionAsync(int carId)
        {
            var url = $"http://127.0.0.1:8000/api/pricing/recommend?carId={carId}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(jsonString);

            return jObj["RecommendedAction"]?.ToString()
                   ?? throw new Exception("RecommendedAction bulunamadı");
        }

    }
}
