using EmployeeDAA.Api.Models.Common;
using FluentValidation;

namespace EmployeeDAA.Api.Models.Order
{
    public class OrderCartModel:BaseModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderCartModelValidator : AbstractValidator<OrderCartModel>
    {
        public OrderCartModelValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}
