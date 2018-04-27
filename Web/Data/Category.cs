namespace Web.Data
{
	public class Category
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public ApplicationUser User { get; set; }
		public string UserId { get; set; }
	}
}