using DTOs.Setting;

namespace Service.Interface
{
    public interface ISettingsService
    {
        Task<IEnumerable<SettingDto>> GetSettingsAsync();
        Task UpdateSettingAsync(UpdateSettingDto updateSettingDto);
    }

}
