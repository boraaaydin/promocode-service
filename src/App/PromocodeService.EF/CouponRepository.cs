using PromocodeService.Domain.Models;
using PromocodeService.EF.Helpers;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeService.EF
{
    public class CouponRepository:ICouponRepository
    {
        private readonly ApplicationDbContext _Db;
        public CouponRepository(ApplicationDbContext db)
        {
            _Db = db;
        }

        public async Task<CouponCode> GetSingleCouponCode(string code, bool isForA, bool isForB)
        {
            var query = _Db.CouponCodes
                .Include(x => x.CouponManagement)
                .Where(x => x.Code == code && x.IsActive == true).AsQueryable();

            if (isForA)
                query = query.Where(x => x.CouponManagement.isForA == true);

            if (isForB)
                query = query.Where(x => x.CouponManagement.isForB == true);

            var coupon = await query.FirstOrDefaultAsync();
            return ValidateCoupon(coupon);
        }

        public CouponManagement GetCouponManagementById(int id, bool includeCouponCodeTable)
        {
            var query = _Db.CouponManagements
                .Where(x => x.Id == id).AsQueryable();

            if (includeCouponCodeTable)
            {
                query = query.Include(c => c.Codes);
            }
            return query.FirstOrDefault();
        }

        public async Task<RepositoryActionResult<List<string>>> GetCodesByManagementId(int id)
        {
            var codes = await _Db.CouponCodes
                .Where(x => x.CouponManagementId == id)
                .Select(x => x.Code)
                .ToListAsync();
            if (codes.Count != 0)
                return new RepositoryActionResult<List<string>>(RepositoryActionStatus.Found, codes);
            return new RepositoryActionResult<List<string>>(RepositoryActionStatus.NotFound, null);
        }

        public async Task<List<CouponCode>> GetCouponsByTerm(string _term, bool isForA, bool isForB)
        {
            var query = _Db.CouponCodes
                .Include(x => x.CouponManagement)
                .Where(x => x.IsActive == true && x.Code == _term).AsQueryable();

            if (isForA)
                query = query.Where(x => x.CouponManagement.isForA == true);

            if (isForB)
                query = query.Where(x => x.CouponManagement.isForB == true);

            var list = await query.ToListAsync();

            return ValidateCoupons(list);
        }

        List<CouponCode> ValidateCoupons(List<CouponCode> list)
        {
            var newlist = new List<CouponCode>();

            foreach (var coupon in list)
            {
                var validated = ValidateCoupon(coupon);
                if (validated == null)
                    break;
                newlist.Add(coupon);
            }
            return newlist;
        }

        CouponCode ValidateCoupon(CouponCode coupon)
        {
            if (coupon == null)
                return null;
            if (coupon.CouponManagement.IsInfinite == false && coupon.CouponManagement.ExpireDate < DateTime.Now)
                return null;
            if (coupon.CouponManagement.IsForOneUse == true && coupon.IsUsed == true)
                return null;
            return coupon;
        }



        public async Task<RepositoryActionResult<CouponManagement>> CreateAsync(CouponManagement model, string userId, int quantity, int length)
        {
            try
            {
                //throw new Exception("manuel  hata");
                var service = new CouponGenerator();
                if (!model.IsBulk)
                {
                    var code = model.Codes.FirstOrDefault().Code;
                    var existing = await _Db.CouponCodes.Where(x => x.Code == code).FirstOrDefaultAsync();
                    if (existing != null)
                    {
                        return new RepositoryActionResult<CouponManagement>
                        {
                            Status = RepositoryActionStatus.Found,
                            Message = "Veritabanına kayıtlı böyle bir kod bulundu: " + code
                        };
                    }
                }
                model.CreatedAt = DateTime.UtcNow;
                model.CreatedBy = userId;
                await _Db.CouponManagements.AddAsync(model);
                var resultInsertManagement = await _Db.SaveChangesAsync();
                if (model.IsBulk)
                {
                    if (resultInsertManagement > 0)
                    {
                        var remainingCodes = length;
                        var totalCodes = new List<string>();

                        while (totalCodes.Count != quantity)
                        {
                            totalCodes.AddRange(service.GenerateCode(quantity, remainingCodes));
                            totalCodes = service.RemoveDublicateCode(totalCodes, out int deletedCount);
                            remainingCodes = deletedCount;
                        }

                        var someCollectionOfEntitiesToInsert = totalCodes.Select(x => new CouponCode
                        {
                            CouponManagementId = model.Id,
                            Code = x,
                            IsActive = true,
                            IsUsed = false
                        }).ToList();
                        await _Db.BulkInsertAsync(someCollectionOfEntitiesToInsert);
                    }
                }
                return new RepositoryActionResult<CouponManagement>
                {
                    Entity = model,
                    Status = RepositoryActionStatus.Created
                };
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<CouponManagement>
                {
                    Status = RepositoryActionStatus.Error,
                    Message = ex.Message + ex.InnerException == null ? "" : ex.InnerException.Message
                };
            }
        }

        public RepositoryActionResult<CouponManagement> Update(CouponManagement model)
        {
            try
            {
                CouponManagement existing;
                if (model.IsBulk)
                {
                    existing = GetCouponManagementById(model.Id, false);
                }
                else
                {
                    existing = GetCouponManagementById(model.Id, true);
                    existing.Codes.FirstOrDefault().Code = model.Codes.FirstOrDefault().Code;
                }
                existing.Description = model.Description;
                existing.DiscountAmount = model.DiscountAmount;
                existing.DiscountType = model.DiscountType;
                existing.ExpireDate = model.ExpireDate;
                existing.IsInfinite = model.IsInfinite;
                existing.UpdatedAt = model.UpdatedAt;
                existing.UpdatedBy = model.UpdatedBy;
                existing.IsForOneUse = model.IsForOneUse;
                existing.isForA = model.isForA;
                existing.isForB = model.isForB;

                _Db.SaveChanges();
                return new RepositoryActionResult<CouponManagement>
                {
                    Entity = model,
                    Status = RepositoryActionStatus.Updated
                };
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<CouponManagement>
                {
                    Status = RepositoryActionStatus.Error,
                    Message = ex.InnerException.Message
                };
            }
        }

        public List<CouponManagement> GetAllCoupons(string couponcode = "", string descriptionTerm = "", bool canceledcoupon = false)
        {
            var query = _Db.CouponManagements.AsQueryable();

            if (!string.IsNullOrEmpty(descriptionTerm))
            {
                query = query.Where(x => x.Description.Contains(descriptionTerm));
            }

            if (!string.IsNullOrEmpty(couponcode))
            {
                query = query.Where(x => x.Codes.Any(y => y.Code == couponcode));
            }
            if (canceledcoupon)
            {
                query = query.Where(x => x.IsCanceled == true);
            }
            else
            {
                query = query.Where(x => x.IsCanceled == false);
            }

            return query
                .OrderBy(x => x.Description)
                .ToList();
        }

        public CouponManagement GetAllCouponById(int id)
        {
            return _Db.CouponManagements.Where(x => x.Id == id).Include(x => x.Codes).FirstOrDefault();
        }

        public List<CouponCode> GetCouponsByManagementId(int couponManagementId)
        {
            return _Db.CouponCodes.Where(x => x.CouponManagementId == couponManagementId).ToList();
        }

        public async Task<RepositoryActionResult<CouponManagement>> DeleteByManagementId(int couponManagementId)
        {
            var couponManagement = _Db.CouponManagements
                .Where(x => x.Id == couponManagementId)
                .FirstOrDefault();

            couponManagement.IsCanceled = true;

            if (await _Db.SaveChangesAsync() > 0)
            {
                return new RepositoryActionResult<CouponManagement>
                {
                    Status = RepositoryActionStatus.Deleted,
                    Entity = couponManagement
                };
            }
            return new RepositoryActionResult<CouponManagement> { Status = RepositoryActionStatus.Error };
        }

    }
}
