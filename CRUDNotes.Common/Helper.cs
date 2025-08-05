using System.Text.Json;

namespace CRUDNotes.Common
{
    public static class Helper
    {
        public static T? FromByteArray<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default;

            return JsonSerializer.Deserialize<T>(data);
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            return JsonSerializer.SerializeToUtf8Bytes(obj, obj.GetType());
        }
    }
}