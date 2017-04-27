using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Backend.Models
{
    [NotMapped]
    public class UserView : User
    {
        [Display(Name="Logo")]
        public HttpPostedFileBase LogoFile { get; set; }

    }
}