using Microsoft.EntityFrameworkCore;

namespace GameWeb.Models{
    
    
    public class Field{
        public int FieldId {get;init;}
        public int FieldIndex {get;init;}
        public int? FieldValue {get;set;} 
        public Board Board;
    }
    
}