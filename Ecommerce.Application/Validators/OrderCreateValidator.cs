namespace Ecommerce.Application.Validators
{
    using Ecommerce.Application.DTOs;
    using FluentValidation;

    public class OrderCreateValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateValidator()
        {
            RuleFor(model => model.Customer).NotNull().WithMessage("El nodo Customer es obligatorio");
            RuleFor(model => model.Items).NotNull().WithMessage("El nodo Items es obligatorio");

            When(model => model.Customer != null, () => {
                RuleFor(model => model.Customer.Name).NotEmpty().WithMessage("El campo Nombre es obligatorio");
                RuleFor(model => model.Customer.Email).NotEmpty().WithMessage("El campo Correo es obligatorio");
            });

            When(model => model.Items != null, () => {
                RuleFor(model => model.Items).NotEmpty().WithMessage("La lista de productos no puede estar vacía");
            });
        }
    }
}
