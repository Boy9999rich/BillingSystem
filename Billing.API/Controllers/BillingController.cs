using Billing.Application.DTOs;
using Billing.Application.Services;
using Billing.Infrastructure.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Billing.API.Controllers
{
    [Route("api/billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly KafkaProducerService _kafkaProducer;

        public BillingController(
            IBillingService billingService,
            KafkaProducerService kafkaProducer)
        {
            _billingService = billingService;
            _kafkaProducer = kafkaProducer;
        }
        [HttpPost("process")]
        public async Task<IActionResult> ProcessUsage([FromBody] UsageEventDto dto)
        {
            try
            {
                await _kafkaProducer.PublishAsync(dto);

                return Ok(new
                {
                    message = "Event successfully published to Kafka",
                    eventId = dto.EventId,
                    userId = dto.UserId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to publish event",
                    details = ex.Message
                });
            }
        }
        [HttpGet("users/{userId}/balance")]
        public async Task<IActionResult> GetBalance(string userId)
        {
            try
            {
                var balance = await _billingService.GetBalanceAsync(userId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to retrieve balance",
                    details = ex.Message
                });
            }
        }

    }
}
