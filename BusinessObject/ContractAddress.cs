namespace BusinessObject
{
    public class ContractAddress
    {
        public int ContractAddressId { get; set; }

        public string? AddressBody { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }


        public virtual Contract? Contract { get; set; }

    }
}
