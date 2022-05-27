using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
namespace GlobalizationUseSettings.Pages
{
    public class SearchModel : PageModel
    {
        // private readonly IConfiguration Configuration;
        private readonly ApplicationContext Context;
        private readonly ILogger<IndexModel> _logger;
        private Regex rx { get; set; } = new Regex(@"\w.*",RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public SearchModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            Context = new ApplicationContext();
            Context.Languages.Add(ApplicationContext.English);
            Context.Languages.Add(ApplicationContext.Japanese);
            Context.Languages.Add(ApplicationContext.Spanish);
            configuration.GetSection(Context.Languages.FirstOrDefault()).Bind(Context);
            _logger = logger;
       }

        public void OnGet()
        {
            ViewData["Message"] = Context.Languages.Count;
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

            ViewData["Date"] = Context.Pages.Where(p => p.ActionKey=="Search").FirstOrDefault()!.Areas.FirstOrDefault()!.MovieInfo.Date;
            ViewData["Start"] = Context.Pages.Where(p => p.ActionKey=="Search").FirstOrDefault()!.Areas.FirstOrDefault()!.MovieInfo.SlotStart;
            ViewData["End"] = Context.Pages.Where(p => p.ActionKey=="Search").FirstOrDefault()!.Areas.FirstOrDefault()!.MovieInfo.SlotEnd;
            ViewData["MovieTitle"] = Context.Pages.Where(p => p.ActionKey=="Search").FirstOrDefault()!.Areas.FirstOrDefault()!.MovieInfo.MovieTitle;
            ViewData["AreaTitle"] = Context.Pages.Where(p => p.ActionKey=="Search").FirstOrDefault()!.Areas.FirstOrDefault()!.Title;
            ViewData["AreaLead"] = Context.Pages.Where(p => p.ActionKey=="Search").FirstOrDefault()!.Areas.FirstOrDefault()!.Lead;

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
}