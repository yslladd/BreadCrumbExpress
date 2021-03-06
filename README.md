BreadCrumbExpress
=================

## 1. **How to use**

**First download the NuGet project for your application.**
 [Donwload](https://www.nuget.org/packages/BreadCrumbExpress/1.0.0)

**After that add in your application class with one static method like this:**
 
 ```c#
 using BreadCrumbExpress; 
 public static class HtmlHelper  
 {
	public static string BreadCrumbs(this System.Web.Mvc.HtmlHelper html)
    {
		//call plugin NuGet
        return BreadCrumb.BreadCrumbs(html);
    }
 }
 ```
 
 **After that make the call to the Helper from within the View of MVC.**
 **For MVC 2 :**
 ```c#
 <%=Html.BreadCrumbs() %> 
 ```
 
## 2. **Creating and Configuring File SiteMap**

 
 First Right click on your project and select Add > New Item
 Then select file SiteMap. xml file will be created that maps our pages.
 
 Made it put a code snippet explaining how to set up our us, these will be important to set the breadcrumb links.
 We registering nodes respecting the hierarchy of pages. example:
 
 **Exemple of SiteMap**
 ```xml
 <?xml version="1.0" encoding="utf-8" ?>
<siteMap xmlns="http://schemas.microsoft.com/AspNet/SiteMap-File-1.0">  
  <!-- first page Home -->
  <siteMapNode title="Home" url="~/">
	<siteMapNode title="About" url="~/About">
		 <siteMapNode title="About Me" url="~/About/Me"/>
	</siteMapNode>
  </siteMapNode>  
 ```
 
  
## 3. **Return Html**

 ```html
 <ul class="navegacao">
	<li><a href="/">Home</a></li>
	<li><a href="/About">About</a></li>
	<li class="navegacao selected">Me</li>
 </ul>
 ``` 
 

 