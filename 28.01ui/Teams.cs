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
    
    public partial class Teams
    {
        public Teams()
        {
            this.Pilots = new HashSet<Pilots>();
        }
    
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string TeamCountry { get; set; }
        public string TeamLogo { get; set; }
        public string TeamBunner { get; set; }
    
        public virtual ICollection<Pilots> Pilots { get; set; }
    }
}
