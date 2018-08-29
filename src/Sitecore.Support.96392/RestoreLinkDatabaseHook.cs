
namespace Sitecore.Support.RestoreLink
{
  using Sitecore.Configuration;
  using Sitecore.Events.Hooks;
  using Sitecore.Eventing;
  using Sitecore.Pipelines;
  using Sitecore.Services;
  using Sitecore.Data.Archiving;
  using Sitecore.Diagnostics;
  using Sitecore.Data.Items;
  using System;

  public class RestoreLinkDatabaseHook : IHook
    {
       

       
        public void Initialize()
        {
            EventManager.Subscribe<RestoreItemCompletedEvent>(delegate(RestoreItemCompletedEvent e, EventContext c)
            {
                Sitecore.Data.ID itemId = new Sitecore.Data.ID(e.ItemId);
                Sitecore.Caching.CacheManager.ClearAllCaches();
                Item item = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(itemId);
                Assert.IsNotNull(item, "the item doesn't exist!");
                Sitecore.Links.LinkDatabase linkDatabase = Sitecore.Globals.LinkDatabase;          
                if (linkDatabase != null)
                {
                    linkDatabase.UpdateItemVersionReferences(item);
                }
                else
                {
                    Log.Info("Linkdatabase doesn't exist!", this);
                }
            });
        }

    }
}
