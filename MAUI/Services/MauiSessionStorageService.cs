using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using RCL.Data.Interfaces;
using System.Text.Json;


namespace MAUI.Services
{
    internal class MauiSessionStorageService : ISessionStorageService
    {
        public ValueTask SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            Preferences.Set(key, json);
            return ValueTask.CompletedTask;
        }

        public ValueTask<T> GetItemAsync<T>(string key)
        {
            var json = Preferences.Get(key, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return ValueTask.FromResult(default(T));
            }
            var value = JsonSerializer.Deserialize<T>(json);
            return ValueTask.FromResult(value);
        }

        public ValueTask RemoveItemAsync(string key)
        {
            Preferences.Remove(key);
            return ValueTask.CompletedTask;
        }
    }
}
