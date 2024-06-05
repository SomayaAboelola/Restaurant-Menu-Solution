using System.ComponentModel.DataAnnotations;

namespace PLayer.Models
{
	public class RegisterViewModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		
		[EmailAddress(ErrorMessage = "InValid")]

		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "ConfirmPassword not match with Password")]
		public string ConfirmPassword { get; set; }
		public bool Agree { get; set; }

	}
}
