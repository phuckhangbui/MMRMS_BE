namespace DTOs.Dashboard
{
    public class DashboardDto
    {
    }

    public class DataTotalAdminDto
    {
        public int TotalCustomer { get; set; }
        public int TotalStaff { get; set; }
        public int TotalContent { get; set; }
    }

    public class DataUserAdminDto
    {
        public string Time { get; set; }
        public int Customer { get; set; }
    }

    public class DataTotalManagerDto
    {
        public int TotalMachine { get; set; }
        public int TotalRentingRequest { get; set; }
        public int TotalContract { get; set; }
        public double TotalMoney { get; set; }
    }
}
