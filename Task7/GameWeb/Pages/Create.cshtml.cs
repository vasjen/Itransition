using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameWeb.Pages;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ILogger<CreateModel> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult OnGet()
    { 
        return Page();
    }
}

