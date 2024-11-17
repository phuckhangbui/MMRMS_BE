using DTOs.Setting;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;
using System.Text.Json;
namespace Service.Implement
{

    public class SettingsService : ISettingsService
    {
        private const string SettingsFilePath = "systemsetting.json";

        private readonly IHubContext<SettingHub> _settingHub;

        public SettingsService(IHubContext<SettingHub> settingHub)
        {
            _settingHub = settingHub;
        }

        public async Task<IEnumerable<SettingDto>> GetSettingsAsync()
        {
            var settings = await ReadSettingsFileAsync();
            return settings;
        }

        public async Task UpdateSettingAsync(UpdateSettingDto updateSettingDto)
        {
            var settings = await ReadSettingsFileAsync();

            var setting = settings.FirstOrDefault(s => s.Name == updateSettingDto.Name);

            if (setting == null)
            {
                throw new ServiceException($"Setting '{updateSettingDto.Name}' not found.");
            }

            if (!IsValidType(updateSettingDto.Value, setting.ValueType))
            {
                throw new ServiceException($"Invalid value type for setting '{updateSettingDto.Name}'. Expected {setting.ValueType}.");
            }

            // Update the setting value
            setting.Value = ConvertValueToType(updateSettingDto.Value, setting.ValueType);

            await WriteSettingsFileAsync(settings);

            await _settingHub.Clients.All.SendAsync("OnUpdateSetting");
        }

        private bool IsValidType(JsonElement value, string expectedType)
        {
            try
            {
                return expectedType switch
                {
                    "int" => value.TryGetInt32(out _),
                    "double" => value.TryGetDouble(out _),
                    "string" => value.ValueKind == JsonValueKind.String,
                    _ => false
                };
            }
            catch
            {
                return false;
            }
        }

        private object ConvertValueToType(JsonElement value, string targetType)
        {
            return targetType switch
            {
                "int" => value.GetInt32(),
                "double" => value.GetDouble(),
                "string" => value.GetString(),
                _ => throw new InvalidCastException($"Cannot convert value to type {targetType}.")
            };
        }


        private async Task<List<SettingDto>> ReadSettingsFileAsync()
        {
            var json = await File.ReadAllTextAsync(SettingsFilePath);
            return JsonConvert.DeserializeObject<List<SettingDto>>(json);
        }

        private async Task WriteSettingsFileAsync(IEnumerable<SettingDto> settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            await File.WriteAllTextAsync(SettingsFilePath, json);
        }

    }

}
