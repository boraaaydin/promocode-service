using System;
using System.Threading.Tasks;
using CouponService.Models.Api;
using CouponService.Models.Api.Request;
using Microsoft.AspNetCore.Mvc;
using CouponService.EF;

namespace CouponService.Controllers.Api
{
    [Produces("application/json")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _repos;
        public CouponController(ICouponRepository repos)
        {
            _repos = repos;
        }

        [Route("api/v1/coupons/select2")]
        public async Task<IActionResult> GetCouponsForSelect2(string term, string site)
        {
            var isValidated = ValidateSite(site, out IsForSite web);
            if (isValidated)
            {
                var coupons = await _repos.GetCouponsByTerm(term, web.isForA, web.isForB);
                ApiSelect2Result result = CouponModelFactory.CreateCouponListForSelect2(coupons);
                return Ok(result);
            }
            return Ok(new ApiResultError("error"));
        }

        [Route("api/v1/coupons/details/select2")]
        public async Task<IActionResult> GetCouponDetailsForSelect2(string couponcode, string site)
        {
            var isValidated = ValidateSite(site, out IsForSite web);
            if (isValidated)
            {
                var Coupon = await _repos.GetSingleCouponCode(couponcode, web.isForA, web.isForB);
                if (Coupon != null)
                {
                    ApiSelect2Result result = CouponModelFactory.CreateCouponDetailForSelect2(Coupon);
                    return Ok(result);
                }
            }
            return Ok(new ApiResultError("error"));
            //return BadRequest("Kupon Bulunamadı");
        }

        private bool ValidateSite(string site, out IsForSite web)
        {
            bool isValidated = false;
            web = new IsForSite();
            if (string.IsNullOrEmpty(site))
                return false;
            var website = new IsForSite();
            if (site == "isForA")
            {
                website.isForA = true;
                isValidated = true;
            }
            if (site == "isForB")
            {
                website.isForB = true;
                isValidated = true;
            }
            return isValidated;
        }

        [Route("api/v1/coupons/details")]
        public async Task<IActionResult> GetCouponDetails(string couponcode, string site)
        {
            var isValidated = ValidateSite(site, out IsForSite web);
            if (isValidated)
            {
                var couponCode = await _repos.GetSingleCouponCode(couponcode, web.isForA, web.isForB);
                if (couponCode != null)
                {
                    var dto = CouponModelFactory.CreateCouponDetailDTO(couponCode);
                    return Ok(dto);
                }
            }
            return Ok(new ApiResultError("error"));
        }

        [Route("api/v1/coupon/getcouponcode/{couponid}")]
        public IActionResult GetCouponCode(int couponid)
        {
            var coupon = _repos.GetCouponManagementById(couponid, true);

            var dto = CouponModelFactory.CreateCouponCodeDTO(coupon);

            return Ok(dto);
        }

        [Route("api/v1/getallsitecoupons")]
        public IActionResult Index(string searchCode, string searchDescription)
        {
            var coupons = _repos.GetAllCoupons(searchCode, searchDescription);
            var model = CouponModelFactory.CreateCouponIndex(coupons);
            return Ok(model);
        }

        [Route("api/v1/createcoupon")]
        public async Task<IActionResult> Create(string userid, CouponCreateOrEditRequestModel request)
        {
            try
            {
                var model = CouponModelFactory.CreateCouponManagement(request);
                var result = await _repos.CreateAsync(model, userid, request.Quantity, request.CouponLength);
                if (result.Status != RepositoryActionStatus.Created)
                {
                    return Ok(new ApiResultError("Hata Oluştu:" + result.Message));
                }
                var dto = CouponModelFactory.CreateCouponDTO(result.Entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return ReturnErrorApiResult(ex);
            }
        }

        [Route("api/v1/deletecoupon")]
        public async Task<IActionResult> Delete(int couponid)
        {
            try
            {
                var result = await _repos.DeleteByManagementId(couponid);

                if (result.Status != RepositoryActionStatus.Deleted)
                {
                    return Ok(new ApiResultError("Hata Oluştu:" + result.Message));
                }
                var dto = CouponModelFactory.CreateModelDeleteCouponDTO(result.Entity);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return ReturnErrorApiResult(ex);
            }
        }

        [Route("api/v1/editcoupon")]
        public IActionResult Edit(int couponid)
        {
            try
            {
                var coupon = _repos.GetCouponManagementById(couponid, false);
                if (!coupon.IsBulk)
                {
                    coupon = _repos.GetCouponManagementById(couponid, true);
                }
                var dto = CouponModelFactory.EditCouponDTO(coupon);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return ReturnErrorApiResult(ex);
            }
        }

        [Route("api/v1/editcouponpost")]
        public IActionResult EditPost(string userid, CouponCreateOrEditRequestModel viewModel)
        {
            try
            {
                var model = CouponModelFactory.CreateCouponManagement(viewModel);
                model.UpdatedBy = userid;

                var result = _repos.Update(model);

                if (result.Status != RepositoryActionStatus.Updated)
                {
                    return Ok(new ApiResultError("Hata Oluştu:" + result.Message));
                }
                var dto = CouponModelFactory.EditCouponDTO(result.Entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return ReturnErrorApiResult(ex);
            }
        }

        [Route("api/v1/getcouponscanceled")]
        public IActionResult Canceled(string couponcode, string descriptionterm)
        {
            var coupons = _repos.GetAllCoupons(couponcode, descriptionterm, true);

            var model = CouponModelFactory.CreateCouponIndex(coupons);

            return Ok(model);
        }

        [Route("api/v1/coupondetails")]
        public IActionResult GetCouponDetailsAsFile(int couponid)
        {
            var codes = _repos.GetCodesByManagementId(couponid);
            var model = CouponModelFactory.GetCouponCodesForManagement(codes.Result.Entity);
            return Ok(model);
        }

        private IActionResult ReturnErrorApiResult(Exception ex)
        {
            return Ok(new ApiResultError(ex.Message + "-" + (ex.InnerException == null ? "" : ex.InnerException.Message)));
        }
    }

    public class IsForSite
    {
        public bool isForA { get; set; }
        public bool isForB { get; set; }
    }
}