//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _28._01ui
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public int Id { get; set; }
        public Nullable<int> IdEvent { get; set; }
        public Nullable<int> IdUser { get; set; }
        public Nullable<int> Price { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> BuyDate { get; set; }
    
        public virtual Events Events { get; set; }
        public virtual Users Users { get; set; }
    }
}
