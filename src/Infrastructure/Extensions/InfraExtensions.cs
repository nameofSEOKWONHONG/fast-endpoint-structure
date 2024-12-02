using System.Text.Json;
using System.Text.Json.Serialization;
using eXtensionSharp;
using FluentValidation.Results;

namespace Infrastructure.Extensions;

public static class InfraExtensions
{
    public static byte[] ToByteArray(this Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        // If the stream is seekable, rewind it to the beginning
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public static string ToSerialize<T>(this T data) where T : class
    {
        if (data.xIsEmpty()) return default;
        var option = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };
        
        return JsonSerializer.Serialize(data, option);
    }

    public static T ToDeserialize<T>(this string json) where T : class
    {
        if (json.xIsEmpty()) return default;
        var option = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,            
        };
        return JsonSerializer.Deserialize<T>(json, option);
    }
    
    public static long ToUnixTimestamp(this DateTime utc)
    {
        return new DateTimeOffset(utc).ToUnixTimeSeconds();
    }

    public static long ToUnixTimestampMs(this DateTime utc)
    {
        return new DateTimeOffset(utc).ToUnixTimeMilliseconds();
    }

    public static DateTime ToKoreaDate(this DateTime utc)
    {
        // 한국 시간대 (KST) 가져오기
        TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utc, koreaTimeZone);
    }

    public static DateTime ToKoreaDate(this long timestamp)
    {
        DateTime utcDateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
        TimeZoneInfo kstZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, kstZone);
    }

    public static DateTime ToUniversalTime(this long timestampms)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timestampms).UtcDateTime;
    }
    
    public static DateTime ToUtcDate(this DateTime kstDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(kstDateTime);
    }
    
    public static Dictionary<string, string> ToErrors(this ValidationResult result)
    {
        var errors = result.Errors.Select(m => new ValueTuple<string, string>(m.PropertyName, m.ErrorMessage)).ToList();
        if (errors.xIsNotEmpty())
        {
            var results = new Dictionary<string, string>();
            foreach (var valueTuple in errors)
            {
                if (results.ContainsKey(valueTuple.Item1).xIsFalse())
                {
                    results.Add(valueTuple.Item1, valueTuple.Item2);
                }
            }

            return results;    
        }

        return default;
    }
}