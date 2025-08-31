using System;
using System.Security.Cryptography;
using System.Text;

namespace FabFilterKeyGen;

public class KeyGen
{
    public static string GenerateLicense(string productName, string licenseeName)
    {
        // Step 1: Format input string
        string input = $"Product: {productName}; Licensee: {licenseeName};";
        
        // Step 2: Compute MD5 hash (mimicking function_100019ad)
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes;
        using (MD5 md5 = MD5.Create())
        {
            md5.Initialize();
            hashBytes = md5.ComputeHash(inputBytes); // 16 bytes
        }
        
        // Step 3: Generate 128-byte input (simulate function_10002522)
        byte[] expandedInput = new byte[128];
        string hexKey1 = "2be6951adc5b22410a5fd";
        string hexKey2 = "437ae92817f9fc85b7e5";
        byte[] keyBytes = HexStringToBytes(hexKey1 + hexKey2); // 20 bytes
        
        // Mimic function_100075ac and function_10008264
        expandedInput[0] = 0x30; // From function_100075ac (a5 == 13)
        
        // Compute length using function_10009937 logic
        int v2 = hashBytes.Length + keyBytes.Length;
        int v3 = v2 + ((v2 & 7) == 0 ? 1 : 0);
        int v4 = v3 + 1;
        int v5 = v4;
        if (v3 >= 128)
        {
            int v6 = v3;
            v5++;
            while (v6 >= 256)
            {
                v6 /= 256;
                v5++;
            }
        }
        expandedInput[1] = (byte)(v5 + 1);
        
        // Copy MD5 hash and keys
        Array.Copy(hashBytes, 0, expandedInput, 2, 16);
        Array.Copy(keyBytes, 0, expandedInput, 18, 20); // 36 bytes total
        
        // Fill remaining bytes with iterative MD5 hashing
        byte[] temp = hashBytes;
        using (MD5 md5 = MD5.Create())
        {
            for (int i = 0; i < 5; i++) // 128 - 38 = 90 bytes, 5 * 16 = 80
            {
                byte[] combined = new byte[temp.Length + 16];
                Array.Copy(temp, 0, combined, 0, temp.Length);
                Array.Copy(keyBytes, keyBytes.Length - 16, combined, temp.Length, 16);
                temp = md5.ComputeHash(combined);
                Array.Copy(temp, 0, expandedInput, i * 16 + 38, 16);
            }
        }
        
        // Step 4: Base64 encode
        byte[] truncatedBytes = new byte[111];
        Array.Copy(expandedInput, truncatedBytes, 111); // Truncate to 111 bytes
        string encoded = Convert.ToBase64String(truncatedBytes);
        
        // Step 5: Format output with newlines every 64 characters
        StringBuilder result = new StringBuilder();
        result.Append($"Product: {productName};\r\n");
        result.Append($"Licensee: {licenseeName};\r\n");
        
        for (int i = 0; i < encoded.Length; i += 64)
        {
            int length = Math.Min(64, encoded.Length - i);
            result.Append(encoded.Substring(i, length) + "\r\n");
        }
        
        return result.ToString().TrimEnd('\n', '\r');
    }

    private static byte[] HexStringToBytes(string hex)
    {
        int length = hex.Length / 2;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }
}