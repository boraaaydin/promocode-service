using CouponService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CouponService.EF
{
    public interface ICouponRepository
    {
        Task<CouponCode> GetSingleCouponCode(string code, bool isForA, bool isForB);
        CouponManagement GetCouponManagementById(int id, bool includeCouponCodeTable);
        Task<List<CouponCode>> GetCouponsByTerm(string _term, bool isForA, bool isForB);
        Task<RepositoryActionResult<CouponManagement>> CreateAsync(CouponManagement model, string userId, int quantity, int length);
        RepositoryActionResult<CouponManagement> Update(CouponManagement model);
        List<CouponManagement> GetAllCoupons(string couponcode = "", string descriptionTerm = "", bool canceledcoupon = false);
        CouponManagement GetAllCouponById(int id);
        List<CouponCode> GetCouponsByManagementId(int couponManagementId);
        Task<RepositoryActionResult<CouponManagement>> DeleteByManagementId(int couponManagementId);
        Task<RepositoryActionResult<List<string>>> GetCodesByManagementId(int id);
    }
}
