using System;
using CookComputing.XmlRpc;

/**
 * it really does bother me that I can't find the original source
 * for this particular set of structures.
 * ---------------------------------------------------------------
 * Granted, it isn't very complicated, however I'm thankful
 * considering the fact that it is responsible for showing me
 * the 'rules' of using CookComputing.XmlRpc library for working
 * WordPress XmlRpc API.
 * ---------------------------------------------------------------
 * I had found the source once, and that means that I probably have
 * the sources lying around somewhere but I just can't remember 
 * at the moment and you know how fast time can pass.
 * -------------
 * The following source has been modified.
 */

namespace CodeSane.MetaWeblog.Api
{
	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public struct Enclosure
	{
		public int length;
		public string type;
		public string url;
	}

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public struct Source
	{
		public string name;
		public string url;
	}

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public struct Post
	{
		[XmlRpcMissingMapping(MappingAction.Error)]
		[XmlRpcMember(Description="Required when posting.")]
		
		public DateTime dateCreated;
		public DateTime CRD {
			get { return dateCreated; }
			set { dateCreated = value; }
		}
		
		[XmlRpcMissingMapping(MappingAction.Error)]
		[XmlRpcMember(Description="Required when posting.")]
		public string description;
		
		[XmlRpcMember(Description="Required when posting.")]
		public string title;
		
		public string Title {
			get { return System.Web.HttpUtility.HtmlDecode(title); }
			set { title = System.Web.HttpUtility.HtmlEncode(value); }
		}

		public string[] categories;
		public Enclosure enclosure;
		public string link;
		public string permalink;
		
		[XmlRpcMember(Description="Not required when posting. Depending on server may be either string or integer. Use Convert.ToInt32(postid) to treat as integer or Convert.ToString(postid) to treat as string")]
		public object postid;
		
		public Source source;
		public string userid;

		public object mt_allow_comments;
		public object mt_allow_pings;
		public object mt_convert_breaks;
		public string mt_text_more;
		public string mt_excerpt;
		/// <summary>
		/// Last minute tfw HACK: for wordpress.
		/// </summary>
		public object mt_keywords;
		
		public string[] Keywords {
			get { return mt_keywords.ToString().Split(','); }
			set { mt_keywords = string.Join(", ", value); }
		}
		
		public object Tags {
			get { return mt_keywords; }
			set { mt_keywords = value; }
		}
	}

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public struct CategoryInfo
	{
		public string description;
		public string htmlUrl;
		public string rssUrl;
		public string title;
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public string Title { get { return title; } set { title = value; } }
		
		public object categoryid;
	}
	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public struct WordPressCategory
	{
		public object categoryid;
		public object parentId;
		public string description;
		public string categoryName;
		
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public string CategoryName {
			get { return categoryName; }
			set { categoryName = value; }
		}
		public string htmlUrl;
		public string rssUrl;
		
	}

	public struct Category
	{
		public string categoryId;
		public string categoryName;
	}

	public struct FileData
	{
		public byte[] bits;
		public string name;
		public string type;
	}

	public struct UrlData
	{
		public string url;
	}

	public struct BlogInfo
	{
		public string blogid;
		public string url;
		public string blogName;
	}

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public struct TagInfo
	{
		public object tag_id;
		public string name;
		
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public string Name {
			get { return name; }
			set { name = value; }
		}
		public object count;
		public string slug;
		public string html_url;
		public string rss_url;
	}
	
	/// <summary>
	/// See 'wp.getAuthors'
	/// </summary>
	public struct AuthorInfo
	{
		/// <summary>INT</summary>
		public object user_id;
		public string user_login;
		public string display_name;
		public string meta_value; // serialized PHP data
		
	}
	
	// HACK: Added
	/// <summary>
	/// wp.getUserBlogs
	/// </summary>
	public struct BlogInfo2
	{
		public bool isAdmin;
		public string url;
		public string blogid;
		public string blogName;
		public string xmlrpc;
		
//		public int blog_id;
//		public string username;
		public string password;
	}
	// HACK: Added
	/// <summary>
	/// wp.getUserBlogs
	/// </summary>
	public struct UserInfo
	{
		public string url;
		public string blogid;
		public string blogName;
		public string xmlrpc;
		
		public int blog_id;
		public string username;
		public string password;
	}
	
	
	public interface IMetaWeblog:IXmlRpcProxy
	{
		// HACK: Added wP Tag Support
		[XmlRpcMethod("wp.getTags",Description = "Retrieves a list of post tags.")]
		TagInfo[] getTags(string blogid,string username,string password);
		// HACK: Added wP Tag Support
		[XmlRpcMethod("wp.getCategories",Description = "Retrieves a list of post tags.")]
		WordPressCategory[] getWpCategories(string blogid,string username,string password);
		
		/// <summary>
		/// This is described to return a boolean value.
		/// </summary>
		/// <param name="postid"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="post"></param>
		/// <param name="publish"></param>
		/// <returns></returns>
		[XmlRpcMethod("metaWeblog.editPost",Description="Updates and existing post to a designated blog using the metaWeblog API. Returns true if completed.")]
		object editPost(string postid,string username,string password,Post post,bool publish);

		[XmlRpcMethod("metaWeblog.getCategories",Description="Retrieves a list of valid categories for a post using the metaWeblog API. Returns the metaWeblog categories struct collection.")]
		CategoryInfo[] getCategories(string blogid,string username,string password);

		[XmlRpcMethod("metaWeblog.getPost",Description="Retrieves an existing post using the metaWeblog API. Returns the metaWeblog struct.")]
		Post getPost(string postid,string username,string password);

		[XmlRpcMethod("metaWeblog.getRecentPosts",Description="Retrieves a list of the most recent existing post using the metaWeblog API. Returns the metaWeblog struct collection.")]
		Post[] getRecentPosts(string blogid,string username,string password,int numberOfPosts);

		[XmlRpcMethod("metaWeblog.newPost",Description="Makes a new post to a designated blog using the metaWeblog API. Returns postid as a string.")]
		string newPost(string blogid,string username,string password,Post post,bool publish);

		[XmlRpcMethod("metaWeblog.newMediaObject",Description = "Makes a new file to a designated blog using the metaWeblog API. Returns url as a string of a struct.")]
		UrlData newMediaObject(string blogid,string username,string password, FileData file);
	}

	public interface ICSMetaWeblog:IXmlRpcProxy
	{
		// Methods
		[return: XmlRpcReturnValue(Description = "Always returns true.")]
		[XmlRpcMethod("blogger.deletePost", Description = "Deletes a post.")]
		bool deletePost(string appKey, string postid, string username, string password, [XmlRpcParameter(Description = "Where applicable, this specifies whether the blog should be republished after the post has been deleted.")] bool publish);
		
		[XmlRpcMethod("metaWeblog.editPost", Description = "Updates and existing post to a designated blog using the metaWeblog API. Returns true if completed.")]
		bool editPost(string postid, string username, string password, Post post, bool publish);
		
		[XmlRpcMethod("metaWeblog.getCategories", Description = "Retrieves a list of valid categories for a post using the metaWeblog API. Returns the metaWeblog categories struct collection.")]
		CategoryInfo[] getCategories(string blogid, string username, string password);
		
		[XmlRpcMethod("metaWeblog.getPost", Description = "Retrieves an existing post using the metaWeblog API. Returns the metaWeblog struct.")]
		Post getPost(string postid, string username, string password);
		
		[XmlRpcMethod("metaWeblog.getRecentPosts", Description = "Retrieves a list of the most recent existing post using the metaWeblog API. Returns the metaWeblog struct collection.")]
		Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts);
		
		[XmlRpcMethod("blogger.getUsersBlogs", Description = "Returns information on all the blogs a given user is a member.")]
		BlogInfo[] getUsersBlogs(string appKey, string username, string password);
		
		[XmlRpcMethod("metaWeblog.newPost", Description = "Makes a new post to a designated blog using the metaWeblog API. Returns postid as a string.")]
		string newPost(string blogid, string username, string password, Post post, bool publish);
	}

}

