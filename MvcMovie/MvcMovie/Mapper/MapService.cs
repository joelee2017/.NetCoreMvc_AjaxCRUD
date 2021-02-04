using AutoMapper;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Mapper
{
    /// <summary>
    /// 使用TinyMapper作為資料表Map工具
    /// </summary>
    public static class MapService
    {
        /// <summary>
        /// TinyMapper.Map
        /// </summary>
        /// <typeparam name="Copy">複製型別</typeparam>
        /// <typeparam name="Paste">貼上型別</typeparam>
        /// <param name="copyData">複製資料</param>
        /// <returns>貼上資料</returns>
        public static Paste Map<Copy, Paste>(this Copy copyData, bool usingAutoMapperWhenException = false)
        {
            Paste paste = default(Paste);

            try
            {
                paste = TinyMapper.Map<Paste>(copyData);
            }
            catch
            {
                if (usingAutoMapperWhenException)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Copy, Paste>());

                    var mapper = config.CreateMapper();

                    paste = mapper.Map<Copy, Paste>(copyData);
                }
            }

            return paste;
        }

        /// <summary>
        /// TinyMapper.Map
        /// </summary>
        /// <typeparam name="Copy">複製型別集合</typeparam>
        /// <typeparam name="Paste">貼上型別集合</typeparam>
        /// <param name="copyData">複製資料集合</param>
        /// <returns>貼上資料集合</returns>
        public static IEnumerable<Paste> Map<Copy, Paste>(this IEnumerable<Copy> copyData, bool usingAutoMapperWhenException = false)
        {
            IEnumerable<Paste> paste = null;

            try
            {
                paste = TinyMapper.Map<List<Paste>>(copyData);
            }
            catch
            {
                if (usingAutoMapperWhenException)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Copy, Paste>());

                    var mapper = config.CreateMapper();

                    paste = mapper.Map<IEnumerable<Copy>, IEnumerable<Paste>>(copyData);
                }
            }

            return paste;
        }

        public static IEnumerable<Paste> ProjectTo<Copy, Paste>(this IQueryable<Copy> copyData)
        {
            IEnumerable<Paste> paste = null;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Copy, Paste>());

            var mapper = config.CreateMapper();

            paste = mapper.ProjectTo<Paste>(copyData);

            return paste;
        }

        /// <summary>
        /// TinyMapper.ProjectTo 針對 IQueryable
        /// </summary>
        /// <typeparam name="Copy">複製型別集合</typeparam>
        /// <typeparam name="Paste">貼上型別集合</typeparam>
        /// <param name="copyData">複製資料集合</param>
        /// <returns>貼上資料集合</returns>
        public static IQueryable<Paste> Map<Copy, Paste>(this IQueryable<Copy> copyData)
        {
            IQueryable<Paste> paste = null;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Copy, Paste>());

            var mapper = config.CreateMapper();

            paste = mapper.ProjectTo<Paste>(copyData);
            return paste;
        }

        /// <summary>
        /// TinyMapper.Bind
        /// </summary>
        /// <typeparam name="Copy">複製型別集合</typeparam>
        /// <typeparam name="Paste">貼上型別集合</typeparam>
        /// <param name="config">對應欄位設定</param>
        public static void Bind<Copy, Paste>(Action<Nelibur.ObjectMapper.Bindings.IBindingConfig<Copy, Paste>> config)
        {
            TinyMapper.Bind(config);
        }
    }
}