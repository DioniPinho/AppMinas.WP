using System;
using System.Collections;

namespace AppStudio.Data
{
    /// <summary>
    /// Implementation of the FlickrSchema class.
    /// </summary>
    public class FlickrSchema : BindableSchemaBase, IEquatable<FlickrSchema>, IComparable<FlickrSchema>
    {
        private string _title;
        private string _summary;
        private string _imageUrl;
        private DateTime _published;
        private string _feedUrl;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
 
        public string Summary
        {
            get { return _summary; }
            set { SetProperty(ref _summary, value); }
        }
 
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }
 
        public DateTime Published
        {
            get { return _published; }
            set { SetProperty(ref _published, value); }
        }

        public string FeedUrl
        {
            get { return _feedUrl; }
            set { SetProperty(ref _feedUrl, value); }
        }

        public override string DefaultTitle
        {
            get { return Title; }
        }

        public override string DefaultSummary
        {
            get { return Summary; }
        }

        public override string DefaultImageUrl
        {
            get { return ImageUrl; }
        }

        public override string DefaultContent
        {
            get { return Summary; }
        }

        override public string GetValue(string fieldName)
        {
            if (!String.IsNullOrEmpty(fieldName))
            {
                switch (fieldName.ToLowerInvariant())
                {
                    case "title":
                        return String.Format("{0}", Title);
                    case "summary":
                        return String.Format("{0}", Summary);
                    case "imageurl":
                        return String.Format("{0}", ImageUrl);
                    case "published":
                        return String.Format("{0}", Published);
                    case "feedurl":
                        return String.Format("{0}", FeedUrl);
                    case "defaulttitle":
                        return String.Format("{0}", DefaultTitle);
                    case "defaultsummary":
                        return String.Format("{0}", DefaultSummary);
                    case "defaultimageurl":
                        return String.Format("{0}", DefaultImageUrl);
                    default:
                        break;
                }
            }
            return String.Empty;
        }

        public bool Equals(FlickrSchema other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return this.FeedUrl == other.FeedUrl;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FlickrSchema);
        }

        public override int GetHashCode()
        {
            return this.FeedUrl.GetHashCode();
        }

        public int CompareTo(FlickrSchema other)
        {
            return -1 * this.Published.CompareTo(other.Published);
        }
    }
}
