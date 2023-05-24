namespace Task3.RockPaperScissors
{
    public static class Extensions
    {
        public static string BytesAsString(this byte[] bytes) 
            => BitConverter.ToString(bytes).Replace("-","");
       }
}