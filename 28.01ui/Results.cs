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
    
    public partial class Results
    {
        public int Id { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> PilotId { get; set; }
        public Nullable<int> Points { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
    
        public virtual Events Events { get; set; }
        public virtual Pilots Pilots { get; set; }
    }
}
