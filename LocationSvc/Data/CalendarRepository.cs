using LocationSvc.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace LocationSvc.Data
{
    public class CalendarRepository : ICalendarRepository
    {
        public CalendarRepository()
        {

        }

        public IEnumerable<CalendarItem> GetCalendarItems(string siteUrl, string listName)
        {
            List<CalendarItem> calItems = new List<CalendarItem>();
            string realm = ConfigurationManager.AppSettings["ida:Audience"];
            string appId = ConfigurationManager.AppSettings["ida:ClientId"];
            string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];

            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext context = authManager.GetAppOnlyAuthenticatedContext(siteUrl, appId, appSecret))
            {
                try
                {
                    List oList = context.Web.Lists.GetByTitle(listName);
                    CamlQuery camlQuery = CamlQuery.CreateAllItemsQuery(100);
                    ListItemCollection listItems = oList.GetItems(camlQuery);
                    context.Load(listItems);
                    context.ExecuteQuery();
                    foreach (ListItem oListItem in listItems)
                    {
                        var fields = oListItem.FieldValues;
                        var calItem = new CalendarItem();
                        foreach (var field in fields)
                        {
                            switch (field.Key)
                            {
                                case "ID":
                                    calItem.ID = Convert.ToInt32(field.Value);
                                    break;
                                case "Title":
                                    calItem.Title = field.Value.ToString();
                                    break;
                                case "Description":
                                    calItem.Description = WebUtility.HtmlDecode(field.Value.ToString());
                                    break;
                                case "FileDirRef":
                                    calItem.FileDirRef = field.Value.ToString();
                                    break;
                                case "FileRef":
                                    calItem.FileRef = field.Value.ToString();
                                    break;
                                case "Location":
                                    calItem.Location = field.Value.ToString();
                                    break;
                                case "Created":
                                    calItem.Created = DateTime.Parse(field.Value.ToString());
                                    break;
                                case "Modified":
                                    calItem.Modified = DateTime.Parse(field.Value.ToString());
                                    break;
                                case "EndDate":
                                    calItem.EndDate = DateTime.Parse(field.Value.ToString());
                                    break;
                                case "EventDate":
                                    calItem.EventDate = DateTime.Parse(field.Value.ToString());
                                    break;
                                case "Author":
                                    FieldUserValue itemAuthor = field.Value as FieldUserValue;
                                    var author = new Models.UserModel();
                                    author.Email = itemAuthor.Email;
                                    author.LookupId = itemAuthor.LookupId;
                                    author.LookupValue = itemAuthor.LookupValue;
                                    calItem.Author = author;
                                    break;
                                case "Editor":
                                    FieldUserValue itemEditor = field.Value as FieldUserValue;
                                    var editor = new Models.UserModel();
                                    editor.Email = itemEditor.Email;
                                    editor.LookupId = itemEditor.LookupId;
                                    editor.LookupValue = itemEditor.LookupValue;
                                    calItem.Editor = editor;
                                    break;
                            }
                        }
                        calItem.ListName = listName;
                        calItem.SiteUrl = siteUrl;
                        calItems.Add(calItem);
                    }
                    return calItems;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public CalendarItem GetCalendarItemById(string siteUrl, string listName, int itemId)
        {
            string realm = ConfigurationManager.AppSettings["ida:Audience"];
            string appId = ConfigurationManager.AppSettings["ida:ClientId"];
            string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
            CalendarItem calItem = new CalendarItem();

            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext context = authManager.GetAppOnlyAuthenticatedContext(siteUrl, appId, appSecret))
            {
                try
                {
                    List oList = context.Web.Lists.GetByTitle(listName);
                    ListItem oItem = oList.GetItemById(itemId);
                    context.Load(oItem);
                    context.ExecuteQuery();

                    calItem.Title = oItem["Title"].ToString();
                    calItem.ID = oItem.Id;
                    calItem.Description = oItem["Description"].ToString();
                    calItem.Created = DateTime.Parse(oItem["Created"].ToString());
                    calItem.EndDate = DateTime.Parse(oItem["EndDate"].ToString());
                    calItem.EventDate = DateTime.Parse(oItem["EventDate"].ToString());
                    calItem.FileDirRef = oItem["FileDirRef"].ToString();
                    calItem.FileRef = oItem["FileRef"].ToString();
                    calItem.Location = oItem["Location"].ToString();
                    calItem.Modified = DateTime.Parse(oItem["Modified"].ToString());
                    FieldUserValue itemAuthor = oItem["Author"] as FieldUserValue;
                    var author = new Models.UserModel();
                    author.Email = itemAuthor.Email;
                    author.LookupId = itemAuthor.LookupId;
                    author.LookupValue = itemAuthor.LookupValue;
                    calItem.Author = author;
                    FieldUserValue itemEditor = oItem["Editor"] as FieldUserValue;
                    var editor = new Models.UserModel();
                    editor.Email = itemEditor.Email;
                    editor.LookupId = itemEditor.LookupId;
                    editor.LookupValue = itemEditor.LookupValue;
                    calItem.Editor = editor;
                    calItem.SiteUrl = siteUrl;
                    calItem.ListName = listName;
                    return calItem;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool DeleteCalendarItemById(string siteUrl, string listName, int itemId)
        {
            string realm = ConfigurationManager.AppSettings["ida:Audience"];
            string appId = ConfigurationManager.AppSettings["ida:ClientId"];
            string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
            bool success = false;

            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext context = authManager.GetAppOnlyAuthenticatedContext(siteUrl, appId, appSecret))
            {
                try
                {
                    List oList = context.Web.Lists.GetByTitle(listName);
                    ListItem oItem = oList.GetItemById(itemId);
                    oList.DeleteObject();
                    context.ExecuteQuery();
                    success = true;
                    return success;
                }
                catch (Exception ex)
                {
                    return success;
                }
            }
        }

        public CalendarItemModel AddCalendarItem(string siteUrl, string listName, CalendarItemModel calItem)
        {
            string realm = ConfigurationManager.AppSettings["ida:Audience"];
            string appId = ConfigurationManager.AppSettings["ida:ClientId"];
            string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext context = authManager.GetAppOnlyAuthenticatedContext(siteUrl, appId, appSecret))
            {
                try
                {
                    List oList = context.Web.Lists.GetByTitle(listName);
                    ListItemCreationInformation oItemCreateInfo = new ListItemCreationInformation();
                    ListItem oItem = oList.AddItem(oItemCreateInfo);
                    oItem["Description"] = calItem.Description;
                    oItem["Location"] = calItem.Location;
                    oItem["EventDate"] = calItem.EventDate;
                    oItem["EndDate"] = calItem.EndDate;
                    oItem["Author"] = calItem.Author;
                    oItem["Editor"] = calItem.Editor;
                    oItem.Update();
                    context.ExecuteQuery();
                    return calItem;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            }

        }

        public CalendarItemModel UpdateCalendarItem(string siteUrl, string listName, CalendarItemModel calItem)
        {
            string realm = ConfigurationManager.AppSettings["ida:Audience"];
            string appId = ConfigurationManager.AppSettings["ida:ClientId"];
            string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext context = authManager.GetAppOnlyAuthenticatedContext(siteUrl, appId, appSecret))
            {
                try
                {
                    List oList = context.Web.Lists.GetByTitle(listName);
                    ListItemCreationInformation oItemCreateInfo = new ListItemCreationInformation();
                    ListItem oItem = oList.GetItemById(calItem.ID);

                    oItem["Description"] = calItem.Description;
                    oItem["Location"] = calItem.Location;
                    oItem["EventDate"] = calItem.EventDate;
                    oItem["EndDate"] = calItem.EndDate;
                    oItem["Author"] = calItem.Author;
                    oItem["Editor"] = calItem.Editor;
                    oItem.Update();
                    context.ExecuteQuery();
                    return calItem;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            }

        }

    }
}