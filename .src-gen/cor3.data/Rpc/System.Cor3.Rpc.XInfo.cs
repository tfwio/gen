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
using CodeSane.MetaWeblog.Api;
using CookComputing.XmlRpc;

namespace System.Cor3.Rpc
{
	public class XInfo<TRpcI> where TRpcI:IXmlRpcProxy
	{
		internal TRpcI proxy;
		internal readonly UserInfo User;
		internal readonly System.Text.Encoding EncodingDefault;
		
		
		void InitializeProxy()
		{
			this.proxy.Url = this.User.url;
			this.proxy.XmlEncoding = this.EncodingDefault;
		}
		
		
		public XInfo(string blogid, string user, string password, string Url) : this(blogid,user,password,Url,System.Text.Encoding.UTF8) {}
		public XInfo(string blogid, string user, string password, string Url, System.Text.Encoding Encoding)
		{
			proxy = XmlRpcProxyGen.Create<TRpcI>();
			this.User.blogid = blogid;
			this.User.blog_id = 1;
			this.User.username = user;
			this.User.password = password;
			this.User.url = Url;
			this.EncodingDefault = Encoding;
			InitializeProxy();
		}
	}
}
