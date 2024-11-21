using System.ComponentModel.DataAnnotations;

namespace SampleProjectWeb.Models.ViewModels
{
    public record SignInViewModel([Required] string Email, [Required] string Password);
}
