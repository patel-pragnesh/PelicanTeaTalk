﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using WPSTORE.Helpers;

namespace WPSTORE.Models.Posts
{

    public class Post
    {
        public int id { get; set; }
        [JsonIgnore]
        public string CategoryName => GlobalSettings.WpCategories?.FirstOrDefault(x => x.id == categories.FirstOrDefault())?.name;
        public DateTime date { get; set; }
        [JsonIgnore]
        public string CreatedDate => date.ToString("MMM d, yyyy").ToUpper();
        public DateTime date_gmt { get; set; }
        public Guid guid { get; set; }
        public DateTime modified { get; set; }
        public DateTime modified_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public Title title { get; set; }
        public Content content { get; set; }
        public Excerpt excerpt { get; set; }
        [JsonIgnore]
        public string ExcerptContent => excerpt != null ? excerpt.rendered?.RemoveHtmlTag() : string.Empty;
        public int author { get; set; }
        public int featured_media { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public bool sticky { get; set; }
        public string template { get; set; }
        public string format { get; set; }
        public Meta meta { get; set; }
        public int[] categories { get; set; }
        public int[] tags { get; set; }
        public string jetpack_featured_media_url { get; set; }
        public object[] jetpack_publicize_connections { get; set; }
        public bool jetpack_sharing_enabled { get; set; }
        public string jetpack_shortlink { get; set; }
        public _Links _links { get; set; }
    }

    public class Guid
    {
        public string rendered { get; set; }
    }

    public class Title
    {
        public string rendered { get; set; }
    }

    public class Content
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Excerpt
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Meta
    {
        public string spay_email { get; set; }
        public string jetpack_publicize_message { get; set; }
    }

    public class _Links
    {
        public Self[] self { get; set; }
        public Collection[] collection { get; set; }
        public About[] about { get; set; }
        public Author[] author { get; set; }
        public Reply[] replies { get; set; }
        public VersionHistory[] versionhistory { get; set; }
        public PredecessorVersion[] predecessorversion { get; set; }
        public WpFeaturedmedia[] wpfeaturedmedia { get; set; }
        public WpAttachment[] wpattachment { get; set; }
        public WpTerm[] wpterm { get; set; }
        public Cury[] curies { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class About
    {
        public string href { get; set; }
    }

    public class Author
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Reply
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class VersionHistory
    {
        public int count { get; set; }
        public string href { get; set; }
    }

    public class PredecessorVersion
    {
        public int id { get; set; }
        public string href { get; set; }
    }

    public class WpFeaturedmedia
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class WpAttachment
    {
        public string href { get; set; }
    }

    public class WpTerm
    {
        public string taxonomy { get; set; }
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Cury
    {
        public string name { get; set; }
        public string href { get; set; }
        public bool templated { get; set; }
    }

    
}
