using LobbyServer.DB;
using System;
using System.Collections.Generic;
using System.Text;

public static class Extensions
{
    public static bool SaveChangesEx(this AppDbContext db)
    {
        try
        {
            db.SaveChanges();
            return true;
        }
        catch
        {
            Console.WriteLine("Error: fail to save DB");
            return false;
        }
    }
    public static bool SaveChangesEx(this WebDbContext db)
    {
        try
        {
            db.SaveChanges();
            return true;
        }
        catch
        {
            Console.WriteLine("Error: fail to save DB");
            return false;
        }
    }
}