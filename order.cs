//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pp11
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int user_id { get; set; }
        public string price { get; set; }
        public string count { get; set; }
        public string sum { get; set; }
    
        public virtual prodact prodact { get; set; }
        public virtual users users { get; set; }
    }
}
