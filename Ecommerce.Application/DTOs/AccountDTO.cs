﻿namespace Ecommerce.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class LoginDTO
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}
