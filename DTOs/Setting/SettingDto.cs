namespace DTOs.Setting
{
    public class SettingDto
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string ValueType { get; set; }
    }

    public class MachineConditionRateDto
    {
        public int MachineConditionPercent { get; set; }
        public int RentalPricePercent { get; set; }
    }

    public class MachineConditionDaysDto
    {
        public int MachineConditionPercent { get; set; }
        public int RentedDays { get; set; }
    }

    public class MachineSettingDto
    {
        public List<MachineConditionRateDto> RateData { get; set; }
        public List<MachineConditionDaysDto> DaysData { get; set; }
    }
}
