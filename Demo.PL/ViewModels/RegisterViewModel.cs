using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
        public string FName { get; set; }
		[Required]
        public string LName { get; set; }
        [Required(ErrorMessage ="Email is Required")]
		[EmailAddress(ErrorMessage ="Invaild Email")]
	    public string Email { get; set; }

		[Required(ErrorMessage ="Password is Required!!")]
		[MinLength(5 ,ErrorMessage ="Minimum Password Lenght is 5")]
		[DataType(DataType.Password)]
        public string Password  { get; set; }
		 

		[Required(ErrorMessage ="Confirm Password is Required ")]
		[Compare(nameof(Password) ,ErrorMessage ="Confirm Password Does not Match Password")]
		[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool  IsAgree { get; set; }
    }
}
