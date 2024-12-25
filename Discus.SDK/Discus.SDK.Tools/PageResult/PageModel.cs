using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.SDK.Tools.PageResult
{
    public class PageModel<T>
    {
        public List<T> Data { get; set; }

        public int Count { get; set; }

        public PageModel()
        {

        }

        public PageModel(List<T> data, int count)
        {
            this.Data = data;
            this.Count = count;
        }

        public PageModel<TOut> ConvertTo<TOut>(IMapper mapper)
        {
            var model = new PageModel<TOut>();

            if (Data != null)
            {
                model.Data = mapper.Map<List<TOut>>(Data);
            }
            model.Count = mapper.Map<int>(Count);
            return model;
        }
    }
}
