using System.Text.Json;

namespace DTOs.Setting
{
    public class UpdateSettingDto
    {
        public string Name { get; set; }
        public JsonElement Value { get; set; }

        public T GetTypedValue<T>()
        {
            return Value.Deserialize<T>();
        }
    }

}
