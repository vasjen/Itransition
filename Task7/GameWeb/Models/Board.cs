using Microsoft.EntityFrameworkCore;

namespace GameWeb.Models{

    
    public class Board{
        public int BoardId {get;init;}
        public int FieldId {get;set;}
        public List<Field>? Fields {get;set;} 
    }
    
}