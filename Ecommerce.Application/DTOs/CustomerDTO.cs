namespace Ecommerce.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CustomerDTO
    {
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El correo electrónico del cliente es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; }
    }
}
