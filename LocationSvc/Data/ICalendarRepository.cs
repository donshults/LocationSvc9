using System.Collections.Generic;
using LocationSvc.Models;

namespace LocationSvc.Data
{
    public interface ICalendarRepository
    {
        CalendarItemModel AddCalendarItem(string siteUrl, string listName, CalendarItemModel calItem);
        bool DeleteCalendarItemById(string siteUrl, string listName, int itemId);
        CalendarItem GetCalendarItemById(string siteUrl, string listName, int itemId);
        IEnumerable<CalendarItem> GetCalendarItems(string siteUrl, string listName);
        CalendarItemModel UpdateCalendarItem(string siteUrl, string listName, CalendarItemModel calItem);
    }
}