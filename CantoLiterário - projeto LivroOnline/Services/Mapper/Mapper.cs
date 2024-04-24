namespace CantoLiterário___projeto_LivroOnline.Services.Mapper
{
	public class Mapper : IMapper
	{
		public T Map<T>(object obj) where T : new()
		{
			T obj2 = new();
			var propertiesObj1 = obj.GetType().GetProperties();
			var propertiesObj2 = obj2.GetType().GetProperties();
			foreach (var prop in propertiesObj1)
			{
				foreach (var prop2 in propertiesObj2)
				{
					if (prop.Name == prop2.Name)
					{
						prop2.SetValue(obj2, prop.GetValue(obj));
					}
				}
			}
			return obj2;
		}

	}
}
