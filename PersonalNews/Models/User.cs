using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonalNews.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        /// </summary>
    }
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name="专栏名称")]
        public string CategoryName { get; set; }
    }

    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        /// <summary>
        /// 栏目id
        /// </summary>
        [Display(Name = "栏目ID")]
        [Required(ErrorMessage = "×")]
        public int CategoryId { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [Display(Name = "来源")]
        [StringLength(255, ErrorMessage = "×")]
        public string Source { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [StringLength(50, ErrorMessage = "×")]
        public string Author { get; set; }
        /// <summary>
        /// 摘要
        /// </summary> [NotMapped]
        [Display(Name = "摘要")]
        public string Intro { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "×")]
        [DataType(DataType.Html)]
        public string Content { get; set; }
    }
    public class userDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}