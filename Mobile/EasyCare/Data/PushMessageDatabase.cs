using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Models;
using SQLite;

namespace EasyCare.Data
{
    public class PushMessageDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public PushMessageDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<PushMessage>().Wait();
        }

        public Task<List<PushMessage>> GetPushMessageAsync()
        {
            return _database.Table<PushMessage>().ToListAsync();
        }

        public Task<PushMessage> GetPushMessageAsync(int id)
        {
            return _database.Table<PushMessage>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SavePushMessageAsync(PushMessage pushMessage)
        {
            if (pushMessage.ID != 0)
            {
                return _database.UpdateAsync(pushMessage);
            }
            else
            {
                return _database.InsertAsync(pushMessage);
            }
        }

        public Task<int> DeletePushMessageAsync(PushMessage pushMessage)
        {
            return _database.DeleteAsync(pushMessage);
        }

        public Task<int> DeletePushMessageAsync()
        {
            return _database.DeleteAllAsync<PushMessage>();
        }

    }
}
