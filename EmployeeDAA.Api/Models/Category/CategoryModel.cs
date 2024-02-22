using EmployeeDAA.Api.Models.Common;
using FluentValidation;

namespace EmployeeDAA.Api.Models.Category
{

    public class CategoryModel : BaseModel
    {
        public string CategoryName { get; set; }
        public double? SortOrder { get; set; }
        public int IsActive { get; set; }
    }

    public class CategoryModellValidator : AbstractValidator<CategoryModel>
    {
        public CategoryModellValidator()
        {
            RuleFor(x => x.CategoryName).NotNull().NotEmpty().WithMessage("Document class is required.");
            RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0).WithMessage("Enter valid sort order");
        }
    }
}
