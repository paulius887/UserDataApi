namespace UserDataApi.Models {
    public class Album {
        public string _id {  get; set; }
        public string artist { get; set; }
        public int year { get; set; }
        public string title { get; set; }
        public float price { get; set; }
        public int stock { get; set; }
        public string publisher { get; set; }
        public string description { get; set; }
    }
}
