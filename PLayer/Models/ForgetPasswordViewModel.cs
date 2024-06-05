using System.ComponentModel.DataAnnotations;

namespace PLayer.Models
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "InValid")]
        public string Email { get; set; }
    }
}
