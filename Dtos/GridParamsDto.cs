namespace DatingApp.API.Dtos
{
    public class GridParamsDto
    {
        public int Page { get; set; } = 1;
        
        protected const int MaxLimit = 50;

        protected int limit = 10;

        public int Limit
        {
            get => limit;
            set => limit = value > MaxLimit ? MaxLimit : value;
        }
    }
}