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
}
