//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PizzaWepApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comments
    {
        public int CommentId { get; set; }
        public int AuthorId { get; set; }
        public System.DateTime CommentDate { get; set; }
        public string CommentMessage { get; set; }
    
        public virtual User User { get; set; }
    }
}