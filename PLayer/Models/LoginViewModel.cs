using System.ComponentModel.DataAnnotations;

namespace PLayer.Models
{
	public class LoginViewModel
	{
		[EmailAddress(ErrorMessage = "InValid")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
