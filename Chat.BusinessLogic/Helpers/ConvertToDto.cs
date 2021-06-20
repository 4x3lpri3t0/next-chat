using AutoMapper;
using Chat.BusinessLogic.Dtos;
using Chat.DataAccess.Entities;

namespace Chat.BusinessLogic.Helpers
{
	public static class ConvertToDto
	{
		public static IMapper Mapper = null;

		public static TDto ToDto<TDto>(this BaseEntity obj) where TDto : BaseDto
		{
			if (Mapper == null)
			{
				throw new System.Exception("Incorrect initialization for AutoMapper Helper");
			}
			return Mapper.Map<BaseEntity, TDto>(obj);
		}
	}
}