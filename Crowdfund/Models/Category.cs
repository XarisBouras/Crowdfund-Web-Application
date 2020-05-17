using System.ComponentModel.DataAnnotations;

namespace Crowdfund.Models
{
    public enum Category
    {
        Art,
        Comics,
        Crafts,
        Dance,
        Design,
        Fashion,
        Film,
        Food,
        Games,
        Hardware,
        Journalism,
        Kids,
        Music,
        Photography,
        Publishing,
        [Display(Name = "Product Design")]
        ProductDesign,
        Software,
        Technology, 
        Theater
    }
}