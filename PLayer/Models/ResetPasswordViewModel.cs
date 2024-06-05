using System.ComponentModel.DataAnnotations;

namespace PLayer.Models
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "ConfirmPassword not match with Password")]
		public string ConfirmNewPassword { get; set; }
	}
}
