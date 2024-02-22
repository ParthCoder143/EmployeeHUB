    using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Api.Models.Category;
using EmployeeDAA.Api.Models.Product;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Permissions;

namespace EmployeeDAA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {

        #region Fields
        private readonly IConfiguration _configuration;

        private readonly IProductServices _productServices;

        #endregion

        #region Ctor

        public ProductsController(IProductServices productServices, IConfiguration configuration)
        {
            _productServices = productServices;
            _configuration = configuration;
        }

        #endregion
        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMasProduct, Permission = PagePermission.View)]

        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<Product> TypesList = await _productServices.GetAllAsync(objGrid);
            return Ok(TypesList.ToGridResponse(objGrid, "Product List"));
        }
        [HttpGet("{id}")]
        [Permission(Page = PageName.AdmMasProduct, Permission = PagePermission.View)]

        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            Product data = await _productServices.GetByIdAsync(id);
            return data.ToSingleResponse<Product, ProductModel>("Product");
        }

        [HttpPost]

        public async Task<ApiResponse> Post([FromForm] ProductModel model)
        {


            if (model.Imgupload != null)
            {
                string path = Path.Combine(_configuration["FileUploadSettings:ProductImgUploadBasePath"]);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                model.Photo = System.Guid.NewGuid().ToString() + Path.GetExtension(model.Imgupload.FileName);
                FileStream stream = new($"{path}/{model.Photo}", FileMode.Create);
                await model.Imgupload.CopyToAsync(stream);
                stream.Dispose();
            }

            if (model.Id == 0)
            {

                await _productServices.InsertAsync(model.MapTo<Product>());
            }
            else if (model.Id != 0)
            {
                Product data = await _productServices.GetByIdAsync(model.Id);
                data.CategoryId = model.CategoryId;
                data.ProductName = model.ProductName;

                data.SortOrder = model.SortOrder;
                data.IsDeleted = model.IsDeleted;
                data.Price = model.Price;
                data.Photo = model.Photo;
                //data.CameraUrl = model.CameraUrl;
                await _productServices.UpdateAsync(data);
                await _productServices.UpdateAsync(model.MapTo<Product>());

            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, model.Id == 0 ? "Product Added" : "Product Updated", model);

        }

        [HttpDelete]
        [Permission(Page = PageName.AdmMasProduct, Permission = PagePermission.Delete)]

        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<Product> obj = await _productServices.GetByIdsAsync(Ids).ConfigureAwait(false);
            if (obj == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            }

            await _productServices.DeleteAsync(obj);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Product Delete");

        }

        [HttpPost]
        [Route("[action]")]
        [Permission]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            if (id > 0)
            {
                Product UpdateObj = await _productServices.GetByIdAsync(id);
                UpdateObj.IsActive = !UpdateObj.IsActive;
                await _productServices.UpdateAsync(UpdateObj);
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, Message.GetMessage("CommUpdate"));
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "No data found.");
        }

        [HttpGet]
        [Route("[action]")]
        [Permission]
        public async Task<FileResult> GetProductDocument(string filename)
        {
            MemoryStream mst = new();
            string folderpath = Path.Combine(_configuration["FileUploadSettings:ProductImgUploadBasePath"], filename);
            if (System.IO.File.Exists(folderpath))
            {
                byte[] imgByte = await System.IO.File.ReadAllBytesAsync(folderpath);
                return new FileStreamResult(new MemoryStream(imgByte), "image/jpeg");  // You can use your own method over here.         
            }
            return new FileStreamResult(mst, "image/jpeg");
        }

        [HttpGet("getByIds")]
        public async Task<IActionResult> GetProductsByIdsAsync([FromQuery] int[] productIds)
        {
            if (productIds == null || productIds.Length == 0)
            {
                return BadRequest("Product IDs must be provided.");
            }

            try
            {
                var products = await _productServices.GetByIdsAsync(productIds);

                if (products == null || !products.Any())
                {
                    return NotFound("No products found with the provided IDs.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}



