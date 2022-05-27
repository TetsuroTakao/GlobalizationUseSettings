using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace GlobalizationUseSettings.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration Configuration;
    private readonly ApplicationContext Context;
    private readonly ILogger<IndexModel> _logger;
    private Regex rx { get; set; } = new Regex(@"\w.*",RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        Configuration = configuration;
        Context = new ApplicationContext();
        Configuration.GetSection(ApplicationContext.Japanese).Bind(Context);
        _logger = logger;
    }

    public void OnGet()
    {
            ViewData["CultureKey"] = Context.CultureKey;
            ViewData["Brand"] = Context.Brand;
            ViewData["Pages"] = Context.Pages;
            ViewData["Languages"] = Context.Languages;
            rx = new Regex(@"(\w.*)=(\w.*)",RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var m = rx.Matches(Request.QueryString.ToString());
            if(m.FirstOrDefault() != null){
                if(Request.Query["handler"].Count > 0)
                {
                    Response.Redirect("/" + Request.Query["handler"]!.ToString());
                }
            }
            ViewData["Message"] = "index";
            var p = Request.HttpContext.GetRouteData()!.Values["Page"]!;
            rx = new Regex(@"\w.*",RegexOptions.Compiled | RegexOptions.IgnoreCase);
            m = rx.Matches(p.ToString()!);
            ViewData["Action"] = m.FirstOrDefault()!.Value;
            if (ViewData["Action"]!.GetType() == typeof(String)) {
                var current = Context.Pages.Where(p => p.ActionKey == ViewData["Action"]!.ToString()).FirstOrDefault();
                if(current != null){
                    ViewData["Title"] = current.Title;
                }
            }
    }
    public void OnPost(){
        ViewData["CultureKey"] = Context.CultureKey;
        ViewData["Brand"] = Context.Brand;
        ViewData["Pages"] = Context.Pages;
        ViewData["Languages"] = Context.Languages;
        rx = new Regex(@"(\w.*)=(\w.*)",RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var m = rx.Matches(Request.QueryString.ToString());
        if(m.FirstOrDefault() != null){
            if(Request.Query["handler"].Count > 0)
            {
                Response.Redirect("/" + Request.Query["handler"]!.ToString());
            }
        }
        ViewData["Message"] = "post";
        var p = Request.HttpContext.GetRouteData()!.Values["Page"]!;
        rx = new Regex(@"\w.*",RegexOptions.Compiled | RegexOptions.IgnoreCase);
        m = rx.Matches(p.ToString()!);
        ViewData["Action"] = m.FirstOrDefault()!.Value;
        if (ViewData["Action"]!.GetType() == typeof(String)) {
            var current = Context.Pages.Where(p => p.ActionKey == ViewData["Action"]!.ToString()).FirstOrDefault();
            if(current != null){
                ViewData["Title"] = current.Title;
            }
        }
    }
}
