using Billing.Application.DTOs;
using Billing.Application.Services;
using Billing.Infrastructure.EventQueue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Billing.API.Controllers
{
    [Route("api/billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly InMemoryEventQueue _queue;

        public BillingController(
            IBillingService billingService,
            InMemoryEventQueue queue)
        {
            _billingService = billingService;
            _queue = queue;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessUsage(
            UsageEventDto dto)
        {
            await _queue.PublishAsync(dto);

            return Ok("Event queued");
        }





        //private readonly IBillingService _billingService;

        //public BillingController(IBillingService billingService)
        //{
        //    _billingService = billingService;
        //}

        //// Eventni qo'lda yuborib test qilish uchun
        //[HttpPost("process")]
        //public async Task<IActionResult> ProcessUsage(
        //    [FromBody] UsageEventDto dto)
        //{
        //    await _billingService.ProcessUsageAsync(dto);
        //    return Ok(new { message = "Event processed" });
        //}

        //// User balansini olish
        //[HttpGet("users/{userId}/balance")]
        //public async Task<IActionResult> GetBalance(string userId)
        //{
        //    var result = await _billingService.GetBalanceAsync(userId);
        //    return Ok(result);
        //}
    }
}
