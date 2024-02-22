using EmployeeDAA.Api.Models.Common;
using EmployeeDAA.Core;
using FluentValidation;

namespace EmployeeDAA.Api.Models.Product
{
    public class ProductModel: BaseModel
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }

        public IFormFile Imgupload { get; set; }
        public string CameraUrl { get; set; }

        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public double SortOrder { get; set; }
    }
    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(x => x.ProductName).NotNull().NotEmpty().WithMessage("Product Name is required.");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Select valid Category Name.");
            RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0).WithMessage("Enter valid sort order");
        }
    }
}
