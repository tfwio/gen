/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 04/01/2012
 * Time: 01:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Cor3.Data.Engine;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Windows;
using CodeSane.MetaWeblog.Api;
using CookComputing.XmlRpc;

namespace System.Cor3.Rpc
{
	
	
	/// <summary>
	/// The purpose of this class seems to have become neglected.
	/// This class is to harness the information within a blog which seems
	/// to be a step procedure.
	/// <para>1. Load blogs from account</para>
	/// <para>2. Load User information and Blog information.</para>
	/// <para>3. Retrieve an index of Keys, Categories and Posts (Pages may help)</para>
	/// <hr />
	/// <para>Sub-Routines: SavePost, NewPost</para>
	/// <para>Utility-Routines: Bulk-Export, Bulk-Import</para>
	/// </summary>
	public class WordPressInfo : XInfo<IMetaWeblog>, INotifyPropertyChanged
	{
		/// <summary>
		/// A stack of posts to maintain.
		/// </summary>
		
		public Stack<Post> Post { get;set; }
		
		/**
		 * 
		 */
		
		/// <summary>
		/// There should be some protocol for this method.
		/// <para>1 - Store the Post from UI to Data-Bound Array Item.</para>
		/// <para>2 - Store the Post via HTTP.</para>
		/// <para>3 - Get an updated version of the Post into the Data-Bound Array Item.</para>
		/// <para>4 - Update the UI or INotifyPropertyChanged Event</para>
		/// </summary>
		/// <remarks>
		/// FIXME: We need to fix time-zone information in perhaps a configuration file in order
		/// NOT to make a mess inserting and updating entries.
		/// </remarks>
		/// <param name="post"></param>
		/// <returns></returns>
		public Post EditPost(Post post)
		{
			DateTime originalCRD = post.CRD;
			post.CRD = originalCRD.ToUniversalTime();
			bool returnTrue = (bool) proxy.editPost(post.postid.ToString(),this.User.username,this.User.password,post,true);
			if (!returnTrue) System.Windows.Forms.MessageBox.Show("There was an error sending XML-RPC POST", "Error");
			// reset the CRD to the date that had been in the post, or simply re-retrieve the post.
			post.CRD = originalCRD;
			return post;
		}
		
		/// <summary>
		/// Create a new post.
		/// </summary>
		/// <remarks>
		/// FIXME: We need to fix time-zone information in perhaps a configuration file in order
		/// NOT to make a mess inserting and updating entries.
		/// </remarks>
		/// <param name="post"></param>
		/// <returns>the post</returns>
		public Post NewPost(Post post)
		{
			// returns postid as a string.
			string postid = proxy.newPost(this.User.blogid,this.User.username,this.User.password,post,true);
			post.postid = postid;
			return post;
		}
		
		/**
		 * Pages
		 */
		//——————————————————————————————— blank
		/**
		 * Posts
		 */
		static int recent_post_count_max = 50;
		public List<Post> Posts { get;set; }
		
		public void GetRecentPosts() { GetRecentPosts(recent_post_count_max); }
		public void GetRecentPosts(int max) {
			this.Posts = new List<Post>(proxy.getRecentPosts(this.User.blogid,this.User.username,this.User.password,max));
			OnPropertyChanged("Posts");
		}
		
		/**
		 * Tag Info
		 */
		
		public List<TagInfo> Tags { get;set; }
		
		public void GetTags()
		{
			// no get formats function!
			
			#if !DEBUG
			try {
			#endif	
			this.Tags = new List<TagInfo>(proxy.getTags(this.User.blogid, this.User.username,this.User.password));
			#if !DEBUG	
			} catch (Exception ex) { XLog.Warn("Error",": Couldn't get tags."); XLog.Warn("Error",": {0}",ex.ToString()); }
			#endif
			
			OnPropertyChanged("Tags");
		}
		
		/**
		 * Category Info
		 */
		
//		public List<Category> Cagegory { get;set; }
		public List<WordPressCategory> Categories { get;set; }
		public void GetCategories()
		{
			#if !DEBUG
			try {
			#endif	
			this.Categories = new List<WordPressCategory>(proxy.getWpCategories(this.User.blogid, this.User.username, this.User.password));
			#if !DEBUG	
			} catch (Exception ex) { XLog.Warn("Error",": Couldn't get tags."); XLog.Warn("Error",": {0}",ex.ToString()); }
			#endif
			OnPropertyChanged("CategoryInfo");
		}
		
		
		public WordPressInfo(string blogid, string user, string password, string Url) : this(blogid,user,password,Url,System.Text.Encoding.UTF8) {}
		public WordPressInfo(string blogid, string user, string password, string Url, System.Text.Encoding Encoding) : base(blogid,user,password,Url) {
			this.Post = new Stack<Post>();
		}
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, e);
			}
		}
	}
}
