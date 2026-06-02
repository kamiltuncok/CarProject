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
using Microsoft.EntityFrameworkCore;
using DataAccess.Concrete.EntityFramework;
using EFCore.BulkExtensions;


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
            // 1. Fetch all cars async
            var carsResult = await _carService.GetAllAsync();
            var cars = carsResult?.Data ?? new List<Car>();

            // Filter active cars that are not rented
            var activeCars = cars.Where(car => car.Status != CarStatus.Rented).ToList();
            if (!activeCars.Any())
            {
                return new List<PricingResponse>();
            }

            // 2. Fetch all rentals in one query to avoid N+1 DB reads
            var rentalsResult = await _rentalService.GetAllAsync();
            var rentals = rentalsResult?.Data ?? new List<Rental>();

            // Group rentals in-memory by CarId
            var rentalCounts = rentals
                .GroupBy(r => r.CarId)
                .ToDictionary(g => g.Key, g => g.Count());

            // 3. Build a single list of requests
            var requests = activeCars.Select(car => new PricingRequest
            {
                CarId = car.Id,
                CurrentPrice = car.DailyPrice,
                RentalCount = rentalCounts.TryGetValue(car.Id, out var count) ? count : 0,
                SegmentId = car.SegmentId
            }).ToList();

            // 4. POST the single JSON payload to the Python FastAPI batch endpoint
            var json = JsonSerializer.Serialize(requests);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://127.0.0.1:8001/update-prices-batch", content);
            response.EnsureSuccessStatusCode();

            var respJson = await response.Content.ReadAsStringAsync();
            
            // Deserialize response as a JSON array of PricingResponse
            var results = JsonSerializer.Deserialize<List<PricingResponse>>(respJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PricingResponse>();

            // Step A: Extract all CarId values from the results list into a local variable.
            var carIds = results.Select(r => r.CarId).ToList();

            using (var context = new RentACarContext())
            {
                // Step B: Perform exactly ONE database read query to fetch all relevant cars into memory.
                var carsToUpdate = await context.Cars.Where(c => carIds.Contains(c.Id)).ToListAsync();

                // Step C: Iterate through the fetched entities in-memory and update their DailyPrice property by matching them with the results list.
                foreach (var car in carsToUpdate)
                {
                    var result = results.First(r => r.CarId == car.Id);
                    car.DailyPrice = (decimal)result.NewPrice;
                }

                // Step D: Perform exactly ONE database write query using await context.BulkUpdateAsync(carsToUpdate);.
                await context.BulkUpdateAsync(carsToUpdate);
            }

            return results;
        }



        public async Task<string> GetRecommendedActionAsync(int carId)
        {
            var url = $"http://127.0.0.1:8001/api/pricing/recommend?carId={carId}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(jsonString);

            return jObj["RecommendedAction"]?.ToString()
                   ?? throw new Exception("RecommendedAction bulunamadı");
        }

    }
}
