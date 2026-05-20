using System.ComponentModel.DataAnnotations;

namespace WebApp.web.models;

public class Brand
{
    [Key]
    public Guid ID{get;set;}
    
    [Required]
    public string Name{get;set;} =  String.Empty;

    [Display(Name = "Established Year")]
    public int EstablishedYear{get;set;}


    [Display(Name = "Brand Logo")]
    public String BrandLogo{get;set;} = String.Empty;
}