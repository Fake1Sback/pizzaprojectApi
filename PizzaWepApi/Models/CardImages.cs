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
    
    public partial class CardImages
    {
        public int AdditionalImageId { get; set; }
        public int CardId { get; set; }
        public string ImageURL { get; set; }
    
        public virtual Card Card { get; set; }
    }
}
