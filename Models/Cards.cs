//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FintechBank.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cards
    {
        public int CardID { get; set; }
        public Nullable<int> AccountID { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    
        public virtual Accounts Accounts { get; set; }
    }
}