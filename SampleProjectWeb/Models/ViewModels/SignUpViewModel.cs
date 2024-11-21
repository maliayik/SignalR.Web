using System.ComponentModel.DataAnnotations;

namespace SampleProjectWeb.Models.ViewModels
{
    public record SignUpViewModel([Required] string Email, [Required] string Password, [Required] string ConfirmPassword);

}
