namespace Repository.Interface
{
    public interface IAddressRepository
    {
        Task<bool> CheckAddressValid(int addressId, int accountId);
    }
}
