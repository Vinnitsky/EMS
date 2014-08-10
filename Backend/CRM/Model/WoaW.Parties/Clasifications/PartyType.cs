namespace WoaW.CMS.Model.Clasifications
{
    public class PartyType
    {
        public string Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public PartyType()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public PartyType(string title, string id = null)
        {
            if (string.IsNullOrWhiteSpace(id) == false)
                Id = id;
        }
    }

}

