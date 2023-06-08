using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messenger.Pages;

public class NewModel : PageModel
{
    private readonly ILogger<NewModel> _logger;
    string User {get;set;}

    public NewModel(ILogger<NewModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

