using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.Data
{
    public class FlickrDataSource : DataSourceBase<FlickrSchema>
    {
        private const string _url = @"http://api.flickr.com/services/feeds/photos_public.gne?tags=Minas+Gerais";

        protected override string CacheKey
        {
            get { return "FlickrDataSource"; }
        }

        public override bool HasStaticData
        {
            get { return false; }
        }

        public async override Task<IEnumerable<FlickrSchema>> LoadDataAsync()
        {
            try
            {
                var rssDataProvider = new RssDataProvider(_url);
                var syndicationItems = await rssDataProvider.Load();
                return (from r in syndicationItems
                         select new FlickrSchema()
                         {
                             Title = r.Title,
                             Summary = r.Summary,
                             // Change medium images to large
                             ImageUrl = r.ImageUrl.Replace("_m.jpg", "_b.jpg"),
                             Published = r.PublishDate,
                             FeedUrl = r.FeedUrl
                         }).ToArray();
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("FlickrDataSourceDataSource.LoadData", ex.ToString());
                return new FlickrSchema[0];
            }
        }
    }
}
