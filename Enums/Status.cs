namespace LetsGo.Enums
{
    public enum Status
    {
        NotDefined = 0,//неопределён
        New = 1,//новый
        Rejected = 2,//отклонен
        Published = 3,//опубликован
        UnPublished = 4,//неопубликован
        Edited = 5,//отредактирован
        Expired = 6,//истекший
        ReviewPublished = 7,//в ожидании публикации
        ReviewUnPublished = 8//в ожидании снятия с публикации
    }
}