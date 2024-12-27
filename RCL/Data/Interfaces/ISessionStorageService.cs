using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface ISessionStorageService
    {
        ValueTask SetItemAsync<T>(string key, T value);
        ValueTask<T> GetItemAsync<T>(string key);
        ValueTask RemoveItemAsync(string key);
    }
}
