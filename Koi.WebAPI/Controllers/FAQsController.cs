using Koi.BusinessObjects;
using Koi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Koi.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQsController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        // GET: api/v1/FAQs
        [HttpGet]
        public async Task<ActionResult<List<FAQ>>> GetFAQs()
        {
            var faqs = await _faqService.GetFAQs();
            return Ok(faqs);
        }

        // GET: api/v1/FAQs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FAQ>> GetFAQById(int id)
        {
            var faq = await _faqService.GetFAQById(id);
            if (faq == null)
            {
                return NotFound(new { message = "FAQ not found" });
            }
            return Ok(faq);
        }

        // POST: api/v1/FAQs
        [HttpPost]
        public async Task<ActionResult<FAQ>> CreateFAQ([FromBody] FAQ faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdFAQ = await _faqService.CreateFAQ(faq);
            return CreatedAtAction(nameof(GetFAQById), new { id = createdFAQ.Id }, createdFAQ);
        }

        // PUT: api/v1/FAQs/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<FAQ>> UpdateFAQ(int id, [FromBody] FAQ faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedFAQ = await _faqService.UpdateFAQ(id, faq);
            if (updatedFAQ == null)
            {
                return NotFound(new { message = "FAQ not found" });
            }
            return Ok(updatedFAQ);
        }

        // DELETE: api/v1/FAQs/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFAQ(int id)
        {
            var result = await _faqService.DeleteFAQ(id);
            if (!result)
            {
                return NotFound(new { message = "FAQ not found" });
            }
            return NoContent();
        }
    }
}
