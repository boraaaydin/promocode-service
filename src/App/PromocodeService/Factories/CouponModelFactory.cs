using PromocodeService.Domain.Models;
using PromocodeService.DTO;
using PromocodeService.Enums;
using PromocodeService.Models.Api;
using PromocodeService.Models.Api.Request;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromocodeService.Factories
{
    public class CouponModelFactory
    {
        public static ApiResult CreateCouponIndex(List<CouponManagement> coupons)
        {
            var list = new List<CouponManagementDTO>();
            foreach (var coupon in coupons)
            {
                var viewmodel = new CouponManagementDTO()
                {
                    Id = coupon.Id,
                    Description = coupon.Description,
                    DiscountType = coupon.DiscountType,
                    ExpireDate = coupon.ExpireDate,
                    IsInfinite = coupon.IsInfinite,
                    DiscountAmount = coupon.DiscountAmount,
                    IsBulk = coupon.IsBulk,
                    IsCanceled = coupon.IsCanceled,
                    IsForOneUse = coupon.IsForOneUse,
                    Codes = coupon.Codes,
                    isForB = coupon.isForB,
                    isForA = coupon.isForA,
                    CreatedAt = coupon.CreatedAt
                };
                list.Add(viewmodel);
            }

            return new ApiResult
            {
                Entity = list,
                Message = "ok"
            };
        }
        public static ApiResult CreateCouponDTO(CouponManagement dto)
        {
            var coupon = new CouponManagementDTO
            {
                Id = dto.Id,
                Description = dto.Description
            };
            return new ApiResult
            {
                Entity = coupon,
                Message = "ok"
            };
        }
        public static ApiResult CreateModelDeleteCouponDTO(CouponManagement dto)
        {
            var coupon = new CouponManagementDTO
            {
                Description = dto.Description
            };
            return new ApiResult
            {
                Entity = coupon,
                Message = "ok"
            };
        }
        public static ApiResult EditCouponDTO(CouponManagement model)
        {
            var dto = new ApiResult()
            {
                Entity = new CouponManagementDTO
                {
                    Id = model.Id,
                    Code = model.IsBulk != true ? model.Codes.FirstOrDefault().Code : "Çoklu Kod",
                    Description = model.Description,
                    DiscountAmount = model.DiscountAmount,
                    DiscountType = model.DiscountType,
                    ExpireDate = model.ExpireDate,
                    IsForOneUse = model.IsForOneUse,
                    IsInfinite = model.IsInfinite,
                    IsBulk = model.IsBulk,
                    isForA = model.isForA,
                    isForB = model.isForB
                }
            };
            return dto;
        }
        public static ApiResult GetCouponCodesForManagement(List<string> codes)
        {
            return new ApiResult
            {
                Entity = codes,
                Message = "ok"
            };
        }

        public static ApiResult CreateCouponDetailDTO(CouponCode coupon)
        {
            var dto = new ApiResult()
            {
                IsSuccess = true,
                Entity = new CouponDetailDTO
                {
                    DiscountAmount = coupon.CouponManagement.DiscountAmount,
                    DiscountType = coupon.CouponManagement.DiscountType,
                    Couponcode = coupon.Code,
                    IsBulk = coupon.CouponManagement.IsBulk
                }
            };
            return dto;
        }
        public static ApiResult CreateCouponCodeDTO(CouponManagement couponCode)
        {
            var dto = new ApiResult()
            {
                Entity = new CouponCodeDTO
                {
                    Code = couponCode.Codes.FirstOrDefault().Code
                }
            };

            return dto;
        }

        public static ApiSelect2Result CreateCouponListForSelect2(List<CouponCode> coupons)
        {
            var apiresult = new ApiSelect2Result();
            foreach (var item in coupons)
            {
                apiresult.Results.Add(new ApiSelect2DTO
                {
                    Id = item.Id.ToString(),
                    Text = item.Code
                });
            }
            apiresult.Message = "Başarılı";
            return apiresult;

        }
        public static ApiSelect2Result CreateCouponDetailForSelect2(CouponCode coupon)
        {
            var apiresult = new ApiSelect2Result();
            var type = coupon.CouponManagement.DiscountType;

            apiresult.Results.Add(new ApiSelect2DTO()
            {
                DiscountAmount = coupon.CouponManagement.DiscountAmount,
                //Kupon varsa zaten 2 tip 
                DiscountType = coupon.CouponManagement.DiscountType == DiscountType.Amount ? 2 : 1
            });

            apiresult.Message = "Başarılı";

            return apiresult;
        }
        public static CouponManagement CreateCouponManagement(CouponCreateOrEditRequestModel viewModel)
        {
            var coupon = new CouponManagement
            {
                Id = viewModel.Id,
                Description = viewModel.Description,
                DiscountAmount = viewModel.DiscountAmount,
                DiscountType = viewModel.DiscountType,
                IsInfinite = viewModel.IsInfinite,
                IsBulk = viewModel.IsBulk,
                IsForOneUse = viewModel.IsForOneUse,
                UpdatedAt = DateTime.UtcNow.ToLocalTime(),
                isForB = viewModel.isForB,
                isForA = viewModel.isForA
            };
            if (!viewModel.IsBulk)
            {
                coupon.Codes = new List<CouponCode> {
                    new CouponCode {
                        Code = viewModel.Code,
                        IsActive=viewModel.IsActive
                    } };
            }
            if (!viewModel.IsInfinite)
            {
                coupon.ExpireDate = viewModel.ExpireDate;
            }
            return coupon;
        }

    }
}
