namespace CouponService.EF
{
    public enum RepositoryActionStatus
    {
        Ok,
        Created,
        Updated,
        NotFound,
        Deleted,
        NothingModified,
        Error,
        Found,
        NotEnoughStock,
        NotCreated
    }
}
