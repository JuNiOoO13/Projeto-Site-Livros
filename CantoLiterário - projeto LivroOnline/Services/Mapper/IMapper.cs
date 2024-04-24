namespace CantoLiterário___projeto_LivroOnline.Services.Mapper
{
	public interface IMapper
	{
		public T Map<T>(object obj) where T : new();
	}
}
