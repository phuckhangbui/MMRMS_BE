using DTOs.Setting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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

        private const string MachineSettingFilePath = "machineRateData.json";

        private readonly IHubContext<SettingHub> _settingHub;
        private readonly ILogger<SettingsService> _logger;


        public SettingsService(IHubContext<SettingHub> settingHub, ILogger<SettingsService> logger)
        {
            _settingHub = settingHub;
            _logger = logger;
        }

        public async Task<IEnumerable<SettingDto>> GetSettingsAsync()
        {
            var settings = await ReadSystemSettingsFileAsync();
            return settings;
        }

        public async Task<MachineSettingDto> GetMachineSettingsAsync()
        {
            var settings = await ReadMachineSettingsFileAsync();
            return settings;
        }



        public async Task UpdateSettingAsync(UpdateSettingDto updateSettingDto)
        {
            var settings = await ReadSystemSettingsFileAsync();

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

            await WriteSystemSettingsFileAsync(settings);

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


        private async Task<List<SettingDto>> ReadSystemSettingsFileAsync()
        {
            var json = await File.ReadAllTextAsync(SettingsFilePath);
            return JsonConvert.DeserializeObject<List<SettingDto>>(json);
        }

        private async Task WriteSystemSettingsFileAsync(IEnumerable<SettingDto> settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            await File.WriteAllTextAsync(SettingsFilePath, json);
        }

        private async Task<MachineSettingDto?> ReadMachineSettingsFileAsync()
        {
            var json = await File.ReadAllTextAsync(MachineSettingFilePath);

            // Deserialize JSON into the list of SettingDto
            var settingData = JsonConvert.DeserializeObject<MachineSettingDto>(json);

            return settingData;
        }

        public void CheckSettingsFilesOnStartup()
        {
            if (!File.Exists(SettingsFilePath) || !File.Exists(MachineSettingFilePath))
            {
                _logger.LogError("One or more required files are missing: {SettingsFilePath}, {MachineSettingFilePath}", SettingsFilePath, MachineSettingFilePath);
                Environment.Exit(1);
            }

            if (new FileInfo(SettingsFilePath).Length == 0 || new FileInfo(MachineSettingFilePath).Length == 0)
            {
                _logger.LogError("One or more required files are empty: {SettingsFilePath}, {MachineSettingFilePath}", SettingsFilePath, MachineSettingFilePath);
                Environment.Exit(1);
            }

            if (!IsValidJson(SettingsFilePath) || !IsValidJson(MachineSettingFilePath))
            {
                _logger.LogError("One or more required files are not in valid JSON format: {SettingsFilePath}, {MachineSettingFilePath}", SettingsFilePath, MachineSettingFilePath);
                Environment.Exit(1);
            }

            if (!IsValidFormat(SettingsFilePath, typeof(List<SettingDto>)) || !IsValidFormat(MachineSettingFilePath, typeof(MachineSettingDto)))
            {
                _logger.LogError("One or more required files do not match the expected format.");
                Environment.Exit(1);
            }


            _logger.LogInformation("Settings files are valid.");
        }

        private bool IsValidJson(string filePath)
        {
            try
            {
                var fileContent = File.ReadAllText(filePath);
                JsonConvert.DeserializeObject(fileContent);
                return true;
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON format in file: {FilePath}", filePath);
                return false;
            }
        }

        private bool IsValidFormat(string filePath, Type expectedType)
        {
            try
            {
                var fileContent = File.ReadAllText(filePath);
                var deserializedObject = JsonConvert.DeserializeObject(fileContent, expectedType);
                return deserializedObject != null;
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Invalid format in file: {FilePath}", filePath);
                return false;
            }
        }
    }

}
