using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SavePath = Application.persistentDataPath + "/gamesave.dat";
    private static readonly string EncryptionKey = "abcde1234567890o";

    public static void SaveGame(GameData data)
    {
        string jsonData = JsonUtility.ToJson(data);

        string encryptedData = Encrypt(jsonData, EncryptionKey);
        File.WriteAllText(SavePath, encryptedData);
        Debug.Log("Game Saved to: " + SavePath);
    }

    private static string Encrypt(string plainText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16]; 
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static GameData LoadGame()
    {
        
        if (File.Exists(SavePath))
        {
            try
            {
                string encryptedData = File.ReadAllText(SavePath);
                string jsonData = Decrypt(encryptedData, EncryptionKey);

                return JsonUtility.FromJson<GameData>(jsonData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load game: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + SavePath);
        }
        return null;
    }

    private static string Decrypt(string cipherText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];  
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
