using System.ComponentModel.DataAnnotations;

namespace BrawnyFinal.ViewModels;

public class ContactForm
{
    [Required, EmailAddress] public string Email { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Message { get; set; }
}